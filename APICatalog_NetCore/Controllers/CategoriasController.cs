using APICatalog_NetCore.Context;
using APICatalog_NetCore.Models;
using APICatalog_NetCore.Repository;
using APICatalog_NetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly IUnitOfWork _uof;
        //private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public CategoriasController(IUnitOfWork uof, IConfiguration config, 
            ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _configuration = config;
            _logger = logger;
        }

        [HttpGet("autor")]
        public string GetAutor()
        {
            var autor = _configuration["autor"];
            return $"Autor: {autor}";
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico,
            string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _uof.CategoriaRepository.Get().ToList();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao tentar obter categorias.");
            }

        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
        {
            _logger.LogInformation("====== Você está acessando Categorias/Produtos ======");
            return _uof.CategoriaRepository.Get().Include(p=> p.Produtos).ToList();
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
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

                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();

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

                _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();
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
                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                //var produto = _context.Produtos.Find(id); /*A vantagem do find é que vai procurar primeiro na memória, se achar não vai procurar no banco, porém 
                /* o find só posso utilizar se o id for a chave primária da tabela*/

                if (id != categoria.CategoriaId)
                    return BadRequest($"Não foi possível encontrar o registro com o id = {id}");

                _uof.CategoriaRepository.Delete(categoria);
                _uof.Commit();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Algo deu errado com atualização.");
            }
        }

    }
}
