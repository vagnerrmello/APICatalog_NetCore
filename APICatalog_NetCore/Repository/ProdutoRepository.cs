using APICatalog_NetCore.Context;
using APICatalog_NetCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace APICatalog_NetCore.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }
    }
}
