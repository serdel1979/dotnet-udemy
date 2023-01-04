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
        public async Task<ActionResult<LibroDTOConAutores>> getLibro(int id)
        {
            var libro = await context.Libros
                .Include(librodb => librodb.AutoresLibros)  
                .ThenInclude(autorLibroDb=> autorLibroDb.Autor)
                .FirstOrDefaultAsync(x => x.id == id);
            if (libro == null)
            {
                return NotFound();
            }

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.orden).ToList();

            return mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> post(LibroDTO libro)
        {

            if(libro.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            var autoresIds = await context.Autores
                .Where(autorbd=> libro.AutoresIds.Contains(autorbd.id)).Select(x=>x.id)
                .ToListAsync();

            if (libro.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("Error en los autores enviados");
            }

            var librodto = mapper.Map<Libro>(libro);

            if(librodto.AutoresLibros != null)
            {
                for(int i=0; i < librodto.AutoresLibros.Count; i++)
                {
                    librodto.AutoresLibros[i].orden = i;
                }
            }


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

            context.Remove(new Libro() { id = id });
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
