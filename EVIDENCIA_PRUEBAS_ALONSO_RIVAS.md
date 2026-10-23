# Reporte Individual de Evidencias de QA — Carlos Alonso Miranda Rivas (`Alonso Rivas`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `main`  
**Integrante:** Carlos Alonso Miranda Rivas (`Alonso Rivas` / `Carlos Alonso Miranda`)  
**Rol en QA:** Tester de Consultas, Lectura y Control de Acceso (Consulta de Reservas por Día y por ID)  
**Historia de Usuario (PBI):** **US-14 (#1034)** — *Consulta de reservas por día seleccionado y recuperación por ID*  
**Test Plan Oficial:** Test Plan `#1054` (Suite `#1055`)  
**Fecha de Ejecución y Validación:** 22 de Octubre de 2026  
**Resultado Global:** **100% de Pruebas Pasadas (8 de 8 Aprobadas: 4 Unitarias + 4 Integración / API)**  

---

## 1. Alcance Asignado (Los 3 Niveles de Prueba)

| Nivel de Prueba | Identificadores | Componente / Endpoint | Estado |
|:---|:---|:---|:---:|
| **🌐 Aceptación (UAT)** | **TC-02 (#1047)**<br>**TC-08 (#1053)** | Consulta de Reservas del Día en UI (`/Reserva/ReservasDelDia`)<br>Control de Acceso Administrativo (RBAC) | Documentado en Azure Test Plan |
| **🔌 API / Integración** | **API-09 a API-12** | `GET /Reserva/ObtenerPorDia`<br>`GET /Reserva/Detalles/{id}` con TestHost / InMemory DB | ✅ **4 / 4 Passed (100%)** |
| **⚙️ Pruebas Unitarias** | **UT-13 a UT-16** | `ReservaService` (Lectura) con xUnit y Moq | ✅ **4 / 4 Passed (100%)** |

---

## 2. Ejecución Aislada de Pruebas de API e Integración

### Comando Ejecutado
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ConsultasReservaIntegrationTests" --logger "console;verbosity=detailed"
```

### Matriz de Casos de API / Integración Evaluados
| # | Caso | Método de Prueba | Condición Probada | Payload / Parámetros | HTTP Esperado | Comprobación en BD InMemory | Estado |
|:---:|:---:|:---|:---|:---|:---:|:---|:---:|
| 1 | **API-09** | `GetReservasPorDia_ConFechaConEstancia_Retorna200OkYListaReservas` | Día con estancias activas | `fecha = Hoy + 12d` | `200 OK` | Retorna lista con reserva semilla de Carlos Miranda | ✅ **Passed** |
| 2 | **API-10** | `GetReservasPorDia_ConFechaSinEstancias_Retorna200OkYListaVacia` | Día sin ninguna reserva | `fecha = Hoy + 50d` | `200 OK` | Retorna array JSON vacío (`[]`) sin errores 500 | ✅ **Passed** |
| 3 | **API-11** | `GetReservaPorId_ConIdExistente_Retorna200OkYObjetoReserva` | Búsqueda por ID existente | `id = 1` | `200 OK` | Objeto JSON con `Id = 1`, cliente "Carlos Miranda" | ✅ **Passed** |
| 4 | **API-12** | `GetReservaPorId_ConIdInexistente_Retorna404NotFound` | Búsqueda por ID no registrado | `id = 9999` | `404 NotFound` | Manejo semántico de error 404 sin excepciones no controladas | ✅ **Passed** |

### Resumen de Pruebas de Integración
* **Total:** 4 | **Pasadas:** 4 (100%) | **Fallidas:** 0 | **Omitidas:** 0 | **Tiempo:** ~4.50 s

---

## 3. Ejecución Aislada de Pruebas Unitarias

### Comando Ejecutado
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ReservaPublicaServiceTests.ObtenerPorDia_FechaValida_RetornaReservas | FullyQualifiedName~ReservaPublicaServiceTests.ObtenerTodas_ListaCompleta_RetornaReservas | FullyQualifiedName~ReservaPublicaServiceTests.ObtenerPorId_IdExistente_RetornaReserva | FullyQualifiedName~ReservaPublicaServiceTests.ObtenerPorId_IdInexistente_RetornaNull" --logger "console;verbosity=detailed"
```

### Matriz de Casos Unitarios Evaluados (Patrón AAA con Moq)
| # | Caso | Método de Prueba | Condición Probada | Entrada Simulada | Salida Esperada | Resultado Real | Estado |
|:---:|:---:|:---|:---|:---:|:---:|:---:|:---:|
| 1 | **UT-13** | `ObtenerPorDia_FechaValida_RetornaReservas` | Recuperación de reservas activas para fecha dada | `fecha = DateTime.Today` | Lista con 2 reservas (`Count == 2`) | `Verify Times.Once` | ✅ **Passed** |
| 2 | **UT-14** | `ObtenerTodas_ListaCompleta_RetornaReservas` | Recuperación completa del catálogo | 2 reservas en repositorio | Colección con 2 elementos íntegros | `Verify Times.Once` | ✅ **Passed** |
| 3 | **UT-15** | `ObtenerPorId_IdExistente_RetornaReserva` | Consulta puntual por ID existente | `id = 1` | Entidad con `Id == 1` y datos exactos | `Verify Times.Once` | ✅ **Passed** |
| 4 | **UT-16** | `ObtenerPorId_IdInexistente_RetornaNull` | Consulta puntual por ID no registrado | `id = 999` | Retorno nulo controlado (`null`) | `Verify Times.Once` | ✅ **Passed** |

### Resumen de Pruebas Unitarias
* **Total:** 4 | **Pasadas:** 4 (100%) | **Fallidas:** 0 | **Omitidas:** 0 | **Tiempo:** ~1.20 s

---

## 4. Log Oficial de Consola (Compilado de Evidencias)

### Salida de Integración (`ConsultasReservaIntegrationTests`):
```text
Serie de pruebas para SistemaReservaHotel.IntegrationTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.4+50e68bbb8b (64-bit .NET 10.0.11)
[xUnit.net 00:00:00.25]   Discovering: SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:00.35]   Discovered:  SistemaReservaHotel.IntegrationTests
[xUnit.net 00:00:00.41]   Starting:    SistemaReservaHotel.IntegrationTests
info: Microsoft.Hosting.Lifetime[0]
      Application started. Hosting environment: Testing
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetReservasPorDia_ConFechaConEstancia_Retorna200OkYListaReservas [2 s]
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetReservasPorDia_ConFechaSinEstancias_Retorna200OkYListaVacia [14 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetReservaPorId_ConIdExistente_Retorna200OkYObjetoReserva [12 ms]
  Correctas SistemaReservaHotel.IntegrationTests.ConsultasReservaIntegrationTests.GetReservaPorId_ConIdInexistente_Retorna404NotFound [11 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 4
     Correcto: 4
     Fallidas: 0
    Omitidas: 0
 Tiempo total: 4.5120 Segundos
```

### Salida de Unitarias (`ReservaPublicaServiceTests`):
```text
Serie de pruebas para SistemaReservaHotel.UnitTests.dll (.NETCoreApp,Version=v10.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 10.0.11)
[xUnit.net 00:00:00.18]   Discovering: SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.24]   Discovered:  SistemaReservaHotel.UnitTests
[xUnit.net 00:00:00.25]   Starting:    SistemaReservaHotel.UnitTests
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ObtenerPorDia_FechaValida_RetornaReservas [85 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ObtenerTodas_ListaCompleta_RetornaReservas [3 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ObtenerPorId_IdExistente_RetornaReserva [2 ms]
  Correctas SistemaReservaHotel.UnitTests.ReservaPublicaServiceTests.ObtenerPorId_IdInexistente_RetornaNull [2 ms]

La serie de pruebas se ejecutó correctamente.
Pruebas totales: 4
     Correcto: 4
     Fallidas: 0
    Omitidas: 0
 Tiempo total: 1.1980 Segundos
```

---

## 5. Declaración de Cumplimiento
Este reporte certifica que las **8 pruebas automatizadas (4 de integración + 4 unitarias)** asignadas a **Carlos Alonso Miranda Rivas** fueron ejecutadas y aprobadas en un 100%, cumpliendo rigurosamente los criterios de aceptación para la historia **US-14 (#1034)** y los casos de prueba **TC-02 (#1047)** y **TC-08 (#1053)** en Azure DevOps y el Test Plan `#1054`.
