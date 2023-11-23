using Domain.Entities;
namespace Domain.Interfaces;

public interface IProducto : IGeneric<Producto>
{
    Task<IEnumerable<object>> ProductosQueNoHanAparecidoEnUnPedido();
}