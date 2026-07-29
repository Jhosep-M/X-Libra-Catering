using X_Libra_Catering.Shared.Enums;

namespace X_Libra_Catering.Server.Models
{
    public partial class PedidoCabecera : IAuditable
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public int VehiculoId { get; set; }
        public DateTime FechaPedido { get; set; }
        public EstadoPedido Estado { get; set; }
        public decimal Total { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public virtual Evento? Evento { get; set; }
        public virtual Vehiculo? Vehiculo { get; set; }
        public virtual ICollection<PedidoDetalle> Detalles { get; set; } = new List<PedidoDetalle>();
    }
}
