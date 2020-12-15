using APICatalog_NetCore.Context;
using APICatalog_NetCore.Models;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _context.Produtos.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

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

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutId }, produto);
        }


        [HttpGet("{id}")]
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
            var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id);

            //var produto = _context.Produtos.Find(id); /*A vantagem do find é que vai procurar primeiro na memória, se achar não vai procurar no banco, porém 
                                                        /* o find só posso utilizar se o id for a chave primária da tabela*/

            if (id != produto.ProdutId)
                return BadRequest();

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return Ok();
        }
    }
}
