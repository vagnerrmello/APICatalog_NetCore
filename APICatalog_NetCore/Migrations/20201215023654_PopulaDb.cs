using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalog_NetCore.Migrations
{
    public partial class PopulaDb : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert Into Categorias(Nome, ImagemUrl) Values('Programação', 'https://raw.githubusercontent.com/json-api-dotnet/JsonApiDotnetCore/master/logo.png')");

            mb.Sql("Insert Into Produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, Categoria) " +
                "Values('CSharo', 'Linguagem de programação', 299.99, 'https://raw.githubusercontent.com/json-api-dotnet/JsonApiDotnetCore/master/logo.png', 10, now(), " +
                "(Select CategoriaId From Categorias Where Nome = 'Programação')");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete From Produtos");
            mb.Sql("Delete From Categorias");
        }
    }
}
