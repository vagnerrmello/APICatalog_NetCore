using APICatalog_NetCore.Context;
using APICatalog_NetCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace APICatalog_NetCore.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
