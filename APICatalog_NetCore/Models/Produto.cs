using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalog_NetCore.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutId { get; set; }

        [Required(ErrorMessage ="O nome é obrigatório!")]
        [MaxLength(80,ErrorMessage ="Máximo de 80 caractere")]
        [MinLength(5, ErrorMessage ="Deve ter o mínimo de 5 Caractere")]
        public string Nome { get; set; }

        [Required]
        [MaxLength(300)]
        public string Descricao { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImagemUrl { get; set; }

        public float Estoque { get; set; }

        public DateTime DataCadastro { get; set; }

        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }
    }
}
