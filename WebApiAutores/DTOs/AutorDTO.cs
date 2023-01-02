using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class AutorDTO
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(maximumLength: 120, ErrorMessage = "La longitud máxima es 120!!!")]
        public string nombre { get; set; }
    }
}
