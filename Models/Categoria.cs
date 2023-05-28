using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CatalogoAPI.Models;

public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }

    [Key]
    public int Id { get; set; }

    [Display(Name = "Nome da categoria")]
    [Required(ErrorMessage = "Nome da categoria é obrigatório.")]
    [StringLength(80, ErrorMessage = "Valor máximo da categoria é 80 caracteres.")]
    public string? Nome { get; set; }

    [Display(Name = "Url da foto")]
    [Required(ErrorMessage = "Url da foto é obrigatório.")]
    [StringLength(100, ErrorMessage = "Valor máximo da Url da foto é 100 caracteres.")]
    public string? ImagemUrl { get; set; }

    //Propriedade de navegação
    [JsonIgnore]
    public ICollection<Produto>? Produtos { get; set; }
}
