using APICatalog_NetCore.Models;
using APICatalog_NetCore.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalog_NetCore.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
