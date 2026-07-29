namespace X_Libra_Catering.Server.Services;

public interface IEmailService
{
    Task EnviarBienvenida(string email, string nombre);
}
