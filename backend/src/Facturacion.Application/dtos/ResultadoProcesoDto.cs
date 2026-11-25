namespace facturacion.application.dtos;

public record ResultadoProcesoDto(
    int TotalEvaluadas,
    int TotalActualizadas,
    List<DetalleNotificacionDto> Detalles
);

public record DetalleNotificacionDto(
    string NumeroFactura,
    string NombreCliente,
    string EmailCliente,
    string EstadoAnterior,
    string EstadoNuevo
);