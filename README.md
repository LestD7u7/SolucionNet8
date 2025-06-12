# BancaLafise API (.NET 8)

Este proyecto es una API REST desarrollada con **.NET 8**, implementando **Clean Architecture**. Utiliza **MediatR**, **Entity Framework Core**, y una base de datos **SQLite**.

## Estructura del Proyecto

El repositorio está dividido en 5 soluciones principales:

- **BancaLafise.Api**: Capa de presentación (controladores, configuración de servicios, middlewares, etc.)
- **BancaLafise.Application**: Lógica de negocio y casos de uso (MediatR, validaciones, DTOs, etc.)
- **BancaLafise.Domain**: Entidades del dominio.
- **BancaLafise.Infrastructure**: Configuración de acceso a datos, implementación de repositorios y contexto de EF Core.
- **BancaLafise.UnitTests**: Pruebas unitarias.

## Prerrequisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

## Configuración Inicial

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/BancaLafise.git
cd BancaLafise

### 2. Configurar la cadena de conexión

Abre el archivo appsettings.json en el proyecto BancaLafise.Api y actualiza la ruta de la base de datos SQLite según tu entorno:

"ConnectionStringsBD": {
  "SQLite": "Data Source=C:\\Ruta\\A\\Tu\\Proyecto\\BancaLafise.Infrastructure\\LAFISE.db"
}

### 3. Crear y aplicar la migración inicial

Ubícate en la carpeta raíz del repositorio y ejecuta:

dotnet ef migrations add InitialCreate --project BancaLafise.Infrastructure
dotnet ef database update --project BancaLafise.Infrastructure

### 4. Iniciar la API

Ubícate en la carpeta BancaLafise.Api y ejecuta: dotnet run o inicia el proyecto desde visual studio como https

## Primeros Pasos (Inicializar Catálogos)

Una vez levantada la API, realiza los siguientes pasos desde Swagger para inicializar los catálogos:

Ejecuta **POST** /registro-catalogos

Luego ejecuta **GET** /catalogos para verificar que fueron registrados correctamente.

## Autenticación (JWT)

Algunos endpoints requieren autenticación mediante token JWT.

### 1. Registrar un cliente

Ejecuta el endpoint: **POST** /registro-cliente
Proporciona la información del nuevo cliente en el cuerpo de la solicitud.

### 2. Generar un token JWT

Ejecuta: **POST** /generar-token

Proporciona las credenciales del usuario registrado previamente. El token recibido debe ser usado en las llamadas protegidas como Authorization: Bearer <tu_token>.

## Tecnologías Utilizadas

- .NET 8
- Entity Framework Core
- SQLite
- MediatR
- Swagger/OpenAPI
- Clean Architecture
- JWT Authentication
- xUnit