namespace ApplicationLayer.DTOs;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; // ✅ läggs till
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; } // ✅
}
