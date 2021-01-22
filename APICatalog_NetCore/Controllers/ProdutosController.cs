using APICatalog_NetCore.Context;
using APICatalog_NetCore.Filters;
using APICatalog_NetCore.Models;
using APICatalog_NetCore.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalog_NetCore.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        //private readonly AppDbContext _context;
        //public ProdutosController(AppDbContext contexto)
        //{
        //    _context = contexto;
        //}

        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork contexto)
        {
            _uof = contexto;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return  _uof.ProdutoRepository.GetProdutosPorPreco().ToList(); 
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public  ActionResult<IEnumerable<Produto>> GetTodos()
        {
            return _uof.ProdutoRepository.Get().ToList();
            //return  _uof.produtos.AsNoTracking().ToList();
        }

        //[HttpGet("{id}")]
        //public ActionResult<IEnumerable<Produto>> Get(int id, [BindRequired] string nome)
        //{
        //    return  _uof.ProdutoRepository.Get().ToList(); 
        //}

        //Pode conter diversas rotas
        [HttpGet("primeiro")]
        //[HttpGet("/primeiro")]
        //[HttpGet("{valor:alpha:length(5)}")] //Restrição: Apenas alfanúmerico e com tamanho de 5
        public ActionResult<Produto> GetPrimeiro()
        {
            return _uof.ProdutoRepository.Get().FirstOrDefault();
        }

        //No segundo parâmetro para ser obrigatório basta retirar o símbolo de interrogação, neste caso com o interrogação é opcional
        //[HttpGet("{id}/{param2?}", Name = "ObterProduto")]
        //[HttpGet("{id:int:min(1)}", Name = "ObterProduto")]//Restrição de rota, o id não poser menor que 1
        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            //var meuParamentro = param2;

            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutId == id);

            if (produto == null)
                return NotFound();

            return produto;
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutId }, produto);
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutId)
                return BadRequest();

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutId == id);

            //var produto = _context.Produtos.Find(id); /*A vantagem do find é que vai procurar primeiro na memória, se achar não vai procurar no banco, porém 
                                                        /* o find só posso utilizar se o id for a chave primária da tabela*/

            if (id != produto.ProdutId)
                return BadRequest();

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return Ok();
        }
    }
}
