using ProductosApi.Models;

namespace ProductosApi.Repositories;

/// <summary>
/// Punto 6: BUSCAR.
/// Búsqueda adicional por texto (nombre/descripción) y/o categoría.
/// Se define aparte para no modificar la interface IProductoRepository solicitada.
/// </summary>
public interface IProductoBusqueda
{
    List<Producto> Buscar(string? texto, string? categoria);
}
