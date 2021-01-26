using APICatalog_NetCore.Models;
using APICatalog_NetCore.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICatalog_NetCore.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutosPorPreco();
    }
}
