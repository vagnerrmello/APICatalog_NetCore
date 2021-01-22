using APICatalog_NetCore.Models;
using System.Collections.Generic;

namespace APICatalog_NetCore.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
