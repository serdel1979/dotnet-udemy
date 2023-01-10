using Microsoft.EntityFrameworkCore;

namespace WebApiAutores.Entidades
{
    public static class HttpContextExtensions
    {
        public async static Task paginationHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            double cantidad = await queryable.CountAsync();
            httpContext.Response.Headers.Add("porpagina", cantidad.ToString());
        }
    }
}
