using APICatalog_NetCore.Models;
using System.Collections.Generic;

namespace APICatalog_NetCore.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
