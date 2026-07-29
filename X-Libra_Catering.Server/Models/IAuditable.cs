namespace X_Libra_Catering.Server.Models;

public interface IAuditable
{
    DateTime? FechaCreacion { get; set; }
    DateTime? FechaModificacion { get; set; }
}