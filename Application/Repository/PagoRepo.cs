using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
namespace Application.Repository;

public class PagoRepo : Generic<Pago>, IPago
{
    protected readonly DbFirstContext _context;

    public PagoRepo(DbFirstContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Pago>> GetAllAsync()
    {
        return await _context.Pagos
            //.Include(p => p.)
            .ToListAsync();
    }

    // * Posible solucion al tema de dos Identificadores
    public override async Task<Pago> GetByIdx2Async(int codigoCliente, string idTransaccion)
    {
        return await _context.Pagos
            .FirstOrDefaultAsync(p => p.CodigoCliente == codigoCliente && p.IdTransaccion == idTransaccion);
    }

    public override async Task<(int totalRegistros, IEnumerable<Pago> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.Pagos as IQueryable<Pago>;

        if (!string.IsNullOrEmpty(search))
        {
            //query = query.Where(p => p.YourPropertyNotString.ToString().ToLower().Contains(search));
            query = query.Where(p => p.FechaPago.ToString().ToLower().Contains(search));
        }

        // * No maneja un ID o Identificador unico, intentando solucionar este problema
        //query = query.OrderBy(p => p.Id);
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }
}