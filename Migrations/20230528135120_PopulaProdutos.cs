using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogoAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Coca-Cola Diet', 'Refrigerante de cola 350ml', 5.45, 'cocacola.jpg', 50, Now(), (Select id from categorias where nome = 'Bebidas'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Lanche de atum', 'Lanche de atum com maionese', 8.50, 'atum.jpg', 10, Now(), (Select id from categorias where nome = 'Lanches'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Pudim 100g', 'Pudim de leite condensado', 6.75, 'pudim.jpg', 20, Now(), (Select id from categorias where nome = 'Sobremesas'));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM produtos WHERE id > 0");
        }
    }
}
