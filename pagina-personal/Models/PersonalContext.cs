using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace pagina_personal.Models;

public partial class PersonalContext : DbContext
{
    public PersonalContext()
    {
    }

    public PersonalContext(DbContextOptions<PersonalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<Header> Headers { get; set; }

    public virtual DbSet<HeaderFotoCarrusel> HeaderFotoCarrusels { get; set; }

    public virtual DbSet<Link> Links { get; set; }

    public virtual DbSet<Personalizacion> Personalizacions { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.IdContacto).HasName("PK__contacto__4B1329C7668F686E");

            entity.ToTable("contacto");

            entity.Property(e => e.IdContacto)
                .ValueGeneratedNever()
                .HasColumnName("idContacto");
            entity.Property(e => e.Foto)
                .HasColumnType("text")
                .HasColumnName("foto");
            entity.Property(e => e.Subtitulo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("subtitulo");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titulo");
        });

        modelBuilder.Entity<Header>(entity =>
        {
            entity.HasKey(e => e.IdHeader).HasName("PK__header__0DC09BB83A2B30FE");

            entity.ToTable("header");

            entity.Property(e => e.IdHeader)
                .ValueGeneratedNever()
                .HasColumnName("idHeader");
            entity.Property(e => e.DocumentoCv)
                .HasColumnType("text")
                .HasColumnName("documentoCv");
            entity.Property(e => e.FotoFondo)
                .HasColumnType("text")
                .HasColumnName("fotoFondo");
            entity.Property(e => e.Subtitulo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("subtitulo");
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("titulo");
        });

        modelBuilder.Entity<HeaderFotoCarrusel>(entity =>
        {
            entity.HasKey(e => e.IdHeaderFotoCarrusel).HasName("PK__header_f__C4700893E8E6A36A");

            entity.ToTable("header_foto_carrusel");

            entity.Property(e => e.IdHeaderFotoCarrusel).HasColumnName("idHeaderFotoCarrusel");
            entity.Property(e => e.Foto)
                .HasColumnType("text")
                .HasColumnName("foto");
        });

        modelBuilder.Entity<Link>(entity =>
        {
            entity.HasKey(e => e.IdLink).HasName("PK__link__1439D9E321A43A8A");

            entity.ToTable("link");

            entity.Property(e => e.IdLink).HasColumnName("idLink");
            entity.Property(e => e.Foto)
                .HasColumnType("text")
                .HasColumnName("foto");
            entity.Property(e => e.Link1)
                .HasColumnType("text")
                .HasColumnName("link");
            entity.Property(e => e.Texto)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("texto");
        });

        modelBuilder.Entity<Personalizacion>(entity =>
        {
            entity.HasKey(e => e.IdPersonalizacion).HasName("PK__personal__18DA781FC3C5FE77");

            entity.ToTable("personalizacion");

            entity.Property(e => e.IdPersonalizacion)
                .ValueGeneratedNever()
                .HasColumnName("idPersonalizacion");
            entity.Property(e => e.Footer)
                .HasColumnType("text")
                .HasColumnName("footer");
            entity.Property(e => e.Nav)
                .HasColumnType("text")
                .HasColumnName("nav");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
