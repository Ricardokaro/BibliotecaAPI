using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaAPI.Entidades
{
    public class LibrosAutores
    {
        public int LibroId { get; set; }
        public int AutorId { get; set; }
        public Libro Pelicula { get; set; }
        public Autor Autor { get; set; }        
    }
}
