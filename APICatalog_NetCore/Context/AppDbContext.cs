using APICatalog_NetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalog_NetCore.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        { }
        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Produto> produtos { get; set; }
    }
}
