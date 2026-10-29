using System.Globalization;
using System.Text;
using mbunayExamen.Models;

namespace mbunayExamen.Views;

public partial class Resumen : ContentPage
{
    private readonly Socio _socio;

    public Resumen(Socio socio)
    {
        InitializeComponent();
        _socio = socio;
        UsuarioLabel.Text = $"Usuario conectado: {socio.Usuario}";
        NombreValor.Text = socio.Nombre;
        ApellidoValor.Text = socio.Apellido;
        EdadValor.Text = socio.Edad.ToString();
        FechaValor.Text = socio.FechaInicio.ToString("dd/MM/yyyy");
        PlanValor.Text = socio.Plan;
        CasilleroValor.Text = socio.Casillero ? "Sí" : "No";
        MontoValor.Text = socio.MontoInicial.ToString("0.00", CultureInfo.InvariantCulture);
        PagoMensualValor.Text = socio.PagoMensual.ToString("0.00", CultureInfo.InvariantCulture);
        PagoTotalValor.Text = socio.PagoTotal.ToString("0.00", CultureInfo.InvariantCulture);
    }

    private async void OnNuevoRegistroClicked(object? sender, EventArgs e)
    {
        var paginas = Navigation.NavigationStack;
        if (paginas.Count >= 2 && paginas[paginas.Count - 2] is Registro registro)
            registro.LimpiarCampos();

        await Navigation.PopAsync();
    }

    private async void OnMigrarTxtClicked(object? sender, EventArgs e)
    {
        try
        {
            string carpeta = FileSystem.AppDataDirectory;
            Directory.CreateDirectory(carpeta);
            string ruta = Path.Combine(carpeta, "socios.txt");

            var texto = new StringBuilder();
            texto.AppendLine($"----- {DateTime.Now:dd/MM/yyyy HH:mm} -----");
            texto.AppendLine($"Usuario conectado: {_socio.Usuario}");
            texto.AppendLine($"Nombre: {_socio.Nombre}");
            texto.AppendLine($"Apellido: {_socio.Apellido}");
            texto.AppendLine($"Edad: {_socio.Edad}");
            texto.AppendLine($"Fecha de inicio: {_socio.FechaInicio:dd/MM/yyyy}");
            texto.AppendLine($"Plan: {_socio.Plan}");
            texto.AppendLine($"Casillero: {(_socio.Casillero ? "Sí" : "No")}");
            texto.AppendLine($"Monto Inicial: {_socio.MontoInicial.ToString("0.00", CultureInfo.InvariantCulture)}");
            texto.AppendLine($"Pago mensual: {_socio.PagoMensual.ToString("0.00", CultureInfo.InvariantCulture)}");
            texto.AppendLine($"Pago Total: {_socio.PagoTotal.ToString("0.00", CultureInfo.InvariantCulture)}");
            texto.AppendLine();

            await File.AppendAllTextAsync(ruta, texto.ToString(), Encoding.UTF8);
            await DisplayAlertAsync("Migrar TXT", $"Datos migrados en:\n{ruta}", "OK");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            await DisplayAlertAsync("Migrar TXT", "No se pudo crear el archivo de texto.", "OK");
        }
    }

    private async void OnCerrarSesionClicked(object? sender, EventArgs e)
    {
        if (Navigation.NavigationStack.FirstOrDefault() is Login login)
            login.Limpiar();

        await Navigation.PopToRootAsync();
    }
}
