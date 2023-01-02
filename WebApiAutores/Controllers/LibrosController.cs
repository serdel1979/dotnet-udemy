using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController:ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTOres>> getLibro(int id)
        {
            var libros = await context.Libros.FirstOrDefaultAsync(x => x.id == id);
            //return await context.Libros.FirstOrDefaultAsync(x => x.id == id);
            return mapper.Map<LibroDTOres>(libros);
        }

        [HttpPost]
        public async Task<ActionResult> post(LibroDTO libro)
        {

            //var existeautor = await context.autores.anyasync(x => x.id == libro.autor_id);
            //if (!existeautor)
            //{
            //    return badrequest($"no existe el autor {libro.autor_id}");
            //}

            var librodto = mapper.Map<Libro>(libro);
            context.Add(librodto);
            await context.SaveChangesAsync();
            return Ok();
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(LibroDTOres libro, int id)
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

            context.Remove(new LibroDTOres() { id = id });
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
