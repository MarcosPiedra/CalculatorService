using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService.Domain
{
    public interface IRepository<T>
    {
        Task Add(T entity);
        IQueryable<T> Query { get; }
        Task Save();
    }
}
