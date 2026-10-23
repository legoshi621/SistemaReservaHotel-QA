# EVIDENCIA DE PRUEBAS — JOSELIN ESMERALDA HERRERA FLORES
## Sistema de Reserva de Hotel · US-15 (#1037) · Catálogo y Disponibilidad

---

| Campo                 | Detalle                                                                                       |
|-----------------------|-----------------------------------------------------------------------------------------------|
| **Integrante**        | Joselin Esmeralda Herrera Flores                                                              |
| **User Story**        | US-15 (#1037) — Catálogo y Disponibilidad                                                     |
| **Fecha de ejecución**| 2026-10-22                                                                                    |
| **Herramienta**       | `dotnet test` — xUnit 2.9.3, Moq 4.20.72, FluentAssertions 8.11.0                           |
| **Framework**         | .NET 10.0 / net10.0                                                                           |
| **Repositorio**       | SistemaReservaHotel-QA — rama `main`                                                         |
| **Tipo de evidencia** | Ejecución real local (Pruebas Unitarias + Pruebas de API e Integración)                       |

---

## 1. RESUMEN EJECUTIVO

| Categoría              | Ejecutadas | Pasadas | Fallidas | Tasa de Éxito |
|------------------------|:----------:|:-------:|:--------:|:-------------:|
| Pruebas Unitarias      | 6          | **6**   | 0        | **100 %**     |
| Pruebas de Integración | 3          | **3**   | 0        | **100 %**     |
| **TOTAL**              | **9**      | **9**   | **0**    | **100 %**     |

---

## 2. PRUEBAS UNITARIAS — `DisponibilidadServiceTests`

**Proyecto:** `SistemaReservaHotel.UnitTests`  
**Clase:** `SistemaReservaHotel.UnitTests.DisponibilidadServiceTests`  
**Comando ejecutado:**
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~DisponibilidadServiceTests" --logger "console;verbosity=detailed"
```

### Resultados de Ejecución

| ID    | Nombre del Test                                                                                     | Estado      | Duración |
|-------|-----------------------------------------------------------------------------------------------------|:-----------:|:--------:|
| UT-07 | `ObtenerDisponiblesAsync_SinReservas_RetornaTodasLasHabitacionesActivas`                           | ✅ PASS     | 4 ms     |
| UT-08 | `ObtenerDisponiblesAsync_HabitacionInactiva_NoSeIncluyeEnDisponibles`                              | ✅ PASS     | 1 ms     |
| UT-09 | `ObtenerDisponiblesAsync_ConReservaActivaEnConflicto_ExcluyeHabitacionOcupada`                     | ✅ PASS     | 4 ms     |
| UT-10 | `ObtenerDisponiblesAsync_ConReservaCanceladaEnMismoRango_NoExcluyeHabitacion`                      | ✅ PASS     | 3 ms     |
| UT-11 | `ObtenerDisponiblesAsync_ReservaFinalizaElMismoDiaDeEntrada_HabitacionDisponible`                  | ✅ PASS     | 2 ms     |
| UT-12 | `ObtenerDisponiblesAsync_ReservaIniciaElMismoDiaDeSalida_HabitacionDisponible`                     | ✅ PASS     | 241 ms   |

**Output real de consola:**
```text
  Correctas  DisponibilidadServiceTests.ObtenerDisponiblesAsync_SinReservas_RetornaTodasLasHabitacionesActivas [4 ms]
  Correctas  DisponibilidadServiceTests.ObtenerDisponiblesAsync_HabitacionInactiva_NoSeIncluyeEnDisponibles [1 ms]
  Correctas  DisponibilidadServiceTests.ObtenerDisponiblesAsync_ConReservaActivaEnConflicto_ExcluyeHabitacionOcupada [4 ms]
  Correctas  DisponibilidadServiceTests.ObtenerDisponiblesAsync_ConReservaCanceladaEnMismoRango_NoExcluyeHabitacion [3 ms]
  Correctas  DisponibilidadServiceTests.ObtenerDisponiblesAsync_ReservaFinalizaElMismoDiaDeEntrada_HabitacionDisponible [2 ms]
  Correctas  DisponibilidadServiceTests.ObtenerDisponiblesAsync_ReservaIniciaElMismoDiaDeSalida_HabitacionDisponible [241 ms]
```

### Descripción de los Casos de Prueba

#### UT-07 — Rango libre sin reservas existentes
* **Objetivo:** Verificar que cuando no hay reservas, todas las habitaciones activas son retornadas.  
* **Arrange:** 2 habitaciones activas (Id=1,2), lista de reservas vacía, rango hoy+1 a hoy+4.  
* **Assert:** `disponibles.Count == 2`, contiene Id=1 e Id=2.  
* **Resultado:** ✅ PASS — Se retornaron las 2 habitaciones activas correctamente.

#### UT-08 — Habitación inactiva no se incluye
* **Objetivo:** Verificar que habitaciones con `Activa = false` son excluidas.  
* **Arrange:** Habitación 101 activa (Id=1) + Habitación 102 inactiva (Id=2), sin reservas.  
* **Assert:** `disponibles.Count == 1`, Id=1.  
* **Resultado:** ✅ PASS — Solo la habitación activa fue incluida.

#### UT-09 — Habitación con reserva activa en conflicto es excluida
* **Objetivo:** Verificar anticolisión: habitación con reserva activa solapada no se ofrece.  
* **Arrange:** 2 habitaciones activas; reserva en Id=1 del día+2 al día+5 (Estado="Activa"); búsqueda día+3 a día+6.  
* **Assert:** `disponibles.Count == 1`, Id=2.  
* **Resultado:** ✅ PASS — Id=1 excluida por colisión; Id=2 disponible.

#### UT-10 — Reserva cancelada no bloquea la habitación
* **Objetivo:** Verificar que `Estado = "Cancelada"` no genera conflicto.  
* **Arrange:** 1 habitación activa (Id=1); reserva cancelada sobre el mismo rango.  
* **Assert:** `disponibles.Count == 1`, Id=1.  
* **Resultado:** ✅ PASS — La cancelación no bloquea la disponibilidad.

#### UT-11 — Reserva que finaliza el mismo día de entrada es aceptada
* **Objetivo:** Lógica de borde: reserva que termina el día que el cliente quiere entrar no genera colisión.  
* **Arrange:** Reserva en Id=1 de día+1 a día+4; búsqueda desde día+4 a día+7.  
* **Assert:** `disponibles.Count == 1`, Id=1.  
* **Resultado:** ✅ PASS — Checkout en mismo día de checkin nuevo no genera conflicto.

#### UT-12 — Reserva que inicia el mismo día de salida es aceptada
* **Objetivo:** Lógica de borde inversa: futura reserva que inicia cuando el cliente ya se fue no bloquea.  
* **Arrange:** Reserva en Id=1 de día+5 a día+8; búsqueda desde día+2 a día+5.  
* **Assert:** `disponibles.Count == 1`, Id=1.  
* **Resultado:** ✅ PASS — Inicio de reserva futura en día de checkout no bloquea.

---

## 3. PRUEBAS DE INTEGRACIÓN — `DisponibilidadIntegrationTests`

**Proyecto:** `SistemaReservaHotel.IntegrationTests`  
**Clase:** `SistemaReservaHotel.IntegrationTests.DisponibilidadIntegrationTests`  
**Infraestructura:** `CustomWebApplicationFactory` + EF Core InMemory + `Microsoft.AspNetCore.Mvc.Testing`  
**Endpoint:** `GET /Disponibilidad?entrada={fecha}&salida={fecha}`  
**Comando ejecutado:**
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~DisponibilidadIntegrationTests" --logger "console;verbosity=detailed"
```

### Resultados de Ejecución

| ID        | API    | Nombre del Test                                                                       | Estado      | Duración |
|-----------|--------|---------------------------------------------------------------------------------------|:-----------:|:--------:|
| TC-INT-01 | API-06 | `GetDisponibilidad_EnRangoSinConflicto_Retorna200OkYHabitacionDisponible`            | ✅ PASS     | 6 s      |
| TC-INT-02 | API-07 | `GetDisponibilidad_EnRangoConReservaActiva_ExcluyeHabitacionOcupada`                 | ✅ PASS     | 24 ms    |
| TC-INT-03 | API-08 | `GetDisponibilidad_HabitacionInactiva_NoSeIncluyeEnDisponibles`                      | ✅ PASS     | 45 ms    |

**Output real de consola:**
```text
  Correctas  DisponibilidadIntegrationTests.GetDisponibilidad_EnRangoSinConflicto_Retorna200OkYHabitacionDisponible [6 s]
  Correctas  DisponibilidadIntegrationTests.GetDisponibilidad_HabitacionInactiva_NoSeIncluyeEnDisponibles [45 ms]
  Correctas  DisponibilidadIntegrationTests.GetDisponibilidad_EnRangoConReservaActiva_ExcluyeHabitacionOcupada [24 ms]
```

### Descripción de los Casos de Prueba

#### TC-INT-01 / API-06 — Rango libre → 200 OK + habitación disponible
* **Objetivo:** El endpoint retorna 200 OK y la habitación semilla (Id=1, Activa=true) en rango libre.  
* **Request:** `GET /Disponibilidad?entrada=<hoy+1>&salida=<hoy+3>`  
* **Asserts:** `StatusCode == 200` ✅ · `disponibles != null` ✅ · `h.Id == 1 && h.Activa` ✅  
* **Resultado:** ✅ PASS

#### TC-INT-02 / API-07 — Reserva activa en conflicto → Id=1 excluida
* **Objetivo:** Habitación con reserva solapada (semilla: día+10 a día+15) no aparece en resultado.  
* **Request:** `GET /Disponibilidad?entrada=<hoy+11>&salida=<hoy+14>`  
* **Asserts:** `StatusCode == 200` ✅ · `disponibles != null` ✅ · `!disponibles.Any(h => h.Id == 1)` ✅  
* **Resultado:** ✅ PASS

#### TC-INT-03 / API-08 — Habitación inactiva → no incluida
* **Objetivo:** Habitación con `Activa = false` insertada en runtime no aparece en resultado.  
* **Precondición:** Se inserta en BD en memoria habitación Id=99, Numero=999, `Activa=false`.  
* **Request:** `GET /Disponibilidad?entrada=<hoy+1>&salida=<hoy+2>`  
* **Asserts:** `StatusCode == 200` ✅ · `disponibles != null` ✅ · `!disponibles.Any(h => h.Id == 99)` ✅  
* **Resultado:** ✅ PASS

---

## 4. COBERTURA DE RESPONSABILIDADES QA — US-15 (#1037)

| Área           | ID       | Descripción                                                           | Estado      |
|----------------|----------|-----------------------------------------------------------------------|:-----------:|
| UAT            | TC-04    | Reporte de reservas por fechas en UI                                  | ✅ Asignado  |
| UAT            | TC-05    | Control anticolisión en pantalla                                      | ✅ Asignado  |
| API / Integr.  | API-06   | GET /Disponibilidad — rango libre, 200 OK + habitación disponible     | ✅ PASS      |
| API / Integr.  | API-07   | GET /Disponibilidad — exclusión de habitaciones ocupadas              | ✅ PASS      |
| API / Integr.  | API-08   | GET /Disponibilidad — filtrado de inactivas                           | ✅ PASS      |
| Unitarias      | UT-07    | Sin reservas → todas las activas disponibles                          | ✅ PASS      |
| Unitarias      | UT-08    | Habitación inactiva → excluida                                        | ✅ PASS      |
| Unitarias      | UT-09    | Reserva activa en conflicto → excluida                                | ✅ PASS      |
| Unitarias      | UT-10    | Reserva cancelada → no bloquea                                        | ✅ PASS      |
| Unitarias      | UT-11    | Checkout = checkin nuevo → no conflicto                               | ✅ PASS      |
| Unitarias      | UT-12    | Checkin = checkout anterior → no conflicto                            | ✅ PASS      |

---

## 5. CONCLUSIÓN Y DECLARACIÓN DE CUMPLIMIENTO

> Todas las pruebas asignadas a **Joselin Esmeralda Herrera Flores** para la **US-15 (#1037)** fueron ejecutadas exitosamente en entorno local con `dotnet test`.  
> **Tasa de aprobación: 100 % (9/9 pruebas: 6 Unitarias + 3 Integración).**  
> La lógica de `DisponibilidadService` valida correctamente: rango libre, anticolisión, filtrado de inactivas, estados de reserva y casos borde de fechas límite sin dependencias externas.
