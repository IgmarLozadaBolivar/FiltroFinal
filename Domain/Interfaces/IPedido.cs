using Domain.Entities;
namespace Domain.Interfaces;

public interface IPedido : IGeneric<Pedido>
{
    Task<IEnumerable<object>> PedidosQueNoFueronEntregadosATiempo();
}