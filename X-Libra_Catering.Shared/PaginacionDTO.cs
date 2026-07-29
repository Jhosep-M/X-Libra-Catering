namespace X_Libra_Catering.Shared;

public class PaginacionDTO
{
    public int Pagina { get; set; } = 1;
    public int Tamano { get; set; } = 20;
    public string? Busqueda { get; set; }
}

public class ResultadoPaginado<T>
{
    public List<T> Items { get; set; } = new();
    public int Total { get; set; }
    public int Pagina { get; set; }
    public int TotalPaginas => (int)Math.Ceiling((double)Total / Tamano);
    public int Tamano { get; set; }
}
