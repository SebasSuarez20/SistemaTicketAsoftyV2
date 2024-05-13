using System.Linq.Expressions;

namespace SistemaTickets.Services
{
    public interface IdbHandler<T> : IDisposable where T : class
    {
        public Task<IEnumerable<T>> GetAllAsyncForAll(Expression<Func<T, bool>> _where = null);
        public Task CreateAllAsync(T entity);
        public Task UpdateAsyncAll(T entity, object _wh);
    }
}
