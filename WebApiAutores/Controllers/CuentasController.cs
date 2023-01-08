using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
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
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signin;
        private readonly IDataProtector dataProtector;

        public CuentasController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signin,
            IDataProtectionProvider dataProtectionProvider) {

            this.userManager = userManager;
            this.configuration = configuration;
            this.signin = signin;

            dataProtector = dataProtectionProvider.CreateProtector("valor_re_secreto");
        }


        [HttpGet("encriptar")]
        public ActionResult encriptar()
        {
            var txt = "frase en texto plano";
            var cifrado = dataProtector.Protect(txt);
            var descifrado = dataProtector.Unprotect(cifrado);
            return Ok(new
            {
                textoPlano = txt,
                textoCifrado = cifrado,
                textoDescifrado = descifrado
            });
        }


        [HttpGet("encriptarPortiempo")]
        public ActionResult encriptarPorTiempo()
        {
            var porTiempo = dataProtector.ToTimeLimitedDataProtector();
            var txt = "frase en texto plano";
            var cifrado = porTiempo.Protect(txt, lifetime: TimeSpan.FromSeconds(5));
            Thread.Sleep(6000);
            var descifrado = porTiempo.Unprotect(cifrado);
            return Ok(new
            {
                textoPlano = txt,
                textoCifrado = cifrado,
                textoDescifrado = descifrado
            });
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
                return await construirToken(credencialUsuario);
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
                return await construirToken(credencialUsuario);
            }
            return BadRequest("Login incorrecto");
        }


        [HttpGet("renovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> renovarToken()
        {

            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var credencialesUsuario = new CredencialUsuario
            {
                Email = email
            };

            return await construirToken(credencialesUsuario);
        }

        private async Task<RespuestaAutenticacion> construirToken(CredencialUsuario credencialUsuario)
        {
            var claims = new List<Claim>(){
                new Claim("email", credencialUsuario.Email)
            };

            var usuario = await userManager.FindByEmailAsync(credencialUsuario.Email);

            var claimsDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimsDB);

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

        [HttpPost("hacerAdmin")]
        public async Task<ActionResult<RespuestaAutenticacion>> hacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);

            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        [HttpPost("borrarAdmin")]
        public async Task<ActionResult<RespuestaAutenticacion>> borrarAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);

            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

    }
}
