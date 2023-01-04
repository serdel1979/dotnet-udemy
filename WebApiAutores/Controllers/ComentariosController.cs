using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{

    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDbContext context, IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTOres>>> Get(int idLibro)
        {
            var comentarios = await context.Comentarios.Where(x => x.idLibro == idLibro).ToListAsync();
            return mapper.Map<List<ComentarioDTOres>>(comentarios);

        }

        [HttpGet("{id:int}", Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTOres>> GetComentarioById(int id)
        {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(x => x.id == id);

            if(comentario == null)
            {
                return NotFound();
            }

            return mapper.Map<ComentarioDTOres>(comentario);
        }


        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioDTO comentarioDto)
        {
            var libro = await context.Libros.AnyAsync(libroBD => libroBD.id == libroId);

            if(!libro) { return NotFound(); }

            var comentario = mapper.Map<Comentario>(comentarioDto);

            comentario.idLibro = libroId;
            context.Add(comentario);
            await context.SaveChangesAsync();

            var comentarioDtoRes = mapper.Map<ComentarioDTOres>(comentario);

            return CreatedAtRoute("ObtenerComentario", new { id = comentarioDtoRes.id, libroId = libroId }, comentarioDtoRes);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int libroId, int id, ComentarioDTO comentarioDto)
        {
            var libro = await context.Libros.AnyAsync(libroBD => libroBD.id == libroId);

            if (!libro) { 
                return NotFound(); 
            }

            var existeComentario = await context.Comentarios.AnyAsync(comentario => comentario.id == id);
            if(!existeComentario)
            {
                return NotFound();
            }


            var comentarioBD = mapper.Map<Comentario>(comentarioDto);
            comentarioBD.id = id;
            comentarioBD.idLibro = libroId;
            context.Update(comentarioBD);
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
