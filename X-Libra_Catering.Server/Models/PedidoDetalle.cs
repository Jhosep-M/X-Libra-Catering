namespace X_Libra_Catering.Server.Models
{
    public partial class PedidoDetalle : IAuditable
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int MenuId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public virtual PedidoCabecera? Pedido { get; set; }
        public virtual Menu? Menu { get; set; }
    }
}
