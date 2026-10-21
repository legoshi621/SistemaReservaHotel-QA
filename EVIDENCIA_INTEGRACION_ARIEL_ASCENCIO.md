# Reporte Individual de Evidencias de Integración — Ariel Isai Ascencio Ramos (`Ariel Ascencio`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `feature/pruebas-integracion`  
**Integrante:** Ariel Isai Ascencio Ramos (`Ariel Ascencio` / `Isai Ascencio`)  
**Rol en QA:** Tester de Reglas Cronológicas y Calendario  
**Historia de Usuario (PBI):** **US-16 (#1040)** — *Validación de rango de fechas y reglas cronológicas de reserva*  
**Caso de Prueba Azure Test Plan:** **TC-05 (#1050)** (Test Plan #1054 / Suite #1055)  
**Archivo Evaluado:** `SistemaReservaHotel.IntegrationTests/ValidadorFechasIntegrationTests.cs`  
**Endpoint y Componentes Bajo Prueba:** `POST /Reserva/Crear` (Pipeline HTTP ASP.NET Core MVC, Enrutamiento, ModelBinding, `ValidadorFechas`, Persistencia transaccional en `AppDbContext` InMemory)  
**Infraestructura de Pruebas:** `CustomWebApplicationFactory<Program>` con `Microsoft.EntityFrameworkCore.InMemory`  
**Fecha de Ejecución y Validación:** 21 de Octubre de 2026  
**Resultado Global:** **100% SUPERADO (5 de 5 pruebas aprobadas — 0 fallidas — 0 omitidas)**  

---

## 1. Comando Oficial Ejecutado

```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ValidadorFechasIntegrationTests" --logger "console;verbosity=detailed"
```

---

## 2. Resumen de Ejecución Individual

| Métrica | Valor |
|:---|:---:|
| Total de pruebas de integración ejecutadas | **5** |
| Pruebas superadas (Passed) | **5 (100%)** |
| Pruebas fallidas (Failed) | **0 (0%)** |
| Pruebas omitidas (Skipped) | **0 (0%)** |
| Tiempo total de ejecución | **6.01 s** |
| Framework de Pruebas | **xUnit 2.9.3 (.NET 10.0)** |
| Infraestructura Web | **ASP.NET Core TestHost (`WebApplicationFactory`)** |
| Motor de Persistencia de Prueba | **EF Core InMemory 10.0.13** |
| Librería de Aserciones | **FluentAssertions 8.1.1** |

---

## 3. Matriz de Casos de Prueba de Integración Evaluados (Pipeline HTTP & Persistencia)

| # | Método de Prueba | Condición Probada | Payload Enviado (JSON) | Código HTTP Esperado | Comportamiento y Verificación en BD InMemory | Estado |
|:---:|:---|:---|:---|:---:|:---|:---:|
| **1** | `PostReserva_ConFechasValidas_Retorna200OkYPersisteReserva` | Rango futuro válido (`entrada < salida && entrada >= Hoy`) | `Entrada: Hoy+20`, `Salida: Hoy+25`, `Hab: 1`, `$300` | **200 OK** | Entidad persistida exitosamente en `AppDbContext.Reservas` con `Id > 0` y `Estado = "Activa"`. | 🟢 **Passed** |
| **2** | `PostReserva_ConFechaEntradaMayorQueSalida_Retorna400BadRequest` | Rango temporal invertido (`entrada > salida`) | `Entrada: Hoy+25`, `Salida: Hoy+20`, `Hab: 1`, `$150` | **400 BadRequest** | Petición interceptada por `ValidadorFechas`; 0 registros persistidos en base de datos. | 🟢 **Passed** |
| **3** | `PostReserva_ConFechaEntradaIgualASalida_Retorna400BadRequest` | Estancia de 0 noches (mismo día de entrada y salida) | `Entrada: Hoy+5`, `Salida: Hoy+5`, `Hab: 1`, `$100` | **400 BadRequest** | Petición rechazada preventivamente por regla de negocio; no altera la base de datos. | 🟢 **Passed** |
| **4** | `PostReserva_ConFechaEntradaEnElPasado_Retorna400BadRequest` | Check-in en el pasado con fecha de salida futura | `Entrada: Hoy-2`, `Salida: Hoy+3`, `Hab: 1`, `$200` | **400 BadRequest** | Bloqueo preventivo por validación cronológica en el pipeline HTTP; retorno de error 400. | 🟢 **Passed** |
| **5** | `PostReserva_ConAmbasFechasEnElPasado_Retorna400BadRequest` | Rango de fechas completamente caducado | `Entrada: Hoy-10`, `Salida: Hoy-5`, `Hab: 1`, `$250` | **400 BadRequest** | Bloqueo por fechas caducadas; solicitud denegada antes de procesar persistencia. | 🟢 **Passed** |

---

## 4. Log Oficial de Consola (Evidencia de Ejecución)

```text
Serie de pruebas para C:\Users\isaia\Downloads\ReservaHotel1\SistemaReservaHotel\SistemaReservaHotel.IntegrationTests\bin\Debug\net10.0\SistemaReservaHotel.IntegrationTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
C:\Users\isaia\Downloads\ReservaHotel1\SistemaReservaHotel\SistemaReservaHotel.IntegrationTests\bin\Debug\net10.0\SistemaReservaHotel.IntegrationTests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.4+50e68bbb8b (64-bit .NET 10.0.11)
[xUnit.net 00:00:01.05]   Discovering: SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:01.11]   Discovered:  SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:01.16]   Starting:    SistemaReservaHotel.IntegrationTests
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Testing
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\isaia\Downloads\ReservaHotel1\SistemaReservaHotel\SistemaReservaHotel.WebMVC
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechaEntradaEnElPasado_Retorna400BadRequest [2 s]
[xUnit.net 00:00:04.39]   Finished:    SistemaReservaHotel.IntegrationTests
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechasValidas_Retorna200OkYPersisteReserva [139 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechaEntradaIgualASalida_Retorna400BadRequest [9 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConAmbasFechasEnElPasado_Retorna400BadRequest [3 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechaEntradaMayorQueSalida_Retorna400BadRequest [3 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 5
     Correcto: 5
    Fallidas: 0
    Omitidas: 0
 Tiempo total: 6,0173 Segundos
```

---

## 5. Declaración de Autonomía y Conformidad QA

Este reporte formal certifica que las **5 pruebas de integración asignadas a Ariel Isai Ascencio Ramos** fueron ejecutadas y aprobadas al **100% en verde** de manera completamente aislada e independiente en `ValidadorFechasIntegrationTests.cs`, evaluando de extremo a extremo el endpoint `POST /Reserva/Crear` dentro del pipeline HTTP de ASP.NET Core con persistencia en `EF Core InMemory`.

Se corroboró de forma rigurosa que:
1. El sistema rechaza preventivamente con código **HTTP 400 (BadRequest)** cualquier rango temporal incoherente (fechas invertidas, estancias de 0 días o fechas en el pasado).
2. El sistema autoriza y procesa con código **HTTP 200 (OK)** las reservas con fechas válidas, persistiendo de manera atómica y consistente el registro en base de datos.
3. Se cumplen a cabalidad todos los criterios de aceptación y especificaciones de calidad para la Historia de Usuario **US-16 (#1040)** y el Caso de Prueba **TC-05 (#1050)** registrados en **Azure DevOps**.
