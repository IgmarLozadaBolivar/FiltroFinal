using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
namespace Application.Repository;

public class OficinaRepo : Generic<Oficina>, IOficina
{
    protected readonly DbFirstContext _context;

    public OficinaRepo(DbFirstContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Oficina>> GetAllAsync()
    {
        return await _context.Oficinas
            //.Include(p => p.)
            .ToListAsync();
    }

    public override async Task<Oficina> GetByIdAsync(string codigoOficina)
    {
        return await _context.Oficinas
            //.Include(p => p.)
            .FirstOrDefaultAsync(p => p.CodigoOficina == codigoOficina);
    }

    public override async Task<(int totalRegistros, IEnumerable<Oficina> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Oficinas as IQueryable<Oficina>;

        if (!string.IsNullOrEmpty(search))
        {
            //query = query.Where(p => p.YourPropertyNotString.ToString().ToLower().Contains(search));
            query = query.Where(p => p.Ciudad.ToLower().Contains(search));
        }

        query = query.OrderBy(p => p.CodigoOficina);
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    public async Task<IEnumerable<object>> OficinasDondeNoTrabajaNingunEmpleadoEnGamaFrutal()
    {
        var mensaje = "Oficinas donde No trabaja ningun empleado en la gama frutal".ToUpper();

        var consulta = await _context.Oficinas
            .GroupJoin(
                _context.Empleados,
                o => o.CodigoOficina,
                e => e.CodigoOficina,
                (o, empleadosGroup) => new { Oficina = o, Empleados = empleadosGroup }
            )
            .SelectMany(
                x => x.Empleados.DefaultIfEmpty(),
                (oficina, empleado) => new { Oficina = oficina.Oficina, Empleado = empleado }
            )
            .Where(x => x.Empleado == null || !_context.Clientes.Any(c =>
                        c.CodigoEmpleadoRepVentas == x.Empleado.CodigoEmpleado &&
                        _context.Pedidos.Any(pd =>
                            pd.CodigoCliente == c.CodigoCliente &&
                            _context.DetallePedidos.Any(dp =>
                                dp.CodigoPedido == pd.CodigoPedido &&
                                _context.Productos.Any(p =>
                                    dp.CodigoProducto == p.CodigoProducto &&
                                    p.Gama == "Frutales"
                                )
                            )
                        )
                    )
            )
            .Select(x => new
            {
                CodigoOficina = x.Oficina.CodigoOficina,
                Ciudad = x.Oficina.Ciudad,
                Region = x.Oficina.Region,
                Pais = x.Oficina.Pais,
                CodigoPostal = x.Oficina.CodigoPostal,
                Telefono = x.Oficina.Telefono,
                LineasDeDireccion = $"{x.Oficina.LineaDireccion1}, {x.Oficina.LineaDireccion2}"
            })
            .ToListAsync();

        var resultadoFinal = new List<object>
        {
            new { Msg = mensaje, DatosConsultados = consulta }
        };

        return resultadoFinal;
    }
}