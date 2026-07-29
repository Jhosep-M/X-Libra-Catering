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
