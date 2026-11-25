using System.Net;
using System.Net.Mail;
using facturacion.application.interfaces;
using facturacion.infrastructure.settings;
using Microsoft.Extensions.Options;

namespace Facturacion.Infrastructure.Email;

public class SmtpEmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public SmtpEmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml)
    {
        using var mensaje = new MailMessage
        {
            From = new MailAddress(_settings.EmailRemitente, _settings.NombreRemitente),
            Subject = asunto,
            Body = cuerpoHtml,
            IsBodyHtml = true
        };
        mensaje.To.Add(destinatario);

        using var cliente = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl
        };

        if (!string.IsNullOrWhiteSpace(_settings.Username))
        {
            cliente.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
        }

        await cliente.SendMailAsync(mensaje);
    }
}