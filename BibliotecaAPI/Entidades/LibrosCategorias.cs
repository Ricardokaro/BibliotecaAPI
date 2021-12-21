using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaAPI.Entidades
{
    public class LibrosCategorias
    {
        public int LibroId { get; set; }
        public int CategoriaId { get; set; }
        public Libro Libro { get; set; }
        public Categoria Categoria { get; set; }
    }
}
