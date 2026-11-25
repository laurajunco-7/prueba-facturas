const API_URL = "http://localhost:5027/api/facturas";

export const obtenerFacturas = async () => {
  const res = await fetch(API_URL);
  if (!res.ok) throw new Error("Error al conectar con el servidor");
  return res.json();
};

export const crearFactura = async (datos) => {
  const res = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(datos),
  });
  if (!res.ok) throw new Error("No se pudo guardar la factura");
  return res.json();
};

export const procesarRecordatorios = async () => {
  const res = await fetch(`${API_URL}/procesar-recordatorios`, {
    method: "POST",
  });
  if (!res.ok) throw new Error("Error al enviar los recordatorios");
  return res.json();
};