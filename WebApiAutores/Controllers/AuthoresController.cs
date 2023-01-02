using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AuthoresController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AuthoresController(ApplicationDbContext context, IMapper mapper) { 
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<AutorDTOres>>> Get()
        {
            var autores = await context.Autores.ToListAsync();
            return mapper.Map<List<AutorDTOres>>(autores);
        }


       

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorDTOres>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x=>x.id == id);
            if(autor == null)
            {
                return NotFound();
            }

            return mapper.Map<AutorDTOres>(autor);
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
