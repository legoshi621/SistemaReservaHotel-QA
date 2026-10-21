# Reporte Individual de Evidencias — Ariel Isai Ascencio Ramos (`Isai Ascencio`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `feature/pruebas-qa` / `main`  
**Integrante:** Ariel Isai Ascencio Ramos (`Isai Ascencio`)  
**Rol en QA:** Tester de Reglas Cronológicas y Calendario  
**Historia de Usuario (PBI):** **US-16 (#1040)** — *Validación de rango de fechas y reglas cronológicas de reserva*  
**Caso de Prueba Azure Test Plan:** **TC-05 (#1050)**  
**Archivo Evaluado:** `SistemaReservaHotel.UnitTests/ValidadorFechasTests.cs`  
**Clase / Método Bajo Prueba:** `SistemaReservaHotel.Business.Helpers.ValidadorFechas.FechasValidas(DateTime entrada, DateTime salida)`  
**Fecha de Ejecución y Validación:** 20 de Octubre de 2026  
**Resultado Global:** **100% SUPERADO (6 de 6 pruebas aprobadas — 0 fallidas — 0 omitidas)**  

---

## 1. Comando Oficial Ejecutado

```powershell
dotnet test SistemaReservaHotel0/SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ValidadorFechasTests" --logger "console;verbosity=detailed"
```

---

## 2. Resumen de Ejecución Individual

| Métrica | Valor |
|:---|:---:|
| Total de pruebas ejecutadas | **6** |
| Pruebas superadas (Passed) | **6 (100%)** |
| Pruebas fallidas (Failed) | **0 (0%)** |
| Pruebas omitidas (Skipped) | **0 (0%)** |
| Tiempo total de ejecución | **3.79 s** |
| Framework de Pruebas | **xUnit 2.9.3 (.NET 10.0)** |

---

## 3. Matriz de Casos de Prueba Unitarios Evaluados

| # | Método de Prueba | Condición Probada | Entrada Simula | Salida Esperada | Resultado Real | Estado |
|:---:|:---|:---|:---|:---:|:---:|:---:|
| **1** | `FechasValidas_EntradaFuturaYMenorQueSalida_RetornaTrue` | Rango normal de fechas futuras válidas | `Hoy + 2d` a `Hoy + 5d` | `True` | `True` | 🟢 **Passed** |
| **2** | `FechasValidas_EntradaHoyYSalidaManana_RetornaTrue` | Check-in inmediato hoy con estancia de 1 noche | `Hoy` a `Hoy + 1d` | `True` | `True` | 🟢 **Passed** |
| **3** | `FechasValidas_EntradaMayorQueSalida_RetornaFalse` | Fecha de entrada posterior a la de salida (invertidas) | `Hoy + 5d` a `Hoy + 2d` | `False` | `False` | 🟢 **Passed** |
| **4** | `FechasValidas_EntradaIgualASalida_RetornaFalse` | Check-in y check-out el mismo día (estancia 0 noches) | `Hoy + 2d` a `Hoy + 2d` | `False` | `False` | 🟢 **Passed** |
| **5** | `FechasValidas_EntradaEnElPasado_RetornaFalse` | Check-in en fecha pasada con salida futura | `Hoy - 1d` a `Hoy + 2d` | `False` | `False` | 🟢 **Passed** |
| **6** | `FechasValidas_EntradaYSalidaEnElPasado_RetornaFalse` | Ambas fechas en el pasado (rango caducado) | `Hoy - 5d` a `Hoy - 2d` | `False` | `False` | 🟢 **Passed** |

---

## 4. Log Oficial de Consola (Evidencia de Ejecución)

```text
Serie de pruebas para C:\Users\isaia\Downloads\ReservaHotel1\SistemaReservaHotel0\SistemaReservaHotel\SistemaReservaHotel.UnitTests\bin\Debug\net10.0\SistemaReservaHotel.UnitTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.4+50e68bbb8b (64-bit .NET 10.0.11)
[xUnit.net 00:00:00.81]   Discovering: SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.88]   Discovered:  SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.94]   Starting:    SistemaReservaHotel.UnitTests
[xUnit.net 00:00:01.53]   Finished:    SistemaReservaHotel.UnitTests
  Correctas SistemaReservaHotel.UnitTests.ValidadorFechasTests.FechasValidas_EntradaYSalidaEnElPasado_RetornaFalse [494 ms]
  Correctas SistemaReservaHotel.UnitTests.ValidadorFechasTests.FechasValidas_EntradaHoyYSalidaManana_RetornaTrue [< 1 ms]
  Correctas SistemaReservaHotel.UnitTests.ValidadorFechasTests.FechasValidas_EntradaIgualASalida_RetornaFalse [< 1 ms]
  Correctas SistemaReservaHotel.UnitTests.ValidadorFechasTests.FechasValidas_EntradaFuturaYMenorQueSalida_RetornaTrue [< 1 ms]
  Correctas SistemaReservaHotel.UnitTests.ValidadorFechasTests.FechasValidas_EntradaMayorQueSalida_RetornaFalse [< 1 ms]
  Correctas SistemaReservaHotel.UnitTests.ValidadorFechasTests.FechasValidas_EntradaEnElPasado_RetornaFalse [< 1 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 6
     Correcto: 6
     Fallidas: 0
    Omitidas: 0
 Tiempo total: 3,7916 Segundos
```

---

## 5. Declaración de Autonomía
Este reporte evidencia que las **6 pruebas unitarias asignadas a Ariel Isai Ascencio Ramos** fueron ejecutadas y validadas con éxito sin interferir con las pruebas de los otros 3 integrantes, cumpliendo el 100% de los criterios de aceptación para la historia **US-16 (#1040)** y el caso de prueba **TC-05 (#1050)** en Azure DevOps.
