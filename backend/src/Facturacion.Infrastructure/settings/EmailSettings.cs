namespace facturacion.infrastructure.settings;

public class EmailSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool EnableSsl { get; set; } = false;
    public string NombreRemitente { get; set; } = string.Empty;
    public string EmailRemitente { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}