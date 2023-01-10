using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Utilidades;

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


        [HttpGet(Name = "ObtenerAutores")]
        [AllowAnonymous]    //<-- permite consultar sin estar autenticado
        public async Task<ActionResult<List<AutorDTOres>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Autores.AsQueryable();
            await HttpContext.paginationHeader(queryable);

            var autores = await queryable.OrderBy(autor => autor.nombre).Paginar(paginacionDTO).ToListAsync();
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

            var dto = mapper.Map<AutorDTOConLibros>(autor);
            GenerarEnlaces(dto);
            return dto;
        }


        private void GenerarEnlaces(AutorDTOres autorDto)
        {
            autorDto.recursos.Add(new DatoHATEOAS(
                enlace: Url.Link("ObtenerAutor", 
                new {id= autorDto.id }),
                    descripcion: "obtener un autor",
                    metodo:"GET"
                )
                );

            autorDto.recursos.Add(new DatoHATEOAS(
               enlace: Url.Link("EditarAutor",
               new { id = autorDto.id }),
                   descripcion: "Editar un autor",
                   metodo: "PUT"
               )
               );

            autorDto.recursos.Add(new DatoHATEOAS(
               enlace: Url.Link("EliminarAutor",
               new { id = autorDto.id }),
                   descripcion: "Eliminar un autor",
                   metodo: "DELETE"
               )
               );
        }

        [HttpGet("{nombre}", Name = "ObtenerAutorPorNombre")]
        public async Task<ActionResult<List<AutorDTOres>>> Get(string nombre)
        {
            var autores = await context.Autores.Where(x => x.nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTOres>>(autores);
        }


        [HttpPost(Name = "CrearAutor")]
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



        [HttpPut("{id:int}", Name = "EditarAutor")]
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


        [HttpDelete("{id:int}", Name = "EliminarAutor")]
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
