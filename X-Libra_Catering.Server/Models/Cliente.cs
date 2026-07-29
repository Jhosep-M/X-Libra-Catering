namespace X_Libra_Catering.Server.Models
{
    public partial class Cliente : IAuditable
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
    }
}
