using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCitasMedicas.Entidades;

namespace WebApiCitasMedicas.Controllers
{
    [ApiController]
    [Route("api/medicos")]

    public class MedicosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public MedicosController(ApplicationDbContext context)
        {

            this.dbContext = context;

        }

        [HttpGet]

        public async Task<ActionResult<List<Medico>>> Get()
        {

            return await dbContext.Medicos.ToListAsync();

        }

        [HttpPost]

        public async Task<ActionResult> Post(Medico medico)
        {

            dbContext.Add(medico);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete(int id)
        {

            var exist = await dbContext.Medicos.AnyAsync(x => x.Id == id);

            if (!exist)
            {

                return NotFound("No se encontro el registro en la base de datos");

            }

            dbContext.Remove(new Medico()
            {

                Id = id

            });
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(Medico medico, int id)
        {

            if (medico.Id != id)
            {

                return BadRequest("El id del medico no coincide con el establecido en la url");

            }

            var existeMedico = await dbContext.Medicos.AnyAsync(c => c.Id == medico.Id);

            if (!existeMedico)
            {

                return BadRequest("No existe el medico");

            }

            dbContext.Update(medico);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

    }

}