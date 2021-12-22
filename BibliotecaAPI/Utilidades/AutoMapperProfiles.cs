using AutoMapper;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaAPI.Utilidades
{
    public class AutoMapperProfiles:Profile 
    {
        public AutoMapperProfiles()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<CategoriaCreacionDTO, Categoria>();

            CreateMap<Autor, AutorDTO>().ReverseMap();
            CreateMap<AutorCreacionDTO, Autor>()
                .ForMember(x => x.Foto, options => options.Ignore());

            CreateMap<Libro, LibroDTO>()
               .ForMember(x => x.Categorias, options => options.MapFrom(MapearCategoria))
               .ForMember(x => x.Autores, options => options.MapFrom(MapearAutores));

            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(x => x.LibrosAutores, options => options.MapFrom(MapearLibrosAutores))
                .ForMember(x => x.LibrosCategorias, options => options.MapFrom(MapearLibrosCategorias));

            CreateMap<IdentityUser, UsuarioDTO>();
        }

        private List<CategoriaDTO> MapearCategoria(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<CategoriaDTO>();
            if (libroDTO.Categorias != null)
            {
                foreach (var lib in libro.LibrosCategorias)
                {
                    resultado.Add(new CategoriaDTO()
                    {
                        Id = lib.CategoriaId,
                        Nombre = lib.Categoria.Nombre
                    });
                }
            }            
            return resultado;
        }

        private List<AutorDTO> MapearAutores(Libro libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();
            if (libroDTO.Autores != null)
            {
                foreach (var a in libro.LibrosAutores)
                {
                    resultado.Add(new AutorDTO()
                    {
                        Id = a.AutorId,
                        Nombre = a.Autor.Nombre,
                        Biografia = a.Autor.Biografia,
                        FechaNacimiento = a.Autor.FechaNacimiento,
                        Foto = a.Autor.Foto
                    });
                }
            }            
            return resultado;
        }

        private List<LibrosAutores> MapearLibrosAutores(LibroCreacionDTO libroCreacionDTO,
            Libro libro)
        {
            var resultado = new List<LibrosAutores>();

            if (libroCreacionDTO.Autores == null) { return resultado; }

            foreach (var autor in libroCreacionDTO.Autores)
            {
                resultado.Add(new LibrosAutores() { AutorId = autor.Id });
            }

            return resultado;
        }

        private List<LibrosCategorias> MapearLibrosCategorias(LibroCreacionDTO libroCreacionDTO,
            Libro libro)
        {
            var resultado = new List<LibrosCategorias>();

            if (libroCreacionDTO.CategoriasIds == null) { return resultado; }

            foreach (var id in libroCreacionDTO.CategoriasIds)
            {
                resultado.Add(new LibrosCategorias() { CategoriaId = id });
            }

            return resultado;
        }
    }
}
