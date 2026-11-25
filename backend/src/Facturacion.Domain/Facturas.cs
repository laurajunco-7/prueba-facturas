namespace facturacion.domain;

public class Factura
{
    public string Id { get; set; } = string.Empty;
    public string NumeroFactura { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public DateTime FechaEmision { get; set; }
    public string Estado { get; set; } = string.Empty;
}