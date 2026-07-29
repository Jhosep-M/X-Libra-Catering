# Mejoras UX, Auditoría y Dashboard — Plan de Implementación

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Implementar las 6 mejoras de prioridad media: loading states, toasts, paginación, auditoría, soft delete y dashboard con gráficos.

**Architecture:** Capa Server (API) + Capa Cliente (Blazor WASM). Las mejoras se distribuyen entre ambas: auditoría/soft-delete son principalmente server-side con migraciones; loading/toasts/dashboard son cliente-side; paginación toca ambas.

**Tech Stack:** .NET 8/9, EF Core 9, Blazor WASM, Chart.js (via JS interop), CSS custom properties.

---

## File Structure

### Crear:
- `X-Libra_Catering.Cliente/Shared/LoadingSpinner.razor` — componente reutilizable de carga
- `X-Libra_Catering.Cliente/Shared/ToastNotification.razor` — componente de notificaciones toast
- `X-Libra_Catering.Cliente/Services/ServicioNotificacion.cs` — servicio singleton para cola de toasts
- `X-Libra_Catering.Cliente/Pages/Dashboard.razor` — página de dashboard con gráficos
- `X-Libra_Catering.Cliente/wwwroot/js/dashboard-charts.js` — JS interop para Chart.js
- `X-Libra_Catering.Server/Controllers/DashboardController.cs` — endpoint de datos agregados
- `X-Libra_Catering.Shared/DashboardCompletoDTO.cs` — DTO con datos para gráficos

### Modificar:
- **Server:** BdXLibraCateringContext.cs (audit interceptors), todos los Models (add audit/soft-delete fields), todos los Controllers (pagination params, soft-delete queries)
- **Shared:** todos los DTOs (add audit fields)
- **Cliente:** todas las páginas Razor (loading states, toasts), todos los servicios (pagination params), NavMenu.razor (dashboard link), index.html (Chart.js CDN), app.css (toast/loading styles)

---

## Tasks

### Task 1: LoadingSpinner + ToastNotification Components

**Files:**
- Create: `X-Libra_Catering.Cliente/Shared/LoadingSpinner.razor`
- Create: `X-Libra_Catering.Cliente/Services/ServicioNotificacion.cs`
- Create: `X-Libra_Catering.Cliente/Shared/ToastNotification.razor`
- Modify: `X-Libra_Catering.Cliente/Program.cs` (registrar servicio)
- Modify: `X-Libra_Catering.Cliente/Layout/MainLayout.razor` (incluir ToastNotification)
- Modify: `X-Libra_Catering.Cliente/wwwroot/css/app.css` (estilos toast + spinner)

**Interfaces:**
- Consumes: nothing
- Produces: `ServicioNotificacion` (singleton), `LoadingSpinner` (component), `ToastNotification` (component)

- [ ] **Step 1: Crear ServicioNotificacion.cs**

```csharp
namespace X_Libra_Catering.Cliente.Services;

public class ServicioNotificacion
{
    public event Action<string, string>? OnNotificar;
    public enum Tipo { Exito, Error, Info }

    public void Mostrar(string mensaje, Tipo tipo = Tipo.Exito)
    {
        OnNotificar?.Invoke(mensaje, tipo switch
        {
            Tipo.Exito => "exito",
            Tipo.Error => "error",
            Tipo.Info => "info",
            _ => "info"
        });
    }
}
```

- [ ] **Step 2: Crear ToastNotification.razor**

```razor
@inject ServicioNotificacion Notificacion

<div class="toast-container">
    @foreach (var t in toasts)
    {
        <div class="toast toast-@t.Tipo">@t.Mensaje</div>
    }
</div>

@code {
    private List<(string Mensaje, string Tipo)> toasts = new();

    protected override void OnInitialized()
    {
        Notificacion.OnNotificar += (msg, tipo) =>
        {
            toasts.Add((msg, tipo));
            StateHasChanged();
            _ = Task.Run(async () =>
            {
                await Task.Delay(3000);
                toasts.Remove((msg, tipo));
                await InvokeAsync(StateHasChanged);
            });
        };
    }
}
```

- [ ] **Step 3: Crear LoadingSpinner.razor**

```razor
@if (Visible)
{
    <div class="loading-overlay">
        <div class="loading-spinner"></div>
        <p>@Mensaje</p>
    </div>
}

@code {
    [Parameter] public bool Visible { get; set; }
    [Parameter] public string Mensaje { get; set; } = "Cargando...";
}
```

- [ ] **Step 4: Registrar servicio en Program.cs**

```csharp
builder.Services.AddSingleton<ServicioNotificacion>();
```

- [ ] **Step 5: Incluir en MainLayout.razor**

Agregar `<ToastNotification />` dentro del layout.

- [ ] **Step 6: Agregar estilos en app.css**

```css
.toast-container {
  position: fixed;
  top: 16px;
  right: 16px;
  z-index: 9999;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.toast {
  padding: 12px 20px;
  border-radius: var(--radius-md);
  color: #fff;
  font-weight: 500;
  box-shadow: 0 4px 12px rgba(0,0,0,0.15);
  animation: slideIn 0.3s ease;
}

.toast-exito { background: var(--color-primary, #0d9488); }
.toast-error { background: var(--color-danger, #dc2626); }
.toast-info  { background: #2563eb; }

@keyframes slideIn {
  from { transform: translateX(100%); opacity: 0; }
  to   { transform: translateX(0); opacity: 1; }
}

.loading-overlay {
  position: fixed;
  inset: 0;
  background: rgba(255,255,255,0.7);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  z-index: 9998;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  border: 4px solid var(--color-border);
  border-top-color: var(--color-primary);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}
```

---

### Task 2: Integrar Loading + Toasts en todas las páginas

**Files:**
- Modify: `X-Libra_Catering.Cliente/Pages/Clientes.razor`
- Modify: `X-Libra_Catering.Cliente/Pages/ClienteForm.razor`
- Modify: `X-Libra_Catering.Cliente/Pages/EventosDashboard.razor`
- Modify: `X-Libra_Catering.Cliente/Pages/EventoForm.razor`
- Modify: `X-Libra_Catering.Cliente/Pages/Menus.razor`
- Modify: `X-Libra_Catering.Cliente/Pages/MenuForm.razor`
- Modify: `X-Libra_Catering.Cliente/Pages/Vehiculos.razor`
- Modify: `X-Libra_Catering.Cliente/Pages/VehiculoForm.razor`
- Modify: `X-Libra_Catering.Cliente/Pages/Pedidos.razor`
- Modify: `X-Libra_Catering.Cliente/Pages/PedidoForm.razor`

**Interfaces:**
- Consumes: `LoadingSpinner`, `ServicioNotificacion` (from Task 1)

- [ ] **Step 1: Cada página — agregar `cargando` field + `LoadingSpinner`**

```razor
<LoadingSpinner Visible="cargando" Mensaje="Cargando datos..." />

@code {
    private bool cargando;
}
```

- [ ] **Step 2: Envolver llamadas API con loading**

```csharp
cargando = true;
StateHasChanged();
try {
    lista = await Servicio.Lista();
} finally {
    cargando = false;
    StateHasChanged();
}
```

- [ ] **Step 3: Reemplazar `Console.WriteLine` con toasts en errores**

```csharp
catch (Exception ex) {
    notificacion.Mostrar($"Error: {ex.Message}", ServicioNotificacion.Tipo.Error);
}
```

- [ ] **Step 4: Agregar toast de éxito en formularios (Guardar/Eliminar)**

```csharp
notificacion.Mostrar("Guardado correctamente", ServicioNotificacion.Tipo.Exito);
```

Inyectar `@inject ServicioNotificacion Notificacion` en cada página.

---

### Task 3: Auditoría (FECHA_CREACION + FECHA_MODIFICACION)

**Files:**
- Modify: `X-Libra_Catering.Server/Models/Cliente.cs`
- Modify: `X-Libra_Catering.Server/Models/Evento.cs`
- Modify: `X-Libra_Catering.Server/Models/Menu.cs`
- Modify: `X-Libra_Catering.Server/Models/Vehiculo.cs`
- Modify: `X-Libra_Catering.Server/Models/PedidoCabecera.cs`
- Modify: `X-Libra_Catering.Server/Models/PedidoDetalle.cs`
- Modify: `X-Libra_Catering.Server/Data/BdXLibraCateringContext.cs`
- Create: migration `AddAuditFields`
- Modify: `X-Libra_Catering.Shared/ClienteDTO.cs`
- Modify: `X-Libra_Catering.Shared/EventoDTO.cs`
- Modify: `X-Libra_Catering.Shared/MenuDTO.cs`
- Modify: `X-Libra_Catering.Shared/VehiculoDTO.cs`
- Modify: `X-Libra_Catering.Shared/PedidoCabeceraDTO.cs`
- Modify: `X-Libra_Catering.Shared/PedidoDetalleDTO.cs`

**Interfaces:**
- Consumes: nothing
- Produces: audit fields on all entities

- [ ] **Step 1: Agregar campos a cada modelo**

```csharp
public DateTime? FechaCreacion { get; set; }
public DateTime? FechaModificacion { get; set; }
```

- [ ] **Step 2: Mapear columnas en DbContext**

```csharp
entity.Property(e => e.FechaCreacion).HasColumnName("FECHA_CREACION");
entity.Property(e => e.FechaModificacion).HasColumnName("FECHA_MODIFICACION");
```

- [ ] **Step 3: Auto-asignar en SaveChangesAsync — override en DbContext**

```csharp
public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
{
    foreach (var entry in ChangeTracker.Entries<IAuditable>())
    {
        if (entry.State == EntityState.Added)
            entry.Entity.FechaCreacion = DateTime.UtcNow;
        if (entry.State == EntityState.Modified)
            entry.Entity.FechaModificacion = DateTime.UtcNow;
    }
    return await base.SaveChangesAsync(ct);
}
```

Crear interfaz `IAuditable` en Models con las dos propiedades, implementar en todas las entidades.

- [ ] **Step 4: Crear migración**

```bash
dotnet ef migrations add AddAuditFields --project src/Server
dotnet ef database update --project src/Server
```

- [ ] **Step 5: Agregar campos a todos los DTOs en Shared**

```csharp
public DateTime? FechaCreacion { get; set; }
public DateTime? FechaModificacion { get; set; }
```

- [ ] **Step 6: Mapear en todos los Controllers (Lista + Buscar)**

Agregar `FechaCreacion = e.FechaCreacion, FechaModificacion = e.FechaModificacion` en cada mapeo entidad→DTO.

---

### Task 4: Soft Delete (ACTIVO)

**Files:**
- Modify: `X-Libra_Catering.Server/Models/Cliente.cs`
- Modify: `X-Libra_Catering.Server/Models/Evento.cs`
- Modify: `X-Libra_Catering.Server/Models/Menu.cs`
- Modify: `X-Libra_Catering.Server/Models/Vehiculo.cs`
- Modify: `X-Libra_Catering.Server/Data/BdXLibraCateringContext.cs`
- Create: migration `AddSoftDelete`
- Modify: `X-Libra_Catering.Server/Controllers/ClientesController.cs`
- Modify: `X-Libra_Catering.Server/Controllers/EventosController.cs`
- Modify: `X-Libra_Catering.Server/Controllers/MenusController.cs`
- Modify: `X-Libra_Catering.Server/Controllers/VehiculosController.cs`
- Modify: `X-Libra_Catering.Server/Controllers/PedidosController.cs`

**Interfaces:**
- Consumes: Task 3 (audit fields pattern)
- Produces: soft-delete on all entities

- [ ] **Step 1: Agregar campo Activo a IAuditable o a cada entidad**

```csharp
public bool Activo { get; set; } = true;
```

Columna: `ACTIVO` bit not null default 1.

- [ ] **Step 2: Crear migration**

```bash
dotnet ef migrations add AddSoftDelete --project src/Server
dotnet ef database update --project src/Server
```

- [ ] **Step 3: Modificar Lista en cada controller — filtrar solo activos**

```csharp
var lista = await _context.Clientes.Where(c => c.Activo).ToListAsync();
```

- [ ] **Step 4: Modificar Eliminar — cambiar flag en vez de Remove**

```csharp
// Antes:
_context.Clientes.Remove(entidad);

// Después:
entidad.Activo = false;
entidad.FechaModificacion = DateTime.UtcNow;
```

---

### Task 5: Paginación + Búsqueda Server-Side

**Files:**
- Create: `X-Libra_Catering.Shared/PaginacionDTO.cs`
- Modify: `X-Libra_Catering.Server/Controllers/ClientesController.cs` — endpoint Lista
- Modify: `X-Libra_Catering.Server/Controllers/EventosController.cs`
- Modify: `X-Libra_Catering.Server/Controllers/MenusController.cs`
- Modify: `X-Libra_Catering.Server/Controllers/VehiculosController.cs`
- Modify: `X-Libra_Catering.Server/Controllers/PedidosController.cs`
- Modify: `X-Libra_Catering.Cliente/Services/ServicioCliente.cs`
- Modify: `X-Libra_Catering.Cliente/Services/ServicioEvento.cs`
- Modify: `X-Libra_Catering.Cliente/Services/ServicioMenu.cs`
- Modify: `X-Libra_Catering.Cliente/Services/ServicioVehiculo.cs`
- Modify: `X-Libra_Catering.Cliente/Services/ServicioPedido.cs`
- Modify: `X-Libra_Catering.Cliente/Pages/Clientes.razor`

**Interfaces:**
- Consumes: ResponseAPI<T> pattern
- Produces: paginated endpoints + UI

- [ ] **Step 1: Crear PaginacionDTO.cs en Shared**

```csharp
namespace X_Libra_Catering.Shared;

public class PaginacionDTO
{
    public int Pagina { get; set; } = 1;
    public int Tamano { get; set; } = 20;
    public string? Busqueda { get; set; }
}

public class ResultadoPaginado<T>
{
    public List<T> Items { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int TotalPaginas => (int)Math.Ceiling((double)Total / Tamano);
    public int Tamano { get; set; }
}
```

- [ ] **Step 2: Modificar endpoints Lista — aceptar query params paginación**

Agregar sobrecarga o modificar Lista existente:

```csharp
[HttpGet("Lista")]
public async Task<IActionResult> Lista([FromQuery] int pagina = 1, [FromQuery] int tamano = 20, [FromQuery] string? busqueda = null)
{
    var query = _context.Clientes.Where(c => c.Activo).AsQueryable();

    if (!string.IsNullOrWhiteSpace(busqueda))
        query = query.Where(c => c.Nombre.Contains(busqueda) || c.Email.Contains(busqueda));

    var total = await query.CountAsync();
    var items = await query
        .OrderBy(c => c.Nombre)
        .Skip((pagina - 1) * tamano)
        .Take(tamano)
        .Select(c => new ClienteDTO { ... })
        .ToListAsync();

    return Ok(new ResponseAPI<ResultadoPaginado<ClienteDTO>>
    {
        EsCorrecto = true,
        Valor = new ResultadoPaginado<ClienteDTO> { Items = items, Total = total, Pagina = pagina, Tamano = tamano }
    });
}
```

- [ ] **Step 3: Actualizar servicios del cliente para pasar query params**

```csharp
public async Task<ResultadoPaginado<ClienteDTO>> Lista(int pagina = 1, int tamano = 20, string? busqueda = null)
{
    var r = await Http.GetFromJsonAsync<ResponseAPI<ResultadoPaginado<ClienteDTO>>>(
        $"api/Clientes/Lista?pagina={pagina}&tamano={tamano}&busqueda={Uri.EscapeDataString(busqueda ?? "")}");
    return r?.Valor ?? new();
}
```

- [ ] **Step 4: Agregar UI de paginación a al menos una página (ej: Clientes)**

```razor
@if (resultado.TotalPaginas > 1)
{
    <div class="paginacion">
        <button class="btn btn-outline" disabled="@(resultado.Pagina <= 1)" @onclick="() => CambiarPagina(resultado.Pagina - 1)">←</button>
        <span>@resultado.Pagina de @resultado.TotalPaginas</span>
        <button class="btn btn-outline" disabled="@(resultado.Pagina >= resultado.TotalPaginas)" @onclick="() => CambiarPagina(resultado.Pagina + 1)">→</button>
        <span class="text-muted">(@resultado.Total registros)</span>
    </div>
}
```

- [ ] **Step 5: Estilos de paginación en app.css**

```css
.paginacion {
  display: flex;
  align-items: center;
  gap: 12px;
  justify-content: center;
  margin-top: 20px;
}
```

---

### Task 6: Dashboard con Gráficos (Chart.js)

**Files:**
- Create: `X-Libra_Catering.Cliente/Pages/Dashboard.razor`
- Create: `X-Libra_Catering.Cliente/wwwroot/js/dashboard-charts.js`
- Create: `X-Libra_Catering.Shared/DashboardCompletoDTO.cs`
- Create: `X-Libra_Catering.Server/Controllers/DashboardController.cs`
- Modify: `X-Libra_Catering.Cliente/Layout/NavMenu.razor` — link a Dashboard
- Modify: `X-Libra_Catering.Cliente/wwwroot/index.html` — Chart.js CDN
- Modify: `X-Libra_Catering.Cliente/wwwroot/css/app.css` — estilos dashboard

**Interfaces:**
- Consumes: Task 1 (loading states, toasts)

- [ ] **Step 1: Crear DashboardCompletoDTO.cs**

```csharp
namespace X_Libra_Catering.Shared;

public class DashboardCompletoDTO
{
    public int TotalEventos { get; set; }
    public int EventosPendientes { get; set; }
    public int EventosCompletados { get; set; }
    public int TotalPedidos { get; set; }
    public int PedidosEntregados { get; set; }
    public int VehiculosDisponibles { get; set; }
    public int VehiculosTotales { get; set; }
    public decimal IngresosMes { get; set; }
    public List<DatoGrafico> EventosPorMes { get; set; } = new();
    public List<DatoGrafico> PedidosPorEstado { get; set; } = new();
    public List<DatoGrafico> IngresosPorMes { get; set; } = new();
}

public class DatoGrafico
{
    public string Label { get; set; } = "";
    public decimal Valor { get; set; }
}
```

- [ ] **Step 2: Crear DashboardController.cs**

Endpoint único que agrupa todos los KPI desde las tablas existentes.

- [ ] **Step 3: Agregar Chart.js CDN a index.html**

```html
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
```

- [ ] **Step 4: Crear dashboard-charts.js**

Funciones `crearGraficoBarras(elementoId, labels, datos)` y `crearGraficoDona(elementoId, labels, datos)`.

- [ ] **Step 5: Crear Dashboard.razor**

Grid de KPI cards arriba + 3 gráficos (eventos por mes, pedidos por estado, ingresos por mes) usando Chart.js via JS interop. Incluir LoadingSpinner.

- [ ] **Step 6: Agregar NavMenu link**

```razor
<a class="nav-link" href="dashboard">📊 Dashboard</a>
```

- [ ] **Step 7: Estilos dashboard en app.css**

```css
.dashboard-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); gap: 16px; }
.dashboard-kpi-card { ... }
.dashboard-charts { display: grid; grid-template-columns: 1fr 1fr; gap: 20px; }
```

---

## Resumen de esfuerzo

| Task | Archivos | Dificultad | Dependencia |
|------|----------|-----------|-------------|
| 1. Loading + Toast | 6 (3 create, 3 modify) | Fácil | — |
| 2. Integrar en páginas | 10 modify | Fácil-Medio | Task 1 |
| 3. Auditoría | 14 (1 migration, 13 modify) | Medio | — |
| 4. Soft Delete | 10 (1 migration, 9 modify) | Medio | Task 3 |
| 5. Paginación | 11 (1 create, 10 modify) | Medio | — |
| 6. Dashboard | 8 (4 create, 4 modify) | Medio | Task 1 |

**Orden sugerido:** Task 1 → Task 2 → Task 3 → Task 4 → Task 5 → Task 6
