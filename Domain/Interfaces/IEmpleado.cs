using Domain.Entities;
namespace Domain.Interfaces;

public interface IEmpleado : IGeneric<Empleado>
{
    Task<IEnumerable<object>> EmpleadosQueNoTieneUnCliente();
}