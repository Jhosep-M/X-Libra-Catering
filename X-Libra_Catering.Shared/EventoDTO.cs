using X_Libra_Catering.Shared.Enums;

namespace X_Libra_Catering.Shared
{
    public class EventoDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string NombreEvento { get; set; } = string.Empty;
        public TipoEvento TipoEvento { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public int NumInvitados { get; set; }
        public string? ClienteNombre { get; set; }
    }
}
