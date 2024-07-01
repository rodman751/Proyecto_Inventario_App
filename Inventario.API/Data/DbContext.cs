using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Inventario.Entidades;

    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext (DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<Inventario.Entidades.AjusteProducto> AjusteProducto { get; set; } = default!;

        public DbSet<Inventario.Entidades.DetalleAjusteProducto> DetalleAjusteProducto { get; set; } = default!;

        public DbSet<Inventario.Entidades.Producto> Producto { get; set; } = default!;
        public DbSet<Inventario.Entidades.MovimientoInventario> MovimientoInventario { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>()
                .Property(p => p.Costo)
                .HasColumnType("decimal(18,2)"); // o utiliza HasPrecision(18, 2)

            modelBuilder.Entity<Producto>()
                .Property(p => p.PVP)
                .HasColumnType("decimal(18,2)"); // o utiliza HasPrecision(18, 2)
        }



}
