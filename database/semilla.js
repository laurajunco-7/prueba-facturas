db = db.getSiblingDB("FacturacionDB");

db.facturas.deleteMany({});

db.facturas.insertMany([
  {
    numeroFactura: "F-001",
    nombreCliente: "Juan Perez",
    emailCliente: "juan.perez@gmail.com",
    monto: 150000,
    fechaEmision: new Date("2023-01-15"),
    estado: "primerrecordatorio"
  },
  {
    numeroFactura: "F-002",
    nombreCliente: "Miguel Lopez",
    emailCliente: "miguel.lopez@gmail.com",
    monto: 50000,
    fechaEmision: new Date("2024-07-11"),
    estado: "segundorecordatorio"
  },
  {
    numeroFactura: "F-003",
    nombreCliente: "Sara Gomez",
    emailCliente: "sara.gomez@gmail.com",
    monto: 240000,
    fechaEmision: new Date("2024-10-20"),
    estado: "desactivado"
  }
]);