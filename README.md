# 🚀 Empleados API (.NET8 Core RESTful API)

Este es el backend de un sistema de gestión de empleados, desarrollado con una arquitectura **RESTful** utilizando **.NET8**. Proporciona los servicios necesarios para realizar operaciones CRUD (Create, Read, Update, Delete) sobre una base de datos de SQL Server.

## 🛠️ Tecnologías y Herramientas
* **Framework:** .NET8 Core Web API (.NET 8.0+)
* **Base de Datos:** SQL Server
* **ORM:** Entity Framework Core / Dapper
* **Documentación:** Swagger (OpenAPI)
* **CORS:** Configurado para consumo desde Angular

---

## ⚙️ Instalación y Ejecución en Local

Sigue estos pasos para levantar la API en tu máquina:

### 1. Requisitos Previos
* Tener instalado [.NET SDK](https://dotnet.microsoft.com/download) (Versión 8.0 recomendada).
* Tener instalado [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).
* Un editor como **Visual Studio 2022** o **VS Code**.

### 2. Configuración de la Base de Datos
1. Localiza la carpeta `BD/` en este repositorio.
2. Ejecuta el archivo `querys SP.sql` en tu **SQL Server Management Studio (SSMS)**.
3. El script creará automáticamente la base de datos `RRHH` y las tablas necesarias.

### 3. Configurar la Cadena de Conexión
Busca el archivo `appsettings.json` en la raíz del proyecto y actualiza la cadena de conexión con tus credenciales locales:

```json
"ConnectionStrings": {
  "CadenaSQL": "Server=TU_SERVIDOR; Database=DB_Empleados; Integrated Security=True; TrustServerCertificate=True;"
}
