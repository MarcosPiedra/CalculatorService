using CalculatorService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalculatorServices.SqlDataAccess.Configs
{
    public class OperationConfig : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.ToTable("Operation");

            builder.Property(op => op.Id).HasColumnName("Id");
            builder.Property(op => op.TrackId).HasColumnName("TrackId");
            builder.Property(op => op.OperationType).HasColumnName("Type");
            builder.Property(op => op.DateTime).HasColumnName("Date").HasConversion<string>(); 
            builder.Property(op => op.Calculation).HasColumnName("Calculation");

            builder.HasKey(a => a.Id);
        }
    }
}
