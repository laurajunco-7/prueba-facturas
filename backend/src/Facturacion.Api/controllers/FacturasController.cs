using facturacion.application.services;
using facturacion.application.dtos;
using Microsoft.AspNetCore.Mvc;

namespace facturacion.api.controllers;

[ApiController]
[Route("api/[controller]")]
public class FacturasController : ControllerBase
{
    private readonly RecordatorioService _recordatorioService;

    public FacturasController(RecordatorioService recordatorioService)
    {
        _recordatorioService = recordatorioService;
    }

    [HttpPost]
    public async Task<IActionResult> CrearFactura([FromBody] CrearFacturaDto dto)
    {
        var facturaCreada = await _recordatorioService.CrearFacturaAsync(dto);
        return CreatedAtAction(nameof(ObtenerResumen), new { id = facturaCreada.Id }, facturaCreada);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerResumen()
    {
        var facturas = await _recordatorioService.ObtenerResumenFacturasAsync();
        return Ok(facturas);
    }

    [HttpPost("procesar-recordatorios")]
    public async Task<IActionResult> ProcesarRecordatorios()
    {
        var resultado = await _recordatorioService.ProcesarRecordatoriosAsync();
        return Ok(resultado);
    }
}