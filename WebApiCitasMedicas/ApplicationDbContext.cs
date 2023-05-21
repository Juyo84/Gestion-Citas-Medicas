using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Controllers;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas
{
    public class ApplicationDbContext : IdentityDbContext
    {



        public ApplicationDbContext(DbContextOptions options) : base(options)
        {





        }



        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<HistorialMedico> HistorialMedicos { get; set; }



    }
}