using facturacion.application.dtos;
using facturacion.application.interfaces;
using facturacion.domain;

namespace facturacion.application.services;

public class RecordatorioService
{
    private readonly IFacturaRepository _facturaRepository;
    private readonly IEmailService _emailService;

    private static readonly Dictionary<string, (string nuevoEstado, string asunto, Func<Factura, string> GenerarCuerpoEmail)> _reglasTransicion = new()
    {
        [EstadosFactura.Primerrecordatorio] = (
            EstadosFactura.Segundorecordatorio,
            "Aviso: Segundo Recordatorio de Pago de factura",
            f => $"<p> Buen día <strong> {f.NombreCliente}</strong>, </p>" +
                 $"<p> Te informamos que tu factura con número <strong>{f.NumeroFactura}</strong> por un valor de <strong>{f.Monto:N0}</strong> " +
                 $"<p> ha pasado al estado de <strong>{EstadosFactura.Segundorecordatorio}</strong>. Por favor, realiza el pago correspondiente a la brevedad.</p>"
        ),
        [EstadosFactura.Segundorecordatorio] = (
            EstadosFactura.Desactivado,
            "Aviso: Servicio proximo a desactivación",
            f => $"<p> Buen día <strong> {f.NombreCliente}</strong>, </p>" +
                 $"<p> Te informamos que debido a la falta de pago de la factura <strong>{f.NumeroFactura}</strong> " +
                 $"<p> por un valor de <strong>{f.Monto:N0}</strong>, tu servicio <strong>será desactivado</strong>.</p>"
        )
    };

    public RecordatorioService(IFacturaRepository facturaRepository, IEmailService emailService)
    {
        _facturaRepository = facturaRepository;
        _emailService = emailService;
    }

    public async Task<List<Factura>> ObtenerResumenFacturasAsync()
    {
        return await _facturaRepository.ObtenerFacturasAsync();
    }

    public async Task<ResultadoProcesoDto> ProcesarRecordatoriosAsync()
    {
        var estadosObjetivo = _reglasTransicion.Keys;
        var facturasPendientes = await _facturaRepository.ObtenerFacturasporEstadoAsync(estadosObjetivo);
        var detalles = new List<DetalleNotificacionDto>();
        foreach (var factura in facturasPendientes)
        {
            if (!_reglasTransicion.TryGetValue(factura.Estado, out var regla))
                continue;

            var cuerpoCorreo = regla.GenerarCuerpoEmail(factura);
            await _emailService.EnviarCorreoAsync(factura.EmailCliente, regla.asunto, cuerpoCorreo);
            Console.WriteLine("Enviando Correo...");

            await _facturaRepository.ActualizarEstadoFacturaAsync(factura.Id, regla.nuevoEstado);
            detalles.Add(new DetalleNotificacionDto(
                factura.NumeroFactura,
                factura.NombreCliente,
                factura.EmailCliente,
                factura.Estado,
                regla.nuevoEstado
            ));

            Console.WriteLine("Correo Enviado Exitosamente :)");
        }
        return new ResultadoProcesoDto(facturasPendientes.Count, detalles.Count, detalles);
    }

    public async Task<Factura> CrearFacturaAsync(CrearFacturaDto dto)
    {
        var nuevaFactura = new Factura
        {
            NumeroFactura = dto.NumeroFactura,
            NombreCliente = dto.NombreCliente,
            EmailCliente = dto.EmailCliente,
            Monto = dto.Monto,
            FechaEmision = DateTime.UtcNow,
            Estado = dto.Estado
        };

        await _facturaRepository.CrearFacturaAsync(nuevaFactura);
        return nuevaFactura;
    }
}