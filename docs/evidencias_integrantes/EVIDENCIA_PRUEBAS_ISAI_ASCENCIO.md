# Reporte Individual de Evidencias de QA — Ariel Isai Ascencio Ramos (`Isai Ascencio`)

**Proyecto:** Sistema de Reserva Hotel — QA  
**Repositorio Azure DevOps:** `cc25003/SistemaReservaHotel-QA`  
**Rama:** `main`  
**Integrante:** Ariel Isai Ascencio Ramos (`Isai Ascencio`)  
**Historia de Usuario (PBI):** **US-16 (#1040)** — *Validación de rango de fechas y reglas cronológicas de reserva*  
**Test Plan Oficial:** Test Plan `#1054` (Suite `#1055`)  
**Fecha de Ejecución y Validación:** 22 de Octubre de 2026  
**Resultado Global:** **100% de Pruebas Pasadas (11 de 11 Aprobadas: 6 Unitarias + 5 Integración / API)**  

---

## 1. Alcance Asignado (Los 3 Niveles de Prueba)

| Nivel de Prueba | Identificadores | Componente / Endpoint | Estado |
|:---|:---|:---|:---:|
| **🌐 Aceptación (UAT)** | **TC-01 (#1046)**<br>**TC-07 (#1052)** | Login Admin en UI (`/Account/Login`)<br>Validación criptográfica SHA-256 en Login | Documentado en Azure Test Plan |
| **🔌 API / Integración** | **API-01 a API-05** | `POST /Reserva/Crear` con TestHost / InMemory DB | ✅ **5 / 5 Passed (100%)** |
| **⚙️ Pruebas Unitarias** | **UT-01 a UT-06** | `ValidadorFechas.FechasValidas` con xUnit | ✅ **6 / 6 Passed (100%)** |

---

## 2. Ejecución Aislada de Pruebas de API e Integración

### Comando Ejecutado
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ValidadorFechasIntegrationTests" --logger "console;verbosity=detailed"
```

### Matriz de Casos de API / Integración Evaluados
| # | Caso | Método de Prueba | Condición Probada | HTTP Esperado | HTTP Obtenido | Estado |
|:---:|:---:|:---|:---|:---:|:---:|:---:|
| 1 | **API-01** | `PostReserva_ConFechasValidas_Retorna200OkYPersisteReserva` | Rango futuro válido y persistencia en DB | `200 OK` | `200 OK` | ✅ **Passed** |
| 2 | **API-02** | `PostReserva_ConFechaEntradaMayorQueSalida_Retorna400BadRequest` | Rango cronológico invertido | `400 BadRequest` | `400 BadRequest` | ✅ **Passed** |
| 3 | **API-03** | `PostReserva_ConFechaEntradaIgualASalida_Retorna400BadRequest` | Estancia nula (0 noches) | `400 BadRequest` | `400 BadRequest` | ✅ **Passed** |
| 4 | **API-04** | `PostReserva_ConFechaEntradaEnElPasado_Retorna400BadRequest` | Check-in extemporáneo en el pasado | `400 BadRequest` | `400 BadRequest` | ✅ **Passed** |
| 5 | **API-05** | `PostReserva_ConAmbasFechasEnElPasado_Retorna400BadRequest` | Rango de fechas caducado | `400 BadRequest` | `400 BadRequest` | ✅ **Passed** |

### Resumen de Pruebas de Integración
* **Total:** 5 | **Pasadas:** 5 (100%) | **Fallidas:** 0 | **Omitidas:** 0 | **Tiempo:** ~7.00 s

---

## 3. Ejecución Aislada de Pruebas Unitarias

### Comando Ejecutado
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ValidadorFechasTests" --logger "console;verbosity=detailed"
```

### Matriz de Casos Unitarios Evaluados
| # | Caso | Método de Prueba | Condición Probada | Entrada Simula | Salida Esperada | Resultado Real | Estado |
|:---:|:---:|:---|:---|:---:|:---:|:---:|:---:|
| 1 | **UT-01** | `FechasValidas_EntradaFuturaYMenorQueSalida_RetornaTrue` | Rango normal de fechas futuras válidas | `Hoy + 2d` a `Hoy + 5d` | `True` | `True` | ✅ **Passed** |
| 2 | **UT-02** | `FechasValidas_EntradaHoyYSalidaManana_RetornaTrue` | Check-in inmediato hoy con 1 noche | `Hoy` a `Hoy + 1d` | `True` | `True` | ✅ **Passed** |
| 3 | **UT-03** | `FechasValidas_EntradaMayorQueSalida_RetornaFalse` | Entrada posterior a salida (invertidas) | `Hoy + 5d` a `Hoy + 2d` | `False` | `False` | ✅ **Passed** |
| 4 | **UT-04** | `FechasValidas_EntradaIgualASalida_RetornaFalse` | Check-in y check-out mismo día (0 noches) | `Hoy + 2d` a `Hoy + 2d` | `False` | `False` | ✅ **Passed** |
| 5 | **UT-05** | `FechasValidas_EntradaEnElPasado_RetornaFalse` | Check-in en fecha pasada | `Hoy - 1d` a `Hoy + 2d` | `False` | `False` | ✅ **Passed** |
| 6 | **UT-06** | `FechasValidas_EntradaYSalidaEnElPasado_RetornaFalse` | Ambas fechas en el pasado | `Hoy - 5d` a `Hoy - 2d` | `False` | `False` | ✅ **Passed** |

### Resumen de Pruebas Unitarias
* **Total:** 6 | **Pasadas:** 6 (100%) | **Fallidas:** 0 | **Omitidas:** 0 | **Tiempo:** ~4.89 s

---

## 4. Log Oficial de Consola (Compilado de Evidencias)

### Salida de Integración (`ValidadorFechasIntegrationTests`):
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

### Salida de Unitarias (`ValidadorFechasTests`):
```text
Serie de pruebas para C:\Users\isaia\Downloads\ReservaHotel1\SistemaReservaHotel\SistemaReservaHotel.UnitTests\bin\Debug\net8.0\SistemaReservaHotel.UnitTests.dll (.NETCoreApp,Version=v8.0)
1 archivos de prueba en total coincidieron con el patrón especificado.
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.8.2+699d445a1a (64-bit .NET 8.0.24)
[xUnit.net 00:00:01.37]   Discovering: SistemaReservaHotel.UnitTests
[xUnit.net 00:00:01.55]   Discovered:  SistemaReservaHotel.UnitTests
[xUnit.net 00:00:01.57]   Starting:    SistemaReservaHotel.UnitTests
[xUnit.net 00:00:02.21]   Finished:    SistemaReservaHotel.UnitTests
  Correctas SistemaReservaHotel.UnitTests.ValidadorFechasTests.FechasValidas_EntradaYSalidaEnElPasado_RetornaFalse [483 ms]
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
 Tiempo total: 4,8921 Segundos
```

---

## 5. Declaración de Cumplimiento
Este reporte certifica que las **11 pruebas automatizadas (5 de integración + 6 unitarias)** asignadas a **Ariel Isai Ascencio Ramos** fueron ejecutadas y aprobadas en un 100%, cumpliendo rigurosamente los criterios de aceptación para la historia **US-16 (#1040)** en Azure DevOps y el Test Plan `#1054`.
