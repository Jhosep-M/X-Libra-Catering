# Task 2: Integrar Loading + Toasts en todas las páginas

**Goal:** Add loading spinner and toast notifications to all 10 Blazor pages.

**Note from the plan's spec:** Before starting the work, read each file you need to modify first.

**Files to modify (10 pages):**
- `X-Libra_Catering.Cliente/Pages/Clientes.razor`
- `X-Libra_Catering.Cliente/Pages/ClienteForm.razor`
- `X-Libra_Catering.Cliente/Pages/EventosDashboard.razor`
- `X-Libra_Catering.Cliente/Pages/EventoForm.razor`
- `X-Libra_Catering.Cliente/Pages/Menus.razor`
- `X-Libra_Catering.Cliente/Pages/MenuForm.razor`
- `X-Libra_Catering.Cliente/Pages/Vehiculos.razor`
- `X-Libra_Catering.Cliente/Pages/VehiculoForm.razor`
- `X-Libra_Catering.Cliente/Pages/Pedidos.razor`
- `X-Libra_Catering.Cliente/Pages/PedidoForm.razor`

**Consumes:** `LoadingSpinner` (component in `X-Libra_Catering.Cliente.Shared` namespace, already imported globally), `ServicioNotificacion` (singleton service, inject as `@inject ServicioNotificacion Notificacion`)

**Actions to apply to EACH of the 10 pages:**

### Action 1: Add LoadingSpinner at top of page content

Add after the PageTitle/header, but before the main content:

```razor
<LoadingSpinner Visible="cargando" Mensaje="Cargando datos..." />
```

### Action 2: Add `cargando` field

In the `@code` block, add:

```csharp
private bool cargando;
```

### Action 3: Inject ServicioNotificacion

Add at the top of the page with the other `@inject` directives:

```razor
@inject ServicioNotificacion Notificacion
```

### Action 4: Wrap async data loads with loading state

```csharp
cargando = true;
StateHasChanged();
try {
    // existing data loading call
} finally {
    cargando = false;
    StateHasChanged();
}
```

### Action 5: Replace Console.WriteLine with toast notifications

Replace ALL `Console.WriteLine($"Error: ...")` in catch blocks with:

```csharp
catch (Exception ex) {
    Notificacion.Mostrar($"Error: {ex.Message}", ServicioNotificacion.Tipo.Error);
}
```

### Action 6: Add success toast on forms after Guardar/Eliminar

After successful save/delete operations in Form pages, add:

```csharp
Notificacion.Mostrar("Guardado correctamente", ServicioNotificacion.Tipo.Exito);
```

---

## Detailed per-page notes

### Clientes.razor
- Has `CargarLista()` and `Eliminar()` methods with Console.WriteLine catches
- Add loading to CargarLista and Eliminar
- Add success toast after Eliminar

### ClienteForm.razor
- Has `Guardar()` method with Console.WriteLine catch
- Add loading to OnInitializedAsync and Guardar
- Add success toast after Guardar

### EventosDashboard.razor
- Has `CargarLista()` method with Console.WriteLine catch
- Add loading to CargarLista and CambiarEstado

### EventoForm.razor
- Has `Guardar()` method
- Add loading to OnInitializedAsync and Guardar
- Add success toast after Guardar

### Menus.razor
- Has `CargarLista()` and `Eliminar()` methods
- Add loading to CargarLista and Eliminar
- Add success toast after Eliminar

### MenuForm.razor
- Has `Guardar()` method
- Add loading to OnInitializedAsync and Guardar
- Add success toast after Guardar

### Vehiculos.razor
- Has `CargarLista()` and `Eliminar()` methods
- Add loading to CargarLista and Eliminar
- Add success toast after Eliminar

### VehiculoForm.razor
- Has `Guardar()`, `BuscarDireccion()`, `AbrirMapa()` methods
- Add loading to OnInitializedAsync and Guardar
- Add success toast after Guardar

### Pedidos.razor
- Has `CargarLista()` and `Eliminar()` methods
- Add loading to CargarLista and Eliminar
- Add success toast after Eliminar

### PedidoForm.razor
- Has `Guardar()` method
- Add loading to OnInitializedAsync and Guardar
- Add success toast after Guardar
