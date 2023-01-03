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


        [HttpPost]
        public async Task<ActionResult> Post(int libroId, ComentarioDTO comentarioDto)
        {
            var libro = await context.Libros.AnyAsync(libroBD => libroBD.id == libroId);

            if(!libro) { return NotFound(); }

            var comentario = mapper.Map<Comentario>(comentarioDto);

            comentario.idLibro = libroId;
            context.Add(comentario);
            await context.SaveChangesAsync();
            return Ok();

        }

    }
}
