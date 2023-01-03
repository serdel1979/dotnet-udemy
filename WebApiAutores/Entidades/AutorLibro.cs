namespace WebApiAutores.Entidades
{
    public class AutorLibro
    {
        public int libroId { get; set; }
        public int autorId { get; set; }
        public int orden { get; set; }
        public Libro Libro { get; set; }
        public Autor Autor { get; set; }

    }
}
