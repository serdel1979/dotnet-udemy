using Microsoft.AspNetCore.Identity;

namespace WebApiAutores.Entidades
{
    public class Comentario
    {
        public int id { get; set; }
        public string contenido { get; set; }
        public int idLibro { get; set; }
        public Libro libro { get; set; }
        public string UsuarioId { get; set;}
        public IdentityUser Usuario { get; set; }

    }
}
