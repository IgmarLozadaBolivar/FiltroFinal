using System.Linq.Expressions;
namespace Domain.Interfaces;

public interface IGeneric<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<T> GetByIdAsync(string id);

    // * Caso especial para manejar dos Identificadores como llaves primarias
    Task<T> GetByIdx1Async(int codigoPedido, string codigoProducto);
    Task<T> GetByIdx2Async(int codigoCliente, string idTransaccion);

    Task<IEnumerable<T>> GetAllAsync();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, string search);
    Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(int pageIndex, int pageSize, int search);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity);
}