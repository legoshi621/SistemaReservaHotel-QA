# Reporte Individual de Evidencias — Camila Antonia Calderón Cortez (`cami calderon`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `feature/pruebas-qa` / `main`  
**Integrante:** Camila Antonia Calderón Cortez (`cami calderon` / `Camila Calderón`)  
**Rol en QA:** Tester de Mutación y Persistencia (Operaciones CRUD y Colisión de Disponibilidad)  
**Historia de Usuario (PBI):** **US-SEC-01 (#1043)** — *Ciclo de vida CRUD de reservas y control de colisión de disponibilidad*  
**Casos de Prueba Azure Test Plan:** **TC-07 (#1052)** y **TC-08 (#1053)** (Test Plan #1054 / Suite #1055)  
**Archivo Evaluado:** `SistemaReservaHotel.UnitTests/ReservaPublicaServiceTests.cs`  
**Clase / Métodos Bajo Prueba:** `SistemaReservaHotel.Business.Services.ReservaService` (`ReservarAsync`, `ActualizarAsync`, `EliminarAsync`, `ValidarDisponibilidadAsync`)  
**Dependencia Aislada:** `IReservaRepository` mediante dobles de prueba (*Moq*)  
**Fecha de Ejecución y Validación:** 20 de Octubre de 2026  
**Resultado Global:** **100% SUPERADO (5 de 5 pruebas aprobadas — 0 fallidas — 0 omitidas)**  

---

## 1. Comando Oficial Ejecutado

```powershell
dotnet test SistemaReservaHotel0/SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ReservarAsync|FullyQualifiedName~ActualizarAsync|FullyQualifiedName~EliminarAsync|FullyQualifiedName~ValidarDisponibilidadAsync" --logger "console;verbosity=detailed"
```

---

## 2. Resumen de Ejecución Individual

| Métrica | Valor |
|:---|:---:|
| Total de pruebas ejecutadas | **5** |
| Pruebas superadas (Passed) | **5 (100%)** |
| Pruebas fallidas (Failed) | **0 (0%)** |
| Pruebas omitidas (Skipped) | **0 (0%)** |
| Tiempo total de ejecución | **2.44 s** |
| Framework de Pruebas | **xUnit 2.9.3 (.NET 10.0)** / **Moq 4.20.72** |

---

## 3. Matriz de Casos de Prueba Unitarios Evaluados (AAA)

| # | Método de Prueba | Condición Probada | Entrada Simulada | Salida Esperada | Resultado Real | Estado |
|:---:|:---|:---|:---|:---:|:---:|:---:|
| **1** | `ReservarAsync_CreaReservaYRetornaEntidad` | Creación de reserva con ID generado | `Cliente: "Ana Torres"`, `Hab: 1`, `$150` | `Reserva.Id == 10`, `Nombre == "Ana Torres"` | `Id == 10`, `Persistida` | 🟢 **Passed** |
| **2** | `ActualizarAsync_LlamaMetodoActualizarDelRepositorio` | Modificación y cancelación de reserva | `Reserva Id: 5`, `Estado: "Cancelada"` | Llamada única a `ActualizarAsync` en repositorio | `Verify Times.Once` | 🟢 **Passed** |
| **3** | `EliminarAsync_LlamaMetodoEliminarDelRepositorio` | Eliminación segura de reserva por clave primaria | `Reserva Id: 7` | Llamada única a `EliminarAsync(7)` en repositorio | `Verify Times.Once` | 🟢 **Passed** |
| **4** | `ValidarDisponibilidadAsync_CuandoEstaDisponible_RetornaTrue` | Verificación de fechas sin conflicto ni sobreventa | `HabitacionId: 1`, `Hoy + 2d` a `Hoy + 5d` | `True` (Habitación libre para reservar) | `True` | 🟢 **Passed** |
| **5** | `ValidarDisponibilidadAsync_CuandoNoEstaDisponible_RetornaFalse` | Detección de colisión / solapamiento de fechas | `HabitacionId: 1`, fechas en colisión | `False` (Bloqueo preventivo de sobreventa) | `False` | 🟢 **Passed** |

---

## 4. Log Oficial de Consola (Evidencia de Ejecución)

```text
Serie de pruebas para C:\Users\CLIENTE\Downloads\ReservaHotel1\ReservaHotel1\SistemaReservaHotel0\SistemaReservaHotel0\SistemaReservaHotel\SistemaReservaHotel.UnitTests\bin\Debug\net10.0\SistemaReservaHotel.UnitTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
C:\Users\CLIENTE\Downloads\ReservaHotel1\ReservaHotel1\SistemaReservaHotel0\SistemaReservaHotel0\SistemaReservaHotel\SistemaReservaHotel.UnitTests\bin\Debug\net10.0\SistemaReservaHotel.UnitTests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 10.0.11)
[xUnit.net 00:00:00.26]   Discovering: SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.35]   Discovered:  SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.36]   Starting:    SistemaReservaHotel.UnitTests
[xUnit.net 00:00:01.15]   Finished:    SistemaReservaHotel.UnitTests
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ReservarAsync_CreaReservaYRetornaEntidad [657 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.EliminarAsync_LlamaMetodoEliminarDelRepositorio [5 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ActualizarAsync_LlamaMetodoActualizarDelRepositorio [3 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ValidarDisponibilidadAsync_CuandoNoEstaDisponible_RetornaFalse [8 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ValidarDisponibilidadAsync_CuandoEstaDisponible_RetornaTrue [6 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 5
     Correcto: 5
    Fallidas: 0
    Omitidas: 0
 Tiempo total: 2.4408 Segundos
```

---

## 5. Declaración de Autonomía y Conformidad
Este reporte certifica formalmente que las **5 pruebas unitarias asignadas a Camila Antonia Calderón Cortez** fueron ejecutadas y validadas con éxito de manera 100% aislada e independiente en `ReservaPublicaServiceTests.cs`, verificando el ciclo de vida CRUD y los mecanismos de anticolisión en la capa de negocio sin depender de servicios externos ni de los otros 3 integrantes, satisfaciendo a cabalidad los criterios de aceptación para la historia **US-SEC-01 (#1043)** y los casos de prueba **TC-07 (#1052)** y **TC-08 (#1053)** en Azure DevOps.
