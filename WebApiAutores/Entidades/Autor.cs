using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Entidades
{
    public class Autor
    {
        public int id { get; set; }

        [Required(ErrorMessage ="El campo es requerido")]
        [StringLength(maximumLength:120, ErrorMessage ="La longitud máxima es 120!!!")]
        public string nombre { get; set; }

    }
}
