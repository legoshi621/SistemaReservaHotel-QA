# Reporte Individual de Evidencias de Integración — Carlos Alonso Miranda Rivas (`Alonso Rivas`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `feature/pruebas-integracion`  
**Integrante:** Carlos Alonso Miranda Rivas (`Alonso Rivas` / `Carlos Alonso Miranda`)  
**Rol en QA:** Tester de Consultas y Lectura (Recuperación de Reservas por Día y por ID)  
**Historia de Usuario (PBI):** **US-14 (#1034)** — *Consulta de reservas por día seleccionado y recuperación por ID*  
**Casos de Prueba Azure Test Plan:** **TC-01 (#1046)** y **TC-02 (#1047)** (Test Plan #1054 / Suite #1055)  
**Archivo Evaluado:** `SistemaReservaHotel.IntegrationTests/ConsultasReservaIntegrationTests.cs`  
**Endpoint y Pipeline HTTP Bajo Prueba:** `GET /Reserva/ObtenerPorDia` · `GET /Reserva/Detalles/{id}` — Pipeline completo: ASP.NET Core TestHost ➔ Enrutamiento MVC ➔ `ReservaController` ➔ `ReservaService` ➔ `ReservaRepository` ➔ `AppDbContext` (EF Core InMemory)  
**Infraestructura de Pruebas:** `CustomWebApplicationFactory<Program>` con `Microsoft.EntityFrameworkCore.InMemory` · Datos semilla: Reserva Id=1 (`Carlos Miranda`, Habitación 1, Días +10 a +15)  
**Fecha de Ejecución y Validación:** 21 de Octubre de 2026  
**Resultado Global:** **Estado: Aprobado — 4 de 4 Pruebas de Integración Passed (0 Failed — 0 Skipped)**  

---

## 1. Comando Oficial Ejecutado

```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ConsultasReservaIntegrationTests" --logger "console;verbosity=detailed"
```

---

## 2. Resumen de Ejecución Individual

| Métrica | Valor |
|:---|:---:|
| Total de Pruebas de Integración ejecutadas | **4** |
| Pruebas superadas (Passed) | **4 (100%)** |
| Pruebas fallidas (Failed) | **0 (0%)** |
| Pruebas omitidas (Skipped) | **0 (0%)** |
| Tiempo total de ejecución | **7.32 s** |
| Nivel de Prueba | **Nivel 2 — Integración (Controladores, Servicios y Persistencia)** |
| Framework de Pruebas | **xUnit 2.9.3 / xUnit VSTest Adapter v3.1.4** |
| Infraestructura Web | **ASP.NET Core TestHost (`WebApplicationFactory<Program>`)** |
| Motor de Persistencia de Prueba | **EF Core InMemory 10.0.13** |
| Librería de Aserciones | **FluentAssertions 8.11.0** |
| Runtime | **.NET 10.0.12 (64-bit)** |

---

## 3. Matriz de Casos de Prueba de Integración Evaluados (Patrón AAA)

| # | Método de Prueba | Endpoint HTTP | Condición y Datos de Prueba | Código HTTP Esperado | Verificación de Respuesta | Estado |
|:---:|:---|:---|:---|:---:|:---|:---:|
| **1** | `GetObtenerPorDia_ConFechaConReservas_Retorna200OkYListaReservas` | `GET /Reserva/ObtenerPorDia?fecha={Hoy+12}` | Fecha dentro del rango de la reserva semilla (Id=1, Días +10 a +15); US-14 #1034 | **200 OK** | Lista JSON no nula · Contiene reserva con `Id == 1` confirmada por FluentAssertions | ✅ **Passed** |
| **2** | `GetObtenerPorDia_ConFechaSinReservas_Retorna200OkYListaVacia` | `GET /Reserva/ObtenerPorDia?fecha={Hoy+60}` | Fecha futura lejana sin reservas registradas en `AppDbContext`; US-14 #1034 | **200 OK** | Lista JSON no nula y vacía (`BeEmpty()`) — repositorio no arroja error ante consulta sin resultados | ✅ **Passed** |
| **3** | `GetDetalles_ConIdExistente_Retorna200OkYReservaCorrespondiente` | `GET /Reserva/Detalles/1` | Reserva semilla existente: `Id=1`, `NombreCliente="Carlos Miranda"`, `HabitacionId=1`; US-14 #1034 | **200 OK** | JSON deserializado: `Id == 1`, `NombreCliente == "Carlos Miranda"`, `HabitacionId == 1` — integridad de datos verificada | ✅ **Passed** |
| **4** | `GetDetalles_ConIdInexistente_Retorna404NotFound` | `GET /Reserva/Detalles/9999` | Identificador `9999` inexistente en base de datos InMemory; US-14 #1034 | **404 NotFound** | Respuesta de error controlada; sin excepciones no controladas en el pipeline HTTP | ✅ **Passed** |

---

## 4. Log Oficial de Consola (Evidencia de Ejecución)

```text
Serie de pruebas para SistemaReservaHotel.IntegrationTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
C:\Users\carlo\Desktop\ReservasHotel\SistemaReservaHotel-QA\SistemaReservaHotel\SistemaReservaHotel.IntegrationTests\bin\Debug\net10.0\SistemaReservaHotel.IntegrationTests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.4+50e68bbb8b (64-bit .NET 10.0.12)
[xUnit.net 00:00:01.17]   Discovering: SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:01.28]   Discovered:  SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:01.35]   Starting:    SistemaReservaHotel.IntegrationTests
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 0 entities to in-memory store.
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 18 entities to in-memory store.
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 5 entities to in-memory store.
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 35 entities to in-memory store.
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 1 entities to in-memory store.
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 1 entities to in-memory store.
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Testing
info: Microsoft.Hosting.Lifetime[0]
      Content root path: ...\SistemaReservaHotel.WebMVC
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 2 entities to in-memory store.
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetDetalles_ConIdInexistente_Retorna404NotFound [3 s]
[xUnit.net 00:00:05.38]   Finished:    SistemaReservaHotel.IntegrationTests
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetObtenerPorDia_ConFechaSinReservas_Retorna200OkYListaVacia [137 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetDetalles_ConIdExistente_Retorna200OkYReservaCorrespondiente [41 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetObtenerPorDia_ConFechaConReservas_Retorna200OkYListaReservas [13 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 4
     Correcto: 4
 Tiempo total: 7.3157 Segundos
```

---

## 5. Declaración de Autonomía y Conformidad QA

Este reporte certifica formalmente que las **4 pruebas de integración asignadas a Carlos Alonso Miranda Rivas** en `ConsultasReservaIntegrationTests.cs` fueron ejecutadas y validadas con éxito al 100% de manera completamente aislada e independiente, sin interferir con las suites de los demás integrantes del equipo.

A diferencia de las pruebas unitarias, esta suite comprueba la **cadena de integración completa de extremo a extremo**:

1. Recepción y enrutamiento de peticiones `HTTP GET` en `ReservaController` mediante el pipeline ASP.NET Core MVC.
2. Delegación y ejecución de la lógica de consulta en `ReservaService` (`ObtenerPorDiaAsync`, `ObtenerTodasAsync`, `ObtenerPorIdAsync`).
3. Acceso y recuperación de datos en `ReservaRepository` sobre `AppDbContext` de Entity Framework Core.
4. Serialización de la respuesta JSON y validación de los códigos HTTP (`200 OK` y `404 NotFound`).

Se verificó de forma rigurosa que el sistema:
- Retorna **HTTP 200 (OK)** con la lista correcta de reservas al consultar una fecha con resultados disponibles.
- Retorna **HTTP 200 (OK)** con lista vacía al consultar una fecha sin reservas, sin disparar excepciones.
- Retorna **HTTP 200 (OK)** con la entidad completa y datos exactos al consultar una reserva existente por ID.
- Retorna **HTTP 404 (NotFound)** de forma controlada al consultar un identificador inexistente en la base de datos.

Cumpliendo a cabalidad todos los criterios de aceptación para la Historia de Usuario **US-14 (#1034)** y los Casos de Prueba **TC-01 (#1046)** y **TC-02 (#1047)** registrados en **Azure DevOps**.
