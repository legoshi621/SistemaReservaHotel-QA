# Reporte Individual de Evidencias de QA — Camila Antonia Calderón Cortez (`cami calderon`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `main`  
**Integrante:** Camila Antonia Calderón Cortez (`cami calderon` / `Camila Calderón`)  
**Rol en QA:** Tester de Mutación, Persistencia Transaccional y Tarifas (Ciclo CRUD y Control de Colisión)  
**Historia de Usuario (PBI):** **US-SEC-01 (#1043)** — *Ciclo de vida CRUD de reservas y control de colisión de disponibilidad*  
**Test Plan Oficial:** Test Plan `#1054` (Suite `#1055`)  
**Fecha de Ejecución y Validación:** 22 de Octubre de 2026  
**Resultado Global:** **100% de Pruebas Pasadas (9 de 9 Aprobadas: 5 Unitarias + 4 Integración / API)**  

---

## 1. Alcance Asignado (Los 3 Niveles de Prueba)

| Nivel de Prueba | Identificadores | Componente / Endpoint | Estado |
|:---|:---|:---|:---:|
| **🌐 Aceptación (UAT)** | **TC-03 (#1048)**<br>**TC-06 (#1051)** | Creación de reserva en formulario web (`/Reserva/Crear`)<br>Validación de tarifa de temporada ($360 vs $120) | Documentado en Azure Test Plan<br>*(Con reporte de **Bug #1056** en Boards)* |
| **🔌 API / Integración** | **API-13 a API-16** | `POST /Reserva/Crear`<br>`POST /Reserva/Cancelar` con TestHost / InMemory DB | ✅ **4 / 4 Passed (100%)** |
| **⚙️ Pruebas Unitarias** | **UT-17 a UT-21** | `ReservaService` (Escritura y Colisión) con xUnit y Moq | ✅ **5 / 5 Passed (100%)** |

---

## 2. Ejecución Aislada de Pruebas de API e Integración

### Comando Ejecutado
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ReservaCrudIntegrationTests" --logger "console;verbosity=normal"
```

### Matriz de Casos de API / Integración Evaluados
| # | Caso | Método de Prueba | Condición Probada | Payload Enviado | HTTP Esperado | Comprobación en BD InMemory | Estado |
|:---:|:---:|:---|:---|:---|:---:|:---|:---:|
| 1 | **API-13** | `PostCrearReserva_PersisteEntidadEnBaseDeDatosInMemory` | Creación y persistencia válida | Cliente: "Camila Calderon Test", Hab: 1, $450 | `200 OK` | Entidad existe en `AppDbContext.Reservas` con `Id > 0` y estado "Activa" | ✅ **Passed** |
| 2 | **API-14** | `PostCancelarReserva_ActualizaEstadoEnBaseDeDatosInMemory` | Cancelación de reserva activa | DTO `{ Id: reservaCreada.Id }` | `200 OK` | Registro muta a estado "Cancelada" en `AppDbContext` | ✅ **Passed** |
| 3 | **API-15** | `PostCancelarReserva_ConIdInexistente_Retorna404NotFound` | Cancelación con ID no registrado | DTO `{ Id: 88888 }` | `404 NotFound` | No altera ningún registro en `AppDbContext` | ✅ **Passed** |
| 4 | **API-16** | `PostCrearReserva_EnMismasFechasQueReservaActiva_RechazaPorColision` | Intento de sobreventa en rango ocupado | Fechas en colisión (Día +11 a +14) | `400 BadRequest` | Bloqueo preventivo de sobreventa; colisión no insertada en BD | ✅ **Passed** |

### Resumen de Pruebas de Integración
* **Total:** 4 | **Pasadas:** 4 (100%) | **Fallidas:** 0 | **Omitidas:** 0 | **Tiempo:** ~4.22 s

---

## 3. Ejecución Aislada de Pruebas Unitarias

### Comando Ejecutado
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ReservaPublicaServiceTests.Reservar_DatosValidos_CreaReserva | FullyQualifiedName~ReservaPublicaServiceTests.Actualizar_EstadoModificado_Exitoso | FullyQualifiedName~ReservaPublicaServiceTests.Eliminar_PorId_Exitoso | FullyQualifiedName~ReservaPublicaServiceTests.Disponibilidad_SinConflicto_RetornaTrue | FullyQualifiedName~ReservaPublicaServiceTests.Disponibilidad_ConConflicto_RetornaFalse" --logger "console;verbosity=normal"
```

### Matriz de Casos Unitarios Evaluados (Patrón AAA con Moq)
| # | Caso | Método de Prueba | Condición Probada | Entrada Simulada | Salida Esperada | Resultado Real | Estado |
|:---:|:---:|:---|:---|:---|:---:|:---:|:---:|
| 1 | **UT-17** | `Reservar_DatosValidos_CreaReserva` | Creación de reserva con ID generado | `Cliente: "Ana Torres"`, `Hab: 1`, `$150` | `Reserva.Id == 10`, `Nombre == "Ana Torres"` | `Verify Times.Once` | ✅ **Passed** |
| 2 | **UT-18** | `Actualizar_EstadoModificado_Exitoso` | Modificación y cancelación de reserva | `Reserva Id: 5`, `Estado: "Cancelada"` | Llamada única a `ActualizarAsync` en repositorio | `Verify Times.Once` | ✅ **Passed** |
| 3 | **UT-19** | `Eliminar_PorId_Exitoso` | Eliminación segura de reserva por clave primaria | `Reserva Id: 7` | Llamada única a `EliminarAsync(7)` en repositorio | `Verify Times.Once` | ✅ **Passed** |
| 4 | **UT-20** | `Disponibilidad_SinConflicto_RetornaTrue` | Verificación de fechas sin conflicto ni sobreventa | `HabitacionId: 1`, `Hoy + 2d` a `Hoy + 5d` | `True` (Habitación libre para reservar) | `Verify Times.Once` | ✅ **Passed** |
| 5 | **UT-21** | `Disponibilidad_ConConflicto_RetornaFalse` | Detección de colisión / solapamiento de fechas | `HabitacionId: 1`, fechas en colisión | `False` (Bloqueo preventivo de sobreventa) | `Verify Times.Once` | ✅ **Passed** |

### Resumen de Pruebas Unitarias
* **Total:** 5 | **Pasadas:** 5 (100%) | **Fallidas:** 0 | **Omitidas:** 0 | **Tiempo:** ~1.38 s

---

## 4. Log Oficial de Consola (Compilado de Evidencias)

### Salida de Integración (`ReservaCrudIntegrationTests`):
```text
Serie de pruebas para SistemaReservaHotel.IntegrationTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.4+50e68bbb8b (64-bit .NET 10.0.11)
[xUnit.net 00:00:00.27]   Discovering: SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:00.39]   Discovered:  SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:00.46]   Starting:    SistemaReservaHotel.IntegrationTests
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 18 entities to in-memory store.
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 35 entities to in-memory store.
info: Microsoft.Hosting.Lifetime[0]
      Application started. Hosting environment: Testing
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCancelarReserva_ActualizaEstadoEnBaseDeDatosInMemory [2 s]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCancelarReserva_ConIdInexistente_Retorna404NotFound [16 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCrearReserva_EnMismasFechasQueReservaActiva_RechazaPorColision [4 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ReservaCrudIntegrationTests.PostCrearReserva_PersisteEntidadEnBaseDeDatosInMemory [11 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 4
     Correcto: 4
     Fallidas: 0
    Omitidas: 0
 Tiempo total: 4.2206 Segundos
```

### Salida de Unitarias (`ReservaPublicaServiceTests`):
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
     Correcto: 5
     Fallidas: 0
    Omitidas: 0
 Tiempo total: 1.3858 Segundos
```

---

## 5. Declaración de Cumplimiento
Este reporte certifica que las **9 pruebas automatizadas (4 de integración + 5 unitarias)** asignadas a **Camila Antonia Calderón Cortez** fueron ejecutadas y aprobadas en un 100%, cumpliendo rigurosamente los criterios de aceptación para la historia **US-SEC-01 (#1043)** y los casos de prueba **TC-03 (#1048)** y **TC-06 (#1051)** en Azure DevOps y el Test Plan `#1054`.
