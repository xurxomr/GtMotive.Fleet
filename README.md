# GtMotive.Fleet

Microservicio REST para la **gestión de una flota de renting**, implementado siguiendo una
**arquitectura hexagonal (Ports & Adapters)** con principios de Clean Architecture y DDD.

Proyecto desarrollado como prueba técnica para **Capitole Consulting / GT Motive** (.NET).

> **Objetivo doble de la prueba**
> 1. **Funcional:** exponer, vía REST, la gestión de vehículos y alquileres de una empresa de renting.
> 2. **Dockerización:** empaquetar el servicio con imágenes oficiales de .NET y permitir su ejecución
>    en local sin instalar dependencias externas (basta con Docker / Docker Compose), de modo que el
>    contenedor sea seleccionable como proyecto de inicio en Visual Studio.

---

## Tabla de contenidos

- [GtMotive.Fleet](#gtmotivefleet)
  - [Tabla de contenidos](#tabla-de-contenidos)
  - [Funcionalidad](#funcionalidad)
    - [Reglas de negocio (invariantes)](#reglas-de-negocio-invariantes)
  - [Stack tecnológico](#stack-tecnológico)
  - [Arquitectura](#arquitectura)
  - [Estructura de la solución](#estructura-de-la-solución)
  - [Requisitos previos](#requisitos-previos)
  - [Ejecución con Docker Compose (recomendado)](#ejecución-con-docker-compose-recomendado)
    - [Contenedor como proyecto de inicio en Visual Studio](#contenedor-como-proyecto-de-inicio-en-visual-studio)
  - [Ejecución en local (.NET)](#ejecución-en-local-net)
  - [Configuración](#configuración)
  - [Testing](#testing)
  - [Estado del proyecto](#estado-del-proyecto)
  - [Decisiones de diseño](#decisiones-de-diseño)

---

## Funcionalidad

El microservicio ofrece (vía llamadas REST) la gestión de la flota:

| Acción | Descripción |
| --- | --- |
| **Alta de vehículo** | Registrar un nuevo vehículo en la flota. |
| **Listar disponibles** | Consultar los vehículos disponibles para alquilar. |
| **Alquilar vehículo** | Asignar un vehículo disponible a una persona. |
| **Devolver vehículo** | Registrar la devolución y volver a marcar el vehículo como disponible. |

### Reglas de negocio (invariantes)

- 🚗 **Un alquiler activo por persona:** una misma persona no puede tener más de un vehículo alquilado simultáneamente.
- 📅 **Antigüedad máxima de 5 años:** la flota no admite vehículos cuya fecha de fabricación supere los 5 años.

---

## Stack tecnológico

- **.NET 10** (`net10.0`)
- **ASP.NET Core** (Web API)
- **PostgreSQL** como almacén de datos, mediante **Entity Framework Core** (`Npgsql`)
- **MediatR** para el patrón Mediator (Command/Query) en los casos de uso
- **Serilog** para logging estructurado a consola
- **Swagger / OpenAPI** (Swashbuckle) para documentación e interacción con la API
- **Docker** y **Docker Compose** para la ejecución autocontenida
- **xUnit** para las pruebas automáticas
- Analizadores **StyleCop** y **SonarAnalyzer** (con *warnings as errors* y auditoría de vulnerabilidades de NuGet)

---

## Arquitectura

Arquitectura **hexagonal (Ports & Adapters)**: el núcleo de negocio no conoce ningún detalle de infraestructura ni de framework. Las dependencias apuntan siempre **hacia el dominio**.

```
        (Driver / Left side)                          (Driven / Right side)
   HTTP  ─►  Host / Api  ─►  ApplicationCore  ─►  Domain  ◄─  Infrastructure  ─►  PostgreSQL
                (Adapters)      (Use Cases / Ports)  (Núcleo)     (Adapters)
```

| Capa | Proyecto | Responsabilidad |
| --- | --- | --- |
| **Domain** | `GtMotive.Fleet.Domain` | Entidades, *aggregate roots*, *value objects* e interfaces del núcleo. Sin dependencias externas. |
| **Application** | `GtMotive.Fleet.ApplicationCore` | Casos de uso y *puertos* (contratos) que orquestan las reglas de negocio. |
| **Infrastructure** | `GtMotive.Fleet.Infrastructure` | *Adapters* hacia los actores secundarios (persistencia EF Core / PostgreSQL, logging, etc.). |
| **User Interface** | `GtMotive.Fleet.Api` | Controladores REST, *presenters* y *view models*; traduce HTTP ↔ casos de uso. |
| **Host** | `GtMotive.Fleet.Host` | *Composition root*: arranque de la aplicación, DI, configuración, Swagger y pipeline HTTP. |

Patrones aplicados (según la plantilla de referencia): **Use Case**, **Controller + Command (MediatR)**, **Presenter / ViewModel**, **Repository**, **Unit of Work**, **Factory**, **Value Object**, **Aggregate Root** y **First-Class Collections**.

---

## Estructura de la solución

```
GtMotive.Fleet/
├─ docker-compose.yml            # API + PostgreSQL (Postgres con versión fijada)
├─ .dockerignore
├─ Directory.Build.props/targets # Configuración y versiones de paquetes centralizadas
├─ global.json                   # SDK de .NET fijado
├─ src/
│  ├─ GtMotive.Fleet.slnx        # Solución (formato slnx)
│  ├─ GtMotive.Fleet.Domain/
│  ├─ GtMotive.Fleet.ApplicationCore/
│  ├─ GtMotive.Fleet.Infrastructure/
│  ├─ GtMotive.Fleet.Api/
│  └─ GtMotive.Fleet.Host/       # Contiene el Dockerfile
└─ test/
   ├─ unit/GtMotive.Fleet.UnitTests/
   ├─ functional/GtMotive.Fleet.FunctionalTests/
   └─ infrastructure/GtMotive.Fleet.InfrastructureTests/
```

---

## Requisitos previos

Para ejecutar **solo con Docker** (recomendado, sin instalar nada más):

- [Docker](https://www.docker.com/) con **Docker Compose v2**

Para ejecutar / desarrollar **en local**:

- [SDK de .NET 10](https://dotnet.microsoft.com/download) (versión `10.0.303` o superior, según `global.json`)
- Una instancia de **PostgreSQL** (puedes levantar solo la del `docker-compose.yml`)
- *(Opcional)* **Visual Studio 2026**

---

## Ejecución con Docker Compose (recomendado)

Desde la raíz del repositorio:

```bash
docker compose up --build
```

Esto levanta dos contenedores:

| Servicio | Contenedor | Puerto | Descripción |
| --- | --- | --- | --- |
| `fleet-api` | `gtmotive-fleet-api` | `8080` | El microservicio (imagen construida desde el `Dockerfile`). |
| `fleet-db` | `gtmotive-fleet-db` | `5432` | PostgreSQL (imagen `postgres:17.2-alpine`, con volumen persistente). |

Una vez arrancado:

- **API / Swagger:** <http://localhost:8080/swagger>
- **PostgreSQL:** `localhost:5432` (BD `fleet`, usuario `fleet`, contraseña `fleet`)

Para detener y eliminar los contenedores:

```bash
docker compose down          # conserva los datos (volumen)
docker compose down -v       # elimina también el volumen de datos
```

### Contenedor como proyecto de inicio en Visual Studio

El `Dockerfile` reside en el proyecto **Host** y usa las imágenes oficiales
`mcr.microsoft.com/dotnet/aspnet:10.0` y `mcr.microsoft.com/dotnet/sdk:10.0`. Al abrir la solución en Visual Studio, el contenedor queda disponible como opción de arranque (Docker / Docker Compose).

---

## Ejecución en local (.NET)

1. Levanta únicamente la base de datos:

   ```bash
   docker compose up -d fleet-db
   ```

2. Ejecuta el Host:

   ```bash
   dotnet run --project src/GtMotive.Fleet.Host
   ```

   La consola indicará la URL de escucha. La documentación Swagger estará disponible en `/swagger`.

Compilar y ejecutar los tests de toda la solución:

```bash
dotnet build src/GtMotive.Fleet.slnx
dotnet test  src/GtMotive.Fleet.slnx
```

---

## Configuración

La configuración se resuelve por `appsettings.json` + `appsettings.Development.json` y se puede
sobrescribir con **variables de entorno** (lo que usa `docker-compose.yml`).

| Clave | Variable de entorno | Descripción |
| --- | --- | --- |
| `ConnectionStrings:FleetDb` | `ConnectionStrings__FleetDb` | Cadena de conexión a PostgreSQL. |
| `PathBase` | `PathBase` | Prefijo de ruta base (útil tras un *reverse proxy*). |
| — | `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución (`Development`, `Production`). |
| — | `ASPNETCORE_HTTP_PORTS` | Puerto HTTP de escucha dentro del contenedor (`8080`). |

Ejemplo de cadena de conexión:

```
Host=localhost;Port=5432;Database=fleet;Username=fleet;Password=fleet
```

> Las credenciales por defecto son solo para desarrollo local. En un entorno real deben
> proporcionarse mediante variables de entorno / gestor de secretos.

---

## Testing

La solución está preparada para los tres niveles de prueba solicitados. Cada uno tiene su proyecto
y su infraestructura de arranque:

| Tipo | Proyecto | Alcance |
| --- | --- | --- |
| **Unitaria** | `GtMotive.Fleet.UnitTests` | Valida un caso de uso / lógica de forma aislada, sin sus dependencias. |
| **Funcional** | `GtMotive.Fleet.FunctionalTests` | Prueba de integración de los casos de uso, excluyendo el Host. |
| **Infraestructura** | `GtMotive.Fleet.InfrastructureTests` | Prueba a nivel de Host (recepción de la llamada REST y validación del modelo) mediante `TestServer`. |

```bash
dotnet test src/GtMotive.Fleet.slnx
```

---

## Estado del proyecto

Base de entorno y arquitectura preparada y verificada (compila sin *warnings* con `TreatWarningsAsErrors`):

- [x] Plantilla renombrada a `GtMotive.Fleet.*` y migrada a **.NET 10**
- [x] Solución en formato **`.slnx`**
- [x] Stack mínimo autocontenido (Serilog + Swagger); sin dependencias externas de nube/auth
- [x] **Dockerfile** (.NET 10) + **docker-compose** con **PostgreSQL** (versión fijada)
- [x] Estructura para pruebas unitarias, funcionales y de infraestructura
- [ ] Modelo de dominio (`Vehicle`, `Rental`, *value objects* e invariantes de negocio)
- [ ] Casos de uso, controladores REST y contratos (DTOs)
- [ ] `DbContext` de EF Core, migraciones y repositorios sobre PostgreSQL
- [ ] Un ejemplo de cada tipo de prueba (unitaria, funcional, infraestructura)

---

## Decisiones de diseño

- **PostgreSQL en lugar de un almacén documental:** el dominio (vehículos, alquileres) es
  relacional y sus invariantes se benefician de garantías ACID. En concreto, *"un alquiler activo por
  persona"* se modela de forma natural con un **índice único parcial** en la base de datos.
- **Stack mínimo:** el enunciado no requiere autenticación ni servicios de nube. Se retiraron las
  piezas de la plantilla acopladas a servicios externos (autenticación con proveedor de identidad,
  gestor de secretos y telemetría de nube) para que cualquiera pueda clonar y ejecutar el proyecto
  en local únicamente con Docker.
- **Versiones fijadas:** las imágenes de contenedor y el SDK se fijan a versiones concretas
  (no `latest`) para builds reproducibles.
