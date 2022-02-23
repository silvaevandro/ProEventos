namespace ProEventos.Application.ViewModels
{
    public class PalestranteViewModel
    {
        public int Id { get; set; }
        public string? MiniCurriculo { get; set; }
        public int UserId { get; set; }
        public UserUpdateViewModel? User { get; set; }
        public IEnumerable<RedeSocialViewModel>? RedesSociais { get; set; }
        public IEnumerable<EventoViewModel>? Eventos { get; set; }
    }
}