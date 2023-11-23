using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence;
namespace Application.Repository;

public class GamaProductoRepo : Generic<GamaProducto>, IGamaProducto
{
    protected readonly DbFirstContext _context;

    public GamaProductoRepo(DbFirstContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<GamaProducto>> GetAllAsync()
    {
        return await _context.GamaProductos
            //.Include(p => p.)
            .ToListAsync();
    }

    public override async Task<GamaProducto> GetByIdAsync(string gama)
    {
        return await _context.GamaProductos
            //.Include(p => p.)
            .FirstOrDefaultAsync(p => p.Gama == gama);
    }

    public override async Task<(int totalRegistros, IEnumerable<GamaProducto> registros)> GetAllAsync(int pageIndex, int pageSize, string search)
    {
        var query = _context.GamaProductos as IQueryable<GamaProducto>;

        if (!string.IsNullOrEmpty(search))
        {
            //query = query.Where(p => p.YourPropertyNotString.ToString().ToLower().Contains(search));
            query = query.Where(p => p.DescripcionTexto.ToLower().Contains(search));
        }

        query = query.OrderBy(p => p.Gama);
        var totalRegistros = await query.CountAsync();
        var registros = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (totalRegistros, registros);
    }
}