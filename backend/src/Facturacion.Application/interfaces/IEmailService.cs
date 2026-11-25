namespace facturacion.application.interfaces;

public interface IEmailService
{
    Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo);
}