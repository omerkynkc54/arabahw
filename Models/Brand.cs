using System.Reflection;

public class Brand
{
    public int BrandId { get; set; }
    public string? Name { get; set; }
    public ICollection<Model>? Models { get; set; }
}