namespace X_Libra_Catering.Server.Models
{
    public partial class Vehiculo
    {
        public int Id { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public decimal CapacidadKg { get; set; }
        public bool TieneRefrigeracion { get; set; }
        public bool Disponible { get; set; }
        public string? Direccion { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public virtual ICollection<PedidoCabecera> Pedidos { get; set; } = new List<PedidoCabecera>();
    }
}
