using apiSolicitudes.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace apiSolicitudes.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
