using MySqlConnector;

namespace ProductosApi.Data;

/// <summary>
/// Punto 1: CONEXIÓN.
/// Centraliza la creación de conexiones a MySQL a partir de la cadena
/// de conexión definida en appsettings.json ("ConnectionStrings:MySql").
/// </summary>
public class ConexionDb
{
    private readonly string _cadenaConexion;

    public ConexionDb(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("MySql")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión 'MySql' en appsettings.json.");
    }

    /// <summary>
    /// Crea y abre una nueva conexión. Quien la usa debe liberarla con `using`.
    /// </summary>
    public MySqlConnection ObtenerConexion()
    {
        var conexion = new MySqlConnection(_cadenaConexion);
        conexion.Open();
        return conexion;
    }

    /// <summary>
    /// Verifica si la base de datos responde. Útil para el endpoint de prueba de conexión.
    /// </summary>
    public bool ProbarConexion(out string mensaje)
    {
        try
        {
            using var conexion = ObtenerConexion();
            mensaje = $"Conexión exitosa a MySQL {conexion.ServerVersion}, base de datos '{conexion.Database}'.";
            return true;
        }
        catch (Exception ex)
        {
            mensaje = $"Error de conexión: {ex.Message}";
            return false;
        }
    }
}
