# 🏨 Sistema de Reserva de Hotel — Aseguramiento de la Calidad (QA)

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C# 13](https://img.shields.io/badge/C%23-13.0-239120?logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![xUnit](https://img.shields.io/badge/Testing-xUnit%20%26%20Moq-005A9C)](https://xunit.net/)
[![Azure DevOps](https://img.shields.io/badge/Azure%20DevOps-cc25003-0078D7?logo=azuredevops&logoColor=white)](https://dev.azure.com/cc25003/SistemaReservaHotel-QA)
[![ISO/IEC/IEEE 29119](https://img.shields.io/badge/Norma-ISO%2FIEC%2FIEEE%2029119-success)](https://www.iso.org/)
[![Test Execution](https://img.shields.io/badge/Test%20Execution-100%25%20Passed-brightgreen)](#-ejecución-de-pruebas)

> **Institución:** Escuela Superior Franciscana Especializada (ESFE - AGAPE)  
> **Asignatura:** Aseguramiento de la Calidad de Software (QA)  
> **Docente:** Ing. Gerber Alexis Gómez Hidalgo  
> **Organización Azure DevOps:** [`cc25003/SistemaReservaHotel-QA`](https://dev.azure.com/cc25003/SistemaReservaHotel-QA)  
> **Repositorio Oficial:** [`SistemaReservaHotel-QA`](https://github.com/legoshi621/SistemaReservaHotel-QA)

---

## 📌 Entregables Oficiales de la Auditoría y Planificación

Todos los documentos definitivos de la evaluación se encuentran centralizados en el directorio [`docs/entregables/`](docs/entregables/):

| Entregable | Formato | Descripción | Enlace Directo |
|:---|:---:|:---|:---:|
| **Informe de Diseño de Pruebas** | `Word (.docx)` | Documento formal con código en tema oscuro, 5 capturas oficiales de Azure DevOps y reflexiones | [📥 Descargar DOCX](docs/entregables/Presentacion_Diseno_Casos_Prueba_SistemaReservaHotel.docx) |
| **Informe de Diseño de Pruebas** | `PDF (.pdf)` | Documento oficial exportado (22 páginas, sin portada, con formato institucional completo) | [📄 Ver / Descargar PDF](docs/entregables/Presentacion_Diseno_Casos_Prueba_SistemaReservaHotel.pdf) |
| **Plantilla de Planificación de Pruebas** | `Excel (.xlsx)` | Matriz completa de 3 pestañas: Aceptación (TC-01 a TC-08), API (API-01 a API-16) y Unitarias (UT-01 a UT-21) | [📊 Descargar Excel](docs/entregables/Plantilla_Planificacion_Pruebas_SistemaReservaHotel.xlsx) |
| **Dictamen de Auditoría Final** | `PDF (.pdf)` | Certificación formal de concordancia y cumplimiento normativo ISO 29119 | [📑 Ver / Descargar Dictamen PDF](docs/entregables/Auditoria_Final_Calidad_SistemaReservaHotel.pdf) |
| **Dictamen de Auditoría Final** | `Word (.docx)` | Documento editable del dictamen de auditoría de calidad | [📥 Descargar Dictamen DOCX](docs/entregables/Auditoria_Final_Calidad_SistemaReservaHotel.docx) |

---

## 👥 Matriz de Asignación del Equipo QA (Los 3 Niveles de Prueba)

El ciclo de pruebas del proyecto se diseñó e implementó bajo una estricta distribución modular por **Historia de Usuario (PBI)**, garantizando que cada integrante cubra los **tres niveles fundamentales de prueba** (Unitarias, de API y de Aceptación):

| Integrante | Usuario Azure DevOps | Historia de Usuario (PBI) | Pruebas Unitarias (xUnit + Moq) | Pruebas de API (IntegrationTests) | Pruebas de Aceptación (Azure Test Plans) | Total Casos | Evidencia Individual |
|:---|:---|:---|:---:|:---:|:---:|:---:|:---:|
| **Ariel Isai Ascencio Ramos** | `isai ascencio` | **US-16 (#1040):** Reglas Cronológicas y Tarifas | **UT-01 a UT-06** (6 casos) | **API-01 a API-05** (5 casos) | **TC-01 (#1046)**<br>**TC-07 (#1052)** | **13** | [Ver Evidencia](docs/evidencias_integrantes/EVIDENCIA_PRUEBAS_ISAI_ASCENCIO.md) |
| **Joselin Esmeralda Herrera Flores** | `Joselin Herrera` | **US-15 (#1037):** Disponibilidad y Catálogo | **UT-07 a UT-12** (6 casos) | **API-06 a API-08** (3 casos) | **TC-04 (#1049)**<br>**TC-05 (#1050)** | **11** | [Ver Evidencia](docs/evidencias_integrantes/EVIDENCIA_PRUEBAS_JOSELIN_HERRERA.md) |
| **Carlos Alonso Miranda Rivas** | `Alonso Rivas` | **US-14 (#1034):** Consultas de Reservas y RBAC | **UT-13 a UT-16** (4 casos) | **API-09 a API-12** (4 casos) | **TC-02 (#1047)**<br>**TC-08 (#1053)** | **10** | [Ver Evidencia](docs/evidencias_integrantes/EVIDENCIA_PRUEBAS_ALONSO_RIVAS.md) |
| **Camila Antonia Calderón Cortez** | `cami calderon` | **US-SEC-01 (#1043):** Mutación CRUD y Seguridad | **UT-17 a UT-21** (5 casos) | **API-13 a API-16** (4 casos) | **TC-03 (#1048)**<br>**TC-06 (#1051)** | **11** | [Ver Evidencia](docs/evidencias_integrantes/EVIDENCIA_PRUEBAS_CAMILA_CALDERON.md) |
| **TOTALES CONSOLIDADOS** | *4 Integrantes* | *4 Historias de Usuario* | **21 Casos** | **16 Casos** | **8 Casos** | **45** | [Resumen](docs/entregables/Presentacion_Diseno_Casos_Prueba_SistemaReservaHotel.pdf) |

---

## ☁️ Arquitectura de Gestión de Calidad en Azure DevOps

Todo el ciclo de vida de QA se encuentra administrado en **Azure DevOps** bajo la norma **ISO/IEC/IEEE 29119**:

1. **Test Plan Maestro (#1054):** `Plan de Pruebas - Sistema de Reserva de Hotel`. Define el alcance modular (Suites TS01 a TS04), criterios de entrada/salida y políticas de aceptación.
2. **Test Suite (#1055):** Contiene los **8 Casos de Prueba de Aceptación (TC-01 a TC-08)** formalmente parametrizados y vinculados a sus respectivos PBIs.
3. **Azure Boards (Backlog & Tablero Kanban):** Trazabilidad bidireccional entre requerimientos de usuario y casos de prueba.
4. **Progress Report:** Certificación oficial de ejecución con **100% Run (8/8 pruebas ejecutadas)** y **100% Pass Rate (tasa de aprobación verde)**.

> 📷 Las evidencias visuales oficiales se encuentran en [`docs/capturas_azure/`](docs/capturas_azure/):
> - [`1_test_plan_maestro.png`](docs/capturas_azure/1_test_plan_maestro.png) — Vista del Test Plan Maestro #1054.
> - [`2_test_suite_casos.png`](docs/capturas_azure/2_test_suite_casos.png) — Suite #1055 con los 8 casos de prueba asignados.
> - [`3_backlog_pbis.png`](docs/capturas_azure/3_backlog_pbis.png) — Backlog de PBIs en Azure Boards.
> - [`4_tablero_kanban.png`](docs/capturas_azure/4_tablero_kanban.png) — Tablero Kanban ágil por integrante.
> - [`5_progress_report.png`](docs/capturas_azure/5_progress_report.png) — Gráfica oficial de Progress Report (100% Passed).

---

## 🧪 Ejecución de Pruebas Automatizadas

Para reproducir la ejecución completa de la suite de pruebas desde la terminal:

### 1. Pruebas Unitarias (21 casos — xUnit & Moq)
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --logger "console;verbosity=normal"
```

### 2. Pruebas de API / Integración (16 casos — TestHost & InMemory)
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --logger "console;verbosity=normal"
```

### 3. Pruebas de Aceptación UAT (8 casos — End-to-End)
```powershell
dotnet test SistemaReservaHotel/SistemaReservaHotel.Tests/SistemaReservaHotel.Tests.csproj --logger "console;verbosity=normal"
```

---

## 📂 Estructura del Repositorio

```text
SistemaReservaHotel-QA/
├── .gitignore
├── azure-pipelines.yml              # Pipeline de CI/CD para compilación y pruebas
├── README.md                        # Presentación ejecutiva principal
├── SistemaReservaHotel/             # Código fuente de la solución .NET 10
│   ├── SistemaReservaHotel.Entities/
│   ├── SistemaReservaHotel.DataAccess/
│   ├── SistemaReservaHotel.Business/
│   ├── SistemaReservaHotel.WebMVC/
│   ├── SistemaReservaHotel.UnitTests/        # 21 Pruebas Unitarias (AAA + Moq)
│   ├── SistemaReservaHotel.IntegrationTests/ # 16 Pruebas de API (HTTP TestHost)
│   └── SistemaReservaHotel.Tests/            # 8 Pruebas de Aceptación UAT
└── docs/                            # Documentación y entregables organizados
    ├── entregables/                 # Documentos oficiales finales
    │   ├── Presentacion_Diseno_Casos_Prueba_SistemaReservaHotel.docx
    │   ├── Presentacion_Diseno_Casos_Prueba_SistemaReservaHotel.pdf
    │   └── Plantilla_Planificacion_Pruebas_SistemaReservaHotel.xlsx
    ├── evidencias_integrantes/      # Evidencia individual detallada por estudiante
    │   ├── EVIDENCIA_PRUEBAS_ALONSO_RIVAS.md
    │   ├── EVIDENCIA_PRUEBAS_CAMILA_CALDERON.md
    │   ├── EVIDENCIA_PRUEBAS_ISAI_ASCENCIO.md
    │   └── EVIDENCIA_PRUEBAS_JOSELIN_HERRERA.md
    ├── capturas_azure/              # 5 capturas de pantalla oficiales de Azure DevOps
    │   ├── 1_test_plan_maestro.png
    │   ├── 2_test_suite_casos.png
    │   ├── 3_backlog_pbis.png
    │   ├── 4_tablero_kanban.png
    │   └── 5_progress_report.png
    └── especificaciones_tecnicas/   # Especificaciones técnicas y planes de soporte
        ├── AZURE_DEVOPS_QA.md
        ├── DOCUMENTACION_PRUEBAS_UNITARIAS.md
        ├── DOCUMENTACION_PRUEBAS_INTEGRACION.md
        ├── DOCUMENTACION_SISTEMA.md
        ├── Plan_de_Pruebas.md
        └── Plan_de_Pruebas.docx
```

---

## 🌿 Gestión de Ramas (GitFlow)

- **`main`:** Rama principal de producción y entrega final. Contiene la versión auditada, estable y documentada.
- **`develop`:** Rama de desarrollo e integración activa. Sincronizada con la versión de entrega para garantizar continuidad en el ciclo DevOps.