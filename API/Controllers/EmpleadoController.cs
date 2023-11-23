using API.Dtos;
using API.Helpers.Errors;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]

public class EmpleadoController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public EmpleadoController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<EmpleadoDto>>> Get()
    {
        var data = await unitOfWork.Empleados.GetAllAsync();
        return mapper.Map<List<EmpleadoDto>>(data);
    }

    [HttpGet("{codigoEmpleado}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmpleadoDto>> Get(int codigoEmpleado)
    {
        var data = await unitOfWork.Empleados.GetByIdAsync(codigoEmpleado);
        if (data == null)
        {
            return NotFound();
        }
        return mapper.Map<EmpleadoDto>(data);
    }

    [HttpGet("Pagination")]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<EmpleadoDto>>> GetPagination([FromQuery] Params dataParams)
    {
        var datos = await unitOfWork.Empleados.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        var listData = mapper.Map<List<EmpleadoDto>>(datos.registros);
        return new Pager<EmpleadoDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmpleadoDto>> Post(EmpleadoDto dataDto)
    {
        var data = mapper.Map<Empleado>(dataDto);
        unitOfWork.Empleados.Add(data);
        await unitOfWork.SaveAsync();
        if (data == null)
        {
            return BadRequest();
        }
        dataDto.CodigoEmpleado = data.CodigoEmpleado;
        return CreatedAtAction(nameof(Post), new { codigoEmpleado = dataDto.CodigoEmpleado }, dataDto);
    }

    [HttpPut("{codigoEmpleado}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmpleadoDto>> Put(int codigoEmpleado, [FromBody] EmpleadoDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var data = mapper.Map<Empleado>(dataDto);
        unitOfWork.Empleados.Update(data);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoEmpleado}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int codigoEmpleado)
    {
        var data = await unitOfWork.Empleados.GetByIdAsync(codigoEmpleado);
        if (data == null)
        {
            return NotFound();
        }
        unitOfWork.Empleados.Remove(data);
        await unitOfWork.SaveAsync();
        return NoContent();
    }
}