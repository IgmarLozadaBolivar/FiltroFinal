using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
namespace Application.Repository;

public class DetallePedidoRepo : Generic<DetallePedido>, IDetallePedido
{
    protected readonly DbFirstContext _context;

    public DetallePedidoRepo(DbFirstContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<DetallePedido>> GetAllAsync()
    {
        return await _context.DetallePedidos
            //.Include(p => p.)
            .ToListAsync();
    }

    // * Posible solucion al tema de dos Identificadores
    public override async Task<DetallePedido> GetByIdx1Async(int codigoPedido, string codigoProducto)
    {
        return await _context.DetallePedidos
            .FirstOrDefaultAsync(p => p.CodigoPedido == codigoPedido && p.CodigoProducto == codigoProducto);
    }

    public override async Task<(int totalRegistros, IEnumerable<DetallePedido> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.DetallePedidos as IQueryable<DetallePedido>;

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Cantidad.ToString().ToLower().Contains(search));
        }

        // * No maneja un ID o Identificador unico, intentando ver como solucionar este problema
        //query = query.OrderBy(p => p.Id);
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    public async Task<IEnumerable<object>> VeinteProductosMasVendidos()
    {
        var mensaje = "20 productos mas vendidos y el numero total de ventas de cada uno".ToUpper();

        var consulta = await _context.DetallePedidos
            .GroupBy(detallePedido => detallePedido.CodigoProducto)
            .Select(grp => new
            {
                CodigoDeProducto = grp.Key,
                TotalUnidadesVendidas = grp.Sum(dp => dp.Cantidad)
            })
            .OrderByDescending(result => result.TotalUnidadesVendidas)
            .Take(20)
            .ToListAsync();

        var resultadoFinal = new List<object>
        {
            new { Title = mensaje, DatosConsultados = consulta }
        };

        return resultadoFinal;
    }

    public async Task<object> ProductosQueFacturaronMasDe3000Euros()
    {
        var mensaje = "Productos que facturaron mas de 3000 euros".ToUpper();

        var grupos = await _context.DetallePedidos
        .GroupBy(detallePedido => detallePedido.CodigoProducto)
        .ToListAsync();

        var consulta = grupos
            .Join(
                _context.Productos,
                grupo => grupo.Key,
                producto => producto.CodigoProducto,
                (grupo, producto) => new
                {
                    NombreProducto = producto.Nombre,
                    UnidadesVendidas = grupo.Sum(dp => dp.Cantidad),
                    TotalFacturado = grupo.Sum(dp => dp.Cantidad * dp.PrecioUnidad),
                    TotalFacturadoConIVA = grupo.Sum(dp => dp.Cantidad * dp.PrecioUnidad * (decimal)1.21)
                })
            .Where(resultado => resultado.TotalFacturadoConIVA > 3000)
            .OrderByDescending(resultado => resultado.TotalFacturadoConIVA)
            .ToList();

        var resultadoFinal = new List<object>
        {
            new { Title = mensaje, DatosConsultados = consulta }
        };

        return resultadoFinal;
    }

    public async Task<object> ProductoQueVendioMasUnidades()
    {
        var mensaje = "Producto que vendio mas unidades".ToUpper();

        var productoMasVendido = await _context.DetallePedidos
        .GroupBy(detallePedido => detallePedido.CodigoProducto)
        .OrderByDescending(grupo => grupo.Sum(dp => dp.Cantidad))
        .Select(grupo => grupo.Key)
        .FirstOrDefaultAsync();

        var nombreProductoMasVendido = await _context.Productos
            .Where(producto => producto.CodigoProducto == productoMasVendido)
            .Select(producto => producto.Nombre)
            .FirstOrDefaultAsync();

        var resultado = new List<object>
        {
            new { Title = mensaje, DatosConsultados = nombreProductoMasVendido}
        };

        return resultado;
    }
}