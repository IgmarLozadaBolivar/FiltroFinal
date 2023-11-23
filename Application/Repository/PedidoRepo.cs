using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
namespace Application.Repository;

public class PedidoRepo : Generic<Pedido>, IPedido
{
    protected readonly DbFirstContext _context;

    public PedidoRepo(DbFirstContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Pedido>> GetAllAsync()
    {
        return await _context.Pedidos
            .ToListAsync();
    }

    public override async Task<Pedido> GetByIdAsync(int codigoPedido)
    {
        return await _context.Pedidos
            .FirstOrDefaultAsync(p => p.CodigoPedido == codigoPedido);
    }

    public override async Task<(int totalRegistros, IEnumerable<Pedido> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Pedidos as IQueryable<Pedido>;

        if (!string.IsNullOrEmpty(search))
        {
            //query = query.Where(p => p.YourPropertyNotString.ToString().ToLower().Contains(search));
            query = query.Where(p => p.Estado.ToLower().Contains(search));
        }

        query = query.OrderBy(p => p.CodigoPedido);
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    // * Listado de pedidos que no se entregaron a tiempo por X razon
    public async Task<IEnumerable<object>> PedidosQueNoFueronEntregadosATiempo()
    {
        var mensaje = "Pedidos que no fueron entregados a tiempo".ToUpper();

        var consulta = from p in _context.Pedidos
                       where p.FechaEsperada < p.FechaEntrega
                       select new
                       {
                           CodigoDePedido = p.CodigoPedido,
                           CodigoDeCliente = p.CodigoCliente,
                           FechaEsperada = p.FechaEsperada,
                           FechaEntrega = p.FechaEntrega,
                       };

        var resultado = await consulta.ToListAsync();

        var resultadoFinal = new List<object>
        {
            new { Title = mensaje, DatosConsultados = resultado }
        };

        return resultadoFinal;
    }
}