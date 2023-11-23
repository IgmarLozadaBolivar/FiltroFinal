using Domain.Entities;
namespace Domain.Interfaces;

public interface IDetallePedido : IGeneric<DetallePedido>
{
    Task<IEnumerable<object>> VeinteProductosMasVendidos();
    Task<object> ProductosQueFacturaronMasDe3000Euros();
    Task<object> ProductoQueVendioMasUnidades();
}