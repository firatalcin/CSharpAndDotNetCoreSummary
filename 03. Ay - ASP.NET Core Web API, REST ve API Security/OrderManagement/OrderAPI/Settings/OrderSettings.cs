namespace OrderAPI.Settings;

public class OrderSettings
{
    public const string SectionName = "OrderSettings";
    
    public int MaxItemsPerOrder { get; init; }
    public decimal MinOrderAmount { get; init; }
}