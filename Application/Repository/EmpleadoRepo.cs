using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
namespace Application.Repository;

public class EmpleadoRepo : Generic<Empleado>, IEmpleado
{
    protected readonly DbFirstContext _context;

    public EmpleadoRepo(DbFirstContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Empleado>> GetAllAsync()
    {
        return await _context.Empleados
            //.Include(p => p.)
            .ToListAsync();
    }

    public override async Task<Empleado> GetByIdAsync(int codigoEmpleado)
    {
        return await _context.Empleados
            //.Include(p => p.)
            .FirstOrDefaultAsync(p => p.CodigoEmpleado == codigoEmpleado);
    }

    public override async Task<(int totalRegistros, IEnumerable<Empleado> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Empleados as IQueryable<Empleado>;

        if (!string.IsNullOrEmpty(search))
        {
            //query = query.Where(p => p.YourPropertyNotString.ToString().ToLower().Contains(search));
            query = query.Where(p => p.Nombre.ToLower().Contains(search));
        }

        query = query.OrderBy(p => p.CodigoEmpleado);
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }
}