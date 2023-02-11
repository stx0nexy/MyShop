namespace Basket.Host.Models;

public class ItemResponse<T>
{
    public T Id { get; set; } = default(T) !;
}