using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AuthoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AuthoresController(ApplicationDbContext context) { 
            this.context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await context.Autores.Include(x => x.libros).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {

            if (autor.id != id)
            {
                return BadRequest("Error de id");
            }
            var existe = await context.Autores.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { id = id });
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
