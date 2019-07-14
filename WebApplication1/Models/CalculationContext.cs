using System.Data.Entity;

namespace WebApplication1.Models
{
    public class CalculationContext : DbContext
    {
        public DbSet<Calculation> CalculationHistory { get; set; }
    }
}