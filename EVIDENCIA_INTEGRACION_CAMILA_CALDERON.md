# Reporte Individual de Pruebas de Integración (Nivel 2) — Camila Antonia Calderón Cortez (`cami calderon`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `main`  
**Integrante:** Camila Antonia Calderón Cortez (`cami calderon` / `Camila Calderón`)  
**Rol en QA:** Tester de Mutación y Persistencia (Ciclo CRUD y Control de Colisión)  
**Historia de Usuario (PBI):** **US-SEC-01 (#1043)** — *Ciclo de vida CRUD de reservas y control de colisión de disponibilidad*  
**Casos de Prueba Azure Test Plan:** **TC-07 (#1052)** y **TC-08 (#1053)** (Test Plan #1054 / Suite #1055)  
**Archivo Evaluado:** `SistemaReservaHotel.IntegrationTests/ReservaCrudIntegrationTests.cs`  
**Pipeline HTTP & Componentes:** `CustomWebApplicationFactory` (ASP.NET Core TestHost) ➔ `ReservaController` ➔ `ReservaService` ➔ `ReservaRepository` ➔ `AppDbContext` (EF Core InMemory)  
**Fecha de Ejecución y Validación:** 21 de Octubre de 2026  
**Resultado Global:** **100% SUPERADO (4 de 4 pruebas aprobadas — 0 fallidas — 0 omitidas)**  

---

## 1. Comando Oficial Ejecutado

```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ReservaCrudIntegrationTests" --logger "console;verbosity=detailed"
```

---

## 2. Resumen de Ejecución Individual

| Métrica | Valor |
|:---|:---:|
| Total de pruebas de integración | **4** |
| Pruebas superadas (Passed) | **4 (100%)** |
| Pruebas fallidas (Failed) | **0 (0%)** |
| Pruebas omitidas (Skipped) | **0 (0%)** |
| Tiempo total de ejecución | **5.58 s** |
| Nivel de Prueba | **Nivel 2 — Integración (Servicios, Controladores y Persistencia)** |
| Frameworks y Librerías | **.NET 10.0** / **xUnit 2.9.3** / **Microsoft.AspNetCore.Mvc.Testing** / **EF Core InMemory 10.0.13** / **FluentAssertions** |

---

## 3. Matriz de Casos de Integración Evaluados (Patrón AAA)

| # | Método de Prueba | Endpoint / Flujo HTTP | Condición y Datos Simulados | Salida Esperada | Comprobación en Base de Datos Real (InMemory) | Estado |
|:---:|:---|:---|:---|:---|:---|:---:|
| **1** | `PostCrearReserva_PersisteEntidadEnBaseDeDatosInMemory` | `POST /Reserva/Crear` | Cliente: *"Camila Calderon Test"*, Habitación: 1, Días +30 a +35, $450 | `200 OK`, JSON con `Id > 0` | `AppDbContext.Reservas.FindAsync(Id)` existe, estado *"Activa"*, precio $450 | 🟢 **Passed** |
| **2** | `PostCancelarReserva_ActualizaEstadoEnBaseDeDatosInMemory` | `POST /Reserva/Cancelar` | Id generado de reserva activa previa, `CancelarReservaDto { Id }` | `200 OK` | `AppDbContext.Reservas.FindAsync(Id)` cambia de estado a *"Cancelada"* | 🟢 **Passed** |
| **3** | `PostCancelarReserva_ConIdInexistente_Retorna404NotFound` | `POST /Reserva/Cancelar` | `CancelarReservaDto { Id = 88888 }` (Identificador inexistente) | `404 NotFound` | No altera ningún registro en `AppDbContext` | 🟢 **Passed** |
| **4** | `PostCrearReserva_EnMismasFechasQueReservaActiva_RechazaPorColision` | `POST /Reserva/Crear` | Habitación: 1, Fechas colisionan con reserva semilla (Días +11 a +14) | `400 BadRequest` | Bloqueo preventivo de sobreventa; la colisión no se inserta en BD | 🟢 **Passed** |

---

## 4. Log Oficial de Consola (Evidencia de Ejecución)

```text
Serie de pruebas para SistemaReservaHotel.IntegrationTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
[xUnit.net 00:00:00.01] xUnit.net VSTest Adapter v3.1.4+50e68bbb8b (64-bit .NET 10.0.11)
[xUnit.net 00:00:00.44]   Discovering: SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:00.53]   Discovered:  SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:00.61]   Starting:    SistemaReservaHotel.IntegrationTests
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 18 entities to in-memory store.
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 35 entities to in-memory store.
info: Microsoft.Hosting.Lifetime[0]
      Application started. Hosting environment: Testing
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCancelarReserva_ActualizaEstadoEnBaseDeDatosInMemory [3 s]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCancelarReserva_ConIdInexistente_Retorna404NotFound [22 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCrearReserva_EnMismasFechasQueReservaActiva_RechazaPorColision [8 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCrearReserva_PersisteEntidadEnBaseDeDatosInMemory [14 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 4
     Correcto: 4
     Fallidas: 0
    Omitidas: 0
 Tiempo total: 5.5811 Segundos
```

---

## 5. Declaración de Autonomía y Conformidad

Este reporte certifica formalmente que las **4 pruebas de integración asignadas a Camila Antonia Calderón Cortez** en `ReservaCrudIntegrationTests.cs` fueron ejecutadas y validadas con éxito al 100%. 

A diferencia de las pruebas unitarias, esta suite comprueba la **cadena de integración completa de extremo a extremo**:
1. Deserialización de peticiones HTTP en `ReservaController`.
2. Ejecución de las reglas de negocio y cálculo de tarifas en `ReservaService`.
3. Invocación transaccional hacia el repositorio `ReservaRepository`.
4. Persistencia e integridad relacional directa sobre el motor de Entity Framework Core (`AppDbContext`), validando tanto inserciones como transiciones de estado a *"Cancelada"* y rechazos HTTP 400 por colisión de fechas sin dependencias externas ni traslapes con otros miembros del equipo.
