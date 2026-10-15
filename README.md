# MyApp — Registro de Visitas

Aplicación web en **.NET 10** para el registro de entradas y salidas de personas. Implementa una arquitectura limpia (*Clean Architecture*) orientada a casos de uso, con **Minimal APIs**, **Entity Framework Core** y **SQL Server**.

---

## Tabla de contenidos

- [Descripción](#descripción)
- [Arquitectura](#arquitectura)
- [Tecnologías](#tecnologías)
- [Requisitos](#requisitos)
- [Configuración](#configuración)
- [Ejecución](#ejecución)
- [Endpoints de la API](#endpoints-de-la-api)
- [Modelo de dominio](#modelo-de-dominio)
- [Migraciones](#migraciones)

---

## Descripción

El sistema permite administrar personas y registrar sus visitas (entrada/salida). Cada persona tiene un código único que facilita el registro de accesos sin necesidad de conocer su identificador interno. Las reglas de negocio (validaciones, unicidad de código, control de visitas activas) viven dentro de las entidades del dominio.

---

## Arquitectura

El proyecto sigue una arquitectura en capas con dependencias apuntando hacia adentro (hacia el dominio):

```
WebApi (composition root)
  ├── Application  (casos de uso + DTOs)
  ├── Data         (EF Core, repositorios, migraciones)
  └── Domain       (entidades + abstracciones)

Application  →  Domain
Data         →  Domain
Domain       →  (sin dependencias)
```

| Proyecto   | Responsabilidad |
|------------|-----------------|
| `Domain`     | Entidades y reglas de negocio, interfaces de repositorio. Sin dependencias externas. |
| `Application`| Casos de uso (orquestan una operación) y DTOs. Solo depende de `Domain`. |
| `Data`       | Persistencia: `DbContext`, repositorios, configuraciones de EF Core y migraciones. |
| `WebApi`     | Capa de presentación: Minimal APIs, inyección de dependencias y configuración. |

---

## Tecnologías

- **.NET 10** (`net10.0`)
- **ASP.NET Core Minimal APIs**
- **Entity Framework Core 10** con proveedor **SQL Server**
- **SQL Server** (LocalDB / SQL Express)
- Patrón **Repository** + **Unit of Work** (vía `SaveChangesAsync`)

---

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (SQL Express o LocalDB)
- (Opcional) Visual Studio 2022 / VS Code con la extensión de C#

---

## Configuración

1. Clonar el repositorio:

   ```bash
   git clone <url-del-repositorio>
   cd MyApp
   ```

2. Configurar la cadena de conexión en `WebApi/appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=<TU_SERVIDOR>;Database=RegisterDB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
   }
   ```

   > Ajusta `Server` según tu instancia de SQL Server.

---

## Ejecución

Desde la carpeta raíz de la solución:

```bash
# Restaurar paquetes y compilar
dotnet build MyApp.slnx

# Aplicar migraciones a la base de datos
dotnet ef database update --project Data/Data.csproj --startup-project WebApi/WebApi.csproj

# Ejecutar la API
dotnet run --project WebApi/WebApi.csproj
```

La API estará disponible en:

- `https://localhost:7293`
- `http://localhost:5168`

---

## Endpoints de la API

### Personas — `/api/persons`

| Método | Ruta                        | Descripción                        |
|--------|-----------------------------|------------------------------------|
| `GET`    | `/api/persons/{id}`         | Obtener persona por ID             |
| `GET`    | `/api/persons/code/{code}`  | Obtener persona por código         |
| `GET`    | `/api/persons`              | Listar todas las personas          |
| `POST`   | `/api/persons`              | Crear una persona                  |
| `PUT`    | `/api/persons/{id}`         | Actualizar una persona             |
| `DELETE` | `/api/persons/{id}`         | Eliminar una persona               |

### Visitas — `/api/visits`

| Método | Ruta                             | Descripción                          |
|--------|----------------------------------|--------------------------------------|
| `GET`    | `/api/visits`                    | Listar todas las visitas             |
| `GET`    | `/api/visits/active`             | Listar visitas activas               |
| `GET`    | `/api/visits/person/{personId}`  | Listar visitas de una persona        |
| `POST`   | `/api/visits/entry`              | Registrar entrada (por ID o código)  |
| `POST`   | `/api/visits/exit`               | Registrar salida (por ID o código)   |

---

## Modelo de dominio

### `PersonEntity`

| Propiedad     | Tipo      | Notas                                   |
|---------------|-----------|-----------------------------------------|
| `Id`            | `Guid`      | Generado al crear la entidad            |
| `Code`          | `string`    | Único, 3–10 caracteres, se guarda en mayúsculas |
| `FirstName`     | `string`    | 2–50 caracteres                         |
| `LastName`      | `string`    | 2–50 caracteres                         |
| `Email`         | `string`    | Validado con expresión regular          |
| `PhoneNumber`   | `string`    | 7–15 caracteres                         |
| `FullName`      | `string`    | Calculado: `{FirstName} {LastName}`     |
| `Visits`        | `ICollection<VisitEntity>` | Visitas asociadas        |

**Reglas:** el constructor y `UpdatePersonalInfo` validan todos los campos. El código debe ser único.

### `VisitEntity`

| Propiedad    | Tipo        | Notas                                   |
|--------------|-------------|-----------------------------------------|
| `Id`           | `Guid`        | Generado al crear la entidad            |
| `PersonId`     | `Guid`        | FK hacia la persona                     |
| `EntryTime`    | `DateTime`    | Por defecto `DateTime.UtcNow`           |
| `ExitTime`     | `DateTime?`   | `null` = visita activa                  |
| `IsActive`     | `bool`        | Calculado: `ExitTime == null`           |
| `Duration`     | `TimeSpan?`   | Calculado: `ExitTime - EntryTime`       |

**Reglas:** `RegisterExit` no permite registrar una salida dos veces ni una salida anterior a la entrada.

---

## Migraciones

Las migraciones se encuentran en `Data/Migrations`:

1. `InitialCreate` — crea la tabla `Persons`
2. `AddVisitsTable` — crea la tabla `Visits`
3. `CascadeDeleteVisits` — configura el borrado en cascada para las visitas

Para crear una nueva migración:

```bash
dotnet ef migrations add <NombreMigracion> --project Data/Data.csproj --startup-project WebApi/WebApi.csproj
```
