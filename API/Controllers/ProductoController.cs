using API.Dtos;
using API.Helpers.Errors;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]

public class ProductoController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
    {
        var data = await unitOfWork.Productos.GetAllAsync();
        return mapper.Map<List<ProductoDto>>(data);
    }

    [HttpGet("{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductoDto>> Get(string codigoProducto)
    {
        var data = await unitOfWork.Productos.GetByIdAsync(codigoProducto);
        if (data == null)
        {
            return NotFound();
        }
        return mapper.Map<ProductoDto>(data);
    }

    [HttpGet("Pagination")]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<ProductoDto>>> GetPagination([FromQuery] Params dataParams)
    {
        var datos = await unitOfWork.Productos.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        var listData = mapper.Map<List<ProductoDto>>(datos.registros);
        return new Pager<ProductoDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductoDto>> Post(ProductoDto dataDto)
    {
        var data = mapper.Map<Producto>(dataDto);
        unitOfWork.Productos.Add(data);
        await unitOfWork.SaveAsync();
        if (data == null)
        {
            return BadRequest();
        }
        dataDto.CodigoProducto = data.CodigoProducto;
        return CreatedAtAction(nameof(Post), new { codigoProducto = dataDto.CodigoProducto }, dataDto);
    }

    [HttpPut("{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> Put(string codigoProducto, [FromBody] ProductoDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var data = mapper.Map<Producto>(dataDto);
        unitOfWork.Productos.Update(data);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoProducto}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string codigoProducto)
    {
        var data = await unitOfWork.Productos.GetByIdAsync(codigoProducto);
        if (data == null)
        {
            return NotFound();
        }
        unitOfWork.Productos.Remove(data);
        await unitOfWork.SaveAsync();
        return NoContent();
    }

    [HttpGet("ProductosQueNoHanAparecidoEnUnPedido")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> ProductosQueNoHanAparecidoEnUnPedido()
    {
        var data = await unitOfWork.Productos.ProductosQueNoHanAparecidoEnUnPedido();
        return mapper.Map<List<object>>(data);
    }
}