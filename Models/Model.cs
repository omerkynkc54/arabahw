public class Model
{
    public int ModelId { get; set; }
    public string? Name { get; set; }
    public int BrandId { get; set; }
    public Brand? Brand { get; set; }
}