namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public int autor_id { get; set; }
        public Autor Autor { get; set; }
    }
}
