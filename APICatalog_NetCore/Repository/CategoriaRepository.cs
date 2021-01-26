using APICatalog_NetCore.Context;
using APICatalog_NetCore.Models;
using APICatalog_NetCore.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalog_NetCore.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public async Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParameters)
        {
            return await PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.CategoriaId),
                categoriasParameters.PageNumber, categoriasParameters.PageSize);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        { 
            return await Get().Include(x => x.Produtos).ToListAsync();
        }
    }
}
