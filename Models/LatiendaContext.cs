using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace LaTiendaAPI.Models;

public partial class LatiendaContext : DbContext
{
    public LatiendaContext()
    {
    }

    public LatiendaContext(DbContextOptions<LatiendaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<UsuarioRole> UsuarioRoles { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("workstation id=Productos.mssql.somee.com;packet size=4096;user id=Lucesita17_SQLLogin_7;pwd=t7szbj8ll4;data source=Productos.mssql.somee.com;persist security info=False;initial catalog=Productos;TrustServerCertificate=True");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.idCategoria).HasName("PK_cat_id_categoria");

            entity.ToTable("categorias");

            entity.Property(e => e.idCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.estado).HasColumnName("estado");
            entity.Property(e => e.nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.idProducto).HasName("PK_pro_id_producto");

            entity.ToTable("productos");

            entity.Property(e => e.idProducto).HasColumnName("id_producto");
            entity.Property(e => e.estado).HasColumnName("estado");
            entity.Property(e => e.idCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.precio)
                .HasColumnType("decimal(13, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.stock).HasColumnName("stock");

            entity.HasOne(d => d.objCategoria).WithMany(p => p.productos)
                .HasForeignKey(d => d.idCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_pro_id_categoria");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK_Roles");
            entity.ToTable("roles");
            entity.HasIndex(e => e.Nombre, "UQ_Roles_NombreRol").IsUnique();
            entity.Property(e => e.RolId).HasColumnName("rolId");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK_Usuarios");
            entity.ToTable("usuarios");
            entity.HasIndex(e => e.NroDoc, "UQ_Usuarios_NumeroDocumento").IsUnique();
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");
            entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false).HasColumnName("email");
            entity.Property(e => e.Nombre).HasMaxLength(200).IsUnicode(false).HasColumnName("nombre");
            entity.Property(e => e.NroDoc).HasMaxLength(13).IsUnicode(false).HasColumnName("nroDoc");
            entity.Property(e => e.TipoDoc).HasMaxLength(3).IsUnicode(false).HasColumnName("tipoDoc");
        });

        modelBuilder.Entity<UsuarioRole>(entity =>
        {
            entity.HasKey(e => e.UsuarioRolId).HasName("PK_UsuarioRoles");
            entity.ToTable("usuarioRoles");
            entity.HasIndex(e => new { e.UsuarioId, e.RolId }, "UQ_UsuarioRoles").IsUnique();
            entity.Property(e => e.UsuarioRolId).HasColumnName("usuarioRolId");
            entity.Property(e => e.RolId).HasColumnName("rolId");
            entity.Property(e => e.UsuarioId).HasColumnName("usuarioId");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioRoles_Roles");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioRoles_Usuarios");
        });

        OnModelCreatingPartial(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}