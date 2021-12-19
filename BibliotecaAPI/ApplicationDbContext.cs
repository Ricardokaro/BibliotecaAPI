using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliotecaAPI
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            /*modelBuilder.Entity<PeliculasActores>()
                .HasKey(x => new { x.ActorId, x.PeliculaId });  */         

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Categoria> Categorias { get; set; }        
    }
}
