using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogoAPI.Models;

public class Produto
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Nome do produto")]
    [Required(ErrorMessage = "Nome do produto é obrigatório.")]
    [StringLength(80, ErrorMessage = "Valor máximo do Nome do produto é de 80 caracteres.")]
    public string? Nome { get; set; }

    [Display(Name = "Descrição do produto")]
    [Required(ErrorMessage = "Descrição do produto é obrigatório.")]
    [StringLength(200, ErrorMessage = "Valor máximo da Descrição do produto é 200 caracteres.")]
    public string? Descricao { get; set; }

    [Display(Name = "Preço do produto")]
    [Required(ErrorMessage = "Preço do produto é obrigatório.")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [Display(Name = "Url da foto")]
    [Required(ErrorMessage = "Url da foto é obrigatório.")]
    [StringLength(100, ErrorMessage = "Valor máximo da Url da foto é 100 caracteres.")]
    public string? ImagemUrl { get; set; }

    [Display(Name = "Quantidade em estoque")]
    [Required(ErrorMessage = "Quantidade em estoque é obrigatório")]
    [Range(1.0, float.MaxValue, ErrorMessage = "Valor do estoque fora do padrão")]
    public float Estoque { get; set; }

    [Display(Name = "Dado o cadastro")]

    [Required(ErrorMessage = "Data do cadastro é obrigatório")]
    public DateTime DataCadastro { get; set; }

    [Display(Name = "Categoria do produto")]
    [Required(ErrorMessage = "Categoria do produto é obrigatório")]
    public int CategoriaId { get; set; }

    //Propriedade de navegação
    [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingNull)]
    public Categoria? Categoria { get; set; }
}
