using facturacion.domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace facturacion.infrastructure.persistence;

public class FacturaDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("numeroFactura")]
    public string NumeroFactura { get; set; } = string.Empty;

    [BsonElement("nombreCliente")]
    public string NombreCliente { get; set; } = string.Empty;

    [BsonElement("emailCliente")]
    public string EmailCliente { get; set; } = string.Empty;

    [BsonElement("monto")]
    public decimal Monto { get; set; }

    [BsonElement("fechaEmision")]
    public DateTime FechaEmision { get; set; }

    [BsonElement("estado")]
    public string Estado { get; set; } = string.Empty;

    public Factura ToDomain() => new ()
    {
        Id = Id,
        NumeroFactura = NumeroFactura,
        NombreCliente = NombreCliente,
        EmailCliente = EmailCliente,
        Monto = Monto,
        FechaEmision = FechaEmision,
        Estado = Estado
    };
}