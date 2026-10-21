using Microsoft.EntityFrameworkCore;
using SistemaReservaHotel.Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


namespace SistemaReservaHotel.DataAccess.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Database.IsRelational())
            {
                await context.Database.MigrateAsync();
            }
            else
            {
                await context.Database.EnsureCreatedAsync();
            }

            // ═══ SEED PERMISOS ═══
            if (!context.Permisos.Any())
            {
                var permisos = new List<Permiso>
                {
                    // Ventas
                    new Permiso { Nombre = "CrearVenta", Descripcion = "Crear nuevas ventas", Categoria = "Ventas" },
                    new Permiso { Nombre = "EditarVenta", Descripcion = "Editar ventas existentes", Categoria = "Ventas" },
                    new Permiso { Nombre = "AnularVenta", Descripcion = "Anular ventas", Categoria = "Ventas" },
                    new Permiso { Nombre = "VerVentas", Descripcion = "Ver todas las ventas", Categoria = "Ventas" },
                    
                    // Clientes
                    new Permiso { Nombre = "CrearCliente", Descripcion = "Registrar nuevos clientes", Categoria = "Clientes" },
                    new Permiso { Nombre = "EditarCliente", Descripcion = "Editar clientes", Categoria = "Clientes" },
                    new Permiso { Nombre = "EliminarCliente", Descripcion = "Eliminar clientes", Categoria = "Clientes" },
                    new Permiso { Nombre = "VerClientes", Descripcion = "Ver listado de clientes", Categoria = "Clientes" },
                    
                    // Productos
                    new Permiso { Nombre = "CrearProducto", Descripcion = "Registrar nuevos productos", Categoria = "Productos" },
                    new Permiso { Nombre = "EditarProducto", Descripcion = "Editar productos", Categoria = "Productos" },
                    new Permiso { Nombre = "EliminarProducto", Descripcion = "Eliminar productos", Categoria = "Productos" },
                    new Permiso { Nombre = "VerProductos", Descripcion = "Ver catálogo de productos", Categoria = "Productos" },
                    new Permiso { Nombre = "ActualizarStock", Descripcion = "Actualizar inventario", Categoria = "Productos" },
                    
                    // Reportes
                    new Permiso { Nombre = "VerReportes", Descripcion = "Acceso a reportes", Categoria = "Reportes" },
                    new Permiso { Nombre = "ExportarVentas", Descripcion = "Exportar datos de ventas", Categoria = "Reportes" },
                    
                    // Administración
                    new Permiso { Nombre = "GestionarUsuarios", Descripcion = "Gestión de usuarios", Categoria = "Administracion" },
                    new Permiso { Nombre = "GestionarRoles", Descripcion = "Gestión de roles", Categoria = "Administracion" },
                    new Permiso { Nombre = "GestionarPermisos", Descripcion = "Gestión de permisos", Categoria = "Administracion" }
                };

                await context.Permisos.AddRangeAsync(permisos);
                await context.SaveChangesAsync();
            }
            // ═══ SEED ROLES ═══
            if (!context.Roles.Any())
            {
                var roles = new List<Rol>
                {
                    new Rol
                    {
                        Nombre = "Administrador",
                        Descripcion = "Acceso completo al sistema",
                        Activo = true
                    },
                    new Rol
                    {
                        Nombre = "Vendedor",
                        Descripcion = "Puede crear ventas y gestionar clientes",
                        Activo = true
                    },
                    new Rol
                    {
                        Nombre = "Bodeguero",
                        Descripcion = "Gestión de productos e inventario",
                        Activo = true
                    },
                    new Rol
                    {
                        Nombre = "Contador",
                        Descripcion = "Acceso a reportes y consultas",
                        Activo = true
                    },
                    new Rol
                    {
                        Nombre = "Usuario",
                        Descripcion = "Usuario básico del sistema",
                        Activo = true
                    }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }
            // ═══ ASIGNAR PERMISOS A ROLES ═══
            if (!context.RolesPermisos.Any())
            {
                var adminRol = await context.Roles.FirstAsync(r => r.Nombre == "Administrador");
                var vendedorRol = await context.Roles.FirstAsync(r => r.Nombre == "Vendedor");
                var bodegueroRol = await context.Roles.FirstAsync(r => r.Nombre == "Bodeguero");
                var contadorRol = await context.Roles.FirstAsync(r => r.Nombre == "Contador");

                var todosPermisos = await context.Permisos.ToListAsync();

                // Admin tiene TODOS los permisos
                foreach (var permiso in todosPermisos)
                {
                    context.RolesPermisos.Add(new RolPermiso
                    {
                        RolId = adminRol.Id,
                        PermisoId = permiso.Id
                    });
                }

                // Vendedor
                var permisosVendedor = todosPermisos
                    .Where(p => p.Categoria == "Ventas" || p.Categoria == "Clientes" || p.Nombre == "VerProductos")
                    .ToList();

                foreach (var permiso in permisosVendedor)
                {
                    context.RolesPermisos.Add(new RolPermiso
                    {
                        RolId = vendedorRol.Id,
                        PermisoId = permiso.Id
                    });
                }

                // Bodeguero
                var permisosBodeguero = todosPermisos
                    .Where(p => p.Categoria == "Productos")
                    .ToList();

                foreach (var permiso in permisosBodeguero)
                {
                    context.RolesPermisos.Add(new RolPermiso
                    {
                        RolId = bodegueroRol.Id,
                        PermisoId = permiso.Id
                    });
                }
                // Contador
                var permisosContador = todosPermisos
                    .Where(p => p.Categoria == "Reportes" || p.Nombre == "VerVentas")
                    .ToList();

                foreach (var permiso in permisosContador)
                {
                    context.RolesPermisos.Add(new RolPermiso
                    {
                        RolId = contadorRol.Id,
                        PermisoId = permiso.Id
                    });
                }

                await context.SaveChangesAsync();
            }

            // ═══ CREAR USUARIO ADMIN POR DEFECTO ═══
            if (!context.Usuario.Any())
            {
                var adminRol = await context.Roles.FirstAsync(r => r.Nombre == "Administrador");

                var adminUser = new Usuario
                {
                    NombreCompleto = "Administrador del Sistema",
                    Email = "admin@facturacion.com",
                    PasswordHash = HashPassword("Admin123!"),
                    Telefono = "7777-7777",
                    Activo = true,
                    FechaCreacion = DateTime.Now
                };

                await context.Usuario.AddAsync(adminUser);
                await context.SaveChangesAsync();

                // Asignar rol Admin
                context.UsuariosRoles.Add(new UsuarioRol
                {
                    UsuarioId = adminUser.Id,
                    RolId = adminRol.Id,
                    FechaAsignacion = DateTime.Now
                });

                await context.SaveChangesAsync();
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
