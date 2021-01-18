using APICatalog_NetCore.Context;
using APICatalog_NetCore.Models;
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
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext contexto)
        {
            _context = contexto;
        }

        [HttpGet]
        public  ActionResult<IEnumerable<Produto>> GetTodos()
        {
            return  _context.produtos.AsNoTracking().ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Produto>>> Get(int id, [BindRequired] string nome)
        {
            return await _context.produtos.AsNoTracking().ToListAsync();
        }

        //Pode conter diversas rotas
        [HttpGet("primeiro")]
        //[HttpGet("/primeiro")]
        //[HttpGet("{valor:alpha:length(5)}")] //Restrição: Apenas alfanúmerico e com tamanho de 5
        public ActionResult<Produto> GetPrimeiro()
        {
            return _context.produtos.FirstOrDefault();
        }

        //No segundo parâmetro para ser obrigatório basta retirar o símbolo de interrogação, neste caso com o interrogação é opcional
        //[HttpGet("{id}/{param2?}", Name = "ObterProduto")]
        //[HttpGet("{id:int:min(1)}", Name = "ObterProduto")]//Restrição de rota, o id não poser menor que 1
        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> Get([FromQuery]int id)
        {
            //var meuParamentro = param2;

            var produto = await _context.produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutId == id);

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

            _context.produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutId }, produto);
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutId)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _context.produtos.AsNoTracking().FirstOrDefault(p => p.ProdutId == id);

            //var produto = _context.Produtos.Find(id); /*A vantagem do find é que vai procurar primeiro na memória, se achar não vai procurar no banco, porém 
                                                        /* o find só posso utilizar se o id for a chave primária da tabela*/

            if (id != produto.ProdutId)
                return BadRequest();

            _context.produtos.Remove(produto);
            _context.SaveChanges();
            return Ok();
        }
    }
}
