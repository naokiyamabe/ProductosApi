using MySqlConnector;
using ProductosApi.Data;
using ProductosApi.Models;

namespace ProductosApi.Repositories;

/// <summary>
/// Implementación de IProductoRepository usando ADO.NET (MySqlConnector) y SQL parametrizado.
/// </summary>
public class ProductoRepository : IProductoRepository, IProductoBusqueda
{
    private readonly ConexionDb _conexionDb;

    private const string Columnas =
        "id, nombre, descripcion, categoria, precio, stock, fecha_creacion";

    public ProductoRepository(ConexionDb conexionDb)
    {
        _conexionDb = conexionDb;
    }

    // ---------------------------------------------------------------
    // Punto 2: CONSULTAS
    // ---------------------------------------------------------------

    /// <summary>Devuelve todos los productos ordenados por Id.</summary>
    public List<Producto> ObtenerTodos()
    {
        var productos = new List<Producto>();

        using var conexion = _conexionDb.ObtenerConexion();
        using var comando = new MySqlCommand($"SELECT {Columnas} FROM productos ORDER BY id;", conexion);
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            productos.Add(Mapear(lector));
        }

        return productos;
    }

    /// <summary>Devuelve un producto por su Id o null si no existe.</summary>
    public Producto? ObtenerPorId(int id)
    {
        using var conexion = _conexionDb.ObtenerConexion();
        using var comando = new MySqlCommand($"SELECT {Columnas} FROM productos WHERE id = @id;", conexion);
        comando.Parameters.AddWithValue("@id", id);

        using var lector = comando.ExecuteReader();
        return lector.Read() ? Mapear(lector) : null;
    }

    // ---------------------------------------------------------------
    // Punto 3: INSERT
    // ---------------------------------------------------------------

    /// <summary>Inserta un producto y devuelve el Id generado.</summary>
    public int Insertar(Producto producto)
    {
        const string sql = @"
            INSERT INTO productos (nombre, descripcion, categoria, precio, stock)
            VALUES (@nombre, @descripcion, @categoria, @precio, @stock);
            SELECT LAST_INSERT_ID();";

        using var conexion = _conexionDb.ObtenerConexion();
        using var comando = new MySqlCommand(sql, conexion);
        AgregarParametros(comando, producto);

        var nuevoId = Convert.ToInt32(comando.ExecuteScalar());
        producto.Id = nuevoId;
        return nuevoId;
    }

    // ---------------------------------------------------------------
    // Punto 4: ACTUALIZAR
    // ---------------------------------------------------------------

    /// <summary>Actualiza un producto existente. Devuelve false si el Id no existe.</summary>
    public bool Actualizar(Producto producto)
    {
        const string sql = @"
            UPDATE productos
               SET nombre = @nombre,
                   descripcion = @descripcion,
                   categoria = @categoria,
                   precio = @precio,
                   stock = @stock
             WHERE id = @id;";

        using var conexion = _conexionDb.ObtenerConexion();
        using var comando = new MySqlCommand(sql, conexion);
        AgregarParametros(comando, producto);
        comando.Parameters.AddWithValue("@id", producto.Id);

        // MySqlConnector devuelve filas "encontradas" (UseAffectedRows=false por defecto),
        // así que 0 significa que el Id no existe, aunque los datos no hayan cambiado.
        return comando.ExecuteNonQuery() > 0;
    }

    // ---------------------------------------------------------------
    // Punto 5: ELIMINAR
    // ---------------------------------------------------------------

    /// <summary>Elimina un producto por Id. Devuelve false si no existía.</summary>
    public bool Eliminar(int id)
    {
        using var conexion = _conexionDb.ObtenerConexion();
        using var comando = new MySqlCommand("DELETE FROM productos WHERE id = @id;", conexion);
        comando.Parameters.AddWithValue("@id", id);

        return comando.ExecuteNonQuery() > 0;
    }

    // ---------------------------------------------------------------
    // Punto 6: BUSCAR
    // ---------------------------------------------------------------

    /// <summary>Busca productos cuyo nombre o descripción contengan el texto y/o por categoría.</summary>
    public List<Producto> Buscar(string? texto, string? categoria)
    {
        var productos = new List<Producto>();
        var condiciones = new List<string>();

        using var conexion = _conexionDb.ObtenerConexion();
        using var comando = new MySqlCommand { Connection = conexion };

        if (!string.IsNullOrWhiteSpace(texto))
        {
            condiciones.Add("(nombre LIKE @texto OR descripcion LIKE @texto)");
            comando.Parameters.AddWithValue("@texto", $"%{texto.Trim()}%");
        }

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            condiciones.Add("categoria = @categoria");
            comando.Parameters.AddWithValue("@categoria", categoria.Trim());
        }

        var where = condiciones.Count > 0 ? "WHERE " + string.Join(" AND ", condiciones) : string.Empty;
        comando.CommandText = $"SELECT {Columnas} FROM productos {where} ORDER BY nombre;";

        using var lector = comando.ExecuteReader();
        while (lector.Read())
        {
            productos.Add(Mapear(lector));
        }

        return productos;
    }

    // ---------------------------------------------------------------
    // Utilidades privadas
    // ---------------------------------------------------------------

    private static void AgregarParametros(MySqlCommand comando, Producto producto)
    {
        comando.Parameters.AddWithValue("@nombre", producto.Nombre);
        comando.Parameters.AddWithValue("@descripcion", (object?)producto.Descripcion ?? DBNull.Value);
        comando.Parameters.AddWithValue("@categoria", producto.Categoria);
        comando.Parameters.AddWithValue("@precio", producto.Precio);
        comando.Parameters.AddWithValue("@stock", producto.Stock);
    }

    private static Producto Mapear(MySqlDataReader lector) => new()
    {
        Id = lector.GetInt32("id"),
        Nombre = lector.GetString("nombre"),
        Descripcion = lector.IsDBNull(lector.GetOrdinal("descripcion")) ? null : lector.GetString("descripcion"),
        Categoria = lector.GetString("categoria"),
        Precio = lector.GetDecimal("precio"),
        Stock = lector.GetInt32("stock"),
        FechaCreacion = lector.GetDateTime("fecha_creacion")
    };
}
