using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id:int}", Name = "ObtenerLibro")]
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

        [HttpPost(Name = "CrearLibro")]
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

            AsignarOrdenAutores(librodto);

            context.Add(librodto);
            await context.SaveChangesAsync();

            var libroDtoRes = mapper.Map<LibroDTOres>(librodto);

            return CreatedAtRoute("ObtenerLibro", new { id = librodto.id }, libroDtoRes);
        }



        [HttpPut("{id:int}", Name = "EditaUnLibro")]
        public async Task<ActionResult> Put(LibroDTO libroDTO, int id)
        {

            var libroDB = await context.Libros
                .Include(x=> x.AutoresLibros)
                .FirstOrDefaultAsync(x=> x.id == id);

            if(libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroDTO, libroDB);

            AsignarOrdenAutores(libroDB);

            await context.SaveChangesAsync();
            return NoContent();

        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].orden = i;
                }
            }
        }



        [HttpPatch("{id:int}", Name = "PatchLibro")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await context.Libros
                .FirstOrDefaultAsync(x=>x.id == id);

            if(libroDB == null)
            {
                return NotFound();  
            }

            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);

            patchDocument.ApplyTo(libroDTO, ModelState);

            mapper.Map(libroDTO, libroDB);

            var esValido = TryValidateModel(libroDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            

            await context.SaveChangesAsync();

            return NoContent();

        }





        [HttpDelete("{id:int}", Name = "BorraLibro")]
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
