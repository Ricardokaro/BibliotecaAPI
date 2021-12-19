using AutoMapper;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
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
        }
    }
}
