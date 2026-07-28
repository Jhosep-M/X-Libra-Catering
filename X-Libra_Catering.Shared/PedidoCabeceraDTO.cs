using X_Libra_Catering.Shared.Enums;

namespace X_Libra_Catering.Shared
{
    public class PedidoCabeceraDTO
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public int VehiculoId { get; set; }
        public DateTime FechaPedido { get; set; }
        public EstadoPedido Estado { get; set; }
        public decimal Total { get; set; }
        public string? EventoNombre { get; set; }
        public string? VehiculoPlaca { get; set; }
        public List<PedidoDetalleDTO> Detalles { get; set; } = new();
    }
}
