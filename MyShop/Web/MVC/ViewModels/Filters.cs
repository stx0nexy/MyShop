namespace MVC.ViewModels;

public class Filters<T>
{
    public int Count { get; set; }
    public IEnumerable<T> Data { get; set; } = new List<T>();
}