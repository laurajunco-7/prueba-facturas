using facturacion.application.interfaces;
using facturacion.domain;
using MongoDB.Driver;

namespace facturacion.infrastructure.persistence;

public class MongoFacturaRepository : IFacturaRepository
{
    private readonly IMongoCollection<FacturaDocument> _coleccion;

    public MongoFacturaRepository(IMongoDatabase database)
    {
        _coleccion = database.GetCollection<FacturaDocument>("facturas");
    }

    public async Task<List<Factura>> ObtenerFacturasAsync()
    {
        var documentos = await _coleccion.Find(_ => true).ToListAsync();
        return documentos.Select(doc => doc.ToDomain()).ToList();
    }

    public async Task<List<Factura>> ObtenerFacturasporEstadoAsync(IEnumerable<string> estados)
    {
        var filtro = Builders<FacturaDocument>.Filter.In(f => f.Estado, estados);
        var documentos = await _coleccion.Find(filtro).ToListAsync();
        return documentos.Select(doc => doc.ToDomain()).ToList();
    }

    public async Task ActualizarEstadoFacturaAsync(string id, string nuevoEstado)
    {
        var filtro = Builders<FacturaDocument>.Filter.Eq(f => f.Id, id);
        var actualizacion = Builders<FacturaDocument>.Update.Set(f => f.Estado, nuevoEstado);
        await _coleccion.UpdateOneAsync(filtro, actualizacion);
    }

    public async Task CrearFacturaAsync(Factura factura)
    {
        var documento = new FacturaDocument
        {
            NumeroFactura = factura.NumeroFactura,
            NombreCliente = factura.NombreCliente,
            EmailCliente = factura.EmailCliente,
            Monto = factura.Monto,
            FechaEmision = factura.FechaEmision,
            Estado = factura.Estado
        };

        await _coleccion.InsertOneAsync(documento);
        factura.Id = documento.Id;
    }
}