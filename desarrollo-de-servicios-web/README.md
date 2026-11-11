# 🐾 AdoptaMas API - Ecosistema de Microservicios (.NET 8/9 & Docker)

Proyecto canónico y modelo de referencia para el curso **Desarrollo de Servicios Web** en **Zegel (Escuela de Tecnología)**.

---

## 🏛️ Estructura del Repositorio (Soluciones Independientes por Microservicio)

Cada microservicio es completamente autónomo y cuenta con su propia solución (`.sln`), su propio `Dockerfile` y sus propias capas:

```text
AdoptaMasAPI/
├── 📄 docker-compose.yml              <-- Orquestador general (PostgreSQL + Gateway + Microservicios)
├── 📄 .gitignore                      <-- Exclusiones globales (.vs, bin, obj)
├── 📄 README.md                       <-- Guía de ejecución paso a paso
│
├── 📁 postman/
│   └── 📄 AdoptaMasAPI.postman_collection.json  <-- Colección de pruebas automáticas
│
├── 📁 ApiGateway/                     <-- Microservicio Gateway (YARP - Puerto 6000)
│   ├── 📄 ApiGateway.sln              <-- Solución propia
│   ├── 📄 ApiGateway.csproj
│   ├── 📄 appsettings.json
│   ├── 📄 Program.cs
│   └── 🐳 Dockerfile
│
├── 📁 AuthService/                    <-- Microservicio JWT & Usuarios (Puerto 6002)
│   ├── 📄 AuthService.sln             <-- Solución propia
│   ├── 📄 AuthService.csproj
│   ├── 📁 Controllers/ (AuthController.cs)
│   ├── 📁 DTOs/ (LoginDto, RegistroDto, AuthRespuestaDto)
│   ├── 📁 Models/ (Usuario.cs)
│   ├── 📁 Services/ (TokenServicio.cs)
│   ├── 📄 appsettings.json
│   ├── 📄 Program.cs
│   └── 🐳 Dockerfile
│
└── 📁 MascotasService/                <-- Microservicio Core (EF Core PostgreSQL - Puerto 6001)
    ├── 📄 MascotasService.sln         <-- Solución propia
    ├── 📄 MascotasService.csproj
    ├── 📁 Controllers/ (Mascotas, Albergues, Solicitudes)
    ├── 📁 Data/ (AdoptaMasDbContext.cs)
    ├── 📁 DTOs/ (MascotaDtos, AlbergueDtos, SolicitudDtos)
    ├── 📁 Middlewares/ (ManejadorExcepcionesMiddleware.cs RFC 7807)
    ├── 📁 Models/ (Mascota, Albergue, SolicitudAdopcion)
    ├── 📁 Repositories/ (IMascotaRepositorio, MascotaRepositorio)
    ├── 📁 Services/ (MascotaServicio, SolicitudServicio con Transacción ACID)
    ├── 📄 appsettings.json
    ├── 📄 Program.cs (Scalar en /scalar/v1)
    └── 🐳 Dockerfile
```

---

## 🚀 Cómo Ejecutar el Proyecto

### Opción A: Orquestación con Docker Compose (Recomendado)
Requiere **Docker Desktop** activo:
```bash
# 1. Ubicarse en la raíz
cd AdoptaMasAPI

# 2. Levantar todo el ecosistema con un solo comando
docker compose up --build
```
* **ApiGateway (YARP):** `http://localhost:6000`
* **Documentación Scalar:** `http://localhost:6001/scalar/v1`

---

### Opción B: Abrir con Visual Studio 2022 Community
Puedes abrir cada microservicio de forma independiente haciendo doble clic en su archivo `.sln`:
* `ApiGateway/ApiGateway.sln`
* `AuthService/AuthService.sln`
* `MascotasService/MascotasService.sln`

---

### Opción C: Ejecución Local con .NET CLI
```bash
# 1. Levantar contenedor de PostgreSQL
docker run --name adoptamas_postgres -e POSTGRES_DB=adoptamas_db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=ZegelSecretPassword2026! -p 5432:5432 -d postgres:16-alpine

# 2. Terminal 1: MascotasService
dotnet run --project MascotasService --urls "http://localhost:6001"

# 3. Terminal 2: AuthService
dotnet run --project AuthService --urls "http://localhost:6002"

# 4. Terminal 3: ApiGateway
dotnet run --project ApiGateway --urls "http://localhost:6000"
```

---

## 🧪 Usuarios y Credenciales de Prueba

| Rol | Correo Electrónico | Contraseña | Permisos |
| :--- | :--- | :--- | :--- |
| **Admin** | `admin@zegel.pe` | `Admin123!` | Crear/Eliminar Mascotas y Albergues, Aprobar Adopciones |
| **Albergue** | `patitas@adopta.pe` | `Albergue123!` | Registrar y Editar Mascotas, Evaluar Solicitudes |
| **Adoptante** | `juan@gmail.com` | `Juan123!` | Ver Mascotas y Postular a Adopción |

---

## 📄 Conexión con DBeaver
* **Host:** `localhost` | **Port:** `5432` | **Database:** `adoptamas_db`
* **Username:** `postgres` | **Password:** `ZegelSecretPassword2026!`
