import { useEffect, useState } from "react";
import { obtenerFacturas, crearFactura, procesarRecordatorios } from "./services/facturasService";
import "./App.css";

const ESTADOS = {
  primerrecordatorio: { label: "Primer Recordatorio", color: "estado-amarillo" },
  segundorecordatorio: { label: "Segundo Recordatorio", color: "estado-naranja" },
  desactivado: { label: "Desactivado", color: "estado-rojo" },
};

export default function App() {
  const [facturas, setFacturas] = useState([]);
  const [verFormulario, setVerFormulario] = useState(false);
  const [enviando, setEnviando] = useState(false);
  const [mensaje, setMensaje] = useState("");
  const [error, setError] = useState("");
  const [form, setForm] = useState({
    numeroFactura: "",
    nombreCliente: "",
    emailCliente: "",
    monto: "",
    estado: "primerrecordatorio",
  });

  const [filtroEstado, setFiltroEstado] = useState("todos");

  const cargar = async () => {
    try {
      setError("");
      setFacturas(await obtenerFacturas());
    } catch (e) {
      setError(e.message);
    }
  };

  useEffect(() => {
    cargar();
  }, []);

  const handleEnviarCorreos = async () => {
    try {
      setEnviando(true);
      setError("");
      const res = await procesarRecordatorios();
      setMensaje(`Se notificaron y actualizaron ${res.totalActualizadas} de ${res.totalEvaluadas} facturas.`);
      await cargar();
    } catch (e) {
      setError(e.message);
    } finally {
      setEnviando(false);
    }
  };

  const handleCrear = async (e) => {
    e.preventDefault();
    try {
      setError("");
      await crearFactura({ ...form, monto: Number(form.monto), fechaEmision: new Date().toISOString() });
      setForm({ numeroFactura: "", nombreCliente: "", emailCliente: "", monto: "", estado: "primerrecordatorio" });
      setVerFormulario(false);
      await cargar();
    } catch (e) {
      setError(e.message);
    }
  };

  const facturasFiltradas =
    filtroEstado === "todos"
      ? facturas
      : facturas.filter((f) => f.estado === filtroEstado);

  return (
    <div className="contenedor-principal">
      <header className="barra-superior">
        <h1 className="titulo">Control de Facturación</h1>

        <div className="grupo-botones">
          <button className="boton-secundario" onClick={() => setVerFormulario(!verFormulario)}>
            {verFormulario ? "Cerrar" : "+ Nueva Factura"}
          </button>

          <a href="http://localhost:8025" target="_blank" rel="noreferrer" className="boton-secundario">
            Ver Emails
          </a>

          <button className="boton-primario" onClick={handleEnviarCorreos} disabled={enviando}>
            {enviando ? "Procesando..." : "Enviar Recordatorios"}
          </button>
        </div>
      </header>

      {error && <p className="alerta-error">{error}</p>}
      {mensaje && <p className="alerta-exito">{mensaje}</p>}

      {verFormulario && (
        <form className="caja-formulario" onSubmit={handleCrear}>
          <input
            placeholder="N° Factura"
            value={form.numeroFactura}
            onChange={(e) => setForm({ ...form, numeroFactura: e.target.value })}
            required
          />
          <input
            placeholder="Cliente"
            value={form.nombreCliente}
            onChange={(e) => setForm({ ...form, nombreCliente: e.target.value })}
            required
          />
          <input
            type="email"
            placeholder="Correo"
            value={form.emailCliente}
            onChange={(e) => setForm({ ...form, emailCliente: e.target.value })}
            required
          />
          <input
            type="number"
            placeholder="Monto"
            value={form.monto}
            onChange={(e) => setForm({ ...form, monto: e.target.value })}
            required
          />
          <select value={form.estado} onChange={(e) => setForm({ ...form, estado: e.target.value })}>
            <option value="primerrecordatorio">Primer Recordatorio</option>
            <option value="segundorecordatorio">Segundo Recordatorio</option>
            <option value="desactivado">Desactivado</option>
          </select>
          <button type="submit" className="boton-primario">Guardar</button>
        </form>
      )}

      <div className="barra-filtros">
        {[
          { id: "todos", texto: `Todas (${facturas.length})` },
          { id: "primerrecordatorio", texto: "Primer Recordatorio" },
          { id: "segundorecordatorio", texto: "Segundo Recordatorio" },
          { id: "desactivado", texto: "Desactivado" },
        ].map((opcion) => (
          <button
            key={opcion.id}
            type="button"
            className={filtroEstado === opcion.id ? "filtro-activo" : "filtro-btn"}
            onClick={() => setFiltroEstado(opcion.id)}
          >
            {opcion.texto}
          </button>
        ))}
      </div>

      <table className="tabla-facturas">
        <thead>
          <tr>
            <th>Factura</th>
            <th>Cliente</th>
            <th>Correo</th>
            <th>Monto</th>
            <th>Estado</th>
          </tr>
        </thead>
        <tbody>
          {facturasFiltradas.map((f) => {
            const info = ESTADOS[f.estado] || { label: f.estado, color: "" };
            return (
              <tr key={f.id}>
                <td><strong>{f.numeroFactura}</strong></td>
                <td>{f.nombreCliente}</td>
                <td>{f.emailCliente}</td>
                <td>${Number(f.monto).toLocaleString("es-CO")}</td>
                <td><span className={`etiqueta ${info.color}`}>{info.label}</span></td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}