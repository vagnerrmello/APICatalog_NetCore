﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalog_NetCore.Services
{
    public class MeuServico : IMeuServico
    {
        public string Saudacao(string nome)
        {
            return $"Bem-Vindo, {nome} \n\n{DateTime.Now}";
        }
    }
}
