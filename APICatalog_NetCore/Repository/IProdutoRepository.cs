using APICatalog_NetCore.Models;
using APICatalog_NetCore.Pagination;
using System.Collections.Generic;

namespace APICatalog_NetCore.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
