using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OrderAPI.Authorization;
using OrderAPI.Infrastructure;
using OrderAPI.Middlewares;
using OrderAPI.Repositories;
using OrderAPI.Services;
using OrderAPI.Settings;
using OrderAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Konsol loglarında scope bilgisini (CorrelationId) göster
builder.Logging.AddSimpleConsole(options => options.IncludeScopes = true);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// API Versiyonlama
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;                             // "api-supported-versions" header'ı
        options.ApiVersionReader = new UrlSegmentApiVersionReader(); // versiyonu URL'den oku: /api/v1/...
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";        // Swagger grubu adı: "v1"
        options.SubstituteApiVersionInUrl = true; // Swagger'da {version} yerine "1" yazar
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Sadece token'ı yapıştır ('Bearer' yazmana gerek yok)"
    });
    
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement()
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// appsettings.json'daki "OrderSettings" bölümünü OrderSettings sınıfına bağla
builder.Services.Configure<OrderSettings>(builder.Configuration.GetSection(OrderSettings.SectionName));

// Repository: Singleton -> uygulama boyunca tek nesne, veri kaybolmaz
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

// Service: Scoped -> her HTTP isteği için yeni nesne
builder.Services.AddScoped<IOrderService, OrderService>();

// Validators klasöründeki tüm validator sınıflarını otomatik kaydet
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();

// Hata yönetimi
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// 1) JWT ayarlarını oku
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;
builder.Services.AddSingleton<JwtTokenGenerator>();

// 2) Authentication: "Sen kimsin?" -> gelen token'ı doğrula
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,            // süresi geçmiş token reddedilir
            ValidateIssuerSigningKey = true,    // imza bizim anahtarla mı atılmış?
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

// 3) Authorization: "Bunu yapmaya yetkin var mı?"
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("OrderOwnerOrAdmin", policy => policy.AddRequirements(new OrderOwnerOrAdminRequirement()));
});
builder.Services.AddSingleton<IAuthorizationHandler, OrderOwnerOrAdminHandler>();

// 4) Rate Limiting: kullanıcı başına dakikada 5 sipariş
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("PerUserOrderPolicy", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            // Her kullanıcının kendi "kovası" olsun. Token yoksa IP'ye göre ayır.
            partitionKey: httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? httpContext.Connection.RemoteIpAddress?.ToString()
                          ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,                    // 5 istek
                Window = TimeSpan.FromMinutes(1),   // 1 dakikada
                QueueLimit = 0                      // fazlası beklemesin, direkt 429
            }));
});

// 5) CORS: sadece B2B istemcisinin tarayıcıdan çağırmasına izin ver
builder.Services.AddCors(options =>
{
    options.AddPolicy("B2BClient", policy =>
        policy.WithOrigins("https://b2b.orderclient.com")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.MapPost("/dev/token", (string? role, JwtTokenGenerator generator) =>
    {
        var userId = Guid.NewGuid();
        return Results.Ok(new { userId, role, token = generator.Generate(userId, role) });
    });
}

app.UseHttpsRedirection();
app.UseCors("B2BClient");
app.UseAuthentication();  // 1) kim olduğunu bul (User dolar)
app.UseRateLimiter();     // 2) User artık dolu -> kullanıcı bazlı limit çalışır
app.UseAuthorization();
app.MapControllers();

app.Run();
