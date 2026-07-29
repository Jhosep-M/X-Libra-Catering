# Task 3: Auditoría (FECHA_CREACION + FECHA_MODIFICACION)

**Goal:** Add creation/modification timestamps to all 6 database entities, auto-set via DbContext override.

## Files to modify

### Models (6 files) + 1 new interface:
- `X-Libra_Catering.Server/Models/Cliente.cs`
- `X-Libra_Catering.Server/Models/Evento.cs`
- `X-Libra_Catering.Server/Models/Menu.cs`
- `X-Libra_Catering.Server/Models/Vehiculo.cs`
- `X-Libra_Catering.Server/Models/PedidoCabecera.cs`
- `X-Libra_Catering.Server/Models/PedidoDetalle.cs`

### DbContext:
- `X-Libra_Catering.Server/Data/BdXLibraCateringContext.cs`

### DTOs (6 files):
- `X-Libra_Catering.Shared/ClienteDTO.cs`
- `X-Libra_Catering.Shared/EventoDTO.cs`
- `X-Libra_Catering.Shared/MenuDTO.cs`
- `X-Libra_Catering.Shared/VehiculoDTO.cs`
- `X-Libra_Catering.Shared/PedidoCabeceraDTO.cs`
- `X-Libra_Catering.Shared/PedidoDetalleDTO.cs`

### Controllers (5 files):
- `X-Libra_Catering.Server/Controllers/ClientesController.cs`
- `X-Libra_Catering.Server/Controllers/EventosController.cs`
- `X-Libra_Catering.Server/Controllers/MenusController.cs`
- `X-Libra_Catering.Server/Controllers/VehiculosController.cs`
- `X-Libra_Catering.Server/Controllers/PedidosController.cs`

---

## Step 1: Create IAuditable interface

Create `X-Libra_Catering.Server/Models/IAuditable.cs`:

```csharp
namespace X_Libra_Catering.Server.Models;

public interface IAuditable
{
    DateTime? FechaCreacion { get; set; }
    DateTime? FechaModificacion { get; set; }
}
```

## Step 2: Add audit fields to each model + implement IAuditable

Add to each of the 6 models:

```csharp
public DateTime? FechaCreacion { get; set; }
public DateTime? FechaModificacion { get; set; }
```

And add `: IAuditable` after the class declaration (e.g., `public partial class Cliente : IAuditable`).

Note: `PedidoCabecera` and `PedidoDetalle` already have `: IAuditable` added. `Cliente`, `Evento`, `Menu`, `Vehiculo` need it added.

## Step 3: Add column mappings in DbContext

For each of the 6 entities in `BdXLibraCateringContext.cs`, add:

```csharp
entity.Property(e => e.FechaCreacion).HasColumnName("FECHA_CREACION");
entity.Property(e => e.FechaModificacion).HasColumnName("FECHA_MODIFICACION");
```

## Step 4: Override SaveChangesAsync in DbContext

Add to `BdXLibraCateringContext.cs`:

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

## Step 5: Create and apply migration

Run from the solution root:

```powershell
dotnet ef migrations add AddAuditFields --project "X-Libra_Catering.Server\X-Libra_Catering.Server.csproj"
dotnet ef database update --project "X-Libra_Catering.Server\X-Libra_Catering.Server.csproj"
```

## Step 6: Add fields to DTOs in Shared

Add to all 6 DTO classes:

```csharp
public DateTime? FechaCreacion { get; set; }
public DateTime? FechaModificacion { get; set; }
```

## Step 7: Map fields in all controllers

In each controller's `Lista` and `Buscar` methods, add to the entity→DTO mapping:

```csharp
FechaCreacion = e.FechaCreacion,
FechaModificacion = e.FechaModificacion
```
