using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class CuentasController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signin;

        public CuentasController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signin) {

            this.userManager = userManager;
            this.configuration = configuration;
            this.signin = signin;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialUsuario credencialUsuario)
        {

            var usuario = new IdentityUser { 
                UserName = credencialUsuario.Email, 
                Email = credencialUsuario.Email 
            };

            var resultado = await userManager.CreateAsync(usuario, credencialUsuario.Password);
            if (resultado.Succeeded)
            {
                return construirToken(credencialUsuario);
            }
            return BadRequest(resultado.Errors);
        }


        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> login(CredencialUsuario credencialUsuario)
        {
            var resultado = await signin.PasswordSignInAsync(credencialUsuario.Email, credencialUsuario.Password,
                isPersistent: false, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return construirToken(credencialUsuario);
            }
            return BadRequest("Login incorrecto");
        }


        private RespuestaAutenticacion construirToken(CredencialUsuario credencialUsuario)
        {
            var claims = new List<Claim>(){
                new Claim("email", credencialUsuario.Email)
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Secret"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };

        }

    }
}
