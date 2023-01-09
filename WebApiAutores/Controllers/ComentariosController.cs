using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(ApplicationDbContext context, IMapper mapper,
            UserManager<IdentityUser> userManager ) {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }


        [HttpGet(Name = "ObtenerComentariosDeUnLibro")]
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


        [HttpPost(Name = "CreaComentarioDeUnLibro")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioDTO comentarioDto)
        {
            
            var emailClaim = HttpContext.User.Claims.Where(claim=> claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var libro = await context.Libros.AnyAsync(libroBD => libroBD.id == libroId);
            if (!libro) { return NotFound(); }

            var comentario = mapper.Map<Comentario>(comentarioDto);

            comentario.idLibro = libroId;
            comentario.UsuarioId = usuarioId;
            context.Add(comentario);
            await context.SaveChangesAsync();

            var comentarioDtoRes = mapper.Map<ComentarioDTOres>(comentario);

            return CreatedAtRoute("ObtenerComentario", new { id = comentarioDtoRes.id, libroId = libroId }, comentarioDtoRes);
        }


        [HttpPut("{id:int}", Name = "EditaComentario")]
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
