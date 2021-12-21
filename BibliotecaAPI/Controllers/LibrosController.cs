using AutoMapper;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/libros")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string contenedor = "peliculas";

        public LibrosController(ApplicationDbContext context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos,
             UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<LibroDTO>>> Get()
        {
            var top = 6;
            var hoy = DateTime.Today;

            var libros = await context.Libros                
                .OrderBy(x => x.FechaPublicacion)
                .Take(top)
                .ToListAsync();            

            return mapper.Map<List<LibroDTO>>(libros); ;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            var libro = await context.Libros
                .Include(x => x.LibrosCategorias).ThenInclude(x => x.Categoria)
                .Include(x => x.LibrosAutores).ThenInclude(x => x.Autor)                
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null) { return NotFound(); }

            return mapper.Map<LibroDTO>(libro);
        }

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<LibroDTO>>> Filtrar([FromQuery] LibrosFiltrarDTO librosFiltrarDTO)
        {
            var librosQueryable = context.Libros.AsQueryable();

            if (!string.IsNullOrEmpty(librosFiltrarDTO.Titulo))
            {
                librosQueryable = librosQueryable.Where(x => x.Titulo.Contains(librosFiltrarDTO.Titulo));
            }            

            if (librosFiltrarDTO.CategoriaId != 0)
            {
                librosQueryable = librosQueryable
                    .Where(x => x.LibrosCategorias.Select(y => y.CategoriaId)
                    .Contains(librosFiltrarDTO.CategoriaId));
            }

            if (librosFiltrarDTO.AutorId != 0)
            {
                librosQueryable = librosQueryable
                    .Where(x => x.LibrosAutores.Select(y => y.AutorId)
                    .Contains(librosFiltrarDTO.AutorId));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(librosQueryable);

            var peliculas = await librosQueryable.Paginar(librosFiltrarDTO.PaginacionDTO).ToListAsync();
            return mapper.Map<List<LibroDTO>>(peliculas);
        }


        [HttpGet("PostGet")]
        public async Task<ActionResult<LibrosPostGetDTO>> PostGet()
        {
            var categorias = await context.Categorias.ToListAsync();
            var autores = await context.Autores.ToListAsync();

            var categoriasDTO = mapper.Map<List<CategoriaDTO>>(categorias);
            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);

            return new LibrosPostGetDTO() { Categorias = categoriasDTO, Autores = autoresDTO };
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] LibroCreacionDTO libroCreacionDTO)
        {
            var libro = mapper.Map<Libro>(libroCreacionDTO);

            if (libroCreacionDTO.Imagen != null)
            {
                libro.Imagen = await almacenadorArchivos.GuardarArchivo(contenedor, libroCreacionDTO.Imagen);
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return libro.Id;
        }

        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<LibrosPutGetDTO>> PutGet(int id)
        {
            var librosActionResult = await Get(id);
            if (librosActionResult.Result is NotFoundResult) { return NotFound(); }

            var libro = librosActionResult.Value;

            var respuesta = new LibrosPutGetDTO();
            respuesta.Libro = libro;
            respuesta.Categorias = libro.Categorias;
            respuesta.Autores = libro.Autores;            
            return respuesta;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] LibroCreacionDTO libroCreacionDTO)
        {
            var libro = await context.Libros
                .Include(x => x.LibrosAutores)
                .Include(x => x.LibrosCategorias)                
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            libro = mapper.Map(libroCreacionDTO, libro);

            if (libroCreacionDTO.Imagen != null)
            {
                libro.Imagen = await almacenadorArchivos.EditarArchivo(contenedor, libroCreacionDTO.Imagen, libro.Imagen);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var libro = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            context.Remove(libro);
            await context.SaveChangesAsync();
            await almacenadorArchivos.BorrarArchivo(libro.Imagen, contenedor);
            return NoContent();
        }
    }
}
