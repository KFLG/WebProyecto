using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebProyecto.Modelos;

namespace WebProyecto.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bodega> Bodegas { get; set; }

        public DbSet<CategoriaFlor> CategoriasFlor { get; set; }

        public DbSet<CategoriaPerfume> CategoriasPerfume { get; set; }

        public DbSet<Marca> Marcas { get; set; }

        public DbSet<Especie> Especies { get; set; }

        public DbSet<Flor> Flores { get; set; }

        public DbSet<Perfume> Perfumes { get; set; }

        public DbSet<UsuarioAplicacion> UsuarioAplicacion { get; set; }

        public DbSet<BodegaProducto> BodegaProductos { get; set; }

        public DbSet<Inventario> Inventario { get; set; }

        public DbSet<InventarioDetalle> InventarioDetalle { get; set; }

        public DbSet<CarroCompras> CarroCompras { get; set; }

        public DbSet<Orden> Orden { get; set; }

        public DbSet<OrdenDetalle> OrdenDetalle { get; set; }



















    }
}
