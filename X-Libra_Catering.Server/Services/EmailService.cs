using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace X_Libra_Catering.Server.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnviarBienvenida(string email, string nombre)
    {
        var smtp = _config.GetSection("Smtp");
        var mensaje = new MimeMessage();
        mensaje.From.Add(new MailboxAddress("X-Libra Catering", smtp["Usuario"]!));
        mensaje.To.Add(new MailboxAddress(nombre, email));
        mensaje.Subject = "Bienvenido a X-Libra Catering";

        mensaje.Body = new TextPart("plain")
        {
            Text = $@"Hola {nombre},

Gracias por registrarte en X-Libra Catering.

Tus datos han sido guardados exitosamente. Ahora puedes participar en nuestros eventos y disfrutar de nuestros servicios de catering.

Si tienes alguna pregunta, no dudes en contactarnos.

Saludos,
El equipo de X-Libra Catering"
        };

        using var cliente = new SmtpClient();
        var servidor = smtp["Servidor"]!;
        var puerto = int.Parse(smtp["Puerto"]!);
        var usuario = smtp["Usuario"]!;
        var clave = smtp["Clave"]!;
        await cliente.ConnectAsync(servidor, puerto, SecureSocketOptions.StartTls);
        await cliente.AuthenticateAsync(usuario, clave);
        await cliente.SendAsync(mensaje);
        await cliente.DisconnectAsync(true);
    }
}
