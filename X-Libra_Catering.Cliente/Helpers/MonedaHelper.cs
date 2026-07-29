namespace X_Libra_Catering.Cliente.Helpers;

public static class MonedaHelper
{
    public static string Formatear(decimal valor)
    {
        return $"Bs {valor:N2}";
    }
}

public static class UrlHelper
{
    public const string Servidor = "http://localhost:5137";
}
