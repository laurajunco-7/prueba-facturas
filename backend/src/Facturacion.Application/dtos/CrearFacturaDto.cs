namespace facturacion.application.dtos;

public record CrearFacturaDto(
    string NumeroFactura,
    string NombreCliente,
    string EmailCliente,
    decimal Monto,
    string Estado
);