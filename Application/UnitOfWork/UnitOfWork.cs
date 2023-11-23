using Application.Repository;
using Domain.Interfaces;
using Persistence;
namespace Application.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DbFirstContext context;
    private ClienteRepo _clientes;
    private DetallePedidoRepo _detallePedidos;
    private EmpleadoRepo _empleados;
    private GamaProductoRepo _gamaProductos;
    private OficinaRepo _oficinas;
    private PagoRepo _pagos;
    private PedidoRepo _pedidos;
    private ProductoRepo _productos;

    public UnitOfWork(DbFirstContext _context)
    {
        context = _context;
    }

    public ICliente Clientes
    {
        get
        {
            if (_clientes == null)
            {
                _clientes = new ClienteRepo(context);
            }
            return _clientes;
        }
    }

    public IDetallePedido DetallePedidos
    {
        get
        {
            if (_detallePedidos == null)
            {
                _detallePedidos = new DetallePedidoRepo(context);
            }
            return _detallePedidos;
        }
    }

    public IEmpleado Empleados
    {
        get
        {
            if (_empleados == null)
            {
                _empleados = new EmpleadoRepo(context);
            }
            return _empleados;
        }
    }

    public IGamaProducto GamaProductos
    {
        get
        {
            if (_gamaProductos == null)
            {
                _gamaProductos = new GamaProductoRepo(context);
            }
            return _gamaProductos;
        }
    }

    public IOficina Oficinas
    {
        get
        {
            if (_oficinas == null)
            {
                _oficinas = new OficinaRepo(context);
            }
            return _oficinas;
        }
    }

    public IPago Pagos
    {
        get
        {
            if (_pagos == null)
            {
                _pagos = new PagoRepo(context);
            }
            return _pagos;
        }
    }

    public IPedido Pedidos
    {
        get
        {
            if (_pedidos == null)
            {
                _pedidos = new PedidoRepo(context);
            }
            return _pedidos;
        }
    }

    public IProducto Productos
    {
        get
        {
            if (_productos == null)
            {
                _productos = new ProductoRepo(context);
            }
            return _productos;
        }
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public async Task<int> SaveAsync()
    {
        return await context.SaveChangesAsync();
    }
}