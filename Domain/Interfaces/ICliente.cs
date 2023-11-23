using Domain.Entities;
namespace Domain.Interfaces;

public interface ICliente : IGeneric<Cliente>
{
    Task<IEnumerable<object>> ClientesQueNoHayanHechoPagos();
    Task<object> ClientesYCantidadDePedidosRealizados();
}