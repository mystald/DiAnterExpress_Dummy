using Microsoft.EntityFrameworkCore;

namespace DiAnterExpressDummy.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentType> ShipmentTypes { get; set; }
        //public DbSet<TransactionInternal> TransactionInternals { get; set; }
    }
}
