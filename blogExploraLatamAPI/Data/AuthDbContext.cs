using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace blogExploraLatamAPI.Data
{
    public class AuthDbContext: IdentityDbContext
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options):  base(options) { }



        // OnModelCreating, Se usa para crear datos iniciales seed data, como roles y usuarios por defecto.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // IDs unicos para los roles 
            var readerRoleId = "5cff7a25-b488-457c-a66f-8aab78b583ab";
            var writeRoleId = "2ce2be33-a852-4de7-b0ec-48e672f3a502";

            // Lista de roles iniciales
            var roles = new List<IdentityRole>()
            {

                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name ="Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },  {
                new IdentityRole()
                {
                    Id = writeRoleId,
                    Name ="Writer", 
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                }}
            };


            // Guarda los roles en la base de datos cuando se crea o se migra
            builder.Entity<IdentityRole>().HasData(roles);

            // Crear un usuario administrador inicial
            var adminUserId = "7ed2df58-cdf9-4f8a-a432-1331beb5397d";

            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                NormalizedUserName = "admin@gmail.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "admin@1234");
            builder.Entity<IdentityUser>().HasData(admin);

            // Asignar roles al usuario administrador
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>()
                {
                    UserId = adminUserId, // ID del usuario admin
                    RoleId = readerRoleId // Rol de lectura
                },
                new IdentityUserRole<string>()
                {
                    UserId = adminUserId, // ID del usuario admin
                    RoleId = writeRoleId  // Rol de escritura
                }

            };

            // Guarda las relaciones usuario-rol
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
