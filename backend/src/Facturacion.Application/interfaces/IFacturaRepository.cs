using facturacion.domain;

namespace facturacion.application.interfaces;

public interface IFacturaRepository
{
    Task<List<Factura>> ObtenerFacturasAsync();
    Task<List<Factura>> ObtenerFacturasporEstadoAsync(IEnumerable<string> estados);
    Task ActualizarEstadoFacturaAsync(string id, string nuevoEstado);
    Task CrearFacturaAsync(Factura factura);
}
