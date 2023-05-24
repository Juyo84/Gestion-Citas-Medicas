using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/citas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsMedico")]

    public class CitasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public CitasController(ApplicationDbContext context)
        {

            this.dbContext = context;

        }


        //
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Cita>>> Get()
        {

            return await dbContext.Citas.ToListAsync();

        }


        // Punto 5 GET MEDICO
        [HttpGet("Id Medico")]
        public async Task<ActionResult<Cita>> GetById(int id)
        {

            List<Cita> citas = dbContext.Citas.Where(x => x.MedicoId == id).ToList();

            if (citas == null)
            {

                return NotFound();

            }

            return Ok(citas);

        }


        // Punto 5 Y 7 GET NOMBRE PACIENTE
        [HttpGet("Nombre Paciente")]
        [AllowAnonymous]
        public async Task<ActionResult<Cita>> Get(string nombrePaciente)
        {

            var citas = dbContext.Pacientes.FirstOrDefaultAsync(x => (x.Nombre + " " + x.Apellido_Paterno + " " + x.Apellido_Materno).ToUpper() == nombrePaciente.ToUpper()); //PACIENTE ID

            if (citas == null)
            {

                return NotFound();

            }

            List<Cita> datosPaciente = dbContext.Citas.Where(x => x.PacienteId == citas.Result.Id).ToList();

            if (datosPaciente == null)
            {

                return NotFound();

            }

            return Ok(datosPaciente);

        }


        // Punto 5 GET FECHA
        [HttpGet("Fecha")]
        public async Task<ActionResult<Cita>> GetById(string fecha)
        {

            DateTime fechaConv = DateTime.Parse(fecha);

            List<Cita> citas = dbContext.Citas.Where(x => x.Fecha == fechaConv).ToList();

            if (citas == null)
            {

                return NotFound();

            }

            return Ok(citas);

        }


        //PUNTO 2 LIMITACION DE 100 PACIENTES
        [HttpPost("Crear Cita")]
        [AllowAnonymous]
        public async Task<ActionResult> Post(Cita cita)
        {

            var existeMedico = await dbContext.Medicos.AnyAsync(c => c.Id == cita.MedicoId);

            if (!existeMedico)
            {

                return BadRequest("No existe el medico");

            }

            var existePaciente = await dbContext.Pacientes.AnyAsync(c => c.Id == cita.PacienteId);

            if (!existePaciente)
            {

                return BadRequest("No existe el paciente");

            }

            var pacientesMedico = new List<Cita>();
            var listaCitas = await dbContext.Citas.ToListAsync();

            foreach (var listaCita in listaCitas)
            {

                if (listaCita.MedicoId == cita.MedicoId && pacientesMedico.FindIndex(a => a.Id == listaCita.PacienteId) == -1)
                {

                    pacientesMedico.Add(listaCita);

                }

                if (listaCita.MedicoId == cita.MedicoId && cita.Fecha.ToString() == listaCita.Fecha.ToString())
                {

                    return BadRequest("La cita se empalma con otra");

                }

            }

            if (pacientesMedico.Count() >= 100)
            {

                return BadRequest("Se paso de limite de pacientes");

            }


            if (!cita.Fecha.ToString().Contains(":00:00"))
            {

                return BadRequest("La cita debera de ser por hora");

            }

            dbContext.Add(cita);
            await dbContext.SaveChangesAsync();
            return Ok();

        }


        //PUNTO 1 ELIMINAR CITA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {

            var exist = await dbContext.Citas.AnyAsync(x => x.Id == id);

            if (!exist)
            {

                return NotFound("No se encontro el registro en la base de datos");

            }

            dbContext.Remove(new Cita()
            {

                Id = id

            });
            await dbContext.SaveChangesAsync();
            return Ok();

        }


        //PUNTO  MODIFICAR CITAS
        [HttpPut("Modificar Citas")]
        public async Task<ActionResult> Put(Cita cita, int id)
        {

            if (cita.Id != id)
            {

                return BadRequest("El id del la cita no coincide con el establecido en la url");

            }

            var existeCita = await dbContext.Citas.AnyAsync(c => c.Id == cita.Id);

            if (!existeCita)
            {

                return BadRequest("No existe la cita");

            }

            var citas = await dbContext.Citas.ToListAsync();
            

            
            dbContext.Update(cita);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("Estadisticas Medicos")]
        public async Task<ActionResult<Cita>> Get(int id)
        {

            List<Cita> citasMedico = dbContext.Citas.Where(x => x.MedicoId == id).ToList();
            int cantidadCitas = 0;
            DateTime fechaActual = DateTime.Now;
            DateTime fechaReciente = DateTime.Parse("2500-12-30T11:59:59Z");

            foreach (var citaMedico in citasMedico)
            {

                if (citaMedico.Fecha >= fechaActual)
                {

                    cantidadCitas++;

                    if (fechaReciente > citaMedico.Fecha)
                    {

                        fechaReciente = citaMedico.Fecha;

                    }

                }

            }

            List<String> datosMedico = new List<String> { "MedicoId: " + id.ToString(), "Cantidad de citas: " + cantidadCitas.ToString(), "Siguiente Cita: " + fechaReciente.ToString()};

            return Ok(datosMedico);

        }

    }
}