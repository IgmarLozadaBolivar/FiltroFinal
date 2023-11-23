using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DbFirstContext : DbContext
    {
        public DbFirstContext()
        { }

        public DbFirstContext(DbContextOptions<DbFirstContext> options)
            : base(options)
        { }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<DetallePedido> DetallePedidos { get; set; }
        public virtual DbSet<Empleado> Empleados { get; set; }
        public virtual DbSet<GamaProducto> GamaProductos { get; set; }
        public virtual DbSet<Oficina> Oficinas { get; set; }
        public virtual DbSet<Pago> Pagos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;user=root;password=123456;database=gardendb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci").HasCharSet("utf8mb4");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}