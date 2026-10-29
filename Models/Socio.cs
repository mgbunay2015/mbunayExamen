namespace mbunayExamen.Models;

public class Socio
{
    public string Usuario { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public int Edad { get; set; }
    public DateTime FechaInicio { get; set; }
    public string Plan { get; set; } = string.Empty;
    public bool Casillero { get; set; }
    public decimal MontoInicial { get; set; }
    public decimal PagoMensual { get; set; }
    public decimal PagoTotal { get; set; }
}
