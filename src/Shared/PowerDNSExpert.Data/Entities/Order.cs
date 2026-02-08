namespace PowerDNSExpert.Data.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime OrderedAtUtc { get; set; } = DateTime.UtcNow;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
