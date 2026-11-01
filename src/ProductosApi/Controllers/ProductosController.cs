using Microsoft.AspNetCore.Mvc;
using ProductosApi.Data;
using ProductosApi.Models;
using ProductosApi.Repositories;

namespace ProductosApi.Controllers;

/// <summary>
/// Endpoints CRUD para administrar productos. Pruébalos desde /swagger.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductosController : ControllerBase
{
    private readonly IProductoRepository _repositorio;
    private readonly IProductoBusqueda _busqueda;
    private readonly ConexionDb _conexionDb;

    public ProductosController(IProductoRepository repositorio, IProductoBusqueda busqueda, ConexionDb conexionDb)
    {
        _repositorio = repositorio;
        _busqueda = busqueda;
        _conexionDb = conexionDb;
    }

    /// <summary>Prueba la conexión con la base de datos MySQL.</summary>
    [HttpGet("conexion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public IActionResult ProbarConexion()
    {
        var ok = _conexionDb.ProbarConexion(out var mensaje);
        return ok
            ? Ok(new { conectado = true, mensaje })
            : StatusCode(StatusCodes.Status503ServiceUnavailable, new { conectado = false, mensaje });
    }

    /// <summary>Obtiene todos los productos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<Producto>), StatusCodes.Status200OK)]
    public ActionResult<List<Producto>> ObtenerTodos()
    {
        return Ok(_repositorio.ObtenerTodos());
    }

    /// <summary>Obtiene un producto por su Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Producto> ObtenerPorId(int id)
    {
        var producto = _repositorio.ObtenerPorId(id);
        return producto is null
            ? NotFound(new { mensaje = $"No existe un producto con Id {id}." })
            : Ok(producto);
    }

    /// <summary>Busca productos por texto (nombre o descripción) y/o categoría.</summary>
    /// <param name="texto">Texto contenido en el nombre o la descripción. Ej: "laptop"</param>
    /// <param name="categoria">Categoría exacta. Ej: "Electrónica"</param>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(List<Producto>), StatusCodes.Status200OK)]
    public ActionResult<List<Producto>> Buscar([FromQuery] string? texto, [FromQuery] string? categoria)
    {
        return Ok(_busqueda.Buscar(texto, categoria));
    }

    /// <summary>Registra un nuevo producto.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Producto> Insertar([FromBody] Producto producto)
    {
        var nuevoId = _repositorio.Insertar(producto);
        var creado = _repositorio.ObtenerPorId(nuevoId);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevoId }, creado);
    }

    /// <summary>Actualiza un producto existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Producto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Producto> Actualizar(int id, [FromBody] Producto producto)
    {
        producto.Id = id;
        if (!_repositorio.Actualizar(producto))
        {
            return NotFound(new { mensaje = $"No existe un producto con Id {id}." });
        }

        return Ok(_repositorio.ObtenerPorId(id));
    }

    /// <summary>Elimina un producto por su Id.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id)
    {
        return _repositorio.Eliminar(id)
            ? NoContent()
            : NotFound(new { mensaje = $"No existe un producto con Id {id}." });
    }
}
