using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class LibroDTO
    {
        [StringLength(maximumLength: 100)]
        public string titulo { get; set; }
    }
}
