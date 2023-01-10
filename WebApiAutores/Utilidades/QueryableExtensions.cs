using WebApiAutores.DTOs;

namespace WebApiAutores.Utilidades
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.PorPagina)
                .Take(paginacionDTO.PorPagina);
        }
    }
}
