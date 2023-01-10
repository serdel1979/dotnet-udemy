namespace WebApiAutores.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int porPagina = 3;
        private readonly int maxPorPagina = 5;

        public int PorPagina {
            get
            {
                return porPagina;
            }

            set
            {
                porPagina = (value > maxPorPagina) ? maxPorPagina : value;
            }
        }
    }
}
