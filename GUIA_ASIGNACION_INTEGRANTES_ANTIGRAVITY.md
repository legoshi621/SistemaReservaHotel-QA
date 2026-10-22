# Guía de Asignación Integral de Pruebas y Automatización en Antigravity
## Sistema de Reserva Hotel (`SistemaReservaHotel-QA`)

> **Institución:** ESFE-AGAPE (2026)  
> **Organización Azure DevOps:** `cc25003`  
> **Proyecto:** `SistemaReservaHotel-QA`  
> **Test Plan Oficial:** `#1054` (Suite `#1055`) — Sprint 1  
> **Equipo QA:** 4 integrantes  
> **Alcance:** Pruebas de Aceptación (UAT), Pruebas de API (REST / Integración) y Pruebas Unitarias (Moq & AAA)

---

## 🎯 Instrucción Rápida de Uso en Antigravity

> [!IMPORTANT]
> **¿Cómo usar este archivo con Antigravity?**  
> Cuando cualquier integrante del equipo traiga los cambios con `git pull` y abra este proyecto en **Antigravity**, solo debe copiar y pegar su **Prompt Individual** (Sección 3) o escribir su **nombre / alias**:
> 
> * `Isai Ascencio` (o `Ariel Ascencio`, `Ariel Isai Ascencio Ramos`)
> * `Joselin Herrera` (o `Joselin Esmeralda Herrera Flores`)
> * `Alonso Rivas` (o `Carlos Miranda`, `Carlos Alonso Miranda Rivas`)
> * `cami calderon` (o `Camila Calderón`, `Camila Antonia Calderón Cortez`)
> 
> Al recibir el nombre o prompt, Antigravity identificará automáticamente sus casos de Aceptación, sus pruebas de API y sus pruebas unitarias, ejecutará sus pruebas y generará su reporte individual de evidencias listo para entregar.

---

## 1. Matriz Maestra de Asignación por Integrante (Los 3 Niveles)

| Integrante | Historia Azure (PBI) | 🌐 Pruebas de Aceptación (UAT) | 🔌 Pruebas de API (HTTP / REST) | ⚙️ Pruebas Unitarias (Moq / AAA) |
| :--- | :---: | :---: | :---: | :---: |
| **Ariel Isai Ascencio Ramos**<br>`Isai Ascencio` | **US-16 (#1040)**<br>*(Reglas Cronológicas & Auth)* | **TC-01:** Login Admin en UI<br>**TC-07:** Hash SHA-256 en Login | **API-01 a API-05:**<br>`POST /Reserva/Crear`<br>(Válidas, invertidas, 0 noches, pasado, caducado) | **UT-01 a UT-06:**<br>`ValidadorFechasTests`<br>(6 pruebas de validación temporal) |
| **Joselin Esmeralda Herrera Flores**<br>`Joselin Herrera` | **US-15 (#1037)**<br>*(Disponibilidad & Catálogo)* | **TC-04:** Reporte en UI<br>**TC-05:** Anticolisión en UI | **API-06 a API-08:**<br>`GET /Disponibilidad`<br>(Rango libre, colisión, inactiva) | **UT-07 a UT-12:**<br>`DisponibilidadServiceTests`<br>(6 pruebas de inventario y estado) |
| **Carlos Alonso Miranda Rivas**<br>`Alonso Rivas` | **US-14 (#1034)**<br>*(Consultas & Lectura)* | **TC-02:** Consulta del día en UI<br>**TC-08:** Restricción RBAC en UI | **API-09 a API-12:**<br>`GET /Reserva/ObtenerPorDia`<br>`GET /Reserva/Detalles/{id}` (200 / 404) | **UT-13 a UT-16:**<br>`ReservaPublicaService` (Lectura)<br>(4 pruebas de consulta por día e ID) |
| **Camila Antonia Calderón Cortez**<br>`cami calderon` | **US-SEC-01 (#1043)**<br>*(Mutación, CRUD & Tarifas)* | **TC-03:** Formulario Crear Reserva<br>**TC-06:** Tarifa Temporada ($360) | **API-13 a API-16:**<br>`POST /Reserva/Crear` (CRUD)<br>`POST /Reserva/Cancelar` (200 / 404 / Colisión) | **UT-17 a UT-21:**<br>`ReservaPublicaService` (Escritura)<br>(5 pruebas de persistencia, colisión y cancelación) |

---

## 2. Detalle Individual por Integrante y Comandos de Ejecución

---

### 👤 Integrante 1: Ariel Isai Ascencio Ramos (`Isai Ascencio`)
* **Historia asignada:** **US-16 (#1040)** — *Validación de rango de fechas y reglas cronológicas de reserva*.
* **Casos Test Plan Azure:** `#1046 (TC-01)` y `#1052 (TC-07)`.
* **Pruebas de Aceptación (UAT):**
  1. `TC-01`: Inicio de sesión con usuario administrador y acceso a vista protegida en `/Account/Login`.
  2. `TC-07`: Validación criptográfica de contraseñas con hash SHA-256 en formulario de Login.
* **Pruebas de API (5 pruebas en `ValidadorFechasIntegrationTests.cs`):**
  1. `API-01`: `PostReserva_ConFechasValidas_Retorna200OkYPersisteReserva`
  2. `API-02`: `PostReserva_ConFechaEntradaMayorQueSalida_Retorna400BadRequest`
  3. `API-03`: `PostReserva_ConFechaEntradaIgualASalida_Retorna400BadRequest`
  4. `API-04`: `PostReserva_ConFechaEntradaEnElPasado_Retorna400BadRequest`
  5. `API-05`: `PostReserva_ConAmbasFechasEnElPasado_Retorna400BadRequest`
* **Pruebas Unitarias (6 pruebas en `ValidadorFechasTests.cs`):**
  `UT-01` a `UT-06` (Fechas válidas, check-in inmediato, fechas invertidas, cero noches, fechas en el pasado y caducadas).
* **Comandos de Ejecución:**
  ```powershell
  # Ejecutar sus Pruebas de API / Integración
  dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ValidadorFechasIntegrationTests"

  # Ejecutar sus Pruebas Unitarias
  dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ValidadorFechasTests"
  ```

---

### 👤 Integrante 2: Joselin Esmeralda Herrera Flores (`Joselin Herrera`)
* **Historia asignada:** **US-15 (#1037)** — *Disponibilidad de habitaciones por fechas y reporte de ocupación*.
* **Casos Test Plan Azure:** `#1049 (TC-04)` y `#1050 (TC-05)`.
* **Pruebas de Aceptación (UAT):**
  1. `TC-04`: Generación y filtrado de reporte de reservas por rango de fechas en `/Reportes/Reservas`.
  2. `TC-05`: Control anticolisión en interfaz web ante intento de reservar habitación en fechas ocupadas.
* **Pruebas de API (3 pruebas en `DisponibilidadIntegrationTests.cs`):**
  1. `API-06`: `GetDisponibilidad_EnRangoSinConflicto_Retorna200OkYHabitacionDisponible`
  2. `API-07`: `GetDisponibilidad_EnRangoConReservaActiva_ExcluyeHabitacionOcupada`
  3. `API-08`: `GetDisponibilidad_HabitacionInactiva_NoSeIncluyeEnDisponibles`
* **Pruebas Unitarias (6 pruebas en `DisponibilidadServiceTests.cs`):**
  `UT-07` a `UT-12` (Catálogo sin conflictos, exclusión por reserva activa, reserva cancelada permitida, filtro de inactivas, excepciones y catálogo vacío).
* **Comandos de Ejecución:**
  ```powershell
  # Ejecutar sus Pruebas de API / Integración
  dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~DisponibilidadIntegrationTests"

  # Ejecutar sus Pruebas Unitarias
  dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~DisponibilidadServiceTests"
  ```

---

### 👤 Integrante 3: Carlos Alonso Miranda Rivas (`Alonso Rivas`)
* **Historia asignada:** **US-14 (#1034)** — *Consulta de reservas por día seleccionado y recuperación por ID*.
* **Casos Test Plan Azure:** `#1047 (TC-02)` y `#1053 (TC-08)`.
* **Pruebas de Aceptación (UAT):**
  1. `TC-02`: Consulta de reservas del día y cálculo de noches en `/ReservasAdmin/PorFecha`.
  2. `TC-08`: Restricción de acceso administrativo y redirección a `AccessDenied` para usuario sin rol Administrador.
* **Pruebas de API (4 pruebas en `ConsultasReservaIntegrationTests.cs`):**
  1. `API-09`: `GetObtenerPorDia_ConFechaConReservas_Retorna200OkYListaReservas`
  2. `API-10`: `GetObtenerPorDia_ConFechaSinReservas_Retorna200OkYListaVacia`
  3. `API-11`: `GetDetalles_ConIdExistente_Retorna200OkYReservaCorrespondiente`
  4. `API-12`: `GetDetalles_ConIdInexistente_Retorna404NotFound`
* **Pruebas Unitarias (4 pruebas en `ReservaPublicaServiceTests.cs`):**
  `UT-13` a `UT-16` (Reservas del día, lista vacía en fecha libre, búsqueda por ID existente y retorno null en ID inexistente).
* **Comandos de Ejecución:**
  ```powershell
  # Ejecutar sus Pruebas de API / Integración
  dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ConsultasReservaIntegrationTests"

  # Ejecutar sus Pruebas Unitarias
  dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ObtenerPorDiaAsync|FullyQualifiedName~ObtenerPorIdAsync"
  ```

---

### 👤 Integrante 4: Camila Antonia Calderón Cortez (`cami calderon`)
* **Historia asignada:** **US-SEC-01 (#1043)** — *Ciclo de vida CRUD de reservas, control de colisión y temporadas*.
* **Casos Test Plan Azure:** `#1048 (TC-03)` y `#1051 (TC-06)`.
* **Pruebas de Aceptación (UAT):**
  1. `TC-03`: Creación de reserva mediante formulario web `/ReservasAdmin/Crear` y persistencia.
  2. `TC-06`: Validación de cálculo dinámico de tarifa por temporada alta (3 noches x $120.00 = $360.00).
* **Pruebas de API (4 pruebas en `ReservaCrudIntegrationTests.cs`):**
  1. `API-13`: `PostCrearReserva_PersisteEntidadEnBaseDeDatosInMemory`
  2. `API-14`: `PostCancelarReserva_ActualizaEstadoEnBaseDeDatosInMemory`
  3. `API-15`: `PostCancelarReserva_ConIdInexistente_Retorna404NotFound`
  4. `API-16`: `PostCrearReserva_EnMismasFechasQueReservaActiva_RechazaPorColision`
* **Pruebas Unitarias (5 pruebas en `ReservaPublicaServiceTests.cs`):**
  `UT-17` a `UT-21` (Crear reserva con ID, validación de habitación ocupada, fechas inválidas, cancelar reserva activa y cancelar reserva inexistente).
* **Comandos de Ejecución:**
  ```powershell
  # Ejecutar sus Pruebas de API / Integración
  dotnet test SistemaReservaHotel/SistemaReservaHotel.IntegrationTests/SistemaReservaHotel.IntegrationTests.csproj --filter "FullyQualifiedName~ReservaCrudIntegrationTests"

  # Ejecutar sus Pruebas Unitarias
  dotnet test SistemaReservaHotel/SistemaReservaHotel.UnitTests/SistemaReservaHotel.UnitTests.csproj --filter "FullyQualifiedName~ReservarAsync|FullyQualifiedName~CancelarReservaAsync"
  ```

---

## 3. Prompts de Automatización para Cada Integrante (Copiar y Pegar)

Cada integrante solo debe copiar su prompt y enviarlo en el chat de Antigravity:

### 📋 Prompt 1 — Ariel Isai Ascencio Ramos:
```text
Soy Ariel Isai Ascencio Ramos (Isai Ascencio). Mi responsabilidad en QA es la US-16 (#1040) de Reglas Cronológicas y Seguridad:
1. En Aceptación (UAT): Tengo asignados TC-01 (Login Admin en UI) y TC-07 (Validación de contraseña con hash SHA-256).
2. En Pruebas de API: Desarrollé los casos API-01 al API-05 sobre POST /Reserva/Crear validando fechas futuras, bloqueo de fechas invertidas, 0 noches y fechas en el pasado.
3. En Pruebas Unitarias: Diseñé las 6 pruebas de ValidadorFechasTests (UT-01 a UT-06) con xUnit.
Ejecuta mis pruebas de integración y unitarias de forma aislada y muéstrame el reporte de aprobación al 100%.
```

### 📋 Prompt 2 — Joselin Esmeralda Herrera Flores:
```text
Soy Joselin Esmeralda Herrera Flores (Joselin Herrera). Mi responsabilidad en QA es la US-15 (#1037) de Catálogo y Disponibilidad:
1. En Aceptación (UAT): Tengo asignados TC-04 (Reporte de reservas por fechas en UI) y TC-05 (Control anticolisión en pantalla).
2. En Pruebas de API: Desarrollé los casos API-06 al API-08 sobre GET /Disponibilidad validando rango libre, exclusión de habitaciones ocupadas y filtrado de inactivas.
3. En Pruebas Unitarias: Diseñé las 6 pruebas de DisponibilidadServiceTests (UT-07 a UT-12) con Moq.
Ejecuta mis pruebas de integración y unitarias de forma aislada y muéstrame el reporte de aprobación al 100%.
```

### 📋 Prompt 3 — Carlos Alonso Miranda Rivas:
```text
Soy Carlos Alonso Miranda Rivas (Alonso Rivas). Mi responsabilidad en QA es la US-14 (#1034) de Consultas y Lectura de Reservas:
1. En Aceptación (UAT): Tengo asignados TC-02 (Consulta de reservas del día y cálculo de noches en UI) y TC-08 (Restricción RBAC y AccessDenied).
2. En Pruebas de API: Desarrollé los casos API-09 al API-12 sobre GET /Reserva/ObtenerPorDia y GET /Reserva/Detalles/{id} validando respuestas 200 OK y 404 NotFound.
3. En Pruebas Unitarias: Diseñé las 4 pruebas de lectura de ReservaPublicaServiceTests (UT-13 a UT-16) con Moq.
Ejecuta mis pruebas de integración y unitarias de forma aislada y muéstrame el reporte de aprobación al 100%.
```

### 📋 Prompt 4 — Camila Antonia Calderón Cortez:
```text
Soy Camila Antonia Calderón Cortez (cami calderon). Mi responsabilidad en QA es la US-SEC-01 (#1043) de Mutación de Reservas y Transaccionalidad:
1. En Aceptación (UAT): Tengo asignados TC-03 (Crear reserva en formulario web) y TC-06 (Validación de tarifa de temporada alta $120 x 3 = $360).
2. En Pruebas de API: Desarrollé los casos API-13 al API-16 sobre POST /Reserva/Crear y POST /Reserva/Cancelar validando persistencia, cancelación y control anticolisión.
3. En Pruebas Unitarias: Diseñé las 5 pruebas de mutación de ReservaPublicaServiceTests (UT-17 a UT-21) con Moq.
Ejecuta mis pruebas de integración y unitarias de forma aislada y muéstrame el reporte de aprobación al 100%.
```

---

## 4. Archivo Excel de Entrega Oficial

La planificación completa se encuentra consolidada en un único archivo oficial con 3 pestañas:
* **Archivo:** `Plantilla_Planificacion_Pruebas_COMPLETO_3Pestanas_Definitivo.xlsx`
* **Pestaña 1:** `Pruebas de Aceptación (UAT)` — 8 Casos (`TC-01` a `TC-08`).
* **Pestaña 2:** `Pruebas de API` — 16 Casos (`API-01` a `API-16`).
* **Pestaña 3:** `Pruebas Unitarias` — 21 Casos (`UT-01` a `UT-21`).
