using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaAPI.DTOs
{
    public class LibrosPutGetDTO
    {
        public LibroDTO Libro { get; set; }
        public List<CategoriaDTO> CategoriasSeleccionados { get; set; }
        public List<CategoriaDTO> CategoriasNoSeleccionados { get; set; }
        public List<AutorDTO> Autores { get; set; }        
    }
}
