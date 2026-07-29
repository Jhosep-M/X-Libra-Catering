# Task 1: LoadingSpinner + ToastNotification Components

**Goal:** Crear componentes reutilizables de loading spinner y notificaciones toast, más un servicio de notificación singleton.

**Files:**
- Create: `X-Libra_Catering.Cliente/Shared/LoadingSpinner.razor`
- Create: `X-Libra_Catering.Cliente/Services/ServicioNotificacion.cs`
- Create: `X-Libra_Catering.Cliente/Shared/ToastNotification.razor`
- Modify: `X-Libra_Catering.Cliente/Program.cs` (registrar servicio singleton)
- Modify: `X-Libra_Catering.Cliente/Layout/MainLayout.razor` (incluir ToastNotification)
- Modify: `X-Libra_Catering.Cliente/wwwroot/css/app.css` (estilos toast + loading spinner)

**Interfaces:**
- Consumes: nothing
- Produces: `ServicioNotificacion` (singleton DI), `LoadingSpinner` (component), `ToastNotification` (component)

--- 

## Step 1: Crear ServicioNotificacion.cs

Path: `X-Libra_Catering.Cliente\Services\ServicioNotificacion.cs`

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

## Step 2: Crear ToastNotification.razor

Path: `X-Libra_Catering.Cliente\Shared\ToastNotification.razor`

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

## Step 3: Crear LoadingSpinner.razor

Path: `X-Libra_Catering.Cliente\Shared\LoadingSpinner.razor`

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

## Step 4: Registrar en Program.cs

En `X-Libra_Catering.Cliente\Program.cs`, agregar después de los otros AddScoped:

```csharp
builder.Services.AddSingleton<ServicioNotificacion>();
```

## Step 5: Incluir ToastNotification en MainLayout.razor

En `X-Libra_Catering.Cliente\Layout\MainLayout.razor`, agregar `<ToastNotification />` dentro del body del layout (después del contenido principal).

## Step 6: Agregar estilos en app.css

En `X-Libra_Catering.Cliente\wwwroot\css\app.css`, al final:

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
