using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController:ControllerBase
    {

        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Libro>> Get(int id)
        //{
        //    return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x=>x.id == id);  
        //}

        //[HttpPost]
        //public async Task<ActionResult> Post(Libro libro)
        //{

        //    var existeAutor = await context.Autores.AnyAsync(x => x.id == libro.autor_id);
        //    if (!existeAutor)
        //    {
        //        return BadRequest($"No existe el autor {libro.autor_id}");
        //    }

        //    context.Add(libro);
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}



        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Libro libro, int id)
        {

            if (libro.id != id)
            {
                return BadRequest("Error de id");
            }
            var existe = await context.Libros.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Update(libro);
            await context.SaveChangesAsync();
            return Ok();
        }



        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Libros.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Libro() { id = id });
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
