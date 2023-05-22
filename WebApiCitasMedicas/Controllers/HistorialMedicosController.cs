using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;
using System.Data;

namespace WebApiCitasMedicas.Controllers
{

    [ApiController]
    [Route("api/historialMedicos")]

    public class HistorialMedicosController : ControllerBase
    {

        private readonly ApplicationDbContext dbContext;

        public HistorialMedicosController(ApplicationDbContext context)
        {

            this.dbContext = context;

        }

        [HttpGet]
        public async Task<ActionResult<List<HistorialMedico>>> GetAll()
        {
            return await dbContext.HistorialMedicos.ToListAsync();
        }

        [HttpGet("Por PacienteId")]
        public async Task<ActionResult<HistorialMedico>> GetById(int id)
        {
            List<HistorialMedico> hm = dbContext.HistorialMedicos.Where(x => x.PacienteId == id).ToList();

            if (hm == null)
            {

                return NotFound();

            }

            return Ok(hm);
        }

        [HttpPost]
        public async Task<ActionResult> Post(HistorialMedico hm)
        {
            var existePaciente = await dbContext.Pacientes.AnyAsync(x => x.Id == hm.PacienteId);

            if(!existePaciente)
            {
                return BadRequest("No existe el paciente.");
            }

            dbContext.Add(hm);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put(HistorialMedico historialMedico, int id)
        {
            var existeHM = await dbContext.HistorialMedicos.AnyAsync(x => x.Id == id);

            if(!existeHM)
            {
                return BadRequest("No existe el historial medico.");
            }

            if(historialMedico.Id != id)
            {
                return BadRequest("El ID del historial medico no coincide con el establecido en la URL.");
            }

            dbContext.Update(historialMedico);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.HistorialMedicos.AnyAsync(x => x.Id == id);
            if(!exist)
            {
                return NotFound("No se encontró el Historial Medico en la base de datos.");
            }

            dbContext.Remove(new HistorialMedico()
            {
                Id = id
            });

            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
