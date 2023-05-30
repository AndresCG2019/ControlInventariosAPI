using ControlInventariosAPI.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControlInventariosAPI
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Se configuran las llaves foraneas de esta manera para poder tener expuesto el id de la entidad relacionada sin
            // tener que recurrir a su propiedad de navegacion.

            modelBuilder.Entity<Articulo>()
            .HasOne(x => x.Categoria)
            .WithMany(x => x.Articulos)
            .HasForeignKey(x => x.IdCategoria);

            modelBuilder.Entity<ArticuloIngreso>()
            .HasOne(x => x.Articulo)
            .WithMany(x => x.Ingresos)
            .HasForeignKey(x => x.ClaveArticulo);

            modelBuilder.Entity<ArticuloIngreso>()
            .HasOne(x => x.Proveedor)
            .WithMany(x => x.Ingresos)
            .HasForeignKey(x => x.IdProveedor);

            modelBuilder.Entity<Pedido>()
            .HasOne(x => x.Cliente)
            .WithMany(x => x.Pedidos)
            .HasForeignKey(x => x.IdCliente);

            modelBuilder.Entity<ArticuloEgreso>()
            .HasOne(x => x.Articulo)
            .WithMany(x => x.Egresos)
            .HasForeignKey(x => x.ClaveArticulo);

            modelBuilder.Entity<ArticuloEgreso>()
            .HasOne(x => x.Pedido)
            .WithMany(x => x.Egresos)
            .HasForeignKey(x => x.IdPedido);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<ArticuloIngreso> ArticulosIngresos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ArticuloEgreso> ArticuloEgresos { get; set; }
    }
}
