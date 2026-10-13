# 🏨 Sistema de Reserva de Hotel (`SistemaReservaHotel`)

> **Institución:** ESFE-AGAPE (2026)  
> **Plataforma:** .NET 10.0 (C#)  
> **Arquitectura:** N-Capas (N-Layer Architecture) con ASP.NET Core MVC & Blazor

---

## 📖 Documentación Completa del Proyecto

El repositorio cuenta con dos documentos oficiales exhaustivos en la raíz del proyecto:

1. 👉 **[DOCUMENTACION_SISTEMA.md](DOCUMENTACION_SISTEMA.md):** Manual técnico y funcional de la solución, arquitectura N-Capas, catálogo de entidades, base de datos y matriz de endpoints MVC.
2. 👉 **[AZURE_DEVOPS_QA.md](AZURE_DEVOPS_QA.md):** Gestión completa del proyecto en Azure DevOps y QA: desglose de Épicas, Historias de Usuario (US-14, US-15, US-16, US-SEC-01, etc.), Criterios de Aceptación (Gherkin), Tareas técnicas (Tasks), Planes de Pruebas (Test Plans / Suites / Test Cases paso a paso), Gestión de Defectos (Bugs) y Matriz de Trazabilidad RTM.

---

## ⚡ Resumen de Funcionalidades Principales

1. **🔐 Seguridad y Autenticación Basada en Roles (RBAC):**
   - Inicio de sesión con cookies cifradas y validación SHA-256.
   - Registro de usuarios con rol predeterminado.
   - Claims de identidad, roles y permisos granulares.
   - Semilla automática (`DbInitializer`) con 5 roles, 18 permisos y usuario Administrador maestro (`admin@facturacion.com` / `Admin123!`).

2. **👥 Administración de Usuarios y Privilegios (`AdminController` & `RolesController`):**
   - Catálogo de usuarios con estado activo/inactivo y último acceso.
   - Alta, edición y desactivación de usuarios.
   - Asignación y remoción dinámica de roles.
   - Matriz de permisos clasificados por categorías (*Ventas, Clientes, Productos, Reportes, Administración*).

3. **🛏️ Gestión de Habitaciones y Disponibilidad (`HabitacionesController`):**
   - Catálogo de habitaciones con amenidades (WiFi, TV, A/C, Jacuzzi), capacidad y precio base.
   - Algoritmo de validación de disponibilidad sin solapamiento de fechas.
   - Historial de estados de habitación.

4. **📅 Reservas Administrativas (Historias de Usuario):**
   - **US-14 (Ver Reservas por Día):** Consulta de reservas activas filtradas por fecha con cálculo de noches.
   - **US-16 (Crear Reserva con Tarifa por Temporada):** Creación de reservas aplicando automáticamente precios dinámicos según temporada (Alta, Media, Baja).

5. **📊 Reportería Ejecutiva:**
   - **US-15 (Reporte de Reservas):** Auditoría y reportes filtrando por rango de fechas (inicio y fin) con montos totales.

6. **💳 Pagos y Facturación:**
   - Registro de transacciones con precisión decimal y múltiples métodos de pago (Efectivo, Tarjeta, etc.).

---

## 🛠 Stack Tecnológico

- **Lenguaje:** C# 12 / 13
- **Plataforma:** .NET 10.0
- **Framework Web:** ASP.NET Core MVC y Blazor Web App
- **ORM:** Entity Framework Core 10.0.3 (Code-First)
- **Base de Datos:** Microsoft SQL Server
- **Frontend:** Razor Views, Bootstrap 5.3, Bootstrap Icons, jQuery, DataTables, SweetAlert2
- **Patrones:** Repository Pattern, Unit of Work, DTO Pattern, Inyección de Dependencias

---

## 🚀 Puesta en Marcha

1. Configurar la cadena de conexión en `SistemaReservaHotel.WebMVC/appsettings.json`.
2. Ejecutar la aplicación:
   ```bash
   cd SistemaReservaHotel/SistemaReservaHotel.WebMVC
   dotnet run
   ```
3. Iniciar sesión con:
   - **Usuario:** `admin@facturacion.com`
   - **Contraseña:** `Admin123!`