# Reporte Individual de Evidencias Unitarias — Camila Antonia Calderón Cortez (`cami calderon`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `main`  
**Integrante:** Camila Antonia Calderón Cortez (`cami calderon` / `Camila Calderón`)  
**Rol en QA:** Tester de Mutación y Persistencia (Operaciones CRUD y Colisión de Disponibilidad)  
**Historia de Usuario (PBI):** **US-SEC-01 (#1043)** — *Ciclo de vida CRUD de reservas y control de colisión de disponibilidad*  
**Casos de Prueba Azure Test Plan:** **TC-03 (#1048)** y **TC-06 (#1051)** (Test Plan #1054 / Suite #1055, con reporte de **Bug #1056**)  
**Casos Unitarios:** **UT-17, UT-18, UT-19, UT-20 y UT-21**  
**Archivo Evaluado:** `SistemaReservaHotel.UnitTests/ReservaPublicaServiceTests.cs`  
**Clase / Métodos Bajo Prueba:** `SistemaReservaHotel.Business.Services.ReservaService` (`ReservarAsync`, `ActualizarAsync`, `EliminarAsync`, `ValidarDisponibilidadAsync`)  
**Dependencia Aislada:** `IReservaRepository` mediante dobles de prueba (*Moq*)  
**Fecha de Ejecución y Validación:** 22 de Octubre de 2026  
**Resultado Global:** **Estado: Aprobado — 5 de 5 Pruebas Unitarias Passed (100% — 0 Failed — 0 Skipped)**  

---

## 1. Comando Oficial Ejecutado

```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ReservaPublicaServiceTests.Reservar_DatosValidos_CreaReserva | FullyQualifiedName~ReservaPublicaServiceTests.Actualizar_EstadoModificado_Exitoso | FullyQualifiedName~ReservaPublicaServiceTests.Eliminar_PorId_Exitoso | FullyQualifiedName~ReservaPublicaServiceTests.Disponibilidad_SinConflicto_RetornaTrue | FullyQualifiedName~ReservaPublicaServiceTests.Disponibilidad_ConConflicto_RetornaFalse" --logger "console;verbosity=normal"
```

---

## 2. Resumen de Ejecución Individual

| Métrica | Valor |
|:---|:---:|
| Total de pruebas ejecutadas | **5** |
| Pruebas superadas (Passed) | **5 (100%)** |
| Pruebas fallidas (Failed) | **0 (0%)** |
| Pruebas omitidas (Skipped) | **0 (0%)** |
| Tiempo total de ejecución | **1.38 s** |
| Framework de Pruebas | **xUnit 2.9.3 (.NET 10.0)** / **Moq 4.20.72** |

---

## 3. Matriz de Casos de Prueba Unitarios Evaluados (AAA)

| Caso | Método de Prueba | Condición Probada | Entrada Simulada | Salida Esperada | Resultado Real | Estado |
|:---:|:---|:---|:---|:---:|:---:|:---:|
| **UT-17** | `Reservar_DatosValidos_CreaReserva` | Creación de reserva con ID generado | `Cliente: "Ana Torres"`, `Hab: 1`, `$150` | `Reserva.Id == 10`, `Nombre == "Ana Torres"` | `Id == 10`, `Persistida` | ✅ **Passed** |
| **UT-18** | `Actualizar_EstadoModificado_Exitoso` | Modificación y cancelación de reserva | `Reserva Id: 5`, `Estado: "Cancelada"` | Llamada única a `ActualizarAsync` en repositorio | `Verify Times.Once` | ✅ **Passed** |
| **UT-19** | `Eliminar_PorId_Exitoso` | Eliminación segura de reserva por clave primaria | `Reserva Id: 7` | Llamada única a `EliminarAsync(7)` en repositorio | `Verify Times.Once` | ✅ **Passed** |
| **UT-20** | `Disponibilidad_SinConflicto_RetornaTrue` | Verificación de fechas sin conflicto ni sobreventa | `HabitacionId: 1`, `Hoy + 2d` a `Hoy + 5d` | `True` (Habitación libre para reservar) | `True` | ✅ **Passed** |
| **UT-21** | `Disponibilidad_ConConflicto_RetornaFalse` | Detección de colisión / solapamiento de fechas | `HabitacionId: 1`, fechas en colisión | `False` (Bloqueo preventivo de sobreventa) | `False` | ✅ **Passed** |

---

## 4. Log Oficial de Consola (Evidencia de Ejecución)

```text
Serie de pruebas para SistemaReservaHotel.UnitTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 10.0.11)
[xUnit.net 00:00:00.19]   Discovering: SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.25]   Discovered:  SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.26]   Starting:    SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.47]   Finished:    SistemaReservaHotel.UnitTests
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.Eliminar_PorId_Exitoso [94 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.Disponibilidad_SinConflicto_RetornaTrue [13 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.Actualizar_EstadoModificado_Exitoso [3 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.Reservar_DatosValidos_CreaReserva [10 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.Disponibilidad_ConConflicto_RetornaFalse [4 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 5
     Correcto: 5 (100%)
     Fallidas: 0
    Omitidas: 0
 Tiempo total: 1.3858 Segundos
```

---

## 5. Declaración de Autonomía y Conformidad
Este reporte certifica formalmente que las **5 pruebas unitarias asignadas a Camila Antonia Calderón Cortez** fueron ejecutadas y validadas con éxito de manera 100% aislada e independiente en `ReservaPublicaServiceTests.cs`, verificando el ciclo de vida CRUD y los mecanismos de anticolisión en la capa de negocio sin depender de servicios externos ni de los otros 3 integrantes, satisfaciendo a cabalidad los criterios de aceptación para la historia **US-SEC-01 (#1043)** y los casos de prueba **TC-03 (#1048)** y **TC-06 (#1051)** en Azure DevOps.
