namespace X_Libra_Catering.Shared;

public class DashboardCompletoDTO
{
    public int TotalEventos { get; set; }
    public int EventosPendientes { get; set; }
    public int EventosCompletados { get; set; }
    public int TotalPedidos { get; set; }
    public int PedidosEntregados { get; set; }
    public int VehiculosDisponibles { get; set; }
    public int VehiculosTotales { get; set; }
    public decimal IngresosMes { get; set; }
    public List<DatoGrafico> EventosPorMes { get; set; } = new();
    public List<DatoGrafico> PedidosPorEstado { get; set; } = new();
    public List<DatoGrafico> IngresosPorMes { get; set; } = new();
}

public class DatoGrafico
{
    public string Label { get; set; } = "";
    public decimal Valor { get; set; }
}
