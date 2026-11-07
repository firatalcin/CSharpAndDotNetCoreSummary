namespace OrderAPI.Contracts;

public record PagedResponse<T>(
    List<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount
)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}