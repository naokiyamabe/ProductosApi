# API de Productos – CRUD con ASP.NET Core 8 y MySQL

Backend que permite realizar operaciones **CRUD** sobre la entidad `Producto`, usando **MySQL** como base de datos, **ADO.NET (MySqlConnector)** para el acceso a datos y **Swagger** para probar los endpoints.

## Puntos resueltos

| Punto | Dónde se implementa |
|---|---|
| **Conexión** | `Data/ConexionDb.cs` – crea y abre conexiones con la cadena de `appsettings.json`. Endpoint de prueba: `GET /api/productos/conexion` |
| **Consultas** | `ProductoRepository.ObtenerTodos()` y `ObtenerPorId(id)` |
| **Insert** | `ProductoRepository.Insertar(producto)` – devuelve el Id generado (`LAST_INSERT_ID()`) |
| **Actualizar** | `ProductoRepository.Actualizar(producto)` |
| **Eliminar** | `ProductoRepository.Eliminar(id)` |
| **Buscar** | `ObtenerPorId(id)` y `ProductoRepository.Buscar(texto, categoria)` (por nombre/descripción y categoría) |

### Interface implementada

```csharp
public interface IProductoRepository
{
    List<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
    int Insertar(Producto producto);
    bool Actualizar(Producto producto);
    bool Eliminar(int id);
}
```

`ProductoRepository` implementa esta interface sin modificarla. La búsqueda por texto se agregó en una interface aparte (`IProductoBusqueda`).

## Estructura

```
ProductosApi/
├── database/
│   └── script.sql                  # Crea la BD, la tabla y datos de ejemplo
└── src/ProductosApi/
    ├── Controllers/
    │   └── ProductosController.cs  # Endpoints REST
    ├── Data/
    │   └── ConexionDb.cs           # Conexión a MySQL
    ├── Models/
    │   └── Producto.cs             # Entidad
    ├── Repositories/
    │   ├── IProductoRepository.cs  # Interface solicitada
    │   ├── IProductoBusqueda.cs    # Búsqueda adicional
    │   └── ProductoRepository.cs   # Implementación con SQL parametrizado
    ├── Program.cs                  # Inyección de dependencias + Swagger
    └── appsettings.json            # Cadena de conexión
```

## Modelo `Producto`

| Campo | Tipo C# | Columna MySQL |
|---|---|---|
| Id | int | `id INT AUTO_INCREMENT` |
| Nombre | string | `nombre VARCHAR(100)` |
| Descripcion | string? | `descripcion VARCHAR(255)` |
| Categoria | string | `categoria VARCHAR(50)` |
| Precio | decimal | `precio DECIMAL(10,2)` |
| Stock | int | `stock INT` |
| FechaCreacion | DateTime | `fecha_creacion DATETIME` |

## Requisitos

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- MySQL Server 8 (o XAMPP / MariaDB)

## Cómo ejecutarlo

1. **Crear la base de datos** (en MySQL Workbench o en terminal):
   ```bash
   mysql -u root -p < database/script.sql
   ```
2. **Configurar la conexión** en `src/ProductosApi/appsettings.json`, cambiando `TU_PASSWORD` por tu contraseña de MySQL:
   ```json
   "ConnectionStrings": {
     "MySql": "Server=localhost;Port=3306;Database=productos_db;User ID=root;Password=TU_PASSWORD;"
   }
   ```
3. **Ejecutar la API**:
   ```bash
   cd src/ProductosApi
   dotnet run
   ```
4. Abrir **http://localhost:5000/swagger** en el navegador.

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/productos/conexion` | Prueba la conexión a MySQL |
| GET | `/api/productos` | Lista todos los productos |
| GET | `/api/productos/{id}` | Obtiene un producto por Id |
| GET | `/api/productos/buscar?texto=&categoria=` | Busca por texto y/o categoría |
| POST | `/api/productos` | Inserta un producto |
| PUT | `/api/productos/{id}` | Actualiza un producto |
| DELETE | `/api/productos/{id}` | Elimina un producto |

### Ejemplo de cuerpo para POST / PUT

```json
{
  "nombre": "Monitor Samsung 24\"",
  "descripcion": "Monitor Full HD 75 Hz",
  "categoria": "Electrónica",
  "precio": 2499.00,
  "stock": 10
}
```

## Evidencias

Agrega aquí las capturas de Swagger probando cada operación (guárdalas en la carpeta `evidencias/`):

- `evidencias/01-conexion.png`
- `evidencias/02-obtener-todos.png`
- `evidencias/03-obtener-por-id.png`
- `evidencias/04-insertar.png`
- `evidencias/05-actualizar.png`
- `evidencias/06-eliminar.png`
- `evidencias/07-buscar.png`
