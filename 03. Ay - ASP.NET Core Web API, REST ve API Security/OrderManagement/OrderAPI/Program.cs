using System.Text.Json.Serialization;
using Asp.Versioning;
using FluentValidation;
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
builder.Services.AddSwaggerGen();

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


var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
