# Documentación Oficial de Pruebas de Integración (Nivel 2) — Sistema de Reserva Hotel

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama de Aislamiento:** `feature/pruebas-integracion`  
**Fecha de Transición y Validación:** 20 de Octubre de 2026  
**Framework Tecnológico:** .NET 10.0 (`net10.0`) | ASP.NET Core MVC Testing | EF Core InMemory 10.0.13 | FluentAssertions 8.11.0 | xUnit 2.9.3  
**Proyecto de Pruebas:** `SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj`  
**Tasa de Aprobación Global:** **100% SUPERADO (16 de 16 pruebas superadas — 0 fallidas — 0 omitidas)**  
**Tiempo Total de Ejecución:** **4.17 segundos**  

---

## 1. Resumen Ejecutivo y Transición al Nivel 2

En cumplimiento con la estrategia integral de aseguramiento de calidad (QA), el proyecto realizó con éxito la **transición del Nivel 1 (Pruebas Unitarias Aisladas con Moq)** al **Nivel 2 (Pruebas de Integración de Servicios y API)**.

A diferencia de las pruebas unitarias, las pruebas de integración validan el flujo completo de la petición a través del pipeline HTTP de ASP.NET Core:
1. **Enrutamiento y Model Binding:** Deserialización y vinculación correcta de parámetros en peticiones GET y POST (JSON y Query parameters).
2. **Controladores y Capa de Negocio:** Invocación real de los controladores MVC (`ReservaController`, `DisponibilidadController`), servicios (`IReservaService`, `IDisponibilidadService`) y validadores de negocio (`ValidadorFechas`).
3. **Persistencia e Integración de Datos:** Uso de `Microsoft.EntityFrameworkCore.InMemory` para verificar operaciones CRUD reales, transiciones de estado y consultas sobre datos en memoria sin depender de un motor físico SQL Server.

```
┌───────────────────────────────────────────────────────────────────────────┐
│                    ARQUITECTURA DE INTEGRACIÓN (NIVEL 2)                  │
└───────────────────────────────────────────────────────────────────────────┘

 [xUnit Test Client]
         │ (HTTP GET / POST en memoria con TestHost)
         ▼
 [WebApplicationFactory<Program>]
         │
         ├──► Pipeline MVC (Routing, ModelBinding, ActionFilters)
         │           │
         │           ├──► ReservaController  (Crear, PorDia, Detalles, Cancelar)
         │           └──► DisponibilidadController (Index)
         │
         ├──► Lógica de Negocio & Helpers (ValidadorFechas, ReservaService)
         │
         └──► AppDbContext (Microsoft.EntityFrameworkCore.InMemory)
                     │
                     ├── 1 Habitación Activa Semilla (Id: 1, Matrimonial, $75)
                     ├── 1 Usuario Semilla (Id: 1, Tester QA)
                     └── 1 Reserva Activa Semilla (Id: 1, Carlos Miranda, Día +10 a +15)
```

---

## 2. Distribución de Pruebas de Integración por Integrante

| # | Integrante del Equipo | Rol en QA | Historia de Usuario | Archivo de Prueba | Pruebas | Resultado |
|:---:|:---|:---|:---|:---|:---:|:---:|
| 1 | **Ariel Isai Ascencio Ramos** (`Isai Ascencio`) | Tester de Reglas Cronológicas y Calendario | **US-16 (#1040):** Validación de fechas y reglas cronológicas | `ValidadorFechasIntegrationTests.cs` | **5** | 🟢 **100% Passed** |
| 2 | **Joselin Esmeralda Herrera Flores** (`Joselin Herrera`) | Analista de Inventario y Disponibilidad | **US-15 (#1037):** Disponibilidad por fechas y estado | `DisponibilidadIntegrationTests.cs` | **3** | 🟢 **100% Passed** |
| 3 | **Carlos Alonso Miranda Rivas** (`Alonso Rivas`) | Tester de Consultas y Lectura | **US-14 (#1034):** Consulta por día y recuperación por ID | `ConsultasReservaIntegrationTests.cs` | **4** | 🟢 **100% Passed** |
| 4 | **Camila Antonia Calderón Cortez** (`cami calderon`) | Tester de Mutación y Persistencia | **US-SEC-01 (#1043):** Ciclo CRUD y control de colisión | `ReservaCrudIntegrationTests.cs` | **4** | 🟢 **100% Passed** |
| — | **TOTAL GENERAL** | **4 Integrantes** | **4 Historias de Usuario (PBI)** | **4 Suites de Integración** | **16** | 🟢 **16 / 16 (100%)** |

---

## 3. Detalle Exhaustivo de las 16 Pruebas de Integración

---

### Suite 1: `ValidadorFechasIntegrationTests` (5 Pruebas) — Ariel Isai Ascencio Ramos
* **Historia Azure DevOps:** **US-16 (#1040)** — *Validación de rango de fechas y reglas cronológicas de reserva*.
* **Caso de Prueba Test Plan:** **TC-05 (#1050)**.
* **Endpoint Probado:** `POST /Reserva/Crear`.
* **Objetivo:** Validar que el pipeline HTTP rechace de forma estricta (HTTP 400 BadRequest) cualquier solicitud que contravenga las reglas temporales de reserva antes de tocar la base de datos, y que permita y persista únicamente solicitudes con fechas coherentes (HTTP 200 OK).

| Caso | Método de Prueba | Condición Probada | Payload Enviado | Código HTTP | Verificación en BD InMemory | Estado |
|:---:|:---|:---|:---|:---:|:---|:---:|
| **1.1** | `PostReserva_ConFechasValidas_Retorna200OkYPersisteReserva` | Fechas futuras válidas (`entrada < salida && entrada >= Hoy`) | `Entrada: Hoy+20`, `Salida: Hoy+25`, `Hab: 1` | **200 OK** | Entidad persistida en `AppDbContext.Reservas` con `Id > 0` y `Estado = "Activa"` | 🟢 **Passed** |
| **1.2** | `PostReserva_ConFechaEntradaMayorQueSalida_Retorna400BadRequest` | Rango temporal invertido | `Entrada: Hoy+25`, `Salida: Hoy+20` | **400 BadRequest** | No se genera registro en BD | 🟢 **Passed** |
| **1.3** | `PostReserva_ConFechaEntradaIgualASalida_Retorna400BadRequest` | Estancia de 0 noches (mismo día de entrada y salida) | `Entrada: Hoy+5`, `Salida: Hoy+5` | **400 BadRequest** | Petición rechazada preventivamente | 🟢 **Passed** |
| **1.4** | `PostReserva_ConFechaEntradaEnElPasado_Retorna400BadRequest` | Check-in en el pasado con salida futura | `Entrada: Hoy-2`, `Salida: Hoy+3` | **400 BadRequest** | Bloqueo por regla cronológica de negocio | 🟢 **Passed** |
| **1.5** | `PostReserva_ConAmbasFechasEnElPasado_Retorna400BadRequest` | Rango de fechas completamente caducado | `Entrada: Hoy-10`, `Salida: Hoy-5` | **400 BadRequest** | Bloqueo por fechas caducadas | 🟢 **Passed** |

---

### Suite 2: `DisponibilidadIntegrationTests` (3 Pruebas) — Joselin Esmeralda Herrera Flores
* **Historia Azure DevOps:** **US-15 (#1037)** — *Disponibilidad por fechas y estado de inventario*.
* **Casos de Prueba Test Plan:** **TC-03 (#1048)**, **TC-04 (#1049)**.
* **Endpoint Probado:** `GET /Disponibilidad?entrada={entrada}&salida={salida}`.
* **Objetivo:** Validar la consulta HTTP de habitaciones disponibles interactuando directamente con el contexto de Entity Framework en memoria, comprobando exclusión por solapamiento con reservas activas y filtrado de habitaciones inactivas.

| Caso | Método de Prueba | Condición Probada | Parámetros Query | Código HTTP | Resultado Esperado | Estado |
|:---:|:---|:---|:---|:---:|:---|:---:|
| **2.1** | `GetDisponibilidad_EnRangoSinConflicto_Retorna200OkYHabitacionDisponible` | Rango de fechas libre sin colisiones | `entrada: Hoy+1`, `salida: Hoy+3` | **200 OK** | Retorna la habitación activa semilla (`Id: 1`, `Numero: 101`) | 🟢 **Passed** |
| **2.2** | `GetDisponibilidad_EnRangoConReservaActiva_ExcluyeHabitacionOcupada` | Rango con solapamiento sobre la reserva semilla 1 (`+10` a `+15`) | `entrada: Hoy+11`, `salida: Hoy+14` | **200 OK** | La habitación 1 es excluida de la respuesta (lista vacía) | 🟢 **Passed** |
| **2.3** | `GetDisponibilidad_HabitacionInactiva_NoSeIncluyeEnDisponibles` | Habitación configurada con `Activa = false` | Habitación 99 inactiva agregada a InMemory DB | **200 OK** | La habitación inactiva no figura bajo ningún concepto en la respuesta | 🟢 **Passed** |

---

### Suite 3: `ConsultasReservaIntegrationTests` (4 Pruebas) — Carlos Alonso Miranda Rivas
* **Historia Azure DevOps:** **US-14 (#1034)** — *Consulta de reservas por día seleccionado y recuperación por ID*.
* **Casos de Prueba Test Plan:** **TC-01 (#1046)**, **TC-02 (#1047)**.
* **Endpoints Probados:** `GET /Reserva/ObtenerPorDia?fecha={fecha}` y `GET /Reserva/Detalles/{id}`.
* **Objetivo:** Verificar la recuperación precisa de reservas registradas para un día calendario específico y la consulta puntual por clave primaria, garantizando códigos HTTP semánticos (200 OK vs 404 NotFound).

| Caso | Método de Prueba | Condición Probada | Petición HTTP | Código HTTP | Verificación de Datos | Estado |
|:---:|:---|:---|:---|:---:|:---|:---:|
| **3.1** | `GetObtenerPorDia_ConFechaConReservas_Retorna200OkYListaReservas` | Consulta de día en el que existe reserva activa | `GET /Reserva/ObtenerPorDia?fecha=Hoy+12` | **200 OK** | Lista contiene la reserva semilla 1 del cliente "Carlos Miranda" | 🟢 **Passed** |
| **3.2** | `GetObtenerPorDia_ConFechaSinReservas_Retorna200OkYListaVacia` | Consulta de día sin ninguna reserva programada | `GET /Reserva/ObtenerPorDia?fecha=Hoy+60` | **200 OK** | Retorna colección vacía (`[]`) sin errores en servidor | 🟢 **Passed** |
| **3.3** | `GetDetalles_ConIdExistente_Retorna200OkYReservaCorrespondiente` | Consulta por ID existente en el almacén de datos | `GET /Reserva/Detalles/1` | **200 OK** | Retorna la entidad con `Id = 1`, `NombreCliente = "Carlos Miranda"`, `HabitacionId = 1` | 🟢 **Passed** |
| **3.4** | `GetDetalles_ConIdInexistente_Retorna404NotFound` | Consulta de registro inexistente | `GET /Reserva/Detalles/9999` | **404 NotFound** | Retorna objeto JSON de error sin fallos 500 no controlados | 🟢 **Passed** |

---

### Suite 4: `ReservaCrudIntegrationTests` (4 Pruebas) — Camila Antonia Calderón Cortez
* **Historia Azure DevOps:** **US-SEC-01 (#1043)** — *Ciclo de vida CRUD de reservas y control de colisión de disponibilidad*.
* **Casos de Prueba Test Plan:** **TC-07 (#1052)**, **TC-08 (#1053)**.
* **Endpoints Probados:** `POST /Reserva/Crear` y `POST /Reserva/Cancelar`.
* **Objetivo:** Validar la persistencia de nuevas reservas en la base de datos en memoria, la transición segura de estado a "Cancelada", la prevención de sobreventas/colisiones y la gestión de identificadores inexistentes.

| Caso | Método de Prueba | Condición Probada | Petición HTTP | Código HTTP | Verificación en Almacén InMemory | Estado |
|:---:|:---|:---|:---|:---:|:---|:---:|
| **4.1** | `PostCrearReserva_PersisteEntidadEnBaseDeDatosInMemory` | Registro completo de reserva y persistencia | `POST /Reserva/Crear` (Cliente: Camila Calderón, $450) | **200 OK** | Se consulta `db.Reservas.FindAsync` y se comprueban campos exactos y persistencia efectiva | 🟢 **Passed** |
| **4.2** | `PostCancelarReserva_ActualizaEstadoEnBaseDeDatosInMemory` | Cancelación de reserva activa existente | `POST /Reserva/Cancelar` con `{ id: reservaId }` | **200 OK** | La entidad en BD cambia su campo `Estado` a `"Cancelada"` | 🟢 **Passed** |
| **4.3** | `PostCancelarReserva_ConIdInexistente_Retorna404NotFound` | Intento de cancelar una reserva inexistente | `POST /Reserva/Cancelar` con `{ id: 88888 }` | **404 NotFound** | Retorna 404 informando recurso no encontrado | 🟢 **Passed** |
| **4.4** | `PostCrearReserva_EnMismasFechasQueReservaActiva_RechazaPorColision` | Control anticolisión / intento de sobreventa | `POST /Reserva/Crear` solapando fechas de reserva semilla 1 | **400 BadRequest** | Rechazo preventivo por falta de disponibilidad de la habitación | 🟢 **Passed** |

---

## 4. Configuración de la Infraestructura de Pruebas

### 4.1. Factoría Personalizada (`CustomWebApplicationFactory.cs`)
La infraestructura se implementó en `SistemaReservaHotel.IntegrationTests/Infrastructure/CustomWebApplicationFactory.cs`, heredando de `WebApplicationFactory<Program>`:

* **Aislamiento de Base de Datos:** Se reemplaza el proveedor relacional SQL Server mediante `builder.UseEnvironment("Testing")` y `options.UseInMemoryDatabase(databaseName)`.
* **Inicialización de Datos Semilla:** A través de la sobreescritura de `CreateHost(IHostBuilder builder)`, se asegura que la base de datos en memoria se inicialice de forma atómica y consistente antes de atender peticiones HTTP:
  1. **1 Habitación Activa:** Número 101, Tipo Matrimonial, $75/noche, `Activa = true`.
  2. **1 Usuario:** "Tester QA Integración" (`tester.qa@hotel.com`).
  3. **1 Reserva Activa:** Cliente "Carlos Miranda", habitación 1, abarcando del día `Hoy + 10` al día `Hoy + 15`.

### 4.2. Comandos Oficiales de Ejecución

```powershell
# Ejecución completa de la suite de pruebas de integración
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --logger "console;verbosity=detailed"

# Ejecución por integrante (Filtros independientes)
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ValidadorFechasIntegrationTests" # Ariel
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~DisponibilidadIntegrationTests"  # Joselin
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ConsultasReservaIntegrationTests" # Carlos
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ReservaCrudIntegrationTests"       # Camila
```

---

## 5. Log Oficial de Ejecución de la Consola

```text
Serie de pruebas para C:\Users\carlo\Desktop\ReservasHotel\SistemaReservaHotel-QA\SistemaReservaHotel\SistemaReservaHotel.IntegrationTests\bin\Debug\net10.0\SistemaReservaHotel.IntegrationTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.

  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechaEntradaEnElPasado_Retorna400BadRequest [2 s]
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetDetalles_ConIdInexistente_Retorna404NotFound [2 s]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCancelarReserva_ActualizaEstadoEnBaseDeDatosInMemory [2 s]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechasValidas_Retorna200OkYPersisteReserva [38 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetObtenerPorDia_ConFechaSinReservas_Retorna200OkYListaVacia [57 ms]
  Correctas SistemaReservaHotel.IntegrationTests.DisponibilidadIntegrationTests.GetDisponibilidad_EnRangoSinConflicto_Retorna200OkYHabitacionDisponible [2 s]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCancelarReserva_ConIdInexistente_Retorna404NotFound [27 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechaEntradaIgualASalida_Retorna400BadRequest [22 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetDetalles_ConIdExistente_Retorna200OkYReservaCorrespondiente [19 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConAmbasFechasEnElPasado_Retorna400BadRequest [8 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCrearReserva_EnMismasFechasQueReservaActiva_RechazaPorColision [11 ms]
  Correctas SistemaReservaHotel.IntegrationTests.DisponibilidadIntegrationTests.GetDisponibilidad_HabitacionInactiva_NoSeIncluyeEnDisponibles [36 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechaEntradaMayorQueSalida_Retorna400BadRequest [17 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetObtenerPorDia_ConFechaConReservas_Retorna200OkYListaReservas [19 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCrearReserva_PersisteEntidadEnBaseDeDatosInMemory [14 ms]
  Correctas SistemaReservaHotel.IntegrationTests.DisponibilidadIntegrationTests.GetDisponibilidad_EnRangoConReservaActiva_ExcluyeHabitacionOcupada [17 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 16
     Correcto: 16
    Fallidas: 0
    Omitidas: 0
 Tiempo total: 4.1744 Segundos
```

---

## 6. Certificación de Calidad

Este documento certifica que el **Nivel 2 de Pruebas de Integración** se encuentra implementado de manera rigurosa, completa y reproducible en la rama aislada `feature/pruebas-integracion`. Tanto las 21 pruebas unitarias (Nivel 1) como las 16 pruebas de integración (Nivel 2) se ejecutan de forma independiente al 100% de aprobación, validando la estabilidad técnica y la integridad funcional del Sistema de Reserva Hotel.
