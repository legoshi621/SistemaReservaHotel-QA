# Reporte Individual de Evidencias — Carlos Alonso Miranda Rivas (`Alonso Rivas`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `feature/pruebas-qa` / `main`  
**Integrante:** Carlos Alonso Miranda Rivas (`Alonso Rivas` / `Carlos Alonso Miranda`)  
**Rol en QA:** Tester de Consultas y Lectura (Consulta de Reservas por Día y por ID)  
**Historia de Usuario (PBI):** **US-14 (#1034)** — *Consulta de reservas por día seleccionado y recuperación por ID*  
**Casos de Prueba Azure Test Plan:** **TC-01 (#1046)** y **TC-02 (#1047)** (Test Plan #1054 / Suite #1055)  
**Archivo Evaluado:** `SistemaReservaHotel.UnitTests/ReservaPublicaServiceTests.cs`  
**Clase / Métodos Bajo Prueba:** `SistemaReservaHotel.Business.Services.ReservaService` (`ObtenerPorDiaAsync`, `ObtenerTodasAsync`, `ObtenerPorIdAsync`)  
**Dependencia Aislada:** `IReservaRepository` mediante dobles de prueba (*Moq*)  
**Fecha de Ejecución y Validación:** 20 de Octubre de 2026  
**Resultado Global:** **100% SUPERADO (4 de 4 pruebas aprobadas — 0 fallidas — 0 omitidas)**  

---

## 1. Comando Oficial Ejecutado

```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ObtenerPorDiaAsync|FullyQualifiedName~ObtenerTodasAsync|FullyQualifiedName~ObtenerPorIdAsync" --logger "console;verbosity=detailed"
```

---

## 2. Resumen de Ejecución Individual

| Métrica | Valor |
|:---|:---:|
| Total de pruebas ejecutadas | **4** |
| Pruebas superadas (Passed) | **4 (100%)** |
| Pruebas fallidas (Failed) | **0 (0%)** |
| Pruebas omitidas (Skipped) | **0 (0%)** |
| Tiempo total de ejecución | **4.61 s** |
| Framework de Pruebas | **xUnit 2.9.3 (.NET 10.0)** / **Moq 4.20.72** |

---

## 3. Matriz de Casos de Prueba Unitarios Evaluados (AAA)

| # | Método de Prueba | Condición Probada | Entrada Simulada | Salida Esperada | Resultado Real | Estado |
|:---:|:---|:---|:---|:---:|:---:|:---:|
| **1** | `ObtenerPorDiaAsync_RetornaReservasDelDiaIndicado` | Recuperación de reservas activas para la fecha especificada (US-14) | `fecha = DateTime.Today`, 2 reservas asignadas a hoy | Lista con 2 reservas (`Count == 2`) | Lista con 2 reservas (`Verify Times.Once`) | 🟢 **Passed** |
| **2** | `ObtenerTodasAsync_RetornaListaDeReservas` | Recuperación del catálogo general completo de reservas | Colección simulada de 2 reservas ("Juan Perez", "Maria Gomez") | Lista de 2 elementos, primer cliente "Juan Perez" | 2 elementos recuperados íntegros | 🟢 **Passed** |
| **3** | `ObtenerPorIdAsync_IdExistente_RetornaReserva` | Consulta individual por clave primaria existente | `Id = 1`, cliente "Carlos Lopez" | Objeto `Reserva` con `Id == 1` y cliente coincidente | Entidad retornada con datos exactos | 🟢 **Passed** |
| **4** | `ObtenerPorIdAsync_IdInexistente_RetornaNull` | Manejo seguro y resiliente ante ID inexistente en repositorio | `Id = 999` (registro inexistente) | `null` sin disparar excepciones no controladas | `null` verificado de forma controlada | 🟢 **Passed** |

---

## 4. Log Oficial de Consola (Evidencia de Ejecución)

```text
Serie de pruebas para C:\Users\carlo\Desktop\ReservasHotel\SistemaReservaHotel-QA\SistemaReservaHotel\SistemaReservaHotel.UnitTests\bin\Debug\net10.0\SistemaReservaHotel.UnitTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
C:\Users\carlo\Desktop\ReservasHotel\SistemaReservaHotel-QA\SistemaReservaHotel\SistemaReservaHotel.UnitTests\bin\Debug\net10.0\SistemaReservaHotel.UnitTests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 10.0.12)
[xUnit.net 00:00:01.19]   Discovering: SistemaReservaHotel.UnitTests
[xUnit.net 00:00:01.27]   Discovered:  SistemaReservaHotel.UnitTests
[xUnit.net 00:00:01.28]   Starting:    SistemaReservaHotel.UnitTests
[xUnit.net 00:00:02.73]   Finished:    SistemaReservaHotel.UnitTests
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ObtenerPorIdAsync_IdInexistente_RetornaNull [1 s]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ObtenerPorIdAsync_IdExistente_RetornaReserva [8 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ObtenerPorDiaAsync_RetornaReservasDelDiaIndicado [12 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ObtenerTodasAsync_RetornaListaDeReservas [2 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 4
     Correcto: 4
    Fallidas: 0
    Omitidas: 0
 Tiempo total: 4.6170 Segundos
```

---

## 5. Declaración de Autonomía y Conformidad
Este reporte certifica formalmente que las **4 pruebas unitarias asignadas a Carlos Alonso Miranda Rivas** fueron ejecutadas y validadas con éxito de manera 100% aislada e independiente en `ReservaPublicaServiceTests.cs`, verificando la consulta de reservas por día, recuperación por ID y listado general en la capa de negocio sin depender de base de datos externa ni de los otros 3 integrantes, satisfaciendo a cabalidad los criterios de aceptación para la historia **US-14 (#1034)** y los casos de prueba **TC-01 (#1046)** y **TC-02 (#1047)** en Azure DevOps.
