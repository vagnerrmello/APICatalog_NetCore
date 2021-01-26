using APICatalog_NetCore.Context;
using APICatalog_NetCore.DTOs;
using APICatalog_NetCore.Filters;
using APICatalog_NetCore.Models;
using APICatalog_NetCore.Pagination;
using APICatalog_NetCore.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public  ActionResult<IEnumerable<ProdutoDTO>> GetTodos([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
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
        public async Task<ActionResult<ProdutoDTO>> GetPrimeiro()
        {
            var produto = await _uof.ProdutoRepository.Get().FirstOrDefaultAsync();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return produtoDTO;
        }

        //No segundo parâmetro para ser obrigatório basta retirar o símbolo de interrogação, neste caso com o interrogação é opcional
        //[HttpGet("{id}/{param2?}", Name = "ObterProduto")]
        //[HttpGet("{id:int:min(1)}", Name = "ObterProduto")]//Restrição de rota, o id não poser menor que 1
        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutId == id);

            if (produto == null)
                return NotFound();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return produtoDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.Add(produto);
            await _uof.Commit();

            var _produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutId }, _produtoDTO);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDTO)
        {
            

            if (id != produtoDTO.ProdutId)
                return BadRequest();

            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutId == id);

            //var produto = _context.Produtos.Find(id); /*A vantagem do find é que vai procurar primeiro na memória, se achar não vai procurar no banco, porém 
                                                        /* o find só posso utilizar se o id for a chave primária da tabela*/

            if (id != produto.ProdutId)
                return BadRequest();

            _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return produtoDTO;
        }
    }
}
