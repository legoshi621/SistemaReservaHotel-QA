# EVIDENCIA DE PRUEBAS DE INTEGRACIÓN Y API
## Sistema de Reserva de Hotel — Módulo: Validación de Fechas y Reglas Cronológicas

---

| Campo                  | Detalle                                                              |
|------------------------|----------------------------------------------------------------------|
| **Autor**              | Ariel Isai Ascencio Ramos (`Isai Ascencio`)                         |
| **Fecha de ejecución** | 2026-10-22                                                           |
| **Hora**               | 11:39 (UTC-6)                                                        |
| **Archivo de pruebas** | `SistemaReservaHotel.IntegrationTests/ValidadorFechasIntegrationTests.cs` |
| **Framework**          | xUnit 2.9.3 · FluentAssertions · ASP.NET Core TestServer (.NET 10.0) |
| **BD de prueba**       | InMemory (EF Core) — instancia aislada por GUID                      |
| **Historia de usuario**| US-16 · Work Item #1040 — Validación de reglas cronológicas           |
| **Rama**               | `main`                                                               |
| **Repositorio**        | `SistemaReservaHotel-QA` — Azure DevOps `cc25003`                    |
| **Estado global**      | ✅ APROBADO — 5/5 casos de API / Integración Passed (100%)            |

---

## 1. Contexto y Alcance

Las presentes pruebas de integración validan el comportamiento end-to-end del endpoint `POST /Reserva/Crear` expuesto por el controlador `ReservaController`, asegurando que se apliquen estrictamente las reglas cronológicas de negocio:
- Aceptación y persistencia de reservas con rangos de fechas futuras válidos.
- Bloqueo con HTTP `400 Bad Request` ante rangos invertidos (`FechaEntrada > FechaSalida`).
- Bloqueo con HTTP `400 Bad Request` ante estancias de 0 noches (`FechaEntrada == FechaSalida`).
- Bloqueo con HTTP `400 Bad Request` ante check-in en fechas pasadas.
- Bloqueo con HTTP `400 Bad Request` ante rangos completamente caducados.

La infraestructura utiliza `CustomWebApplicationFactory` con una base de datos **InMemory** aislada por GUID y semilla de datos determinista.

---

## 2. Comando Oficial Ejecutado

```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ValidadorFechasIntegrationTests" --logger "console;verbosity=detailed"
```

---

## 3. Matriz de Casos de Integración y API Evaluados

| Caso | Método de Prueba | Condición Probada | Payload / Fechas | Código HTTP Esperado | Código Obtenido | Estado |
|:---:|:---|:---|:---|:---:|:---:|:---:|
| **API-01** | `PostReserva_ConFechasValidas_Retorna200OkYPersisteReserva` | Rango futuro válido y persistencia en DB | `Hoy + 20d` a `Hoy + 25d` | `200 OK` | `200 OK` | ✅ **Passed** |
| **API-02** | `PostReserva_ConFechaEntradaMayorQueSalida_Retorna400BadRequest` | Rango cronológico invertido | `Hoy + 25d` a `Hoy + 20d` | `400 BadRequest` | `400 BadRequest` | ✅ **Passed** |
| **API-03** | `PostReserva_ConFechaEntradaIgualASalida_Retorna400BadRequest` | Estancia nula (0 noches) | `Hoy + 5d` a `Hoy + 5d` | `400 BadRequest` | `400 BadRequest` | ✅ **Passed** |
| **API-04** | `PostReserva_ConFechaEntradaEnElPasado_Retorna400BadRequest` | Check-in extemporáneo en el pasado | `Hoy - 2d` a `Hoy + 3d` | `400 BadRequest` | `400 BadRequest` | ✅ **Passed** |
| **API-05** | `PostReserva_ConAmbasFechasEnElPasado_Retorna400BadRequest` | Rango de fechas caducado | `Hoy - 10d` a `Hoy - 5d` | `400 BadRequest` | `400 BadRequest` | ✅ **Passed** |

---

## 4. Evidencia Oficial de Salida de Consola

```text
Serie de pruebas para C:\Users\isaia\Downloads\ReservaHotel1\SistemaReservaHotel\SistemaReservaHotel.IntegrationTests\bin\Debug\net10.0\SistemaReservaHotel.IntegrationTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.4+50e68bbb8b (64-bit .NET 10.0.11)
[xUnit.net 00:00:01.17]   Discovering: SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:01.24]   Discovered:  SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:01.30]   Starting:    SistemaReservaHotel.IntegrationTests
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechaEntradaEnElPasado_Retorna400BadRequest [3 s]
[xUnit.net 00:00:05.16]   Finished:    SistemaReservaHotel.IntegrationTests
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechasValidas_Retorna200OkYPersisteReserva [158 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechaEntradaIgualASalida_Retorna400BadRequest [10 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConAmbasFechasEnElPasado_Retorna400BadRequest [5 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ValidadorFechasIntegrationTests.PostReserva_ConFechaEntradaMayorQueSalida_Retorna400BadRequest [5 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 5
     Correcto: 5
     Fallidas: 0
    Omitidas: 0
 Tiempo total: 6,9995 Segundos
```

---

## 5. Resumen de Ejecución

| Métrica | Valor |
|:---|:---:|
| Total de pruebas de integración | **5** |
| Pruebas superadas (Passed) | **5 (100%)** |
| Pruebas fallidas (Failed) | **0 (0%)** |
| Pruebas omitidas (Skipped) | **0 (0%)** |
| Tiempo total de ejecución | **7.00 s** |
| Framework de Pruebas | **xUnit 2.9.3 / ASP.NET Core TestHost (.NET 10.0)** |

---

## 6. Firmas

| Rol | Nombre | Fecha |
|---|---|---|
| QA Tester | Ariel Isai Ascencio Ramos | 2026-10-22 |
| Automatización QA | Antigravity AI | 2026-10-22 |
