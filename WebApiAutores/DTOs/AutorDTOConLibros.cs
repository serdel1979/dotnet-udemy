namespace WebApiAutores.DTOs
{
    public class AutorDTOConLibros: AutorDTOres
    {
        public List<LibroDTOres> libros { get; set; }
    }
}
