# Rediseño de Páginas (Clientes, Menús, Vehículos, Pedidos) — Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Reemplazar tablas HTML en Clientes, Menús, Vehículos y Pedidos por interfaces visuales tipo tarjeta con personalidad propia.

**Architecture:** Cada página es un `.razor` independiente que reemplaza el contenido del archivo existente. Los estilos nuevos se agregan al final de `app.css`. No se tocan modelos, servicios, controladores ni formularios.

**Tech Stack:** Blazor WASM .NET 8, CSS nativo con variables, HTML5 drag & drop

## Global Constraints

- No modificar DTOs, servicios, controladores ni archivos Shared/Server
- Usar el mismo design system existente (variables `--color-*`, `--font-*`, `--radius-*`, `--space-*`)
- No agregar librerías externas ni Bootstrap classes
- La ruta `@page` debe permanecer igual en cada archivo
- Mantener los métodos del `@code` (CargarLista, Eliminar) — solo cambiar el HTML y agregar propiedades de UI

---

### Task 1: Estilos CSS compartidos para las 4 páginas

**Files:**
- Modify: `X-Libra_Catering.Cliente/wwwroot/css/app.css` (agregar al final)

**Interfaces:**
- Consumes: Variables CSS existentes (`--color-primary`, `--radius-lg`, etc.)
- Produces: Clases CSS que las 4 páginas usarán (`.page-header`, `.card-grid`, .`contact-card`, `.badge-categoria-*`, `.status-badge`, `.kpi-strip`, `.pedido-kanban`)

- [ ] **Step 1: Agregar estilos base de page header y toolbar**

```css
/* app.css — al final del archivo */

/* ---- Shared Page Patterns ---- */

.page-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  margin-bottom: var(--space-lg);
}

.page-header h3 {
  margin-bottom: 4px;
}

.page-header p {
  margin: 0;
  font-size: var(--font-size-sm);
  color: var(--color-muted);
}

.toolbar {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: var(--space-lg);
  flex-wrap: wrap;
}

.toolbar .search-box {
  display: flex;
  align-items: center;
  gap: 8px;
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  padding: 8px 12px;
  flex: 1;
  min-width: 200px;
  max-width: 360px;
  transition: border-color var(--ease-fast);
}

.toolbar .search-box:focus-within {
  border-color: var(--color-primary);
  box-shadow: 0 0 0 2px var(--color-primary-light);
}

.toolbar .search-box svg {
  color: var(--color-muted);
  flex-shrink: 0;
  width: 16px;
  height: 16px;
}

.toolbar .search-input {
  border: none;
  outline: none;
  background: transparent;
  font-family: var(--font-sans);
  font-size: var(--font-size-base);
  color: var(--color-ink);
  width: 100%;
}

.toolbar .filter-select {
  font-family: var(--font-sans);
  font-size: var(--font-size-sm);
  padding: 8px 12px;
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  background: var(--color-bg);
  color: var(--color-ink);
  min-width: 160px;
  cursor: pointer;
}

.toolbar .filter-select:focus {
  border-color: var(--color-primary);
  outline: none;
}

.result-count {
  font-size: var(--font-size-sm);
  color: var(--color-muted);
  margin-left: auto;
  white-space: nowrap;
}

.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: var(--color-muted);
}

.empty-state svg {
  width: 64px;
  height: 64px;
  margin-bottom: var(--space-md);
  opacity: 0.4;
}

.empty-state h4 {
  font-size: var(--font-size-lg);
  margin-bottom: var(--space-sm);
  color: var(--color-ink);
}

.empty-state p {
  font-size: var(--font-size-sm);
}
```

- [ ] **Step 2: Agregar estilos de card grid y contact-card**

```css
/* ---- Card Grid (shared) ---- */

.card-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: var(--space-md);
}

@media (max-width: 1024px) {
  .card-grid { grid-template-columns: repeat(2, 1fr); }
}

@media (max-width: 640px) {
  .card-grid { grid-template-columns: 1fr; }
}

/* ---- Contact Card (Clientes) ---- */

.contact-card {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: 20px;
  display: flex;
  align-items: flex-start;
  gap: 16px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.06);
  transition: box-shadow var(--ease-fast), transform var(--ease-fast);
}

.contact-card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
  transform: translateY(-1px);
}

.contact-avatar {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  background: var(--color-primary-light);
  color: var(--color-primary);
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  font-size: var(--font-size-lg);
  flex-shrink: 0;
}

.contact-body {
  flex: 1;
  min-width: 0;
}

.contact-body h5 {
  font-size: var(--font-size-base);
  font-weight: 600;
  margin-bottom: 6px;
  color: var(--color-ink);
}

.contact-info {
  display: flex;
  flex-wrap: wrap;
  gap: 4px 16px;
}

.contact-info .info-item {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  font-size: var(--font-size-xs);
  color: var(--color-muted);
}

.contact-info .info-item svg {
  width: 14px;
  height: 14px;
  opacity: 0.6;
  flex-shrink: 0;
}

.card-footer-actions {
  display: flex;
  justify-content: flex-end;
  gap: 6px;
  margin-top: 10px;
  padding-top: 10px;
  border-top: 1px solid var(--color-border-light, #f0f0f0);
  opacity: 0;
  transition: opacity var(--ease-fast);
}

.contact-card:hover .card-footer-actions,
.menu-card:hover .card-footer-actions,
.vehicle-card:hover .card-footer-actions {
  opacity: 1;
}

.btn-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border-radius: var(--radius-sm);
  border: none;
  background: transparent;
  color: var(--color-muted);
  cursor: pointer;
  transition: all var(--ease-fast);
  padding: 0;
}

.btn-icon-edit:hover {
  background: var(--color-primary-light);
  color: var(--color-primary);
}

.btn-icon-delete:hover {
  background: var(--color-danger-bg, #fef2f2);
  color: var(--color-danger);
}
```

- [ ] **Step 3: Agregar estilos de menu-card (catálogo)**

```css
/* ---- Menu Card (Menús) ---- */

.menu-card {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: 20px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.06);
  transition: box-shadow var(--ease-fast), transform var(--ease-fast);
}

.menu-card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
  transform: translateY(-1px);
}

.menu-icon {
  font-size: 2rem;
  margin-bottom: 8px;
}

.menu-card h5 {
  font-size: var(--font-size-base);
  font-weight: 600;
  margin-bottom: 4px;
  color: var(--color-ink);
}

.menu-desc {
  font-size: var(--font-size-xs);
  color: var(--color-muted);
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  line-height: 1.4;
  margin-bottom: 10px;
  min-height: 2.8em;
}

.menu-meta {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 8px;
  margin-bottom: 10px;
}

.menu-price {
  font-size: var(--font-size-lg);
  font-weight: 700;
  color: var(--color-primary);
}

.badge-categoria {
  display: inline-flex;
  align-items: center;
  font-size: var(--font-size-xs);
  font-weight: 500;
  padding: 2px 8px;
  border-radius: 4px;
}

.badge-categoria.entrada { background: #e0f2fe; color: #0369a1; }
.badge-categoria.plato-fuerte { background: #ccfbf1; color: #0f766e; }
.badge-categoria.postre { background: #fef3c7; color: #b45309; }
.badge-categoria.bebida { background: #ede9fe; color: #6d28d9; }
```

- [ ] **Step 4: Agregar estilos de vehículo y pedidos**

```css
/* ---- KPI Strip (Vehículos) ---- */

.kpi-strip {
  display: flex;
  gap: var(--space-md);
  margin-bottom: var(--space-lg);
  flex-wrap: wrap;
}

.kpi-strip-item {
  display: flex;
  align-items: center;
  gap: 8px;
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  padding: 12px 16px;
  font-size: var(--font-size-sm);
  font-weight: 500;
  color: var(--color-muted);
}

.kpi-strip-item strong {
  font-size: var(--font-size-lg);
  color: var(--color-ink);
}

.kpi-strip-item .kpi-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
}

.kpi-dot.green { background: var(--color-success); }
.kpi-dot.red { background: var(--color-danger); }

/* ---- Vehicle Card ---- */

.vehicle-card {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: 20px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.06);
  transition: box-shadow var(--ease-fast), transform var(--ease-fast);
}

.vehicle-card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
  transform: translateY(-1px);
}

.vehicle-top {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 10px;
}

.vehicle-icon {
  font-size: 1.75rem;
}

.vehicle-top h5 {
  font-size: var(--font-size-base);
  font-weight: 600;
  margin: 0;
  color: var(--color-ink);
}

.vehicle-plate {
  font-family: 'Courier New', monospace;
  font-size: var(--font-size-sm);
  font-weight: 600;
  background: var(--color-surface);
  padding: 2px 8px;
  border-radius: var(--radius-sm);
  color: var(--color-ink);
  letter-spacing: 0.05em;
}

.vehicle-details {
  display: flex;
  flex-wrap: wrap;
  gap: 4px 16px;
  margin-bottom: 10px;
}

.vehicle-details .detail-item {
  font-size: var(--font-size-xs);
  color: var(--color-muted);
  display: inline-flex;
  align-items: center;
  gap: 4px;
}

.vehicle-status {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: var(--font-size-sm);
  font-weight: 600;
  padding: 4px 12px;
  border-radius: 20px;
}

.vehicle-status.disponible {
  background: var(--color-success-bg);
  color: var(--color-success);
}

.vehicle-status.ocupado {
  background: var(--color-danger-bg, #fef2f2);
  color: var(--color-danger);
}

/* ---- Pedido Kanban ---- */

.pedido-kanban {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: var(--space-md);
  min-height: 400px;
}

.pedido-kanban .kanban-column {
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  display: flex;
  flex-direction: column;
  min-height: 300px;
}

.pedido-kanban .column-header {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 16px 16px 12px;
  border-bottom: 1px solid var(--color-border);
}

.pedido-kanban .column-header h4 {
  font-size: var(--font-size-sm);
  font-weight: 600;
  margin: 0;
  flex: 1;
  text-transform: uppercase;
  letter-spacing: 0.03em;
}

.pedido-kanban .column-body {
  padding: 12px;
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 10px;
  overflow-y: auto;
}

.pedido-card {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: 14px;
  cursor: grab;
  transition: box-shadow var(--ease-fast), transform var(--ease-fast);
  user-select: none;
}

.pedido-card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
  transform: translateY(-1px);
}

.pedido-card h5 {
  font-size: var(--font-size-base);
  font-weight: 600;
  margin-bottom: 6px;
  color: var(--color-ink);
}

.pedido-card .pedido-meta {
  display: flex;
  flex-wrap: wrap;
  gap: 4px 14px;
  margin-bottom: 4px;
}

.pedido-card .pedido-meta span {
  font-size: var(--font-size-xs);
  color: var(--color-muted);
  display: inline-flex;
  align-items: center;
  gap: 4px;
}

.pedido-card .pedido-total {
  font-weight: 700;
  color: var(--color-primary);
  font-size: var(--font-size-base);
  margin-top: 4px;
}

@media (max-width: 1024px) {
  .pedido-kanban { grid-template-columns: repeat(2, 1fr); }
}

@media (max-width: 640px) {
  .pedido-kanban { grid-template-columns: 1fr; }
}
```

- [ ] **Step 5: Build to verify CSS compiles (no errors expected, just parse check)**

Run: `dotnet build "src/.../X-Libra_Catering.Cliente.csproj"` (will skip since only CSS, but good practice)

- [ ] **Step 6: Commit**

```
git add X-Libra_Catering.Cliente/wwwroot/css/app.css
git commit -m "style: add shared card grid, contact-card, menu-card, vehicle-card, and pedido-kanban styles"
```

---

### Task 2: Clientes — Tarjetero / Directorio

**Files:**
- Modify: `X-Libra_Catering.Cliente/Pages/Clientes.razor`

**Interfaces:**
- Consumes: `.page-header`, `.toolbar`, `.card-grid`, `.contact-card`, `.contact-avatar`, `.contact-body`, `.contact-info`, `.card-footer-actions`, `.btn-icon`, `.btn-icon-edit`, `.btn-icon-delete`, `.empty-state` (CSS from Task 1)
- Produces: Página `@page "/clientes"` funcional con búsqueda y grid de contactos

- [ ] **Step 1: Reescribir Clientes.razor**

```razor
@page "/clientes"
@inject ServicioCliente ServCliente
@inject NavigationManager ServNav

<PageTitle>Clientes</PageTitle>

<div class="page-header">
    <div>
        <h3>Clientes</h3>
        <p class="text-muted">Directorio de contactos registrados</p>
    </div>
    <a href="cliente" class="btn btn-primary">
        <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor">
            <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
        </svg>
        Agregar Cliente
    </a>
</div>

<div class="toolbar">
    <div class="search-box">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/>
        </svg>
        <input type="text" class="search-input" placeholder="Buscar por nombre, telefono o email..."
               @bind="filtroTexto" @bind:event="oninput" />
    </div>
    <span class="result-count">@clientesFiltrados.Count() clientes</span>
</div>

@if (!clientesFiltrados.Any())
{
    <div class="empty-state">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
            <circle cx="9" cy="7" r="4"/>
            <path d="M23 21v-2a4 4 0 0 0-3-3.87"/>
            <path d="M16 3.13a4 4 0 0 1 0 7.75"/>
        </svg>
        <h4>No hay clientes registrados</h4>
        <p>Agrega tu primer cliente para empezar.</p>
    </div>
}
else
{
    <div class="card-grid">
        @foreach (var item in clientesFiltrados)
        {
            <div class="contact-card">
                <div class="contact-avatar">@item.Nombre[0]</div>
                <div class="contact-body">
                    <h5>@item.Nombre</h5>
                    <div class="contact-info">
                        <span class="info-item">
                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"/>
                            </svg>
                            @item.Telefono
                        </span>
                        <span class="info-item">
                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"/>
                                <polyline points="22,6 12,13 2,6"/>
                            </svg>
                            @item.Email
                        </span>
                        <span class="info-item">
                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/>
                                <circle cx="12" cy="10" r="3"/>
                            </svg>
                            @item.Direccion
                        </span>
                    </div>
                    <div class="card-footer-actions">
                        <a href="cliente/@item.Id" class="btn-icon btn-icon-edit" title="Editar">
                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                                <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                            </svg>
                        </a>
                        <button class="btn-icon btn-icon-delete" title="Eliminar" @onclick="() => Eliminar(item.Id)">
                            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                <polyline points="3 6 5 6 21 6"/>
                                <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/>
                            </svg>
                        </button>
                    </div>
                </div>
            </div>
        }
    </div>
}

@code {
    private List<ClienteDTO> lista = new();
    private string? filtroTexto;

    private IEnumerable<ClienteDTO> clientesFiltrados => lista
        .Where(c => string.IsNullOrWhiteSpace(filtroTexto) ||
                     c.Nombre.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase) ||
                     c.Telefono.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase) ||
                     c.Email.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase));

    protected override async Task OnInitializedAsync()
    {
        await CargarLista();
    }

    private async Task CargarLista()
    {
        try { lista = await ServCliente.Lista(); }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    private async Task Eliminar(int Cod)
    {
        try
        {
            await ServCliente.Eliminar(Cod);
            await CargarLista();
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }
}
```

- [ ] **Step 2: Build to verify**

Run: `dotnet build "X-Libra_Catering.Cliente/X-Libra_Catering.Cliente.csproj"`
Expected: Build succeeded, 0 errors

- [ ] **Step 3: Commit**

```
git add X-Libra_Catering.Cliente/Pages/Clientes.razor
git commit -m "feat: replace Clientes table with contact card grid"
```

---

### Task 3: Menús — Catálogo

**Files:**
- Modify: `X-Libra_Catering.Cliente/Pages/Menus.razor`

**Interfaces:**
- Consumes: `.page-header`, `.toolbar`, `.card-grid`, `.menu-card`, `.menu-icon`, `.menu-desc`, `.menu-meta`, `.badge-categoria`, `.menu-price`, `.card-footer-actions`, `.btn-icon`, `.empty-state` (CSS from Task 1)
- Produces: Página `@page "/menus"` funcional con grid catálogo y filtro por categoría

- [ ] **Step 1: Reescribir Menus.razor**

```razor
@page "/menus"
@inject ServicioMenu ServMenu
@inject NavigationManager ServNav

<PageTitle>Menus</PageTitle>

<div class="page-header">
    <div>
        <h3>Menus</h3>
        <p class="text-muted">Catalogo de platillos y bebidas</p>
    </div>
    <a href="menu" class="btn btn-primary">
        <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor">
            <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
        </svg>
        Agregar Menu
    </a>
</div>

<div class="toolbar">
    <div class="search-box">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/>
        </svg>
        <input type="text" class="search-input" placeholder="Buscar por nombre..."
               @bind="filtroTexto" @bind:event="oninput" />
    </div>
    <select class="filter-select" @bind="filtroCategoria">
        <option value="">Todas las categorias</option>
        <option value="Entrada">Entrada</option>
        <option value="Plato Fuerte">Plato Fuerte</option>
        <option value="Postre">Postre</option>
        <option value="Bebida">Bebida</option>
    </select>
    <span class="result-count">@menusFiltrados.Count() menus</span>
</div>

@if (!menusFiltrados.Any())
{
    <div class="empty-state">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <path d="M12 20h9"/>
            <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"/>
        </svg>
        <h4>No hay menus registrados</h4>
        <p>Agrega tu primer menu al catalogo.</p>
    </div>
}
else
{
    <div class="card-grid">
        @foreach (var item in menusFiltrados)
        {
            <div class="menu-card">
                <div class="menu-icon">@ObtenerIconoCategoria(item.Categoria)</div>
                <h5>@item.Nombre</h5>
                <p class="menu-desc">@item.Descripcion</p>
                <div class="menu-meta">
                    <span class="badge-categoria @item.Categoria.ToLower().Replace(" ", "-")">@item.Categoria</span>
                    <span class="menu-price">@item.Precio.ToString("C")</span>
                </div>
                <div style="font-size:var(--font-size-xs);color:var(--color-muted);margin-bottom:10px;">
                    🧊 Refrigeracion: @(item.RequiereRefrigeracion ? "Si" : "No")
                </div>
                <div class="card-footer-actions">
                    <a href="menu/@item.Id" class="btn-icon btn-icon-edit" title="Editar">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                            <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                        </svg>
                    </a>
                    <button class="btn-icon btn-icon-delete" title="Eliminar" @onclick="() => Eliminar(item.Id)">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <polyline points="3 6 5 6 21 6"/>
                            <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/>
                        </svg>
                    </button>
                </div>
            </div>
        }
    </div>
}

@code {
    private List<MenuDTO> lista = new();
    private string? filtroTexto;
    private string? filtroCategoria;

    private IEnumerable<MenuDTO> menusFiltrados => lista
        .Where(m => string.IsNullOrWhiteSpace(filtroTexto) ||
                     m.Nombre.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase))
        .Where(m => string.IsNullOrEmpty(filtroCategoria) || m.Categoria == filtroCategoria);

    protected override async Task OnInitializedAsync()
    {
        await CargarLista();
    }

    private async Task CargarLista()
    {
        try { lista = await ServMenu.Lista(); }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    private async Task Eliminar(int Cod)
    {
        try
        {
            await ServMenu.Eliminar(Cod);
            await CargarLista();
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    private string ObtenerIconoCategoria(string categoria) => categoria switch
    {
        "Entrada" => "\U0001F957",
        "Plato Fuerte" => "\U0001F37D",
        "Postre" => "\U0001F370",
        "Bebida" => "\U0001F964",
        _ => "\U0001F372"
    };
}
```

- [ ] **Step 2: Build to verify**

Run: `dotnet build "X-Libra_Catering.Cliente/X-Libra_Catering.Cliente.csproj"`
Expected: Build succeeded, 0 errors

- [ ] **Step 3: Commit**

```
git add X-Libra_Catering.Cliente/Pages/Menus.razor
git commit -m "feat: replace Menus table with catalog card grid"
```

---

### Task 4: Vehículos — Flota / Garage

**Files:**
- Modify: `X-Libra_Catering.Cliente/Pages/Vehiculos.razor`

**Interfaces:**
- Consumes: `.page-header`, `.toolbar`, `.card-grid`, `.kpi-strip`, `.kpi-strip-item`, `.vehicle-card`, `.vehicle-top`, `.vehicle-icon`, `.vehicle-plate`, `.vehicle-details`, `.vehicle-status`, `.card-footer-actions`, `.btn-icon`, `.empty-state` (CSS from Task 1)
- Produces: Página `@page "/vehiculos"` funcional con KPI strip y grid de vehículos con filtro Disponible/Ocupado

- [ ] **Step 1: Reescribir Vehiculos.razor**

```razor
@page "/vehiculos"
@inject ServicioVehiculo ServVehiculo
@inject NavigationManager ServNav

<PageTitle>Vehiculos</PageTitle>

<div class="page-header">
    <div>
        <h3>Vehiculos</h3>
        <p class="text-muted">Flota de transporte disponible</p>
    </div>
    <a href="vehiculo" class="btn btn-primary">
        <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor">
            <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
        </svg>
        Agregar Vehiculo
    </a>
</div>

<div class="kpi-strip">
    <div class="kpi-strip-item">
        🚛 Total: <strong>@lista.Count</strong>
    </div>
    <div class="kpi-strip-item">
        <span class="kpi-dot green"></span> Disponibles: <strong>@lista.Count(v => v.Disponible)</strong>
    </div>
    <div class="kpi-strip-item">
        <span class="kpi-dot red"></span> Ocupados: <strong>@lista.Count(v => !v.Disponible)</strong>
    </div>
</div>

<div class="toolbar">
    <div class="search-box">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/>
        </svg>
        <input type="text" class="search-input" placeholder="Buscar por marca, modelo o placa..."
               @bind="filtroTexto" @bind:event="oninput" />
    </div>
    <select class="filter-select" @bind="filtroDisponible">
        <option value="">Todos</option>
        <option value="disponible">Disponibles</option>
        <option value="ocupado">Ocupados</option>
    </select>
    <span class="result-count">@vehiculosFiltrados.Count() vehiculos</span>
</div>

@if (!vehiculosFiltrados.Any())
{
    <div class="empty-state">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/>
            <circle cx="12" cy="12" r="3"/>
        </svg>
        <h4>No hay vehiculos registrados</h4>
        <p>Agrega tu primer vehiculo a la flota.</p>
    </div>
}
else
{
    <div class="card-grid">
        @foreach (var item in vehiculosFiltrados)
        {
            <div class="vehicle-card">
                <div class="vehicle-top">
                    <span class="vehicle-icon">🚚</span>
                    <h5>@item.Marca @item.Modelo</h5>
                </div>
                <div class="vehicle-details">
                    <span class="vehicle-plate">@item.Placa</span>
                    <span class="detail-item">📦 @item.CapacidadKg kg</span>
                    <span class="detail-item">🧊 Refrigeracion: @(item.TieneRefrigeracion ? "Si" : "No")</span>
                </div>
                <div class="vehicle-status @(item.Disponible ? "disponible" : "ocupado")">
                    <span>●</span>
                    @(item.Disponible ? "Disponible" : "Ocupado")
                </div>
                <div class="card-footer-actions">
                    <a href="vehiculo/@item.Id" class="btn-icon btn-icon-edit" title="Editar">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                            <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                        </svg>
                    </a>
                    <button class="btn-icon btn-icon-delete" title="Eliminar" @onclick="() => Eliminar(item.Id)">
                        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <polyline points="3 6 5 6 21 6"/>
                            <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/>
                        </svg>
                    </button>
                </div>
            </div>
        }
    </div>
}

@code {
    private List<VehiculoDTO> lista = new();
    private string? filtroTexto;
    private string? filtroDisponible;

    private IEnumerable<VehiculoDTO> vehiculosFiltrados => lista
        .Where(v => string.IsNullOrWhiteSpace(filtroTexto) ||
                     v.Marca.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase) ||
                     v.Modelo.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase) ||
                     v.Placa.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase))
        .Where(v => filtroDisponible switch
        {
            "disponible" => v.Disponible,
            "ocupado" => !v.Disponible,
            _ => true
        });

    protected override async Task OnInitializedAsync()
    {
        await CargarLista();
    }

    private async Task CargarLista()
    {
        try { lista = await ServVehiculo.Lista(); }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    private async Task Eliminar(int Cod)
    {
        try
        {
            await ServVehiculo.Eliminar(Cod);
            await CargarLista();
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }
}
```

- [ ] **Step 2: Build to verify**

Run: `dotnet build "X-Libra_Catering.Cliente/X-Libra_Catering.Cliente.csproj"`
Expected: Build succeeded, 0 errors

- [ ] **Step 3: Commit**

```
git add X-Libra_Catering.Cliente/Pages/Vehiculos.razor
git commit -m "feat: replace Vehiculos table with fleet garage cards"
```

---

### Task 5: Pedidos — Kanban

**Files:**
- Modify: `X-Libra_Catering.Cliente/Pages/Pedidos.razor`

**Interfaces:**
- Consumes: `.page-header`, `.toolbar`, `.pedido-kanban`, `.kanban-column`, `.column-header`, `.column-body`, `.pedido-card`, `.pedido-meta`, `.pedido-total`, `.empty-state` (CSS from Task 1)
- Produces: Página `@page "/pedidos"` funcional con kanban de 4 columnas, drag & drop

- [ ] **Step 1: Reescribir Pedidos.razor**

```razor
@page "/pedidos"
@inject ServicioPedido ServPedido
@inject NavigationManager ServNav

<PageTitle>Pedidos</PageTitle>

<div class="page-header">
    <div>
        <h3>Pedidos</h3>
        <p class="text-muted">Tablero de ordenes por estado</p>
    </div>
    <a href="pedido" class="btn btn-primary">
        <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor">
            <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z"/>
        </svg>
        Agregar Pedido
    </a>
</div>

<div class="toolbar">
    <div class="search-box">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/>
        </svg>
        <input type="text" class="search-input" placeholder="Buscar por evento..."
               @bind="filtroTexto" @bind:event="oninput" />
    </div>
    <span class="result-count">@pedidosFiltrados.Count() pedidos</span>
</div>

@if (!pedidosFiltrados.Any())
{
    <div class="empty-state">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5">
            <path d="M9 5H7a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V7a2 2 0 0 0-2-2h-2"/>
            <rect x="9" y="3" width="6" height="4" rx="1"/>
            <path d="M9 14l2 2 4-4"/>
        </svg>
        <h4>No hay pedidos registrados</h4>
        <p>Crea tu primer pedido para empezar.</p>
    </div>
}
else
{
    <div class="pedido-kanban">
        @foreach (var columna in columnas)
        {
            <div class="kanban-column"
                 @ondragover:preventDefault
                 @ondrop="e => SoltarEnColumna(e, columna.estado)">
                <div class="column-header">
                    <h4>@columna.titulo</h4>
                    <span class="column-count">@columna.pedidos.Count()</span>
                </div>
                <div class="column-body">
                    @foreach (var p in columna.pedidos)
                    {
                        <div class="pedido-card"
                             draggable="true"
                             @ondragstart="e => ArrastrarInicio(e, p)">
                            <h5>#@p.Id — @p.EventoNombre</h5>
                            <div class="pedido-meta">
                                <span>🚚 @(p.VehiculoPlaca ?? "Sin asignar")</span>
                                <span>📅 @p.FechaPedido.ToString("dd/MM/yyyy")</span>
                            </div>
                            <div class="pedido-total">@p.Total.ToString("C")</div>
                        </div>
                    }
                </div>
            </div>
        }
    </div>
}

@code {
    private List<PedidoCabeceraDTO> lista = new();
    private string? filtroTexto;
    private PedidoCabeceraDTO? itemArrastrado;

    private record ColumnaPedidos(string titulo, string estado, IEnumerable<PedidoCabeceraDTO> pedidos);

    private IEnumerable<PedidoCabeceraDTO> pedidosFiltrados => lista
        .Where(p => string.IsNullOrWhiteSpace(filtroTexto) ||
                     (p.EventoNombre?.Contains(filtroTexto, StringComparison.OrdinalIgnoreCase) ?? false));

    private List<ColumnaPedidos> columnas => new()
    {
        new ColumnaPedidos("Pendientes", "Pendiente",
            pedidosFiltrados.Where(p => p.Estado == "Pendiente").OrderByDescending(p => p.FechaPedido)),
        new ColumnaPedidos("En Preparacion", "EnPreparacion",
            pedidosFiltrados.Where(p => p.Estado == "EnPreparacion").OrderByDescending(p => p.FechaPedido)),
        new ColumnaPedidos("En Ruta", "EnRuta",
            pedidosFiltrados.Where(p => p.Estado == "EnRuta").OrderByDescending(p => p.FechaPedido)),
        new ColumnaPedidos("Entregados", "Entregado",
            pedidosFiltrados.Where(p => p.Estado == "Entregado").OrderByDescending(p => p.FechaPedido))
    };

    protected override async Task OnInitializedAsync()
    {
        await CargarDatos();
    }

    private async Task CargarDatos()
    {
        try { lista = await ServPedido.Lista(); }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    private void ArrastrarInicio(DragEventArgs e, PedidoCabeceraDTO p)
    {
        itemArrastrado = p;
    }

    private async Task SoltarEnColumna(DragEventArgs e, string nuevoEstado)
    {
        if (itemArrastrado != null && itemArrastrado.Estado != nuevoEstado)
        {
            try
            {
                await ServPedido.CambiarEstado(itemArrastrado.Id, nuevoEstado);
                itemArrastrado.Estado = nuevoEstado;
                await CargarDatos();
            }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
        itemArrastrado = null;
    }
}
```

- [ ] **Step 2: Agregar endpoint CambiarEstado si no existe en ServicioPedido y PedidosController**

Verificar si `ServicioPedido` tiene método `CambiarEstado(int id, string estado)`. Si no existe, agregarlo:

En `ServicioPedido.cs`:
```csharp
public async Task CambiarEstado(int id, string estado)
{
    var response = await http.PatchAsJsonAsync($"api/pedidos/{id}/estado", new { estado });
    response.EnsureSuccessStatusCode();
}
```

En `PedidosController.cs`:
```csharp
[HttpPatch("{id}/estado")]
public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoRequest request)
{
    var pedido = await context.PedidoCabeceras.FindAsync(id);
    if (pedido == null) return NotFound();
    pedido.Estado = request.Estado;
    await context.SaveChangesAsync();
    return Ok();
}

public record CambiarEstadoRequest(string Estado);
```

- [ ] **Step 3: Build to verify**

Run: `dotnet build "X-Libra_Catering.Cliente/X-Libra_Catering.Cliente.csproj"`
Run: `dotnet build "X-Libra_Catering.Server/X-Libra_Catering.Server.csproj"`
Expected: Both build succeeded, 0 errors

- [ ] **Step 4: Commit**

```
git add X-Libra_Catering.Cliente/Pages/Pedidos.razor
git add X-Libra_Catering.Cliente/Services/ServicioPedido.cs
git add X-Libra_Catering.Server/Controllers/PedidosController.cs
git commit -m "feat: replace Pedidos table with kanban board and add estado change endpoint"
```

---

### Task 6: Full build verification

- [ ] **Step 1: Build entire solution**

Run: `dotnet build "X-Libra_Catering.Cliente/X-Libra_Catering.Cliente.csproj"` && `dotnet build "X-Libra_Catering.Server/X-Libra_Catering.Server.csproj"`
Expected: Both succeed, 0 errors, 0 warnings

- [ ] **Step 2: Commit any remaining changes**

```
git add -A
git commit -m "chore: full build verification after page redesigns"
```
