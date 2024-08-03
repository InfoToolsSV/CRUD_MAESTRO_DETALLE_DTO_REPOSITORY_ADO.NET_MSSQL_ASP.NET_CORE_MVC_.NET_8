# Título del Proyecto
ASP.NET Core MVC C# || CRUD Maestro-Detalle con ADO.NET, DTO, Repositorio y Stored Procedures [2024]

## Descripción
Este proyecto es un ejemplo completo de una aplicación CRUD maestro-detalle utilizando ASP.NET Core MVC, ADO.NET, patrones DTO y Repositorio, y SQL Server con procedimientos almacenados. El objetivo del proyecto es demostrar cómo implementar un sistema de gestión de ventas, donde se puede gestionar tanto la información de las ventas como los detalles de cada venta, incluyendo productos y cantidades.

## Tecnologías Utilizadas
- ASP.NET Core MVC
- ADO.NET
- Patrón DTO (Data Transfer Object)
- Patrón Repositorio
- SQL Server
- Procedimientos Almacenados

## Patrón de Arquitectura
Este proyecto sigue una arquitectura en capas utilizando los patrones DTO y Repositorio. A continuación se describe la estructura básica:

- **Presentación (ASP.NET Core MVC)**: Maneja la interfaz de usuario y las interacciones.
- **Aplicación (Servicios y Controladores)**: Gestiona la lógica de negocio y las operaciones CRUD.
- **Infraestructura (ADO.NET, Repositorio)**: Encargada de la comunicación con la base de datos utilizando ADO.NET y el patrón Repositorio.
- **Base de Datos (SQL Server, Procedimientos Almacenados)**: Almacena y gestiona los datos de la aplicación.

## Archivos del Proyecto
- **/Controllers**: Controladores de MVC que manejan las solicitudes HTTP.
- **/Models**: Modelos de datos y DTOs.
- **/Repositories**: Implementaciones del patrón Repositorio.
- **/Views**: Vistas de MVC.
- **/wwwroot**: Archivos estáticos como CSS, JavaScript, e imágenes.

## Instrucciones de Ejecución
1. Clona el repositorio: `git clone https://github.com/InfoToolsSV/CRUD_MAESTRO_DETALLE_DTO_REPOSITORY_ADO.NET_MSSQL_ASP.NET_CORE_MVC_.NET_8.git`
2. Navega al directorio del proyecto: `cd tu-repositorio`
3. Restaura los paquetes NuGet: `dotnet restore`
4. Configura la cadena de conexión en `appsettings.json` para tu base de datos SQL Server.
5. Ejecuta las migraciones de la base de datos (si es necesario): `dotnet ef database update`
6. Compila y ejecuta el proyecto: `dotnet run`
7. Abre tu navegador y navega a `https://localhost:5001` para ver la aplicación en funcionamiento.

## Agradecimientos
Agradecimientos especiales a los miembros del canal InfoToolsSV por su apoyo y contribuciones al proyecto.

---

¡Gracias por utilizar este proyecto! Si tienes alguna pregunta o sugerencia, no dudes en contactarme.
