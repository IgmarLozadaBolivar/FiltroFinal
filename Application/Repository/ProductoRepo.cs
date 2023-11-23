using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
namespace Application.Repository;

public class ProductoRepo : Generic<Producto>, IProducto
{
    protected readonly DbFirstContext _context;

    public ProductoRepo(DbFirstContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Producto>> GetAllAsync()
    {
        return await _context.Productos
            //.Include(p => p.)
            .ToListAsync();
    }

    public override async Task<Producto> GetByIdAsync(string codigoProducto)
    {
        return await _context.Productos
            //.Include(p => p.)
            .FirstOrDefaultAsync(p => p.CodigoProducto == codigoProducto);
    }

    public override async Task<(int totalRegistros, IEnumerable<Producto> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Productos as IQueryable<Producto>;

        if (!string.IsNullOrEmpty(search))
        {
            //query = query.Where(p => p.YourPropertyNotString.ToString().ToLower().Contains(search));
            query = query.Where(p => p.Nombre.ToLower().Contains(search));
        }

        query = query.OrderBy(p => p.CodigoProducto);
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }

    public async Task<IEnumerable<object>> ProductosQueNoHanAparecidoEnUnPedido()
    {
        var mensaje = "Productos que no han aparecido en un pedido".ToUpper();

        var consulta = from pr in _context.Productos
                       join dp in _context.DetallePedidos
                       on pr.CodigoProducto equals dp.CodigoProducto into gj
                       from subpedido in gj.DefaultIfEmpty()
                       where subpedido == null
                       select new
                       {
                           CodigoProducto = pr.CodigoProducto,
                           NombreProducto = pr.Nombre,
                           GamaProducto = pr.Gama,
                           Dimensiones = pr.Dimensiones,
                           Proveedores = pr.Proveedor,
                           Descripcion = pr.Descripcion,
                           CantidadEnStock = pr.CantidadEnStock,
                           PrecioVenta = pr.PrecioVenta,
                           PrecioProveedor = pr.PrecioProveedor
                       };

        var resultadoFinal = new List<object>
    {
        new { Msg = mensaje, DatosConsultados = await consulta.ToListAsync() }
    };

        return resultadoFinal;
    }
}