using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaAPI.DTOs
{
    public class LibrosPostGetDTO
    {
        public List<CategoriaDTO> Categorias { get; set; }
        public List<AutorDTO> Autores { get; set; }
    }
}
