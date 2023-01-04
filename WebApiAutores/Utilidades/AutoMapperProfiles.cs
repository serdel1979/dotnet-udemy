using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles: Profile
    {


        public AutoMapperProfiles()
        {
            CreateMap<AutorDTO, Autor>();
            CreateMap<Autor, AutorDTOres>()
                .ForMember(autoresDTOres => autoresDTOres.libros, opciones => opciones.MapFrom(MapLibrosDeAutores));
            CreateMap<LibroDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTOres>()
                .ForMember(libroDTOres => libroDTOres.autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<ComentarioDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTOres>();
        }




        private List<LibroDTOres> MapLibrosDeAutores(Autor autor, AutorDTOres autorDTO)
        {
            var resultado = new List<LibroDTOres>();

            if (autor.AutoresLibros == null) { return resultado; }

            foreach (var libroDeAutor in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTOres()
                {
                    id = libroDeAutor.autorId,
                    titulo = libroDeAutor.Libro.titulo
                });
            }

            return resultado;
        }


        private List<AutorDTOres> MapLibroDTOAutores(Libro libro, LibroDTOres libroDTO)
        {
            var resultado = new List<AutorDTOres>();

            if (libro.AutoresLibros == null) { return resultado; }

            foreach (var autorlibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTOres() 
                { 
                    id = autorlibro.autorId,
                    nombre = autorlibro.Autor.nombre
                
                });
            }

            return resultado;
        }


        private List<AutorLibro> MapAutoresLibros(LibroDTO libroDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if (libroDTO.AutoresIds == null) { return resultado; }

            foreach (var autorId in libroDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro() { autorId = autorId });
            }

            return resultado;
        }




    }
}
