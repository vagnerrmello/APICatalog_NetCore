using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalog_NetCore.Models
{
     [Table("Categorias")]
    public class Categoria
    {
        public Categoria()
        {
            Produtos = new Collection<Produto>();
        }

        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório!")]
        [MaxLength(80, ErrorMessage = "Máximo de 80 caractere")]
        [MinLength(5, ErrorMessage = "Deve ter o mínimo de 5 Caractere")]
        public string Nome { get; set; }

        [Required(ErrorMessage ="A imagem é uma propriedade obrigatória")]
        [MaxLength(300, ErrorMessage = "Máximo de 300 caractere")]
        public string ImagemUrl { get; set; }

        public ICollection<Produto> Produtos { get; set; }
    }
}
