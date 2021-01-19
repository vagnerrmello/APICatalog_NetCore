using APICatalog_NetCore.Context;
using APICatalog_NetCore.Models;
using Microsoft.AspNetCore.Http;
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
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriasController(AppDbContext contexto)
        {
            _context = contexto;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _context.Categorias.AsNoTracking().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar obter categorias.");
            }

        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
        {
            return _context.Categorias.Include(p=> p.Produtos).ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> Get([FromQuery]int id)
        {
            try
            {
                var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.CategoriaId == id);
                //var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);

                if (categoria == null)
                    return NotFound($"Categoria com id = {id} não foi encontrada.");

                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar obter categorias.");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                _context.Categorias.Add(categoria);
                _context.SaveChanges();

                return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Algo deu errado com a inserção.");
            }
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {

                if (id != categoria.CategoriaId)
                    return BadRequest($"Não foi possível encontrar o registro com o id = {id}");

                _context.Entry(categoria).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Algo deu errado com atualização.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);

                //var produto = _context.Produtos.Find(id); /*A vantagem do find é que vai procurar primeiro na memória, se achar não vai procurar no banco, porém 
                /* o find só posso utilizar se o id for a chave primária da tabela*/

                if (id != categoria.CategoriaId)
                    return BadRequest($"Não foi possível encontrar o registro com o id = {id}");

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Algo deu errado com atualização.");
            }
        }

    }
}
