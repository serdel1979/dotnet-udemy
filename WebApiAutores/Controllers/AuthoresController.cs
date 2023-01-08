using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AuthoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AuthoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration) { 
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }


        [HttpGet]
        [AllowAnonymous]    //<-- permite consultar sin estar autenticado
        public async Task<ActionResult<List<AutorDTOres>>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTOres>>(autores);
        }

        //var libro = await context.Libros
        //       .Include(librodb => librodb.AutoresLibros)
        //       .ThenInclude(autorLibroDb => autorLibroDb.Autor)
        //       .FirstOrDefaultAsync(x => x.id == id);


        [HttpGet("{id:int}", Name = "ObtenerAutor")]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            var autor = await context.Autores
                .Include(autordb => autordb.AutoresLibros)
                .ThenInclude(autorLibroDb => autorLibroDb.Libro)
                .FirstOrDefaultAsync(x=>x.id == id);

            if(autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOConLibros>(autor);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTOres>>> Get(string nombre)
        {
            var autores = await context.Autores.Where(x => x.nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTOres>>(autores);
        }


        [HttpPost]
        public async Task<ActionResult> Post(AutorDTO autorDto)
        {

            var existe = await context.Autores.AnyAsync(x => x.nombre == autorDto.nombre);
            if (existe)
            {
                return BadRequest($"El autor {autorDto.nombre} ya existe!!!");
            }

            var autor = mapper.Map<Autor>(autorDto);

            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDtoRes = mapper.Map<AutorDTOres>(autor);

            return CreatedAtRoute("ObtenerAutor", new { id = autor.id }, autorDtoRes);
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(AutorDTO autor, int id)
        {

            
            var existe = await context.Autores.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }

            var autorBD = mapper.Map<Autor>(autor);
            autorBD.id = id;

            context.Update(autorBD);
            await context.SaveChangesAsync();
            return NoContent();
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
