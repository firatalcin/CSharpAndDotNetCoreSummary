using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Infrastructure;

namespace OrderAPI.IntegrationTests;

public class OrdersApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string OrdersUrl = "/api/v1/orders";
    private readonly WebApplicationFactory<Program> _factory;

    public OrdersApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    // Verilen kullanıcı için token üretip Authorization header'ına koyan HttpClient
    private HttpClient CreateClientFor(Guid userId, string? role = null)
    {
        var client = _factory.CreateClient();
        var token = _factory.Services.GetRequiredService<JwtTokenGenerator>().Generate(userId, role);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    // 1 x 100 TL = 100 TL -> MinOrderAmount (50) üstünde, geçerli sipariş
    private static object ValidOrder() => new
    {
        items = new[]
        {
            new { productId = Guid.NewGuid(), productName = "Klavye", quantity = 1, unitPrice = 100m }
        }
    };

    [Fact]
    public async Task CreateOrder_WithInvalidData_Returns400()
    {
        var client = CreateClientFor(Guid.NewGuid());
        var invalidOrder = new
        {
            items = new[]
            {
                new { productId = Guid.NewGuid(), productName = "Mouse", quantity = -1, unitPrice = 100m }
            }
        };

        var response = await client.PostAsJsonAsync(OrdersUrl, invalidOrder);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetOrder_WithoutToken_Returns401()
    {
        var client = _factory.CreateClient(); // token YOK

        var response = await client.GetAsync($"{OrdersUrl}/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetOrder_OfAnotherCustomer_Returns403()
    {
        // CustomerB bir sipariş açar
        var customerB = CreateClientFor(Guid.NewGuid());
        var createResponse = await customerB.PostAsJsonAsync(OrdersUrl, ValidOrder());
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var orderUrl = createResponse.Headers.Location!;

        // CustomerA, B'nin siparişini görmeye çalışır
        var customerA = CreateClientFor(Guid.NewGuid());
        var response = await customerA.GetAsync(orderUrl);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetOrder_WhenNotExists_Returns404()
    {
        var client = CreateClientFor(Guid.NewGuid());

        var response = await client.GetAsync($"{OrdersUrl}/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateOrder_SixTimesInAMinute_SixthReturns429()
    {
        var client = CreateClientFor(Guid.NewGuid());

        for (int i = 1; i <= 5; i++)
        {
            var ok = await client.PostAsJsonAsync(OrdersUrl, ValidOrder());
            Assert.Equal(HttpStatusCode.Created, ok.StatusCode);
        }

        var sixth = await client.PostAsJsonAsync(OrdersUrl, ValidOrder());

        Assert.Equal(HttpStatusCode.TooManyRequests, sixth.StatusCode);
    }
}