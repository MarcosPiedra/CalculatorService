using CalculatorService.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService.SqlDataAccess
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;
        private readonly OperationContext dbContext;

        public EFRepository(OperationContext dbContext)
        {
            this.dbSet = dbContext.Set<T>();
            this.dbContext = dbContext;
        }

        public async Task Add(T item) => await dbSet.AddAsync(item);
        public IQueryable<T> Query => dbSet.AsNoTracking();
        public async Task Save() => await dbContext.SaveChangesAsync();
    }
}
