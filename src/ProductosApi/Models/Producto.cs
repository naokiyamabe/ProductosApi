using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Models;

/// <summary>
/// Entidad que representa un producto almacenado en la tabla `productos` de MySQL.
/// </summary>
public class Producto
{
    /// <summary>Identificador único (AUTO_INCREMENT).</summary>
    public int Id { get; set; }

    /// <summary>Nombre del producto.</summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción breve del producto.</summary>
    [StringLength(255)]
    public string? Descripcion { get; set; }

    /// <summary>Categoría a la que pertenece el producto.</summary>
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [StringLength(50)]
    public string Categoria { get; set; } = string.Empty;

    /// <summary>Precio unitario.</summary>
    [Range(0, 9999999.99, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
    public decimal Precio { get; set; }

    /// <summary>Unidades disponibles en inventario.</summary>
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    /// <summary>Fecha en que se registró el producto (la asigna la base de datos).</summary>
    public DateTime FechaCreacion { get; set; }
}
