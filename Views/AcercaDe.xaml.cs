namespace mbunayExamen.Views;

public partial class AcercaDe : ContentPage
{
    private const string Nombre = "Nombre Apellido";
    private const string Carrera = "Carrera";
    private const string Paralelo = "Paralelo";

    public AcercaDe()
    {
        InitializeComponent();
        NombreLabel.Text = $"Nombre: {Nombre}";
        CarreraLabel.Text = $"Carrera: {Carrera}";
        ParaleloLabel.Text = $"Paralelo: {Paralelo}";
    }
}
