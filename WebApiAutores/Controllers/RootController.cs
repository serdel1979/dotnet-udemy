using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class RootController: ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService) {
            this.authorizationService = authorizationService;
        }


        [HttpGet(Name = "ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> Get()
        {   
            var datoHateoas = new List<DatoHATEOAS>();
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            datoHateoas.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerRoot", new { }), descripcion: "self",metodo: "GET"));

            datoHateoas.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutores", new { }), descripcion: "autores", metodo: "GET"));


            if (esAdmin.Succeeded)
            {


                datoHateoas.Add(new DatoHATEOAS(enlace: Url.Link("CrearrAutor", new { }), descripcion: "crea-autor", metodo: "POST"));

                datoHateoas.Add(new DatoHATEOAS(enlace: Url.Link("CrearLibro", new { }), descripcion: "crea-libro", metodo: "POST"));


            }


            return datoHateoas;
        }

    }
}
