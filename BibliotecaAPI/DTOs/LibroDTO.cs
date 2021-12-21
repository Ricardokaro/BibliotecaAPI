using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaAPI.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }        
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }              
        public DateTime FechaPublicacion { get; set; }
        public string Editor { get; set; }        
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public List<AutorDTO> Autores { get; set; }
        public List<CategoriaDTO> Categorias { get; set; }
    }
}
