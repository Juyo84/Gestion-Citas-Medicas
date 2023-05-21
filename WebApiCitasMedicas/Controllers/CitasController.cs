using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/citas")]

    public class CitasController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public CitasController(ApplicationDbContext context)
        {

            this.dbContext = context;

        }

        [HttpGet]

        public async Task<ActionResult<List<Cita>>> Get()
        {

            return await dbContext.Citas.ToListAsync();

        }

        [HttpPost]

        public async Task<ActionResult> Post(Cita cita)
        {

            var existeMedico = await dbContext.Medicos.AnyAsync(c => c.Id == cita.MedicoId);

            if (!existeMedico)
            {

                return BadRequest("No existe el dueño");

            }

            var existePaciente = await dbContext.Pacientes.AnyAsync(c => c.Id == cita.PacienteId);

            if (!existePaciente)
            {

                return BadRequest("No existe el paciente");

            }

            dbContext.Add(cita);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

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

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(Cita cita, int id)
        {

            if (cita.Id != id)
            {

                return BadRequest("El id del cita no coincide con el establecido en la url");

            }

            var existeCita = await dbContext.Citas.AnyAsync(c => c.Id == cita.Id);

            if (!existeCita)
            {

                return BadRequest("No existe la cita");

            }

            dbContext.Update(cita);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

    }
}