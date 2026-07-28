using X_Libra_Catering.Shared.Enums;

namespace X_Libra_Catering.Server.Models
{
    public partial class Evento
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string NombreEvento { get; set; } = string.Empty;
        public TipoEvento TipoEvento { get; set; }
        public EstadoEvento Estado { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public int NumInvitados { get; set; }
        public virtual Cliente? Cliente { get; set; }
        public virtual ICollection<PedidoCabecera> Pedidos { get; set; } = new List<PedidoCabecera>();
    }
}
