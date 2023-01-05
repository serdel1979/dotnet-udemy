using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class LibroPatchDTO
    {
        [StringLength(maximumLength: 100)]
        [Required]
        public string titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
