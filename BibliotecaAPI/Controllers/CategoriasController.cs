using AutoMapper;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaAPI.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class CategoriasController : ControllerBase
    {
        private readonly ILogger<CategoriasController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CategoriasController(ILogger<CategoriasController> logger,
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet] // api/categorias
        public async Task<ActionResult<List<CategoriaDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Categorias.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var categorias = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<CategoriaDTO>>(categorias);
        }

        [HttpGet("todos")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CategoriaDTO>>> Todos()
        {
            var generos = await context.Categorias.OrderBy(x => x.Nombre).ToListAsync();
            return mapper.Map<List<CategoriaDTO>>(generos);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Get(int Id)
        {
            var genero = await context.Categorias.FirstOrDefaultAsync(x => x.Id == Id);

            if (genero == null)
            {
                return NotFound();
            }

            return mapper.Map<CategoriaDTO>(genero);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CategoriaCreacionDTO generoCreacionDTO)
        {
            var categoria = mapper.Map<Categoria>(generoCreacionDTO);
            context.Add(categoria);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);

            if (categoria == null)
            {
                return NotFound();
            }

            categoria = mapper.Map(categoriaCreacionDTO, categoria);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Categorias.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Categoria() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
