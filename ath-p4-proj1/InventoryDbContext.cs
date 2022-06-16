
using Microsoft.EntityFrameworkCore;

namespace ath_p4_proj1
{
    internal class InventoryDbContext : DbContext
    {
        public DbSet<Models.Employee> Employees { get; set; }
        public DbSet<Models.Device> Devices { get; set; }
        public DbSet<Models.DeviceHistory> DeviceHistories { get; set; }
        public DbSet<Models.DeviceMalfunction> DeviceMalfunctions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WB_Proj1;Integrated Security=True;
                                        Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                        MultiSubnetFailover=False";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
