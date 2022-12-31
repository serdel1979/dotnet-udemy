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
            return await context.Autores.ToListAsync();
        }


       

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x=>x.id == id);
            if(autor == null)
            {
                return NotFound();
            }
            return autor;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.nombre.Contains(nombre));
            if (autor == null)
            {
                return NotFound();
            }
            return autor;
        }


        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {

            var existe = await context.Autores.AnyAsync(x => x.nombre == autor.nombre);
            if (existe)
            {
                return BadRequest($"El autor {autor.nombre} ya existe!!!");
            }

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
