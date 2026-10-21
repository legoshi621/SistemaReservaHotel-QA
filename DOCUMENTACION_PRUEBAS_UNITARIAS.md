# Documentación Oficial de Pruebas Unitarias — Sistema de Reserva Hotel

**Proyecto:** Sistema de Reserva Hotel  
**Fecha de Ejecución:** Octubre 2026  
**Framework Tecnológico:** .NET 10.0 (`net10.0`) | xUnit 2.9.3 | Moq 4.21.0 | Coverlet 6.0.4  
**Proyecto de Pruebas:** `SistemaReservaHotel0/SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj`  
**Tasa de Aprobación Global:** **100% (21 de 21 pruebas superadas — 0 fallidas — 0 omitidas)**  
**Tiempo de Ejecución:** ~1.0 segundo  

---

## 1. Resumen Ejecutivo de la Suite de Pruebas

Se diseñó e implementó un conjunto riguroso de **21 pruebas unitarias automatizadas** orientadas a garantizar la confiabilidad, consistencia y robustez de la capa de lógica del negocio (`SistemaReservaHotel.Business`). Todas las pruebas siguen estrictamente el patrón estándar de la industria **AAA (Arrange - Act - Assert)** y emplean aislamiento de dependencias a través de dobles de prueba (*Mocks*) mediante la librería **Moq**, garantizando que las pruebas sean deterministas, ultrarrápidas e independientes de conexiones a bases de datos físicas o redes.

### Distribución de Casos de Prueba por Integrante

| Suite de Pruebas (Clase) | Archivo de Código | Integrante Responsable | Cantidad de Pruebas | Resultado |
|:---|:---|:---|:---:|:---:|
| **1. Validador de Fechas** | `ValidadorFechasTests.cs` | **Ariel Isai Ascencio Ramos** (`Isai Ascencio`) | 6 | **6 Superadas (100%)** |
| **2. Disponibilidad de Habitaciones** | `DisponibilidadServiceTests.cs` | **Joselin Esmeralda Herrera Flores** (`Joselin Herrera`) | 6 | **6 Superadas (100%)** |
| **3. Consultas y Lectura de Reservas** | `ReservaPublicaServiceTests.cs` (lectura) | **Carlos Alonso Miranda Rivas** (`Alonso Rivas`) | 4 | **4 Superadas (100%)** |
| **4. Mutación y Validación de Colisión** | `ReservaPublicaServiceTests.cs` (escritura) | **Camila Antonia Calderón Cortez** (`cami calderon`) | 5 | **5 Superadas (100%)** |
| **TOTAL GENERAL** | — | **4 Integrantes** | **21** | **21 Superadas (100%)** |

> [!IMPORTANT]
> **Distribución equitativa e independiente:** Ariel: 6, Joselin: 6, Camila: 5, Carlos: 4. Ningún integrante comparte pruebas con otro. Cada uno ejecuta su propio filtro `dotnet test` de forma completamente autónoma.

---

## 2. Detalle Exhaustivo de las 21 Pruebas Unitarias

---

### Suite 1: `ValidadorFechasTests` (6 Pruebas) — Ariel Isai Ascencio Ramos
**Ubicación:** `SistemaReservaHotel.UnitTests/ValidadorFechasTests.cs`  
**Clase / Método Bajo Prueba:** `SistemaReservaHotel.Business.Helpers.ValidadorFechas.FechasValidas(DateTime entrada, DateTime salida)`  
**Historia Azure DevOps:** US-16 (#1040) — TC-05 (#1050)  
**Objetivo de Calidad:** Validar las reglas cronológicas de negocio: la fecha de entrada debe ser hoy o futura (`entrada >= DateTime.Today`), y la fecha de salida debe ser estrictamente posterior a la de entrada (`entrada < salida`).

```
                              ┌── entrada >= Hoy ? ──┐
                              │                      │
FechasValidas(entrada, salida)┤                      ├─> True (Solo si ambas cumplen)
                              │                      │
                              └── entrada < salida ? ┘
```

**Comando individual de Ariel:**
```powershell
dotnet test SistemaReservaHotel0/SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ValidadorFechasTests"
```

#### Caso 1.1: `FechasValidas_EntradaFuturaYMenorQueSalida_RetornaTrue`
* **Historia de Usuario / Requisito:** US-16 (#1040) — Validación de reserva con rango de fechas válido en el futuro.
* **Propósito:** Verificar que un rango de fechas plenamente válido (entrada en 2 días y salida en 5 días) sea aceptado por el validador.
* **Arrange (Preparación):**
  * `entrada = DateTime.Today.AddDays(2);`
  * `salida = DateTime.Today.AddDays(5);`
* **Act (Ejecución):**
  * `var resultado = ValidadorFechas.FechasValidas(entrada, salida);`
* **Assert (Validación):**
  * `Assert.True(resultado);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 1.2: `FechasValidas_EntradaHoyYSalidaManana_RetornaTrue`
* **Historia de Usuario / Requisito:** US-16 (#1040) — Reserva inmediata (Check-in el mismo día).
* **Propósito:** Validar el caso límite en el que un huésped llega hoy mismo y sale mañana (estancia mínima de 1 noche permitida).
* **Arrange (Preparación):**
  * `entrada = DateTime.Today;`
  * `salida = DateTime.Today.AddDays(1);`
* **Act (Ejecución):**
  * `var resultado = ValidadorFechas.FechasValidas(entrada, salida);`
* **Assert (Validación):**
  * `Assert.True(resultado);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 1.3: `FechasValidas_EntradaMayorQueSalida_RetornaFalse`
* **Historia de Usuario / Requisito:** US-16 (#1040) — Prevención de fechas invertidas.
* **Propósito:** Asegurar que el sistema rechace solicitudes donde la fecha de entrada sea posterior a la fecha de salida (anomalía temporal).
* **Arrange (Preparación):**
  * `entrada = DateTime.Today.AddDays(5);`
  * `salida = DateTime.Today.AddDays(2);`
* **Act (Ejecución):**
  * `var resultado = ValidadorFechas.FechasValidas(entrada, salida);`
* **Assert (Validación):**
  * `Assert.False(resultado);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 1.4: `FechasValidas_EntradaIgualASalida_RetornaFalse`
* **Historia de Usuario / Requisito:** US-16 (#1040) — Duración mínima de reserva.
* **Propósito:** Validar que no se permitan reservas con 0 noches de estancia (entrada y salida el mismo día calendario).
* **Arrange (Preparación):**
  * `entrada = DateTime.Today.AddDays(3);`
  * `salida = DateTime.Today.AddDays(3);`
* **Act (Ejecución):**
  * `var resultado = ValidadorFechas.FechasValidas(entrada, salida);`
* **Assert (Validación):**
  * `Assert.False(resultado);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 1.5: `FechasValidas_EntradaEnElPasado_RetornaFalse`
* **Historia de Usuario / Requisito:** US-16 (#1040) — Bloqueo de fechas extemporáneas.
* **Propósito:** Prohibir el registro de reservas con fecha de check-in en días anteriores a la fecha actual.
* **Arrange (Preparación):**
  * `entrada = DateTime.Today.AddDays(-1);`
  * `salida = DateTime.Today.AddDays(2);`
* **Act (Ejecución):**
  * `var resultado = ValidadorFechas.FechasValidas(entrada, salida);`
* **Assert (Validación):**
  * `Assert.False(resultado);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 1.6: `FechasValidas_EntradaYSalidaEnElPasado_RetornaFalse`
* **Historia de Usuario / Requisito:** US-16 (#1040) — Bloqueo total de períodos caducados.
* **Propósito:** Asegurar el rechazo inmediato cuando tanto la entrada como la salida pertenecen íntegramente al pasado.
* **Arrange (Preparación):**
  * `entrada = DateTime.Today.AddDays(-5);`
  * `salida = DateTime.Today.AddDays(-2);`
* **Act (Ejecución):**
  * `var resultado = ValidadorFechas.FechasValidas(entrada, salida);`
* **Assert (Validación):**
  * `Assert.False(resultado);`
* **Resultado:** **SUPERADO (Passed)**.

---

### Suite 2: `DisponibilidadServiceTests` (6 Pruebas) — Joselin Esmeralda Herrera Flores
**Ubicación:** `SistemaReservaHotel.UnitTests/DisponibilidadServiceTests.cs`  
**Clase / Método Bajo Prueba:** `SistemaReservaHotel.Business.Services.DisponibilidadService.ObtenerDisponiblesAsync(DateTime entrada, DateTime salida)`  
**Historia Azure DevOps:** US-15 (#1037) — TC-03 (#1048), TC-04 (#1049)  
**Dependencias Mockeadas:** `IHabitacionRepository` y `IReservaRepository` mediante `Moq`.  
**Objetivo de Calidad:** Validar el algoritmo de disponibilidad de habitaciones excluyendo ocupadas y no operativas, verificando la regla de anti-traslape (*overlap*): `(entrada < r.FechaSalida && salida > r.FechaEntrada)`.

```
                    Rango Solicitado:      [ ENTRADA ────────────── SALIDA ]
                    Reserva Existente A:         [ ENTRADA ── SALIDA ]            --> CONFLICTO (Excluida)
                    Reserva Existente B:   [ E ── S ]                             --> LIBRE (Mismo día check-in)
                    Reserva Cancelada C:         [ ENTRADA ── SALIDA ]            --> LIBRE (Cancelada no bloquea)
```

**Comando individual de Joselin:**
```powershell
dotnet test SistemaReservaHotel0/SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~DisponibilidadServiceTests"
```

#### Caso 2.1: `ObtenerDisponiblesAsync_SinReservas_RetornaTodasLasHabitacionesActivas`
* **Historia de Usuario / Requisito:** US-15 (#1037) — Disponibilidad en hotel sin ocupación.
* **Propósito:** Comprobar que en ausencia de reservas en el sistema, todas las habitaciones dadas de alta como activas se reporten como disponibles.
* **Arrange (Preparación):**
  * Habitaciones: Hab 101 (Activa=true), Hab 102 (Activa=true).
  * Mock `IHabitacionRepository.ObtenerTodasAsync()` -> Retorna las 2 habitaciones.
  * Mock `IReservaRepository.ObtenerTodasAsync()` -> Retorna lista vacía.
  * Rango de búsqueda: `Hoy + 1` a `Hoy + 4`.
* **Act (Ejecución):**
  * `var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);`
* **Assert (Validación):**
  * `Assert.Equal(2, disponibles.Count);`
  * `Assert.Contains(disponibles, h => h.Id == 1);`
  * `Assert.Contains(disponibles, h => h.Id == 2);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 2.2: `ObtenerDisponiblesAsync_HabitacionInactiva_NoSeIncluyeEnDisponibles`
* **Historia de Usuario / Requisito:** US-15 (#1037) — Aislamiento de inventario no disponible (mantenimiento/baja).
* **Propósito:** Garantizar que las habitaciones con `Activa = false` jamás aparezcan disponibles, aún cuando no tengan ninguna reserva asignada.
* **Arrange (Preparación):**
  * Habitaciones: Hab 101 (Activa=true), Hab 102 (Activa=false).
  * Mock `IHabitacionRepository` devuelve ambas habitaciones; Mock `IReservaRepository` devuelve lista vacía.
* **Act (Ejecución):**
  * `var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);`
* **Assert (Validación):**
  * `Assert.Single(disponibles);`
  * `Assert.Equal(1, disponibles[0].Id);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 2.3: `ObtenerDisponiblesAsync_ConReservaActivaEnConflicto_ExcluyeHabitacionOcupada`
* **Historia de Usuario / Requisito:** US-15 (#1037) — Algoritmo anti-traslape (*anti-overbooking*).
* **Propósito:** Validar que si una habitación tiene una reserva activa que se solapa con el rango buscado, sea inmediatamente excluida de los resultados.
* **Arrange (Preparación):**
  * Habitación 1 y Habitación 2 activas.
  * Habitación 1 tiene reserva Activa del `Hoy + 2` al `Hoy + 5`.
  * Búsqueda solicitada: del `Hoy + 3` al `Hoy + 6` (colisión parcial interna).
* **Act (Ejecución):**
  * `var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);`
* **Assert (Validación):**
  * `Assert.Single(disponibles);`
  * `Assert.Equal(2, disponibles[0].Id);` (Solo Hab 2 queda libre).
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 2.4: `ObtenerDisponiblesAsync_ConReservaCanceladaEnMismoRango_NoExcluyeHabitacion`
* **Historia de Usuario / Requisito:** US-15 (#1037) — Liberación por cancelación.
* **Propósito:** Confirmar que las reservas en estado `"Cancelada"` no bloqueen la habitación, permitiendo que otros clientes puedan reservarla.
* **Arrange (Preparación):**
  * Habitación 1 activa.
  * Reserva existente sobre Hab 1 en estado `"Cancelada"` del `Hoy + 2` al `Hoy + 5`.
  * Búsqueda solicitada: del `Hoy + 3` al `Hoy + 4`.
* **Act (Ejecución):**
  * `var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);`
* **Assert (Validación):**
  * `Assert.Single(disponibles);`
  * `Assert.Equal(1, disponibles[0].Id);` (Hab 1 disponible).
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 2.5: `ObtenerDisponiblesAsync_ReservaFinalizaElMismoDiaDeEntrada_HabitacionDisponible`
* **Historia de Usuario / Requisito:** US-15 (#1037) — Caso de borde: Check-out y Check-in el mismo día.
* **Propósito:** Validar la regla hotelera estándar: un huésped puede ingresar el mismo día en que el huésped anterior realiza su salida.
* **Arrange (Preparación):**
  * Habitación 1 activa con reserva activa del `Hoy + 1` al `Hoy + 4`.
  * Búsqueda solicitada: del `Hoy + 4` al `Hoy + 7` (`salida_previa == entrada_nueva`).
* **Act (Ejecución):**
  * `var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);`
* **Assert (Validación):**
  * `Assert.Single(disponibles);`
  * `Assert.Equal(1, disponibles[0].Id);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 2.6: `ObtenerDisponiblesAsync_ReservaIniciaElMismoDiaDeSalida_HabitacionDisponible`
* **Historia de Usuario / Requisito:** US-15 (#1037) — Caso de borde: Check-out coincidente con check-in de reserva futura.
* **Propósito:** Validar que un huésped pueda permanecer en la habitación hasta el día en que inicia la siguiente reserva programada.
* **Arrange (Preparación):**
  * Habitación 1 activa con reserva futura del `Hoy + 5` al `Hoy + 8`.
  * Búsqueda solicitada: del `Hoy + 2` al `Hoy + 5` (`salida_nueva == entrada_futura`).
* **Act (Ejecución):**
  * `var disponibles = await _service.ObtenerDisponiblesAsync(entrada, salida);`
* **Assert (Validación):**
  * `Assert.Single(disponibles);`
  * `Assert.Equal(1, disponibles[0].Id);`
* **Resultado:** **SUPERADO (Passed)**.

---

### Suite 3a: `ReservaPublicaServiceTests` — Consultas y Lectura (4 Pruebas) — Carlos Alonso Miranda Rivas
**Ubicación:** `SistemaReservaHotel.UnitTests/ReservaPublicaServiceTests.cs`  
**Clase / Método Bajo Prueba:** `SistemaReservaHotel.Business.Services.ReservaService` (métodos de lectura)  
**Historia Azure DevOps:** US-14 (#1034) — TC-01 (#1046), TC-02 (#1047)  
**Dependencia Mockeada:** `IReservaRepository` mediante `Moq`.  
**Objetivo de Calidad:** Validar la consulta de reservas por día calendario (US-14), la recuperación por ID y el listado global.

**Comando individual de Carlos:**
```powershell
dotnet test SistemaReservaHotel0/SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ObtenerPorDiaAsync|FullyQualifiedName~ObtenerTodasAsync|FullyQualifiedName~ObtenerPorIdAsync"
```

#### Caso 3.1: `ObtenerPorDiaAsync_RetornaReservasDelDiaIndicado`
* **Historia de Usuario / Requisito:** US-14 (#1034) — Ver reservas registradas por día seleccionado.
* **Propósito:** Verificar el cumplimiento estricto del criterio de aceptación de la historia US-14: recuperar la lista de reservas programadas para la fecha seleccionada.
* **Arrange (Preparación):**
  * Fecha de consulta: `DateTime.Today`.
  * Dos reservas asignadas a la fecha de hoy ("Cliente 1", "Cliente 2").
  * Setup mock: `ObtenerPorDiaAsync(DateTime.Today)` retorna la lista simulada.
* **Act (Ejecución):**
  * `var resultado = await _service.ObtenerPorDiaAsync(fecha);`
* **Assert (Validación):**
  * `Assert.NotNull(resultado);`
  * `Assert.Equal(2, resultado.Count);`
  * `_reservaRepoMock.Verify(r => r.ObtenerPorDiaAsync(fecha), Times.Once);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 3.2: `ObtenerTodasAsync_RetornaListaDeReservas`
* **Historia de Usuario / Requisito:** US-14 (#1034) — Listado general de reservas.
* **Propósito:** Verificar que el servicio invoque el repositorio y devuelva la colección completa de reservas existentes.
* **Arrange (Preparación):**
  * Lista simulada con 2 reservas ("Juan Perez", "Maria Gomez").
  * Mock configurado con `_reservaRepoMock.Setup(r => r.ObtenerTodasAsync()).ReturnsAsync(listaEsperada)`.
* **Act (Ejecución):**
  * `var resultado = await _service.ObtenerTodasAsync();`
* **Assert (Validación):**
  * `Assert.NotNull(resultado);`
  * `Assert.Equal(2, resultado.Count);`
  * `Assert.Equal("Juan Perez", resultado[0].NombreCliente);`
  * `_reservaRepoMock.Verify(r => r.ObtenerTodasAsync(), Times.Once);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 3.3: `ObtenerPorIdAsync_IdExistente_RetornaReserva`
* **Historia de Usuario / Requisito:** US-14 (#1034) — Consulta de detalle de reserva.
* **Propósito:** Verificar la obtención precisa de una reserva por su clave primaria cuando sí existe en el repositorio.
* **Arrange (Preparación):**
  * Reserva simulada con `Id = 1`, `NombreCliente = "Carlos Lopez"`.
  * Setup mock: `ObtenerPorIdAsync(1)` retorna la reserva.
* **Act (Ejecución):**
  * `var resultado = await _service.ObtenerPorIdAsync(1);`
* **Assert (Validación):**
  * `Assert.NotNull(resultado);`
  * `Assert.Equal(1, resultado.Id);`
  * `Assert.Equal("Carlos Lopez", resultado.NombreCliente);`
  * `_reservaRepoMock.Verify(r => r.ObtenerPorIdAsync(1), Times.Once);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 3.4: `ObtenerPorIdAsync_IdInexistente_RetornaNull`
* **Historia de Usuario / Requisito:** US-14 (#1034) — Manejo seguro de identificadores no encontrados.
* **Propósito:** Validar que una búsqueda con un ID inexistente retorne `null` limpiamente sin lanzar excepciones no controladas.
* **Arrange (Preparación):**
  * Setup mock: `ObtenerPorIdAsync(999)` retorna `null`.
* **Act (Ejecución):**
  * `var resultado = await _service.ObtenerPorIdAsync(999);`
* **Assert (Validación):**
  * `Assert.Null(resultado);`
  * `_reservaRepoMock.Verify(r => r.ObtenerPorIdAsync(999), Times.Once);`
* **Resultado:** **SUPERADO (Passed)**.

---

### Suite 3b: `ReservaPublicaServiceTests` — Mutación y Validación de Colisión (5 Pruebas) — Camila Antonia Calderón Cortez
**Ubicación:** `SistemaReservaHotel.UnitTests/ReservaPublicaServiceTests.cs`  
**Clase / Método Bajo Prueba:** `SistemaReservaHotel.Business.Services.ReservaService` (métodos de escritura y validación)  
**Historia Azure DevOps:** US-SEC-01 (#1043) — TC-07 (#1052), TC-08 (#1053)  
**Dependencia Mockeada:** `IReservaRepository` mediante `Moq`.  
**Objetivo de Calidad:** Validar el ciclo de vida CRUD completo (crear, actualizar, eliminar) y la validación de disponibilidad/colisión previa a la confirmación de reserva.

**Comando individual de Camila:**
```powershell
dotnet test SistemaReservaHotel0/SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ReservarAsync|FullyQualifiedName~ActualizarAsync|FullyQualifiedName~EliminarAsync|FullyQualifiedName~ValidarDisponibilidadAsync"
```

#### Caso 4.1: `ReservarAsync_CreaReservaYRetornaEntidad`
* **Historia de Usuario / Requisito:** US-SEC-01 (#1043) — Creación y persistencia de reserva.
* **Propósito:** Verificar que una solicitud de reserva nueva se envíe al repositorio y retorne la entidad persistida con su ID autogenerado.
* **Arrange (Preparación):**
  * `nuevaReserva` con cliente "Ana Torres", precio $150m, rango válido.
  * `reservaCreada` simulada con `Id = 10`.
  * Setup mock: `CrearAsync(nuevaReserva)` retorna `reservaCreada`.
* **Act (Ejecución):**
  * `var resultado = await _service.ReservarAsync(nuevaReserva);`
* **Assert (Validación):**
  * `Assert.NotNull(resultado);`
  * `Assert.Equal(10, resultado.Id);`
  * `Assert.Equal("Ana Torres", resultado.NombreCliente);`
  * `_reservaRepoMock.Verify(r => r.CrearAsync(nuevaReserva), Times.Once);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 4.2: `ActualizarAsync_LlamaMetodoActualizarDelRepositorio`
* **Historia de Usuario / Requisito:** US-SEC-01 (#1043) — Modificación de reserva existente.
* **Propósito:** Asegurar que el método de actualización del servicio invoque el método respectivo del repositorio con la entidad modificada.
* **Arrange (Preparación):**
  * Reserva modificada con `Id = 5`, `Estado = "Cancelada"`.
  * Setup mock: `ActualizarAsync` retorna `Task.CompletedTask`.
* **Act (Ejecución):**
  * `await _service.ActualizarAsync(reserva);`
* **Assert (Validación):**
  * `_reservaRepoMock.Verify(r => r.ActualizarAsync(reserva), Times.Once);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 4.3: `EliminarAsync_LlamaMetodoEliminarDelRepositorio`
* **Historia de Usuario / Requisito:** US-SEC-01 (#1043) — Eliminación segura de reservas.
* **Propósito:** Validar que la orden de eliminación transfiera el ID correspondiente al repositorio para su baja física/lógica.
* **Arrange (Preparación):**
  * `reservaId = 7;`
  * Setup mock: `EliminarAsync(7)` retorna `Task.CompletedTask`.
* **Act (Ejecución):**
  * `await _service.EliminarAsync(reservaId);`
* **Assert (Validación):**
  * `_reservaRepoMock.Verify(r => r.EliminarAsync(reservaId), Times.Once);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 4.4: `ValidarDisponibilidadAsync_CuandoEstaDisponible_RetornaTrue`
* **Historia de Usuario / Requisito:** US-SEC-01 (#1043) — Validación previa a confirmación de reserva (Disponible).
* **Propósito:** Comprobar que el servicio retorne `true` cuando la habitación no registra colisiones para el intervalo requerido.
* **Arrange (Preparación):**
  * `habitacionId = 1`, `entrada = Hoy + 2`, `salida = Hoy + 5`.
  * Setup mock: `ValidarDisponibilidadAsync(1, entrada, salida)` retorna `true`.
* **Act (Ejecución):**
  * `var resultado = await _service.ValidarDisponibilidadAsync(habitacionId, entrada, salida);`
* **Assert (Validación):**
  * `Assert.True(resultado);`
  * `_reservaRepoMock.Verify(r => r.ValidarDisponibilidadAsync(1, entrada, salida), Times.Once);`
* **Resultado:** **SUPERADO (Passed)**.

#### Caso 4.5: `ValidarDisponibilidadAsync_CuandoNoEstaDisponible_RetornaFalse`
* **Historia de Usuario / Requisito:** US-SEC-01 (#1043) — Validación previa a confirmación de reserva (No Disponible).
* **Propósito:** Comprobar que el servicio retorne `false` cuando el repositorio detecta colisión de fechas para esa habitación.
* **Arrange (Preparación):**
  * `habitacionId = 1`, fechas con solapamiento.
  * Setup mock: `ValidarDisponibilidadAsync(1, entrada, salida)` retorna `false`.
* **Act (Ejecución):**
  * `var resultado = await _service.ValidarDisponibilidadAsync(habitacionId, entrada, salida);`
* **Assert (Validación):**
  * `Assert.False(resultado);`
  * `_reservaRepoMock.Verify(r => r.ValidarDisponibilidadAsync(1, entrada, salida), Times.Once);`
* **Resultado:** **SUPERADO (Passed)**.

---

## 3. Matriz de Trazabilidad entre Requisitos, Pruebas e Integrantes

| Historia de Usuario / PBI | Nombre de la Característica | IDs de Prueba Unitaria | Integrante Responsable | Cantidad | Estado |
|:---|:---|:---|:---|:---:|:---:|
| **US-14 (#1034)** | Ver reservas registradas por día y por ID | Caso 3.1, Caso 3.2, Caso 3.3, Caso 3.4 | **Carlos** (`Alonso Rivas`) | 4 | **100% Superado** |
| **US-15 (#1037)** | Disponibilidad por fechas y estado de inventario | Caso 2.1, Caso 2.2, Caso 2.3, Caso 2.4, Caso 2.5, Caso 2.6 | **Joselin** (`Joselin Herrera`) | 6 | **100% Superado** |
| **US-16 (#1040)** | Validación de rango de fechas y reglas cronológicas | Caso 1.1, Caso 1.2, Caso 1.3, Caso 1.4, Caso 1.5, Caso 1.6 | **Ariel** (`Isai Ascencio`) | 6 | **100% Superado** |
| **US-SEC-01 (#1043)** | CRUD y control de colisión de disponibilidad | Caso 4.1, Caso 4.2, Caso 4.3, Caso 4.4, Caso 4.5 | **Camila** (`cami calderon`) | 5 | **100% Superado** |
| **TOTAL** | — | **21 Pruebas** | **4 Integrantes** | **21** | **100% ✅** |

---

## 4. Evidencia Oficial de Ejecución de Terminal

> [!NOTE]
> El siguiente bloque muestra la salida real de `dotnet test` con todas las 21 pruebas superadas. Las rutas de compilación son relativas al directorio del proyecto clonado.

```bash
dotnet test SistemaReservaHotel0/SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj
Determinando los proyectos que se van a restaurar...
Todos los proyectos están actualizados para la restauración.
  SistemaReservaHotel.Entities -> ...bin/Debug/net10.0/SistemaReservaHotel.Entities.dll
  SistemaReservaHotel.DataAccess -> ...bin/Debug/net10.0/SistemaReservaHotel.DataAccess.dll
  SistemaReservaHotel.Business -> ...bin/Debug/net10.0/SistemaReservaHotel.Business.dll
  SistemaReservaHotel.UnitTests -> ...bin/Debug/net10.0/SistemaReservaHotel.UnitTests.dll
1 archivos de prueba en total coincidieron con el patrón especificado.

Correctas! - Con error:     0, Superado:    21, Omitido:     0, Total:    21, Duración: 1 s - SistemaReservaHotel.UnitTests.dll (net10.0)
```

---

## 5. Referencia Rápida de Comandos por Integrante

| Integrante | Alias en el Chat | Filtro `dotnet test` | Pruebas |
|:---|:---|:---|:---:|
| Ariel Isai Ascencio Ramos | `Isai Ascencio` / `Ariel Ascencio` | `--filter "FullyQualifiedName~ValidadorFechasTests"` | 6 |
| Joselin Esmeralda Herrera Flores | `Joselin Herrera` | `--filter "FullyQualifiedName~DisponibilidadServiceTests"` | 6 |
| Carlos Alonso Miranda Rivas | `Alonso Rivas` / `Carlos Miranda` | `--filter "FullyQualifiedName~ObtenerPorDiaAsync|FullyQualifiedName~ObtenerTodasAsync|FullyQualifiedName~ObtenerPorIdAsync"` | 4 |
| Camila Antonia Calderón Cortez | `cami calderon` / `Camila Calderón` | `--filter "FullyQualifiedName~ReservarAsync|FullyQualifiedName~ActualizarAsync|FullyQualifiedName~EliminarAsync|FullyQualifiedName~ValidarDisponibilidadAsync"` | 5 |

> [!IMPORTANT]
> Todos los comandos usan rutas relativas al directorio raíz del proyecto (`ReservaHotel1/`). No se requieren rutas absolutas ni configuración previa. Cada integrante puede ejecutar su comando de forma autónoma e inmediata desde la terminal en la raíz del proyecto.
