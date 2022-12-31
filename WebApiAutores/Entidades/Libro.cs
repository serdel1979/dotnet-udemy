using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int id { get; set; }

        [StringLength(maximumLength: 100)]
        public string titulo { get; set; }

    }
}
