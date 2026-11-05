using System.Text.Json.Serialization;
using FluentValidation;
using OrderAPI.Infrastructure;
using OrderAPI.Repositories;
using OrderAPI.Services;
using OrderAPI.Settings;
using OrderAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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
