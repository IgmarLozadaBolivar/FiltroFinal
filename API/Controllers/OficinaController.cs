using API.Dtos;
using API.Helpers.Errors;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;

[ApiVersion("1.0")]
[ApiVersion("1.1")]

public class OficinaController : BaseApiController
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public OficinaController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<OficinaDto>>> Get()
    {
        var data = await unitOfWork.Oficinas.GetAllAsync();
        return mapper.Map<List<OficinaDto>>(data);
    }

    [HttpGet("{codigoOficina}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OficinaDto>> Get(string codigoOficina)
    {
        var data = await unitOfWork.Oficinas.GetByIdAsync(codigoOficina);
        if (data == null)
        {
            return NotFound();
        }
        return mapper.Map<OficinaDto>(data);
    }

    [HttpGet("Pagination")]
    [MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Pager<OficinaDto>>> GetPagination([FromQuery] Params dataParams)
    {
        var datos = await unitOfWork.Oficinas.GetAllAsync(dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
        var listData = mapper.Map<List<OficinaDto>>(datos.registros);
        return new Pager<OficinaDto>(listData, datos.totalRegistros, dataParams.PageIndex, dataParams.PageSize, dataParams.Search);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OficinaDto>> Post(OficinaDto dataDto)
    {
        var data = mapper.Map<Oficina>(dataDto);
        unitOfWork.Oficinas.Add(data);
        await unitOfWork.SaveAsync();
        if (data == null)
        {
            return BadRequest();
        }
        dataDto.CodigoOficina = data.CodigoOficina;
        return CreatedAtAction(nameof(Post), new { codigoOficina = dataDto.CodigoOficina }, dataDto);
    }

    [HttpPut("{codigoOficina}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OficinaDto>> Put(string codigoOficina, [FromBody] OficinaDto dataDto)
    {
        if (dataDto == null)
        {
            return NotFound();
        }
        var data = mapper.Map<Oficina>(dataDto);
        unitOfWork.Oficinas.Update(data);
        await unitOfWork.SaveAsync();
        return dataDto;
    }

    [HttpDelete("{codigoOficina}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string codigoOficina)
    {
        var data = await unitOfWork.Oficinas.GetByIdAsync(codigoOficina);
        if (data == null)
        {
            return NotFound();
        }
        unitOfWork.Oficinas.Remove(data);
        await unitOfWork.SaveAsync();
        return NoContent();
    }

    [HttpGet("OficinasDondeNoTrabajaNingunEmpleadoEnGamaFrutal")]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<object>>> OficinasDondeNoTrabajaNingunEmpleadoEnGamaFrutal()
    {
        var data = await unitOfWork.Oficinas.OficinasDondeNoTrabajaNingunEmpleadoEnGamaFrutal();
        return mapper.Map<List<object>>(data);
    }
}