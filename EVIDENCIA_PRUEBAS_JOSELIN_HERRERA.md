# Reporte Individual de Evidencias — Joselin Esmeralda Herrera Flores (`Joselin Herrera`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `feature/pruebas-qa` / `main`  
**Integrante:** Joselin Esmeralda Herrera Flores (`Joselin Herrera`)  
**Rol en QA:** Analista de Inventario (Disponibilidad de Habitaciones por Fechas y Estado)  
**Historia de Usuario (PBI):** **US-15 (#1037)** — *Disponibilidad de habitaciones por rango de fechas y estado de inventario*  
**Casos de Prueba Azure Test Plan:** **TC-03 (#1048)** y **TC-04 (#1049)** (Test Plan #1054 / Suite #1055)  
**Archivo Evaluado:** `SistemaReservaHotel.UnitTests/DisponibilidadServiceTests.cs`  
**Clase / Métodos Bajo Prueba:** `SistemaReservaHotel.Business.Services.DisponibilidadService` (`ObtenerDisponiblesAsync`)  
**Dependencias Aisladas:** `IHabitacionRepository` e `IReservaRepository` mediante dobles de prueba (*Moq*)  
**Fecha de Ejecución y Validación:** 21 de Octubre de 2026  
**Resultado Global:** **100% SUPERADO (6 de 6 pruebas aprobadas — 0 fallidas — 0 omitidas)**  

---

## 1. Comando Oficial Ejecutado

```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~DisponibilidadServiceTests" --logger "trx;LogFileName=joselin_herrera_results.trx" --verbosity normal
```

---

## 2. Resumen de Ejecución Individual

| Métrica | Valor |
|:---|:---:|
| Total de pruebas ejecutadas | **6** |
| Pruebas superadas (Passed) | **6 (100%)** |
| Pruebas fallidas (Failed) | **0 (0%)** |
| Pruebas omitidas (Skipped) | **0 (0%)** |
| Tiempo total de ejecución | **3.24 s** |
| Framework de Pruebas | **xUnit 2.9.3 (.NET 8.0)** / **Moq 4.20.72** |

---

## 3. Matriz de Casos de Prueba Unitarios Evaluados (AAA)

| # | Método de Prueba | Condición Probada | Entrada Simulada | Salida Esperada | Resultado Real | Estado |
|:---:|:---|:---|:---|:---:|:---:|:---:|
| **1** | `ObtenerDisponiblesAsync_SinReservas_RetornaTodasLasHabitacionesActivas` | Hotel vacío sin reservas: 100% de disponibilidad para habitaciones activas (TC-03) | 2 habitaciones activas, 0 reservas, rango futuro (hoy+1 a hoy+4) | Lista con 2 habitaciones (`Count == 2`) | Lista con 2 habitaciones verificadas por `Id` | 🟢 **Passed** |
| **2** | `ObtenerDisponiblesAsync_HabitacionInactiva_NoSeIncluyeEnDisponibles` | Habitaciones de baja/inactivas quedan excluidas del inventario disponible (TC-03) | 1 activa (Id=1) + 1 inactiva (Id=2, `Activa=false`), 0 reservas | Lista con 1 habitación (solo Id=1) | 1 habitación retornada, Id=1 exacto | 🟢 **Passed** |
| **3** | `ObtenerDisponiblesAsync_ConReservaActivaEnConflicto_ExcluyeHabitacionOcupada` | Habitación con reserva activa en conflicto de fechas es excluida correctamente (TC-04) | 2 habitaciones activas, reserva activa en Id=1 (hoy+2 a hoy+5), consulta hoy+3 a hoy+6 | Lista con 1 habitación (solo Id=2) | Id=2 retornado, Id=1 excluido por conflicto | 🟢 **Passed** |
| **4** | `ObtenerDisponiblesAsync_ConReservaCanceladaEnMismoRango_NoExcluyeHabitacion` | Reserva cancelada no bloquea disponibilidad; la habitación queda libre (TC-04) | 1 habitación activa (Id=1), reserva cancelada (`Estado="Cancelada"`) en mismo rango | Lista con 1 habitación (Id=1 disponible) | Id=1 retornado correctamente, cancelación no bloquea | 🟢 **Passed** |
| **5** | `ObtenerDisponiblesAsync_ReservaFinalizaElMismoDiaDeEntrada_HabitacionDisponible` | Check-out coincide con check-in solicitado: la habitación queda disponible (TC-04) | 1 habitación, reserva activa (hoy+1 a hoy+4), consulta entrada=hoy+4 | Lista con 1 habitación (Id=1) | Habitación disponible, sin solapamiento de fechas | 🟢 **Passed** |
| **6** | `ObtenerDisponiblesAsync_ReservaIniciaElMismoDiaDeSalida_HabitacionDisponible` | Check-out del huésped actual coincide con inicio de siguiente reserva: permitido (TC-04) | 1 habitación, reserva activa (hoy+5 a hoy+8), consulta salida=hoy+5 | Lista con 1 habitación (Id=1) | Habitación disponible, rangos contiguos sin conflicto | 🟢 **Passed** |

---

## 4. Log Oficial de Consola (Evidencia de Ejecución)

```text
Serie de pruebas para /home/bayron/SistemaReservaHotel-QA/SistemaReservaHotel/SistemaReservaHotel.UnitTests/bin/Debug/net8.0/SistemaReservaHotel.UnitTests.dll (.NETCoreApp,Version=v8.0)
Herramienta de línea de comandos de ejecución de pruebas de Microsoft(R), versión 17.8.0 (x64)
Copyright (c) Microsoft Corporation. Todos los derechos reservados.
Iniciando la ejecución de pruebas, espere...
1 archivos de prueba en total coincidieron con el patrón especificado.
[xUnit.net 00:00:00.01] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 8.0.31)
[xUnit.net 00:00:00.27]   Discovering: SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.39]   Discovered:  SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.41]   Starting:    SistemaReservaHotel.UnitTests
  Correctas SistemaReservaHotel.UnitTests.DisponibilidadServiceTests.ObtenerDisponiblesAsync_ReservaIniciaElMismoDiaDeSalida_HabitacionDisponible [273 ms]
[xUnit.net 00:00:01.02]   Finished:    SistemaReservaHotel.UnitTests
  Correctas SistemaReservaHotel.UnitTests.DisponibilidadServiceTests.ObtenerDisponiblesAsync_ConReservaCanceladaEnMismoRango_NoExcluyeHabitacion [5 ms]
  Correctas SistemaReservaHotel.UnitTests.DisponibilidadServiceTests.ObtenerDisponiblesAsync_ConReservaActivaEnConflicto_ExcluyeHabitacionOcupada [5 ms]
  Correctas SistemaReservaHotel.UnitTests.DisponibilidadServiceTests.ObtenerDisponiblesAsync_HabitacionInactiva_NoSeIncluyeEnDisponibles [2 ms]
  Correctas SistemaReservaHotel.UnitTests.DisponibilidadServiceTests.ObtenerDisponiblesAsync_SinReservas_RetornaTodasLasHabitacionesActivas [6 ms]
  Correctas SistemaReservaHotel.UnitTests.DisponibilidadServiceTests.ObtenerDisponiblesAsync_ReservaFinalizaElMismoDiaDeEntrada_HabitacionDisponible [2 ms]

Archivo de resultados: SistemaReservaHotel/SistemaReservaHotel.UnitTests/TestResults/joselin_herrera_results.trx
La serie de pruebas se ejecutó correctamente.
Pruebas totales: 6
     Correcto: 6
 Tiempo total: 3.2378 Segundos
```

---

## 5. Declaración de Autonomía y Conformidad

Este reporte certifica formalmente que las **6 pruebas unitarias asignadas a Joselin Esmeralda Herrera Flores** fueron ejecutadas y validadas con éxito de manera 100% aislada e independiente en `DisponibilidadServiceTests.cs`, verificando la disponibilidad de habitaciones por rango de fechas, exclusión de habitaciones inactivas, conflicto con reservas activas, liberación por cancelación y manejo de rangos contiguos (check-in/check-out en el mismo día) en la capa de negocio sin depender de base de datos externa ni de los otros 3 integrantes, satisfaciendo a cabalidad los criterios de aceptación para la historia **US-15 (#1037)** y los casos de prueba **TC-03 (#1048)** y **TC-04 (#1049)** en Azure DevOps.
