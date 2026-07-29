# Task 4: Soft Delete (ACTIVO)

**Goal:** Replace hard deletes with soft deletes on the 4 master entities (Cliente, Evento, Menu, Vehiculo). Lista queries filter only active records.

## Files to modify

### Models (4 files):
- `X-Libra_Catering.Server/Models/Cliente.cs`
- `X-Libra_Catering.Server/Models/Evento.cs`
- `X-Libra_Catering.Server/Models/Menu.cs`
- `X-Libra_Catering.Server/Models/Vehiculo.cs`

### DbContext:
- `X-Libra_Catering.Server/Data/BdXLibraCateringContext.cs`

### Migration:
- Create `AddSoftDelete`

### Controllers (5 files — 4 soft delete + 1 unchanged to verify running clean):
- `X-Libra_Catering.Server/Controllers/ClientesController.cs`
- `X-Libra_Catering.Server/Controllers/EventosController.cs`
- `X-Libra_Catering.Server/Controllers/MenusController.cs`
- `X-Libra_Catering.Server/Controllers/VehiculosController.cs`
- `X-Libra_Catering.Server/Controllers/PedidosController.cs`
  (PedidosController keeps hard Remove — Pedidos are transactional)

---

## Step 1: Add Activo field to 4 models

Add to Cliente.cs, Evento.cs, Menu.cs, Vehiculo.cs:

```csharp
public bool Activo { get; set; } = true;
```

## Step 2: Add column mapping in DbContext

For each of the 4 entities (inside their existing entity configuration in `OnModelCreating`):

```csharp
entity.Property(e => e.Activo)
    .HasColumnName("ACTIVO")
    .HasDefaultValue(true);
```

## Step 3: Modify Delete methods in 4 controllers

Replace these in each controller (`ClientesController.cs:168`, `EventosController.cs:308`, `MenusController.cs:217`, `VehiculosController.cs:178`):

```csharp
// REPLACE:
_context.Clientes.Remove(entidad);
// WITH:
entidad.Activo = false;
entidad.FechaModificacion = DateTime.UtcNow;
```

Same pattern for Eventos, Menus, Vehiculos.

Keep the existing `_context.PedidoCabeceras.Remove(entidad)` in PedidosController unchanged.

## Step 4: Modify Lista methods to filter active only

In each controller's Lista endpoint, change:

```csharp
// BEFORE:
var lista = await _context.Clientes.ToListAsync();
// AFTER:
var lista = await _context.Clientes.Where(c => c.Activo).ToListAsync();
```

Same for Eventos, Menus, Vehiculos. Keep PedidosController unchanged.

## Step 5: Create and apply migration

```powershell
dotnet ef migrations add AddSoftDelete --project "X-Libra_Catering.Server\X-Libra_Catering.Server.csproj"
dotnet ef database update --project "X-Libra_Catering.Server\X-Libra_Catering.Server.csproj"
```

## Step 6: Build and commit

Build server: `dotnet build "X-Libra_Catering.Server\X-Libra_Catering.Server.csproj"`
Build client: `dotnet build "X-Libra_Catering.Cliente\X-Libra_Catering.Cliente.csproj"`

Commit message: `"feat: add soft delete to master entities (Task 4)"`
