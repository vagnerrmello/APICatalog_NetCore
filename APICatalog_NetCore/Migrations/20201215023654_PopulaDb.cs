using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalog_NetCore.Migrations
{
    public partial class PopulaDb : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert Into catalogodb.categorias(Nome, ImagemUrl) Values('Programação', 'http://www.macoratti.net/Imagens/1.jpg')");

            mb.Sql("Insert Into catalogodb.produtos(Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, CategoriaId) " +
                "Values('CSharp', 'Linguagem de programação', 299.99, 'https://raw.githubusercontent.com/json-api-dotnet/JsonApiDotnetCore/master/logo.png', 10, now(), " +
                "(Select CategoriaId From Categorias Where Nome = 'Programação'))");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete From Produtos");
            mb.Sql("Delete From Categorias");
        }
    }
}
