using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;

public class Product : BaseDomainModel
{
    public string? Nombre { get; set; }
    public decimal Precio { get; set; }
    public string? Descripcion { get; set; }    
    public int Rating { get; set; }
    public string? Vendedor { get; set; }
    public int Stock { get; set; }
    public ProductStatus Status { get; set; }
    public int CategoryId { get; set; }
}