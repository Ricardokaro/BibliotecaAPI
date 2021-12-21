using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaAPI.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 300)]
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }        
        public DateTime FechaPublicacion { get; set; }
        public string Editor { get; set; }

        [StringLength(maximumLength: 1024)]
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public List<LibrosAutores> LibrosAutores { get; set; }
        public List<LibrosCategorias> LibrosCategorias { get; set; }
    }
}
