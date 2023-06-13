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
                "VALUES ('Hineken', 'Cerveja de 600ml', 11.5, 'hineken.jpg', 50, Now(), (Select id from categorias where nome = 'Bebidas'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Capirinha', 'Caiprinha de limão e vodka', 12.0, 'caipirinha.jpg', 50, Now(), (Select id from categorias where nome = 'Bebidas'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Lanche de atum', 'Lanche de atum com maionese', 8.50, 'atum.jpg', 10, Now(), (Select id from categorias where nome = 'Lanches'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
               "VALUES ('Mini Pizza', 'Mini Pizza de frango', 9.50, 'minipizza.jpg', 10, Now(), (Select id from categorias where nome = 'Lanches'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
               "VALUES ('Enrolado de frango', 'Enroladinho de frango e queijo', 6.0, 'enrolado.jpg', 10, Now(), (Select id from categorias where nome = 'Lanches'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Pudim 100g', 'Pudim de leite condensado', 6.75, 'pudim.jpg', 20, Now(), (Select id from categorias where nome = 'Sobremesas'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Bolo no pote', 'Bolo no pote com leite condensado', 7.75, 'bolopote.jpg', 20, Now(), (Select id from categorias where nome = 'Sobremesas'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Bombom de chocolate', 'Bombom de chocolate', 5.75, 'bombom.jpg', 20, Now(), (Select id from categorias where nome = 'Sobremesas'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Espaquete com queijo', 'Espaquete a bolonhesa', 12.75, 'espaquete.jpg', 20, Now(), (Select id from categorias where nome = 'Massas'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Pizza de calabresa', 'Pizza com queijo e calabresa', 30.75, 'pizzagrande.jpg', 20, Now(), (Select id from categorias where nome = 'Massas'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Lasanha', 'Lasanha de frango', 22.75, 'lasanha.jpg', 20, Now(), (Select id from categorias where nome = 'Massas'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Espetinho de carne', 'Espetinho de carne', 6.75, 'espetinho.jpg', 20, Now(), (Select id from categorias where nome = 'Tira Gosto'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Filé com fritas', 'Carne de gato com batata', 22.75, 'filefritas.jpg', 20, Now(), (Select id from categorias where nome = 'Tira Gosto'));");

            migrationBuilder.Sql("INSERT INTO produtos (Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) " +
                "VALUES ('Porção de batata', 'Porção 200g batata frita', 17.75, 'porcaobatata.jpg', 20, Now(), (Select id from categorias where nome = 'Tira Gosto'));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM produtos WHERE id > 0");
        }
    }
}
