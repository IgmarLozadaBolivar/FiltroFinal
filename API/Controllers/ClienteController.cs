using API.Dtos;
using API.Helpers.Errors;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]

public class ClienteController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ClienteController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
    {
        var data = await unitOfWork.Clientes.GetAllAsync();
        return mapper.Map<List<ClienteDto>>(data);
    }

    [HttpGet("{codigoCliente}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> Get(int codigoCliente)
    {
        var data = await unitOfWork.Clientes.GetByIdAsync(codigoCliente);
        if (data == null)
        {
            return NotFound();
        }
        return mapper.Map<ClienteDto>(data);
    }

    [HttpGet("Pagination")]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<ClienteDto>>> GetPagination([FromQuery] Params dataParams)
    {
        var datos = await unitOfWork.Clientes.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        var listData = mapper.Map<List<ClienteDto>>(datos.registros);
        return new Pager<ClienteDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> Post(ClienteDto dataDto)
    {
        var data = mapper.Map<Cliente>(dataDto);
        unitOfWork.Clientes.Add(data);
        await unitOfWork.SaveAsync();
        if (data == null)
        {
            return BadRequest();
        }
        dataDto.CodigoCliente = data.CodigoCliente;
        return CreatedAtAction(nameof(Post), new { codigoCliente = dataDto.CodigoCliente }, dataDto);
    }

    [HttpPut("{codigoCliente}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> Put(int codigoCliente, [FromBody] ClienteDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var data = mapper.Map<Cliente>(dataDto);
        unitOfWork.Clientes.Update(data);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoCliente}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int codigoCliente)
    {
        var data = await unitOfWork.Clientes.GetByIdAsync(codigoCliente);
        if (data == null)
        {
            return NotFound();
        }
        unitOfWork.Clientes.Remove(data);
        await unitOfWork.SaveAsync();
        return NoContent();
    }

    [HttpGet("ClientesQueNoHayanHechoPagos")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> ClientesQueNoHayanHechoPagos()
    {
        var data = await unitOfWork.Clientes.ClientesQueNoHayanHechoPagos();
        return mapper.Map<List<object>>(data);
    }

    [HttpGet("ClientesYCantidadDePedidosRealizados")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> ClientesYCantidadDePedidosRealizados()
    {
        var data = await unitOfWork.Clientes.ClientesYCantidadDePedidosRealizados();
        return mapper.Map<List<object>>(data);
    }
}