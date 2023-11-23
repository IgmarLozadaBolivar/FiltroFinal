using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
namespace Application.Repository;

public class ClienteRepo : Generic<Cliente>, ICliente
{
    protected readonly DbFirstContext _context;

    public ClienteRepo(DbFirstContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return await _context.Clientes
            //.Include(p => p.)
            .ToListAsync();
    }

    public override async Task<Cliente> GetByIdAsync(int codigoCliente)
    {
        return await _context.Clientes
            //.Include(p => p.)
            .FirstOrDefaultAsync(p => p.CodigoCliente == codigoCliente);
    }

    public override async Task<(int totalRegistros, IEnumerable<Cliente> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Clientes.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.NombreCliente.ToLower().Contains(search));
        }

        query = query.OrderBy(p => p.CodigoCliente);
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    public async Task<IEnumerable<object>> ClientesQueNoHayanHechoPagos()
    {
        var mensaje = "Clientes que no hayan hecho pagos, se retorna el nombre del cliente, el nombre y ciudad de la oficina del representante".ToUpper();

        var consulta = from c in _context.Clientes
                       join p in _context.Pagos on c.CodigoCliente equals p.CodigoCliente into pagos
                       join e in _context.Empleados on c.CodigoEmpleadoRepVentas equals e.CodigoEmpleado
                       join o in _context.Oficinas on e.CodigoOficina equals o.CodigoOficina
                       from pago in pagos.DefaultIfEmpty()
                       where pago == null
                       select new
                       {
                           NombreDelCliente = c.NombreCliente,
                           NombreDelRepresentante = e.Nombre,
                           ApellidosDelRepresentante = $"{e.Apellido1} {e.Apellido2}",
                           CiudadDeLaOficina = o.Ciudad
                       };

        var resultado = await consulta.ToListAsync();

        var resultadoFinal = new List<object>
        {
            new { Title = mensaje, DatosConsultados = resultado}
        };

        return resultadoFinal;
    }

    public async Task<object> ClientesYCantidadDePedidosRealizados()
    {
        var mensaje = "Retornar clientes y cantidad de pedidos realizados".ToUpper();

        var clientesConPedidos = await _context.Clientes
        .Select(cliente => new
        {
            cliente.NombreCliente,
            PedidosRealizados = _context.Pedidos.Count(pedido => pedido.CodigoCliente == cliente.CodigoCliente)
        })
        .ToListAsync();

        var resultado = new List<object>
        {
            new { Title = mensaje, DatosConsultados = clientesConPedidos }
        };

        return resultado;
    }
}