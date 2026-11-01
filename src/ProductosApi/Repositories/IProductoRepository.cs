using ProductosApi.Models;

namespace ProductosApi.Repositories;

/// <summary>
/// Interface solicitada en la actividad.
/// </summary>
public interface IProductoRepository
{
    List<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
    int Insertar(Producto producto);
    bool Actualizar(Producto producto);
    bool Eliminar(int id);
}
