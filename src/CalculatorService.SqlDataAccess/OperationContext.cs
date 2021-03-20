using CalculatorServices.SqlDataAccess.Configs;
using Microsoft.EntityFrameworkCore;
using OperatorModel = CalculatorService.Domain.Models.Operation;

namespace CalculatorService.SqlDataAccess
{
    public class OperationContext : DbContext
    {
        public OperationContext() : base()
        {
        }

        public OperationContext(DbContextOptions<OperationContext> options) : base(options)
        {
        }

        public DbSet<OperatorModel> Operations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new OperationConfig());
        }
    }
}
