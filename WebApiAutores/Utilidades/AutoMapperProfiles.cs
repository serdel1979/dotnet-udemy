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
            CreateMap<Autor, AutorDTOres>();
            CreateMap<LibroDTO, Libro>();
            CreateMap<Libro, LibroDTOres>();
        }




    }
}
