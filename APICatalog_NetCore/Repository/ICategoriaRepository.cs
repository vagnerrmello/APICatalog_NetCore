using APICatalog_NetCore.Models;
using APICatalog_NetCore.Pagination;
using System.Collections.Generic;

namespace APICatalog_NetCore.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
