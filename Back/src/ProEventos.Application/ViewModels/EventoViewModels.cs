using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.ViewModels
{
    public class EventoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [MinLength(3, ErrorMessage = "{0} deve ter mais que 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "{0} deve ter no maxímo 50 caracteres.")]
        public string? Local { get; set; }
        public string? DataEvento { get; set; }
        public string? Tema { get; set; }

        [Range(1, 1000, ErrorMessage = "{0} deve estar entre 1 e 1000.")]
        [Display(Name = "Qtd. Pessoas")]
        public int QtdPessoas { get; set; }
        public int Lote { get; set; }

        [Required(ErrorMessage = "Imabem deve ser informada")]
        [RegularExpression(@".*\.(gif|jpg)$", ErrorMessage = "Imagem deve ser do tipo gif ou jpg")]
        public string? ImagemURL { get; set; }

        [Required(ErrorMessage = "{0} deve ser informado")]
        [Phone(ErrorMessage = "Formato do {0} inválido")]
        public string? Telefone { get; set; }

        [Display(Name = "e-mail")]
        [EmailAddress(ErrorMessage = "{0} deve ser um e-mail válido")]
        public string? Email { get; set; }
        public IEnumerable<LoteViewModel>? Lotes { get; set; }
        public IEnumerable<RedeSocialViewModel>? RedesSociais { get; set; }
        //public IEnumerable<PalestranteEvento>? PalestrantesEventos { get; set; }        
    }
}