using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/pacientes")]

    public class PacientesController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;

        public PacientesController(ApplicationDbContext context)
        {

            this.dbContext = context;

        }

        [HttpGet]

        public async Task<ActionResult<List<Paciente>>> Get()
        {

            return await dbContext.Pacientes.ToListAsync();

        }

        [HttpPost]

        public async Task<ActionResult> Post(Paciente paciente)
        {

            dbContext.Add(paciente);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {

            var exist = await dbContext.Pacientes.AnyAsync(x => x.Id == id);

            if (!exist)
            {

                return NotFound("No se encontro el registro en la base de datos");

            }

            dbContext.Remove(new Paciente()
            {

                Id = id

            });
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(Paciente paciente, int id)
        {

            if (paciente.Id != id)
            {

                return BadRequest("El id del paciente no coincide con el establecido en la url");

            }

            var existePaciente = await dbContext.Pacientes.AnyAsync(c => c.Id == paciente.Id);

            if (!existePaciente)
            {

                return BadRequest("No existe el paciente");

            }

            dbContext.Update(paciente);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

    }
}