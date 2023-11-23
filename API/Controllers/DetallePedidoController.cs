using API.Dtos;
using API.Helpers.Errors;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]

public class DetallePedidoController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public DetallePedidoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<DetallePedidoDto>>> Get()
    {
        var data = await unitOfWork.DetallePedidos.GetAllAsync();
        return mapper.Map<List<DetallePedidoDto>>(data);
    }

    // * Posible solucion al tema de dos identificadores en una misma clase o entidad
    [HttpGet("{codigoPedido}/{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DetallePedidoDto>> Get(int codigoPedido, string codigoProducto)
    {
        var data = await unitOfWork.DetallePedidos.GetByIdx1Async(codigoPedido, codigoProducto);
        if (data == null)
        {
            return NotFound();
        }

        return mapper.Map<DetallePedidoDto>(data);
    }

    [HttpGet("Pagination")]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<DetallePedidoDto>>> GetPagination([FromQuery] Params dataParams)
    {
        var datos = await unitOfWork.DetallePedidos.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        var listData = mapper.Map<List<DetallePedidoDto>>(datos.registros);
        return new Pager<DetallePedidoDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DetallePedidoDto>> Post(DetallePedidoDto dataDto)
    {
        var data = mapper.Map<DetallePedido>(dataDto);
        unitOfWork.DetallePedidos.Add(data);
        try
        {
            await unitOfWork.SaveAsync();
            return CreatedAtAction(nameof(Post), new { /* información para la respuesta */ }, dataDto);
        }
        catch (Exception ex)
        {
            return BadRequest("Error al crear el registro: " + ex.Message);
        }
    }

    [HttpPut("{codigoPedido}/{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DetallePedidoDto>> Put(int codigoPedido, string codigoProducto, [FromBody] DetallePedidoDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var existingData = await unitOfWork.DetallePedidos.GetByIdx1Async(codigoPedido, codigoProducto);
        if (existingData == null)
        {
            return NotFound();
        }
        mapper.Map(dataDto, existingData);
        unitOfWork.DetallePedidos.Update(existingData);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoPedido}/{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int codigoPedido, string codigoProducto)
    {
        var existingData = await unitOfWork.DetallePedidos.GetByIdx1Async(codigoPedido, codigoProducto);
        if (existingData == null)
        {
            return NotFound();
        }
        unitOfWork.DetallePedidos.Remove(existingData);
        await unitOfWork.SaveAsync();
        return NoContent();
    }

    [HttpGet("VeinteProductosMasVendidos")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> VeinteProductosMasVendidos()
    {
        var data = await unitOfWork.DetallePedidos.VeinteProductosMasVendidos();
        return mapper.Map<List<object>>(data);
    }

    [HttpGet("ProductosQueFacturaronMasDe3000Euros")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> ProductosQueFacturaronMasDe3000Euros()
    {
        var data = await unitOfWork.DetallePedidos.ProductosQueFacturaronMasDe3000Euros();
        return mapper.Map<List<object>>(data);
    }

    [HttpGet("ProductoQueVendioMasUnidades")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> ProductoQueVendioMasUnidades()
    {
        var data = await unitOfWork.DetallePedidos.ProductoQueVendioMasUnidades();
        return mapper.Map<List<object>>(data);
    }
}
