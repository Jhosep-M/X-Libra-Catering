using X_Libra_Catering.Shared.Enums;

namespace X_Libra_Catering.Server.Models
{
    public partial class Menu
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public CategoriaMenu Categoria { get; set; }
        public decimal Precio { get; set; }
        public bool RequiereRefrigeracion { get; set; }
        public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
    }
}
