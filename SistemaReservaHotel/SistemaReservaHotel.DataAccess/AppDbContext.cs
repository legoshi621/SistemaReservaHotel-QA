using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.Entities;
using System.Reflection.Emit;


namespace SistemaReservaHotel.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // --- Tablas (DbSets) ---
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<TipoHabitacion> TipoHabitaciones { get; set; }
        public DbSet<Comodidad> Comodidades { get; set; }
        public DbSet<HabitacionComodidad> HabitacionComodidades { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<TemporadaPrecio> TemporadasPrecios { get; set; }

        // ═══ NUEVAS ENTIDADES DE SEGURIDAD ═══
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }
        public DbSet<RolPermiso> RolesPermisos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de precisión para tipos decimales (Evita advertencias y pérdida de datos)
            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reserva>()
                .Property(r => r.PrecioTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TemporadaPrecio>()
                .Property(t => t.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Habitacion>()
                .Property(h => h.PrecioBase)
                .HasPrecision(18, 2);
            // ═══ CONFIGURACIÓN NUEVAS ENTIDADES ═══

            // Usuario: índice único en Email
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("Usuarios");
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.FechaCreacion)
                    .HasDefaultValueSql("GETDATE()");
            });

            // Rol: índice único en Nombre
            modelBuilder.Entity<Rol>(e =>
            {
                e.ToTable("Roles");
                e.HasIndex(r => r.Nombre).IsUnique();
            });

            // Permiso: índice único en Nombre
            modelBuilder.Entity<Permiso>(e =>
            {
                e.ToTable("Permisos");
                e.HasIndex(p => p.Nombre).IsUnique();
            });

            // UsuarioRol: índice compuesto único (UsuarioId + RolId)
            modelBuilder.Entity<UsuarioRol>(e =>
            {
                e.ToTable("UsuariosRoles");
                e.HasIndex(ur => new { ur.UsuarioId, ur.RolId }).IsUnique();

                e.HasOne(ur => ur.Usuario)
                    .WithMany(u => u.UsuarioRoles)
                    .HasForeignKey(ur => ur.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ur => ur.Rol)
                    .WithMany(r => r.UsuarioRoles)
                    .HasForeignKey(ur => ur.RolId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RolPermiso: índice compuesto único (RolId + PermisoId)
            modelBuilder.Entity<RolPermiso>(e =>
            {
                e.ToTable("RolesPermisos");
                e.HasIndex(rp => new { rp.RolId, rp.PermisoId }).IsUnique();

                e.HasOne(rp => rp.Rol)
                    .WithMany(r => r.RolPermisos)
                    .HasForeignKey(rp => rp.RolId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(rp => rp.Permiso)
                    .WithMany(p => p.RolPermisos)
                    .HasForeignKey(rp => rp.PermisoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        
    
    // Configuración de Llave Primaria Compuesta para la tabla intermedia
    modelBuilder.Entity<HabitacionComodidad>()
                .HasKey(hc => new { hc.HabitacionId, hc.ComodidadId });

            modelBuilder.Entity<UsuarioRol>()
                        .HasKey(ur => new { ur.UsuarioId, ur.RolId });

            modelBuilder.Entity<RolPermiso>()
                .HasKey(rp => new { rp.RolId, rp.PermisoId });
        }
    }
}