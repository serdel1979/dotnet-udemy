using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class LibroDTOres
    {
        public int id { get; set; }

        [StringLength(maximumLength: 100)]
        public string titulo { get; set; }
    }
}
