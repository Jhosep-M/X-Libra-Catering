namespace X_Libra_Catering.Shared
{
    public class PedidoDetalleDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int MenuId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public string? MenuNombre { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
