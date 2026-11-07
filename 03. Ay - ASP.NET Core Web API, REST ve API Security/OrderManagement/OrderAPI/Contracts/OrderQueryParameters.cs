using OrderAPI.Models;

namespace OrderAPI.Contracts;

public class OrderQueryParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
        get =>  _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    
    public OrderStatus? Status { get; set; }
    public decimal? MinTotalAmount { get; set; }

    public string SortBy { get; set; } = "createdAt";
    public string SortDirection { get; set; } = "desc";
}