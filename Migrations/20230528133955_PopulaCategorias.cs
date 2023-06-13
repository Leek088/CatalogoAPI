using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogoAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO categorias (Nome, ImagemUrl) VALUES ('Bebidas', 'bebidas.jpg')");

            migrationBuilder.Sql("INSERT INTO categorias (Nome, ImagemUrl) VALUES ('Lanches', 'lanches.jpg')");

            migrationBuilder.Sql("INSERT INTO categorias (Nome, ImagemUrl) VALUES ('Sobremesas', 'sobremesas.jpg')");

            migrationBuilder.Sql("INSERT INTO categorias (Nome, ImagemUrl) VALUES ('Massas', 'massa.jpg')");

            migrationBuilder.Sql("INSERT INTO categorias (Nome, ImagemUrl) VALUES ('Tira Gosto', 'tiragosto.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete FROM categorias WHERE Id > 0");
        }
    }
}
