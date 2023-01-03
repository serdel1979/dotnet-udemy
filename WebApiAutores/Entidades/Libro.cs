using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string titulo { get; set; }
        public List<Comentario> comentarios { get; set; }

        public List<AutorLibro> AutoresLibros { get; set; }
    }
}
