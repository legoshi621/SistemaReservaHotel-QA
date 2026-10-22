# Plan de Pruebas — Sistema de Reserva Hotel

**Proyecto:** Sistema de Reserva Hotel  
**Versión del documento:** 1.1  
**Fecha de elaboración:** 08 de octubre de 2026  
**Elaborado por:** Equipo de QA — Proyecto SistemaReservaHotel  
**Referencia normativa:** ISO/IEC/IEEE 29119 — Software and Systems Engineering — Software Testing  
**Rama del repositorio:** `feature/Hotel`  
**Framework tecnológico:** ASP.NET Core MVC (.NET 10), Entity Framework Core 10, SQL Server, Blazor Server  

---

## Índice

1. [Introducción](#1-introducción)
   - 1.1 [Alcance](#11-alcance)
     - 1.1.1 [En Alcance](#111-en-alcance)
     - 1.1.2 [Fuera de Alcance](#112-fuera-de-alcance)
   - 1.2 [Objetivo de Calidad](#12-objetivo-de-calidad)
   - 1.3 [Roles y Responsabilidades](#13-roles-y-responsabilidades)
2. [Metodología de Prueba](#2-metodología-de-prueba)
   - 2.1 [Compendio](#21-compendio)
   - 2.2 [Niveles de Prueba](#22-niveles-de-prueba)
   - 2.3 [Clasificación de Errores](#23-clasificación-de-errores)
   - 2.4 [Criterios de Suspensión y Requisitos de Reanudación](#24-criterios-de-suspensión-y-requisitos-de-reanudación)
   - 2.5 [Completitud de la Prueba](#25-completitud-de-la-prueba)
3. [Entregables de la Prueba](#3-entregables-de-la-prueba)
4. [Necesidades de Recursos y Medio Ambiente](#4-necesidades-de-recursos-y-medio-ambiente)
   - 4.1 [Herramientas de Prueba](#41-herramientas-de-prueba)
   - 4.2 [Entorno de Prueba](#42-entorno-de-prueba)
5. [Términos / Acrónimos](#5-términos--acrónimos)

---

## 1. Introducción

El presente Plan de Pruebas describe la estrategia, el alcance, los recursos, el cronograma y los entregables necesarios para validar la calidad del **Sistema de Reserva Hotel**, una aplicación web empresarial desarrollada en ASP.NET Core MVC (.NET 10) con Entity Framework Core 10 y Microsoft SQL Server.

El sistema implementa una arquitectura multicapa (N-Tier) compuesta por cinco proyectos:

| Capa / Proyecto | Responsabilidad |
|:---|:---|
| **SistemaReservaHotel.Entities** | Modelos de dominio (16 entidades: Usuario, Rol, Permiso, Habitacion, Reserva, Pago, Temporada, etc.) |
| **SistemaReservaHotel.DataAccess** | Contexto de EF Core (`AppDbContext`), Repositorios genéricos y específicos, patrón Unit of Work, migraciones y semilla de datos |
| **SistemaReservaHotel.Business** | Servicios de lógica de negocio (AuthService, UsuarioService, RolService, HabitacionService, ReservaService, DisponibilidadService, ComodidadService), DTOs y Helpers |
| **SistemaReservaHotel.WebMVC** | Interfaz web principal: 7 controladores MVC, 15+ vistas Razor, autenticación por cookies, autorización por roles y permisos |
| **SistemaReservaHotel.UI** | Aplicación complementaria en Blazor Server (actualmente con plantillas base) |

El propósito del sistema es gestionar el ciclo completo de operaciones de un hotel: catálogo de habitaciones, verificación de disponibilidad, registro de reservas con tarifas por temporada, control de pagos, y un sistema integral de seguridad basado en roles (RBAC) con usuarios, roles y permisos granulares.

Este plan se elabora conforme a las directrices de la norma **ISO/IEC/IEEE 29119** (Software and Systems Engineering — Software Testing), que establece un marco estandarizado para la documentación y ejecución de procesos de prueba de software.

---

### 1.1 Alcance

#### 1.1.1 En Alcance

Se probarán los siguientes módulos funcionales y características del sistema, agrupados por módulo:

| # | Módulo | Funcionalidad | Roles aplicables | Descripción de la prueba |
|:---:|:---|:---|:---|:---|
| 1 | **Autenticación** | Inicio de sesión (Login) | Todos los usuarios | Verificar que un usuario registrado pueda autenticarse con email y contraseña correctos; que se genere la cookie de autenticación con Claims de identidad, roles y permisos; que se actualice `UltimoAcceso`; que se redirija al Dashboard (Administrador) o al Home (otros roles). |
| 2 | **Autenticación** | Registro de usuario (Register) | Usuarios anónimos | Verificar que un usuario nuevo pueda registrarse proporcionando nombre completo, email, contraseña (mínimo 6 caracteres), confirmación de contraseña y teléfono; que se le asigne automáticamente el rol "Usuario"; que no se permita registrar emails duplicados. |
| 3 | **Autenticación** | Cierre de sesión (Logout) | Usuarios autenticados | Verificar que la sesión de cookies se destruya correctamente y se redirija al Home público. |
| 4 | **Autenticación** | Acceso denegado (Access Denied) | Usuarios autenticados sin permiso | Verificar que un usuario sin el rol requerido sea redirigido a la vista de acceso denegado al intentar acceder a secciones restringidas. |
| 5 | **Gestión de Usuarios** | Listar usuarios activos | Administrador | Verificar que se muestren todos los usuarios activos con sus datos (nombre, email, teléfono, roles, estado, último acceso) en una tabla con paginación, búsqueda y ordenamiento (DataTables). |
| 6 | **Gestión de Usuarios** | Crear usuario | Administrador | Verificar que se pueda crear un nuevo usuario con nombre, email, contraseña, teléfono, estado activo/inactivo y asignación de uno o múltiples roles simultáneamente; que se valide email duplicado. |
| 7 | **Gestión de Usuarios** | Editar usuario | Administrador | Verificar que se puedan modificar nombre, email, teléfono y estado (activo/inactivo) de un usuario existente; que se controle la colisión de emails al modificar; que se muestre la información adicional (fecha de registro, último acceso). |
| 8 | **Gestión de Usuarios** | Desactivar usuario (Soft Delete) | Administrador | Verificar que al desactivar un usuario se cambie su estado a `Activo = false` (borrado lógico) sin eliminarlo de la base de datos; que ya no pueda iniciar sesión. |
| 9 | **Gestión de Usuarios** | Asignar rol a usuario | Administrador | Verificar que se pueda asignar un rol existente a un usuario; que no se permita asignar un rol duplicado; que la asignación se refleje inmediatamente en la vista de edición. |
| 10 | **Gestión de Usuarios** | Remover rol de usuario | Administrador | Verificar que se pueda revocar un rol previamente asignado a un usuario. |
| 11 | **Gestión de Usuarios** | Cambiar contraseña | Administrador | Verificar que se pueda actualizar la contraseña de un usuario y que el nuevo hash SHA-256 se almacene correctamente. |
| 12 | **Gestión de Roles** | Listar roles activos | Administrador | Verificar que se muestren todos los roles activos en formato de tarjetas con su nombre, descripción, estado, conteo de permisos y lista de los primeros 10 permisos asignados. |
| 13 | **Gestión de Roles** | Crear rol | Administrador | Verificar que se pueda registrar un nuevo rol con nombre (requerido, máx. 50 caracteres), descripción (máx. 200 caracteres) y estado activo. |
| 14 | **Gestión de Roles** | Editar rol | Administrador | Verificar que se pueda modificar el nombre, la descripción y el estado de un rol existente. |
| 15 | **Gestión de Roles** | Asignar permiso a rol | Administrador | Verificar que se pueda otorgar un permiso a un rol; que no se permita duplicar la relación; que la interfaz de acordeón por categoría refleje el cambio (ícono de check verde). |
| 16 | **Gestión de Roles** | Remover permiso de rol | Administrador | Verificar que se pueda revocar un permiso de un rol; que se actualice el conteo de permisos del rol. |
| 17 | **Gestión de Roles** | Listar y asignar matriz de permisos | Administrador | Verificar la carga, visualización y persistencia de los 18 permisos del sistema agrupados por categoría (Ventas, Clientes, Productos, Reportes, Administracion) en la base de datos y en la interfaz de gestión de Roles. **Aclaración de alcance:** La prueba se limita estrictamente a la persistencia y asignación de la matriz de permisos en la base de datos y en la interfaz de Roles; el acceso a las pantallas/vistas finales de "Ventas", "Clientes" y "Productos" no será evaluado ya que se encuentran fuera de alcance por estar en desarrollo en esta versión (conforme a 1.1.2). |
| 18 | **Habitaciones** | Buscar habitaciones | Usuarios autenticados | Verificar que se listen todas las habitaciones registradas mostrando número, tipo y precio base. |
| 19 | **Reservas** | Ver reservas por día (US-14) | Usuarios autenticados | Verificar que al seleccionar una fecha se muestren las reservas del día con cliente, habitación, fecha de entrada, fecha de salida y noches calculadas. |
| 20 | **Reservas** | Crear reserva con precio según temporada (US-16) | Usuarios autenticados | Verificar que al crear una reserva se seleccione el tipo de habitación, se busque automáticamente el precio de `TemporadaPrecio` vinculado al tipo, se calcule el `PrecioTotal` y se almacene la reserva en la base de datos. |
| 21 | **Reservas** | Validar disponibilidad (anti-traslape) | Sistema | Verificar que el método `ValidarDisponibilidadAsync` retorne `false` si existe una reserva activa cuyo rango de fechas se superpone con el rango solicitado (`entrada < r.FechaSalida && salida > r.FechaEntrada`). |
| 22 | **Reportes** | Reporte de reservas por rango de fechas (US-15) | Usuarios autenticados | Verificar que al ingresar fecha inicio y fecha fin, se filtren y muestren las reservas correspondientes con cliente, fecha de entrada, fecha de salida y precio total. |
| 23 | **Disponibilidad** | Consultar habitaciones disponibles | Usuarios autenticados | Verificar que el servicio de disponibilidad excluya las habitaciones que tienen reservas activas en conflicto con el rango de fechas solicitado y devuelva solo las habitaciones activas y libres. |
| 24 | **Semilla de datos** | Seed inicial de base de datos | Sistema (automático) | Verificar que al ejecutar la aplicación por primera vez se creen: 18 permisos, 5 roles con su matriz de permisos, y el usuario administrador (`admin@facturacion.com` / `Admin123!`). |
| 25 | **Navegación y Layout** | Menú dinámico por rol | Todos los usuarios | Verificar que el menú de navegación solo muestre las opciones de "Administración" (Usuarios, Roles, Dashboard) cuando el usuario tenga el rol "Administrador"; que los usuarios no autenticados solo vean "Iniciar Sesión" y "Registrarse". |
| 26 | **Validaciones** | Validaciones de formularios | Todos | Verificar las validaciones de Data Annotations en los DTOs: `[Required]`, `[EmailAddress]`, `[StringLength]`, `[Compare]` en formularios de Login, Register, Crear/Editar Usuario, Crear Rol. |
| 27 | **Helper de Claims** | Verificación de permisos granulares | Sistema | Verificar que las extensiones `TienePermiso()`, `TieneCualquierPermiso()`, `TieneTodosLosPermisos()`, `GetUsuarioId()` y `GetEmail()` funcionen correctamente sobre el `ClaimsPrincipal` del usuario autenticado. |
| 28 | **Validador de Fechas** | Regla de negocio de fechas | Sistema | Verificar que `ValidadorFechas.FechasValidas()` retorne `true` solo cuando `entrada < salida` y `entrada >= DateTime.Today`. |

#### 1.1.2 Fuera de Alcance

Las siguientes características y aspectos **no serán probados** en este ciclo de pruebas:

| # | Elemento excluido | Justificación |
|:---:|:---|:---|
| 1 | **Proyecto SistemaReservaHotel.UI (Blazor Server)** | Actualmente contiene únicamente las plantillas de ejemplo de Blazor (Home, Counter, Weather) sin funcionalidad de negocio implementada. No es la interfaz principal del sistema. |
| 2 | **Pruebas de rendimiento y carga** | No se contempla la medición de tiempos de respuesta bajo estrés, concurrencia de usuarios ni pruebas de carga (load testing) debido a restricciones de tiempo y recursos del ciclo actual. |
| 3 | **Pruebas de seguridad avanzada** (penetración, inyección SQL, XSS) | El alcance actual cubre la funcionalidad de autenticación y autorización, pero no incluye pruebas de seguridad ofensiva ni auditorías de vulnerabilidades. |
| 4 | **Diseño visual y UX/UI** | No se evaluará la apariencia gráfica, la experiencia de usuario, la responsividad de diseño ni la compatibilidad entre navegadores más allá de la funcionalidad. |
| 5 | **Pruebas de despliegue e infraestructura** | No se probará el proceso de publicación en producción, configuración de IIS, Docker ni pipelines de CI/CD. |
| 6 | **Módulos no implementados (Vistas finales)** | Los enlaces del menú y vistas directas de "Clientes", "Productos" y "Ventas" se encuentran actualmente en desarrollo en esta versión; no se probarán ni evaluarán sus pantallas finales. Únicamente se evalúa la asignación y persistencia de sus permisos en la interfaz de Roles y base de datos (conforme a 1.1.1, ítem 17). |
| 7 | **Migraciones de base de datos** | La verificación de integridad de los archivos de migración de EF Core queda fuera del alcance funcional. |
| 8 | **Resolución de conflictos de merge Git** | El archivo `Program.cs` y `appsettings.json` del proyecto WebMVC contienen marcadores de conflicto (`<<<<<<< HEAD`). La resolución de estos conflictos es una actividad de desarrollo, no de pruebas. |

---

### 1.2 Objetivo de Calidad

Los objetivos de calidad de este ciclo de pruebas son:

1. **Verificación funcional completa:** Asegurar que todos los módulos dentro del alcance (Autenticación, Gestión de Usuarios, Gestión de Roles y Permisos, Habitaciones, Reservas, Reportes, Disponibilidad y Semilla de Datos) funcionen de acuerdo a los requisitos y las historias de usuario definidas (US-14, US-15, US-16).

2. **Integridad de las reglas de negocio:** Confirmar que las validaciones críticas del sistema se ejecuten correctamente:
   - No se permitan emails duplicados al registrar o editar usuarios.
   - No se permitan reservas con fechas superpuestas para la misma habitación (validación anti-traslape).
   - Las fechas de reserva cumplan la regla `entrada < salida` y `entrada >= hoy`.
   - Los precios de reserva se calculen correctamente según la temporada y tipo de habitación.
   - El borrado de usuarios sea lógico (soft delete) y no físico.

3. **Seguridad de acceso:** Validar que el sistema de autenticación basada en cookies y autorización basada en roles funcione de extremo a extremo:
   - Solo los usuarios con rol "Administrador" puedan acceder a las secciones de gestión de usuarios, roles y permisos.
   - Las Claims de permisos se generen correctamente al momento del login.
   - Los usuarios desactivados no puedan iniciar sesión.

4. **Fiabilidad de datos iniciales:** Confirmar que la semilla de datos (`DbInitializer`) genere los 18 permisos, 5 roles con su matriz de permisos y el usuario administrador predeterminado.

5. **Cobertura mínima del 100%** de los casos de prueba planificados en ejecución, con una tasa de aprobación objetivo del **85%** o superior.

---

### 1.3 Roles y Responsabilidades

| # | Rol | Responsable | Tareas principales |
|:---:|:---|:---|:---|
| 1 | **Test Manager (Gerente de Pruebas)** | [Nombre Integrante 1] | Planificar, coordinar y dar seguimiento general al plan de pruebas; gestionar recursos y cronograma; tomar decisiones de suspensión o reanudación; aprobar los entregables finales de prueba; consolidar métricas y comunicar el estado del avance a las partes interesadas. |
| 2 | **Analista QA / Diseñador de Pruebas** | [Nombre Integrante 2] | Diseñar y documentar la especificación formal de casos de prueba funcionales; estructurar y mantener actualizada la matriz de trazabilidad de requisitos; definir criterios de aceptación detallados y validar la cobertura de las historias de usuario (US-14, US-15, US-16). |
| 3 | **Tester Principal / Ejecutor de Pruebas** | [Nombre Integrante 3] | Coordinar y liderar las sesiones de ejecución manual de casos de prueba; documentar evidencias gráficas con Snipping Tool; registrar, clasificar por severidad/prioridad y dar seguimiento a los defectos en Azure DevOps Boards; coordinar los ciclos de retesting. |
| 4 | **Administrador de Entorno y Datos (Test Admin)** | [Nombre Integrante 4] | Configurar y mantener operativo el entorno de pruebas local (SQL Server, Kestrel/IIS); gestionar la base de datos `SistemaReservaHotel_Test`; ejecutar migraciones de EF Core y verificar la ejecución correcta de la semilla inicial (`DbInitializer`). |
| 5 | **Aseguramiento de Calidad de Software (SQA)** | [Nombre Integrante 1] y [Nombre Integrante 2] | Auditar el proceso de pruebas asegurando el estricto cumplimiento de la norma ISO/IEC/IEEE 29119; revisar la calidad formal y consistencia de los artefactos generados (plan, casos de prueba, reportes); supervisar las métricas de completitud. |
| 6 | **Soporte Técnico de Desarrollo y Resolución de Defectos** | [Nombre Integrante 3] y [Nombre Integrante 4] | Brindar soporte técnico respecto a la arquitectura N-Tier (.NET 10 / EF Core 10); asistir en la reproducción y diagnóstico de defectos complejos en Visual Studio; colaborar en la resolución de conflictos de merge (`Program.cs`, `appsettings.json`) y verificación de correcciones. |

> **Nota de Asignación y Ejecución Equitativa:**  
> La asignación de roles directivos, analíticos, técnicos y de aseguramiento ha sido balanceada equitativamente entre los 4 integrantes del equipo. Adicionalmente, las actividades prácticas de **ejecución de casos de prueba y sesiones de retesting** serán compartidas equitativamente entre los cuatro miembros ([Nombre Integrante 1], [Nombre Integrante 2], [Nombre Integrante 3], [Nombre Integrante 4]), distribuyendo los módulos funcionales del sistema en cada ciclo de prueba.

---

## 2. Metodología de Prueba

### 2.1 Compendio

Se adopta una **metodología de pruebas Iterativa** para el presente proyecto, dada la naturaleza del sistema:

- **Justificación de la elección:** El Sistema de Reserva Hotel está siendo desarrollado de forma iterativa e incremental, evidenciado por su historial de commits en Git (`Parte 1 Terminada`, `feat: implementacion historias de usuario reservas admin MVC`, `feat: cambios en modulo hotel`). Cada iteración agrega funcionalidad nueva al sistema (primero el módulo de seguridad con usuarios, roles y permisos; luego el módulo de habitaciones y reservas). La metodología iterativa permite alinear los ciclos de prueba con cada entrega de desarrollo, verificando la funcionalidad nueva y realizando pruebas de regresión sobre las funcionalidades previamente entregadas.

- **Ciclo de vida de la prueba:**

```
┌─────────────────────────────────────────────────────────────────┐
│  1. Planificación  →  2. Diseño de   →  3. Preparación del    │
│     de pruebas        casos de prueba     entorno y datos      │
│         │                                      │               │
│         └──────────────────────────────────────┘               │
│                          ↓                                      │
│  4. Ejecución de  →  5. Registro de  →  6. Re-testing y       │
│     pruebas           defectos           pruebas de regresión  │
│                          ↓                                      │
│  7. Evaluación de resultados  →  8. Cierre del ciclo           │
└─────────────────────────────────────────────────────────────────┘
```

- **Técnicas de diseño de pruebas aplicadas (conforme a ISO/IEC/IEEE 29119-4):**
  - **Partición de equivalencia:** Para agrupar valores de entrada válidos e inválidos en los formularios (ej: email válido/inválido, contraseña con menos de 6 caracteres vs. 6+ caracteres).
  - **Análisis de valores límite:** Para probar los bordes de las restricciones de longitud (ej: `NombreCompleto` máx. 100 caracteres, `Nombre` de rol máx. 50 caracteres).
  - **Tabla de decisión:** Para probar las combinaciones de roles y permisos que determinan el acceso a las diferentes secciones del sistema.
  - **Pruebas de flujo de trabajo (escenarios):** Para verificar los flujos completos de extremo a extremo (registrarse → iniciar sesión → crear reserva → ver reporte).

---

### 2.2 Niveles de Prueba

Se ejecutarán los siguientes niveles de prueba:

| # | Nivel de prueba | Descripción | Alcance en el proyecto |
|:---:|:---|:---|:---|
| 1 | **Pruebas Unitarias** | Verifican el comportamiento correcto de funciones y métodos individuales de manera aislada. | Métodos de los servicios de negocio: `AuthService.HashPassword()`, `AuthService.VerifyPasswordHash()`, `ValidadorFechas.FechasValidas()`, `ReservaRepository.ValidarDisponibilidadAsync()`, propiedades computadas (`Reserva.Noches`, `Habitacion.PrecioActual`, `Usuario.NombresRoles`). |
| 2 | **Pruebas de Integración** | Verifican la interacción correcta entre dos o más componentes del sistema combinados. | Interacción entre las capas: Servicio → Repositorio → Base de datos (ej: `AuthService.LoginAsync()` que usa `UsuarioRepository.GetByEmailAsync()` y luego `UnitOfWork.SaveChangesAsync()`). Interacción entre `DbInitializer.SeedAsync()` y las tablas de la base de datos. |
| 3 | **Pruebas de Sistema** | Verifican el sistema completo e integrado contra los requisitos especificados, desde la interfaz del usuario hasta la base de datos. | Flujos de extremo a extremo a través de la interfaz web MVC: Login completo con generación de claims → Navegación al panel de administración → CRUD de usuarios → Asignación de roles y permisos → Creación de reservas → Generación de reportes. |
| 4 | **Pruebas de Aceptación del Usuario (UAT)** | Pruebas realizadas por el usuario final o representante del cliente para validar que el sistema cumple con las necesidades del negocio. | Validación de las historias de usuario: US-14 (ver reservas por día), US-15 (reporte de reservas), US-16 (crear reserva con precio por temporada). |

---

### 2.3 Clasificación de Errores

Los defectos encontrados durante la ejecución de las pruebas se clasificarán según dos dimensiones: **severidad** (impacto técnico) y **prioridad** (urgencia de corrección).

#### Clasificación por Severidad

| Nivel | Severidad | Descripción | Ejemplo en el proyecto |
|:---:|:---|:---|:---|
| S1 | **Crítico (Bloqueante)** | El defecto impide el uso completo del sistema o de un módulo esencial. No existe solución alternativa (workaround). Causa pérdida de datos o corrupción. | El sistema no permite iniciar sesión a ningún usuario; la semilla de datos no se ejecuta y la base de datos queda vacía; la validación anti-traslape de reservas no funciona y se permiten reservas duplicadas. |
| S2 | **Mayor (Alto)** | El defecto afecta una funcionalidad principal, pero existe una solución alternativa temporal. | Un administrador no puede asignar roles a un usuario desde la vista de edición, pero puede hacerlo creando un nuevo usuario con el rol; el reporte de reservas no filtra correctamente por fechas. |
| S3 | **Menor (Moderado)** | El defecto afecta una funcionalidad secundaria o presenta un comportamiento inesperado que no bloquea el flujo principal. | Los mensajes de éxito (`TempData["Success"]`) no se muestran al crear un rol; el campo "Último Acceso" muestra "Nunca" para el usuario admin después de hacer login. |
| S4 | **Trivial (Bajo)** | Defecto cosmético o de baja relevancia que no afecta la funcionalidad. | Un error tipográfico en una etiqueta del formulario; un ícono de Bootstrap Icons que no carga; el formato de fecha no es consistente entre vistas. |

#### Clasificación por Prioridad

| Nivel | Prioridad | Tiempo estimado de resolución |
|:---:|:---|:---|
| P1 | **Urgente** | Debe corregirse inmediatamente (dentro de 24 horas). |
| P2 | **Alta** | Debe corregirse antes de la siguiente iteración de pruebas (dentro de 3 días hábiles). |
| P3 | **Media** | Puede corregirse en la siguiente versión planificada. |
| P4 | **Baja** | Se agenda para una versión futura; no impide el cierre del ciclo de pruebas actual. |

#### Proceso de Triaje de Errores

Se realizarán reuniones de triaje de errores **dos veces por semana** (martes y jueves) con la participación del Test Manager, al menos un Analista QA y un Desarrollador, para:
1. Revisar los nuevos defectos reportados.
2. Validar o ajustar la severidad y prioridad asignada.
3. Asignar el responsable de la corrección.
4. Establecer la fecha comprometida de corrección.
5. Revisar el estado de los defectos pendientes de corrección.

---

### 2.4 Criterios de Suspensión y Requisitos de Reanudación

#### Criterios de Suspensión

Las pruebas se suspenderán **total o parcialmente** cuando se presente cualquiera de las siguientes condiciones:

| # | Condición de suspensión | Tipo de suspensión |
|:---:|:---|:---|
| 1 | El **35% o más** de los casos de prueba ejecutados han fallado en un mismo ciclo de ejecución. | Suspensión total |
| 2 | Se detecta un defecto **S1 (Crítico/Bloqueante)** que impide la ejecución de los demás casos de prueba (ej: la aplicación no arranca, la base de datos no se conecta, el login no funciona para ningún usuario). | Suspensión total |
| 3 | El entorno de pruebas (servidor SQL Server, servidor web) se encuentra inestable o no disponible. | Suspensión total |
| 4 | Los **conflictos de merge** en los archivos `Program.cs` y `appsettings.json` del proyecto WebMVC no han sido resueltos, impidiendo la compilación del proyecto. | Suspensión total |
| 5 | Se descubre que un módulo específico tiene más del **50%** de sus casos de prueba fallidos. | Suspensión parcial (del módulo afectado) |

#### Requisitos de Reanudación

Las pruebas se reanudarán cuando se cumplan todas las siguientes condiciones:

| # | Condición de reanudación |
|:---:|:---|
| 1 | Todos los defectos S1 (Críticos/Bloqueantes) que causaron la suspensión han sido corregidos y verificados por el equipo de desarrollo. |
| 2 | Se ha generado un nuevo build de la aplicación con las correcciones incorporadas. |
| 3 | El entorno de pruebas ha sido restablecido a un estado conocido y funcional (base de datos reseeded, aplicación compilada sin errores). |
| 4 | El Test Manager ha autorizado formalmente la reanudación de las pruebas. |
| 5 | Se ha realizado una prueba de humo (smoke test) exitosa que confirme que las funcionalidades básicas (login, navegación, acceso a BD) operan correctamente. |

---

### 2.5 Completitud de la Prueba

La prueba se considerará **completa** cuando se cumplan todos los siguientes criterios:

| # | Criterio de completitud | Valor objetivo |
|:---:|:---|:---|
| 1 | **Tasa de ejecución** (casos ejecutados / casos planificados) | **100%** — Todos los casos de prueba planificados deben ser ejecutados. Solo se aceptará un porcentaje menor si el Test Manager lo justifica formalmente. |
| 2 | **Tasa de aprobación** (casos aprobados / casos ejecutados) | **≥ 85%** — Al menos el 85% de los casos de prueba ejecutados deben haber pasado exitosamente. |
| 3 | **Defectos S1 (Críticos) abiertos** | **0** — No deben existir defectos de severidad Crítica sin resolver al momento del cierre. |
| 4 | **Defectos S2 (Mayores) abiertos** | **≤ 2** — Se permiten un máximo de 2 defectos de severidad Mayor abiertos, siempre que tengan un workaround documentado y estén programados para la siguiente versión. |
| 5 | **Cobertura de requisitos** | **100%** — Todos los requisitos funcionales dentro del alcance (28 funcionalidades identificadas en la sección 1.1.1) deben tener al menos un caso de prueba asociado. |
| 6 | **Matriz de trazabilidad** | Completada al 100%, mostrando la relación entre cada requisito/historia de usuario y sus casos de prueba correspondientes. |

---

## 3. Entregables de la Prueba

Los artefactos que se generarán durante el ciclo de vida de las pruebas son:

### Antes de la fase de pruebas

| # | Entregable | Descripción |
|:---:|:---|:---|
| 1 | **Plan de Pruebas** (este documento) | Documento maestro que describe la estrategia, alcance, metodología, criterios y recursos del proyecto de pruebas. |
| 2 | **Especificación de Casos de Prueba** | Documento detallado con todos los casos de prueba organizados por módulo, incluyendo: ID del caso, precondiciones, pasos de ejecución, datos de entrada, resultado esperado y resultado real. |
| 3 | **Matriz de Trazabilidad de Requisitos** | Tabla cruzada que vincula cada requisito funcional/historia de usuario (US-14, US-15, US-16 y demás funcionalidades) con sus casos de prueba correspondientes, garantizando cobertura completa. |
| 4 | **Especificación de Datos de Prueba** | Documento con los conjuntos de datos necesarios para ejecutar los casos de prueba: usuarios de prueba, habitaciones, reservas de ejemplo, fechas, roles y permisos. |

### Durante la fase de pruebas

| # | Entregable | Descripción |
|:---:|:---|:---|
| 5 | **Registro de Ejecución de Pruebas** | Bitácora de ejecución con la fecha, hora, ejecutor, caso de prueba, resultado (Paso/Fallo/Bloqueado), y observaciones de cada ejecución. |
| 6 | **Reportes de Defectos (Bugs)** | Cada defecto encontrado se documenta con: ID único, título descriptivo, severidad, prioridad, módulo afectado, pasos de reproducción, resultado obtenido vs. esperado, capturas de pantalla y estado (Nuevo, En progreso, Corregido, Verificado, Cerrado, Reabierto). |
| 7 | **Registro de Retesting** | Documentación de las re-pruebas realizadas sobre defectos corregidos, indicando si la corrección fue exitosa. |

### Después de la fase de pruebas

| # | Entregable | Descripción |
|:---:|:---|:---|
| 8 | **Informe de Resultados de Pruebas** | Resumen ejecutivo del ciclo de pruebas con métricas clave: total de casos ejecutados, tasa de aprobación, tasa de fallo, distribución de defectos por severidad y módulo, tendencias de defectos entre ciclos. |
| 9 | **Informe de Defectos Pendientes** | Listado de defectos que permanecen abiertos al cierre del ciclo, con su severidad, prioridad y plan de acción para la siguiente versión. |
| 10 | **Informe de Cierre de Pruebas** | Documento formal que resume las actividades realizadas, las desviaciones del plan original, las lecciones aprendidas y la recomendación de aprobación o rechazo para la liberación del software. |

---

## 4. Necesidades de Recursos y Medio Ambiente

### 4.1 Herramientas de Prueba

A continuación se definen las herramientas definitivas seleccionadas para el proyecto, eliminando alternativas genéricas para garantizar un estándar único y consistente de trabajo:

| # | Categoría | Herramienta | Propósito en el Proyecto |
|:---:|:---|:---|:---|
| 1 | **Gestión de casos de prueba y trazabilidad** | Microsoft Excel | Documentación, diseño, organización y seguimiento de la ejecución de los casos de prueba, incluyendo la matriz de trazabilidad de requisitos. |
| 2 | **Seguimiento de defectos** | Azure DevOps Boards | Registro centralizado, clasificación (severidad/prioridad), asignación y seguimiento del ciclo de vida de los defectos (bugs) encontrados. |
| 3 | **Control de versiones** | Git (Azure DevOps Repos) | Trazabilidad y control de versiones del código fuente probado. Rama oficial de pruebas: `feature/Hotel`. |
| 4 | **Base de datos** | SQL Server Management Studio (SSMS) | Conexión e inspección directa de la base de datos SQL Server (`SistemaReservaHotel_Test`), verificación de datos insertados, actualización de estados y validación de la semilla de datos. |
| 5 | **Navegador principal** | Google Chrome | Navegador web para la ejecución y validación de las pruebas funcionales de la aplicación web MVC. |
| 6 | **Captura de evidencias** | Snipping Tool | Captura de pantallas y recortes gráficos de los resultados obtenidos durante la ejecución de cada caso de prueba para documentar evidencia. |

---

### 4.2 Entorno de Prueba

#### Requisitos de Hardware (mínimos)

| Componente | Especificación mínima |
|:---|:---|
| **Procesador** | Intel Core i5 de 10ª generación o superior (o equivalente AMD Ryzen 5) |
| **Memoria RAM** | 8 GB como mínimo (16 GB recomendado) |
| **Almacenamiento** | 50 GB de espacio libre en disco (SSD recomendado) |
| **Red** | Conexión a la red local LAN / Wi-Fi; acceso al servidor de base de datos SQL Server |
| **Pantalla** | Resolución mínima de 1366 × 768 px |

#### Requisitos de Software

| Componente | Versión requerida |
|:---|:---|
| **Sistema Operativo** | Windows 10 (21H2 o posterior) / Windows 11 |
| **.NET SDK** | .NET 10 SDK (según los `.csproj` del proyecto) |
| **Motor de Base de Datos** | Microsoft SQL Server 2019 o posterior / SQL Server Express |
| **Herramienta de Base de Datos** | SQL Server Management Studio (SSMS) 19+ |
| **Navegador Web** | Google Chrome (versión más reciente) |
| **Entorno de Desarrollo (IDE)** | Visual Studio 2022 (17.x) o posterior, con carga de trabajo "ASP.NET and web development" instalada |
| **Control de Versiones** | Git for Windows 2.40+ |
| **Node.js (opcional)** | No requerido para el proyecto actual (no usa frontend SPA) |

#### Configuración de la Base de Datos de Prueba

| Parámetro | Valor |
|:---|:---|
| **Servidor** | Instancia local de SQL Server (`localhost` o `.\SQLEXPRESS`) |
| **Nombre de la Base de Datos** | `SistemaReservaHotel_Test` (se recomienda una base de datos dedicada para pruebas, separada de desarrollo) |
| **Cadena de conexión** | `Server=<SERVIDOR>;Database=SistemaReservaHotel_Test;Trusted_Connection=True;TrustServerCertificate=True` |
| **Datos iniciales** | Generados automáticamente por `DbInitializer.SeedAsync()` al primer arranque de la aplicación |
| **Usuario admin de prueba** | Email: `admin@facturacion.com` / Contraseña: `Admin123!` |

#### Condiciones previas al inicio de pruebas

1. Los conflictos de merge en `Program.cs` y `appsettings.json` del proyecto WebMVC deben estar resueltos.
2. La solución debe compilar sin errores.
3. Las migraciones de EF Core deben aplicarse correctamente sobre la base de datos de prueba.
4. La semilla de datos (`DbInitializer`) debe haberse ejecutado exitosamente.
5. El usuario administrador de prueba debe poder iniciar sesión.

---

## 5. Términos / Acrónimos

| Término / Acrónimo | Definición |
|:---|:---|
| **AUT** | *Application Under Test* — Aplicación bajo prueba. En este caso, el Sistema de Reserva Hotel. |
| **RBAC** | *Role-Based Access Control* — Control de acceso basado en roles. Modelo de seguridad implementado en el sistema con entidades Usuario, Rol y Permiso. |
| **EF Core** | *Entity Framework Core* — ORM (Object-Relational Mapper) utilizado para el acceso a la base de datos SQL Server. Versión 10.0.3. |
| **MVC** | *Model-View-Controller* — Patrón de arquitectura utilizado en la capa de presentación web (ASP.NET Core MVC). |
| **DTO** | *Data Transfer Object* — Objeto de transferencia de datos utilizado para transportar información entre la capa de negocio y la capa de presentación sin exponer las entidades del dominio directamente. |
| **UoW** | *Unit of Work* — Patrón de diseño que agrupa múltiples operaciones de repositorio en una sola transacción atómica. |
| **CRUD** | *Create, Read, Update, Delete* — Operaciones básicas de persistencia de datos. |
| **SHA-256** | *Secure Hash Algorithm 256-bit* — Algoritmo de hash criptográfico utilizado para almacenar las contraseñas de los usuarios en la base de datos. |
| **Claim** | Declaración de identidad asociada a un usuario autenticado (ej: nombre, email, roles, permisos). Forma parte del estándar de autenticación de ASP.NET Core. |
| **Seed** | Proceso de carga inicial de datos predefinidos en la base de datos al primer arranque de la aplicación. |
| **Soft Delete** | Borrado lógico. En lugar de eliminar físicamente un registro de la base de datos, se marca como inactivo (`Activo = false`). |
| **Anti-traslape** | Regla de negocio que impide que dos reservas activas se superpongan en fechas para la misma habitación. |
| **US** | *User Story* — Historia de usuario. Requisito funcional expresado desde la perspectiva del usuario final. |
| **US-14** | Historia de usuario: Ver reservas registradas por día seleccionado. |
| **US-15** | Historia de usuario: Generar reporte de reservas filtrado por rango de fechas. |
| **US-16** | Historia de usuario: Crear reserva aplicando precio según la temporada vinculada al tipo de habitación. |
| **QA** | *Quality Assurance* — Aseguramiento de la calidad del software. |
| **SQA** | *Software Quality Assurance* — Proceso y conjunto de actividades para asegurar la calidad del software. |
| **ISO/IEC/IEEE 29119** | Norma internacional para procesos de prueba de software. Establece un marco de referencia para la planificación, diseño, ejecución, monitoreo y cierre de las pruebas. |
| **Smoke Test** | Prueba de humo. Conjunto mínimo de pruebas que verifican que las funcionalidades críticas del sistema (arranque, login, navegación básica) operan correctamente antes de iniciar un ciclo de pruebas completo. |
| **Retesting** | Re-prueba. Ejecución de los casos de prueba que fallaron previamente para verificar que el defecto ha sido corregido. |
| **Prueba de Regresión** | Ejecución de casos de prueba sobre funcionalidades previamente aprobadas para verificar que las correcciones o nuevos desarrollos no hayan introducido defectos colaterales. |
| **Build** | Versión compilada y ejecutable de la aplicación, generada a partir del código fuente. |
| **N-Tier** | Arquitectura de software multicapa donde cada capa tiene una responsabilidad específica (Entities, DataAccess, Business, UI). |
| **DataTables** | Plugin de jQuery utilizado en la vista de Gestión de Usuarios para agregar funcionalidades de paginación, búsqueda en tiempo real y ordenamiento a las tablas HTML. |
| **Bootstrap 5** | Framework CSS utilizado en el frontend de la aplicación web para el diseño responsivo y los componentes de interfaz. |
| **Blazor Server** | Modelo de hosting de Blazor donde la lógica del componente se ejecuta en el servidor y las actualizaciones de la interfaz se envían al cliente mediante SignalR. |

---

> **Nota final:** Este Plan de Pruebas es un documento vivo que puede actualizarse conforme avance el proyecto, se resuelvan los conflictos de merge pendientes, se implementen nuevas funcionalidades o se identifiquen riesgos adicionales. Cualquier modificación debe ser aprobada por el Test Manager y comunicada a todas las partes interesadas.

---

*Documento elaborado conforme a la estructura de Plan de Pruebas basada en la norma ISO/IEC/IEEE 29119-3:2021 y la plantilla de Guru99.*
