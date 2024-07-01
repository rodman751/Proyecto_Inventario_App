﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inventario.API.Migrations
{
    [DbContext(typeof(DbContext))]
    [Migration("20240701150427_newdb3")]
    partial class newdb3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Inventario.Entidades.AjusteProducto", b =>
                {
                    b.Property<int>("ID_Ajuste")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Ajuste"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Impreso")
                        .HasColumnType("bit");

                    b.Property<string>("NumeroAjuste")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID_Ajuste");

                    b.ToTable("AjusteProducto");
                });

            modelBuilder.Entity("Inventario.Entidades.DetalleAjusteProducto", b =>
                {
                    b.Property<int>("ID_DetalleAjuste")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_DetalleAjuste"));

                    b.Property<int?>("AjusteProductoID_Ajuste")
                        .HasColumnType("int");

                    b.Property<int>("CantidadAjustada")
                        .HasColumnType("int");

                    b.Property<int>("ID_Ajuste")
                        .HasColumnType("int");

                    b.Property<int>("ID_Producto")
                        .HasColumnType("int");

                    b.Property<string>("RazonAjuste")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID_DetalleAjuste");

                    b.HasIndex("AjusteProductoID_Ajuste");

                    b.HasIndex("ID_Producto");

                    b.ToTable("DetalleAjusteProducto");
                });

            modelBuilder.Entity("Inventario.Entidades.MovimientoInventario", b =>
                {
                    b.Property<int>("ID_Movimiento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Movimiento"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaMovimiento")
                        .HasColumnType("datetime2");

                    b.Property<int>("ID_Producto")
                        .HasColumnType("int");

                    b.Property<string>("NumeroDocumento")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("StockActual")
                        .HasColumnType("int");

                    b.Property<int>("StockAnterior")
                        .HasColumnType("int");

                    b.Property<string>("TipoMovimiento")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID_Movimiento");

                    b.HasIndex("ID_Producto");

                    b.ToTable("MovimientoInventario");
                });

            modelBuilder.Entity("Inventario.Entidades.Producto", b =>
                {
                    b.Property<int>("ID_Producto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID_Producto"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Costo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("GravaIVA")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("PVP")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ID_Producto");

                    b.ToTable("Producto");
                });

            modelBuilder.Entity("Inventario.Entidades.DetalleAjusteProducto", b =>
                {
                    b.HasOne("Inventario.Entidades.AjusteProducto", null)
                        .WithMany("Detalles")
                        .HasForeignKey("AjusteProductoID_Ajuste");

                    b.HasOne("Inventario.Entidades.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ID_Producto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("Inventario.Entidades.MovimientoInventario", b =>
                {
                    b.HasOne("Inventario.Entidades.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ID_Producto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("Inventario.Entidades.AjusteProducto", b =>
                {
                    b.Navigation("Detalles");
                });
#pragma warning restore 612, 618
        }
    }
}