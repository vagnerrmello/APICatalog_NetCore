﻿using APICatalog_NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalog_NetCore.DTOs
{
    public class CategoriaDTO
    {
        public int CategoriaId { get; set; }

        public string Nome { get; set; }

        public string ImagemUrl { get; set; }

        public ICollection<ProdutoDTO> Produtos { get; set; }
    }
}
