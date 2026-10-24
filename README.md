# personapi-dotnet

Monolito **ASP.NET Core MVC** con patrón **DAO**, **API REST** documentada con **Swagger (OpenAPI 3)** y vistas Razor para el CRUD del modelo `persona_db` (personas, profesiones, estudios y teléfonos).

Laboratorio 1 — Arquitectura de Software.

---

## 1. Stack

Se usan las últimas versiones estables (LTS cuando aplica) disponibles a octubre de 2026:

| Componente | Versión |
|---|---|
| .NET SDK / ASP.NET Core | **10.0 (LTS)** |
| Entity Framework Core | **10.0.x** |
| SQL Server | **2025 Express** (17.x) |
| SQL Server Management Studio | **22** |
| IDE | **Visual Studio Community 2026** |
| Swagger | **Swashbuckle.AspNetCore 10.x** (OpenAPI 3) |
| Control de versiones | Git + GitHub |

> La guía del laboratorio menciona .NET 6/7 y SQL Server 2019/2022. Se actualizó según la indicación del docente de usar siempre la última versión estable LTS. .NET 6 y 7 ya no tienen soporte.

---

## 2. Arquitectura

Monolito en capas: un solo proyecto desplegable, con responsabilidades separadas por carpeta.

```
Vista MVC ─┐
           ├─► Controller ─► Service ─► Repository (DAO) ─► DbContext (EF Core) ─► SQL Server
API REST ──┘        │            │
                  DTOs     Reglas de negocio
```

```
personapi-dotnet/
├── Controllers/
│   ├── Api/                     # API REST ([ApiController], /api/...) → Swagger
│   └── *Controller.cs           # Controladores MVC (vistas)
├── Services/
│   ├── Interfaces/              # Contratos de la capa de negocio
│   └── Implementations/         # Reglas de negocio y orquestación
├── Repositories/                # Capa DAO
│   ├── Interfaces/              # IRepository<T> + interfaces por entidad
│   └── Implementations/         # RepositoryBase<T> + repositorios EF Core
├── Data/
│   └── PersonaDbContext.cs      # Contexto EF Core (generado por scaffold)
├── Models/
│   ├── Entities/                # Entidades (generadas por scaffold desde la BD)
│   └── DTOs/                    # Objetos de transferencia (API y formularios)
├── Mappings/                    # Conversión Entidad ↔ DTO
├── Exceptions/                  # Excepciones de negocio (404, 409, 400)
├── Middleware/                  # Manejo global de errores de la API (ProblemDetails)
├── Extensions/                  # Registro de dependencias por capa
├── Views/                       # Vistas Razor del CRUD
├── appsettings.json             # Configuración común (sin secretos)
├── appsettings.Development.json # Ambiente de desarrollo
├── appsettings.Production.json  # Ambiente de producción
└── scripts/                     # DDL y DML de la base de datos
```

**Responsabilidad de cada capa**

- **Controllers**: reciben la petición, validan el modelo y devuelven vista o JSON. No contienen lógica de negocio ni consultas.
- **Services**: reglas de negocio. Por ejemplo: no se elimina una persona con teléfonos o estudios, no se repiten llaves, la fecha de grado no puede ser futura, las llaves no se modifican y las referencias (dueño, persona, profesión) deben existir.
- **Repositories (DAO)**: acceso a datos con EF Core. Un `RepositoryBase<T>` genérico implementa el CRUD común y cada repositorio agrega sus consultas por llave.
- **DTOs**: la API y las vistas nunca exponen las entidades directamente.
- **Inyección de dependencias**: cada capa depende de interfaces. El registro está en `Extensions/ServiceCollectionExtensions.cs`.

---

## 3. Requisitos

- Windows 10/11
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0), incluido en Visual Studio 2026 con la carga **Desarrollo de ASP.NET y web**
- SQL Server 2025 Express, instancia `SQLEXPRESS`, con **autenticación mixta** y el usuario `sa` habilitado
- SSMS 22 (opcional, para ejecutar los scripts)
- Git

---

## 4. Configurar el ambiente

### 4.1 Clonar

```bash
git clone https://github.com/<usuario>/personapi-dotnet.git
cd personapi-dotnet
```

### 4.2 Base de datos

En SSMS, conectado como `sa` a `localhost\SQLEXPRESS`, ejecuta en orden:

1. `scripts/01_ddl_persona_db.sql`: crea `persona_db` (propietario `sa`) y las tablas `profesion`, `persona`, `estudios` y `telefono`.
2. `scripts/02_dml_persona_db.sql`: inserta datos de prueba.

Adaptaciones del modelo a SQL Server:
- `ENUM('M','F')` se implementó como `CHAR(1)` + `CHECK`.
- `TEXT` se implementó como `VARCHAR(MAX)`, porque `TEXT` está obsoleto.

### 4.3 Cadena de conexión por ambiente

`appsettings.json` **no contiene la contraseña real**. Se define según el ambiente:

| Ambiente | Dónde se define la cadena |
|---|---|
| Development | **User Secrets** (fuera del repositorio) |
| Production | Variable de entorno `ConnectionStrings__DefaultConnection` |

**Development (User Secrets):** en Visual Studio, clic derecho en el proyecto → **Administrar secretos de usuario**. O por consola:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost\SQLEXPRESS;Database=persona_db;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True"
```

**Production (PowerShell):**

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Production"
$env:ConnectionStrings__DefaultConnection = "Server=localhost\SQLEXPRESS;Database=persona_db;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True"
```

### 4.4 Diferencias entre ambientes

| Configuración | Development | Production |
|---|---|---|
| Swagger (`Swagger:Enabled`) | Habilitado | Deshabilitado |
| Logs | `Information` + SQL generado por EF | `Warning` |
| Errores MVC | Página de desarrollador | `/Home/Error` |
| Errores API | JSON ProblemDetails | JSON ProblemDetails |

Para mostrar Swagger en producción: `$env:Swagger__Enabled = "true"`.

---

## 5. Entidades (scaffold Database First)

Las entidades y el contexto se generaron desde la base de datos con EF Core. En la Consola del Administrador de paquetes:

```powershell
Scaffold-DbContext "Name=ConnectionStrings:DefaultConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models/Entities -ContextDir Data -Context PersonaDbContext -NoOnConfiguring -Force
```

- `Name=ConnectionStrings:DefaultConnection` toma la cadena de la configuración (User Secrets), sin escribir la contraseña en el código.
- `-NoOnConfiguring` hace que el contexto reciba la conexión por inyección de dependencias desde `Program.cs`.

Paquetes NuGet: `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`, `Swashbuckle.AspNetCore` y `System.Security.Cryptography.Pkcs` (actualiza una dependencia transitiva con vulnerabilidades conocidas).

---

## 6. Compilar y ejecutar

**Visual Studio:** abre `personapi-dotnet.slnx` y presiona **F5** (perfil `http`).

**Consola:**

```bash
dotnet restore
dotnet build
dotnet run --launch-profile http
```

| Recurso | URL |
|---|---|
| Aplicación MVC | `http://localhost:<puerto>/` |
| Swagger UI | `http://localhost:<puerto>/swagger` |
| OpenAPI JSON | `http://localhost:<puerto>/swagger/v1/swagger.json` |

El puerto está en `Properties/launchSettings.json`.

---

## 7. Despliegue

Publicación como aplicación autocontenida en Kestrel:

```powershell
dotnet publish -c Release -o ./publish

cd publish
$env:ASPNETCORE_ENVIRONMENT = "Production"
$env:ConnectionStrings__DefaultConnection = "Server=localhost\SQLEXPRESS;Database=persona_db;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True"
$env:ASPNETCORE_URLS = "http://localhost:5000"
dotnet personapi-dotnet.dll
```

Luego abre `http://localhost:5000`.

> Para IIS: instala el *ASP.NET Core Hosting Bundle* de .NET 10, crea un sitio que apunte a la carpeta `publish` y define las variables de entorno en la configuración del sitio (o en `web.config`).

---

## 8. API REST

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/personas` | Listar personas |
| GET | `/api/personas/{cc}` | Persona con teléfonos y estudios |
| POST | `/api/personas` | Crear persona |
| PUT | `/api/personas/{cc}` | Actualizar persona |
| DELETE | `/api/personas/{cc}` | Eliminar persona (sin dependencias) |
| GET | `/api/profesiones` | Listar profesiones |
| GET | `/api/profesiones/{id}` | Obtener profesión |
| POST | `/api/profesiones` | Crear profesión |
| PUT | `/api/profesiones/{id}` | Actualizar profesión |
| DELETE | `/api/profesiones/{id}` | Eliminar profesión (sin estudios) |
| GET | `/api/telefonos` | Listar teléfonos |
| GET | `/api/telefonos/{num}` | Obtener teléfono |
| POST | `/api/telefonos` | Crear teléfono |
| PUT | `/api/telefonos/{num}` | Actualizar teléfono |
| DELETE | `/api/telefonos/{num}` | Eliminar teléfono |
| GET | `/api/estudios` | Listar estudios |
| GET | `/api/estudios/{idProf}/{ccPer}` | Obtener estudio |
| POST | `/api/estudios` | Crear estudio |
| PUT | `/api/estudios/{idProf}/{ccPer}` | Actualizar estudio |
| DELETE | `/api/estudios/{idProf}/{ccPer}` | Eliminar estudio |

Códigos de respuesta: `200`, `201` (creado), `204` (eliminado), `400` (validación o regla de negocio), `404` (no existe) y `409` (llave duplicada o registro con dependencias). Los errores se devuelven como `application/problem+json`.

Ejemplo:

```json
POST /api/personas
{
  "cc": 1060606060,
  "nombre": "Sofía",
  "apellido": "Ramírez",
  "genero": "F",
  "edad": 22
}
```

---

## 9. Vistas MVC

| Módulo | Ruta |
|---|---|
| Inicio | `/` |
| Personas | `/Personas` |
| Profesiones | `/Profesiones` |
| Estudios | `/Estudios` |
| Teléfonos | `/Telefonos` |

Cada módulo tiene listado, detalle, creación, edición y eliminación con confirmación.

---

## 10. Versionado

```bash
git tag -a v1.0.0 -m "Laboratorio 1 - personapi-dotnet"
git push origin v1.0.0
```

---

**Autor:** Sebastián Fonseca
