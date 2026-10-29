using System.Globalization;
using mbunayExamen.Models;

namespace mbunayExamen.Views;

public partial class Registro : ContentPage
{
    private const decimal CostoAnual = 1200m;
    private readonly string _usuario;
    private decimal _pagoMensual;
    private decimal _montoCalculado;
    private bool _casilleroCalculado;
    private bool _pagoCalculado;

    public Registro(string usuario)
    {
        InitializeComponent();
        _usuario = usuario;
        UsuarioLabel.Text = $"Usuario conectado: {_usuario}";
        var hoy = DateTime.Today;
        FechaPicker.Date = hoy;
        FechaPicker.MinimumDate = hoy;
    }

    private void OnEdadChanged(object? sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
            return;

        string soloNumeros = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
        if (soloNumeros != e.NewTextValue)
            EdadEntry.Text = soloNumeros;
    }

    private void OnMontoChanged(object? sender, TextChangedEventArgs e)
    {
        if (!_pagoCalculado)
            return;

        if (!TryObtenerMonto(out decimal monto) || monto != _montoCalculado)
            InvalidarPago();
    }

    private void OnCasilleroToggled(object? sender, ToggledEventArgs e)
    {
        if (_pagoCalculado && e.Value != _casilleroCalculado)
            InvalidarPago();
    }

    private async void OnCalcularClicked(object? sender, EventArgs e)
    {
        if (!TryObtenerMonto(out decimal monto))
        {
            await DisplayAlertAsync("Monto inicial", "El monto inicial debe estar entre 120 y 600.", "OK");
            return;
        }

        decimal cuota = (CostoAnual - monto) / 5m;
        decimal pago = cuota + (CostoAnual * 0.02m);
        if (CasilleroSwitch.IsToggled)
            pago += 10m;

        _montoCalculado = monto;
        _casilleroCalculado = CasilleroSwitch.IsToggled;
        _pagoMensual = pago;
        _pagoCalculado = true;
        PagoEntry.Text = pago.ToString("0.00", CultureInfo.InvariantCulture);
    }

    private async void OnVerResumenClicked(object? sender, EventArgs e)
    {
        string nombre = NombreEntry.Text?.Trim() ?? string.Empty;
        string apellido = ApellidoEntry.Text?.Trim() ?? string.Empty;
        if (nombre.Length == 0 || apellido.Length == 0)
        {
            await DisplayAlertAsync("Datos incompletos", "Ingrese nombre y apellido.", "OK");
            return;
        }

        if (!int.TryParse(EdadEntry.Text, out int edad) || edad < 16)
        {
            await DisplayAlertAsync("Edad", "La edad debe ser un número mayor o igual a 16.", "OK");
            return;
        }

        if (PlanPicker.SelectedIndex < 0 || string.IsNullOrWhiteSpace(PlanPicker.SelectedItem?.ToString()))
        {
            await DisplayAlertAsync("Plan", "Seleccione un plan.", "OK");
            return;
        }

        string plan = PlanPicker.SelectedItem!.ToString()!;
        DateTime fecha = FechaPicker.Date ?? DateTime.Today;
        if (fecha.Date < DateTime.Today)
        {
            await DisplayAlertAsync("Fecha de inicio", "No se permiten fechas pasadas.", "OK");
            return;
        }

        if (!TryObtenerMonto(out decimal monto))
        {
            await DisplayAlertAsync("Monto inicial", "El monto inicial debe estar entre 120 y 600.", "OK");
            return;
        }

        if (!_pagoCalculado || monto != _montoCalculado || CasilleroSwitch.IsToggled != _casilleroCalculado)
        {
            await DisplayAlertAsync("Pago mensual", "Calcule el pago mensual antes de continuar.", "OK");
            return;
        }

        var socio = new Socio
        {
            Usuario = _usuario,
            Nombre = nombre,
            Apellido = apellido,
            Edad = edad,
            FechaInicio = fecha,
            Plan = plan,
            Casillero = CasilleroSwitch.IsToggled,
            MontoInicial = monto,
            PagoMensual = _pagoMensual,
            PagoTotal = monto + (_pagoMensual * 5m)
        };

        await Navigation.PushAsync(new Resumen(socio));
    }

    public void LimpiarCampos()
    {
        NombreEntry.Text = string.Empty;
        ApellidoEntry.Text = string.Empty;
        EdadEntry.Text = string.Empty;
        FechaPicker.Date = DateTime.Today;
        PlanPicker.SelectedIndex = -1;
        CasilleroSwitch.IsToggled = false;
        MontoEntry.Text = string.Empty;
        PagoEntry.Text = string.Empty;
        _pagoMensual = 0m;
        _montoCalculado = 0m;
        _casilleroCalculado = false;
        _pagoCalculado = false;
    }

    private void InvalidarPago()
    {
        _pagoCalculado = false;
        _pagoMensual = 0m;
        if (!string.IsNullOrEmpty(PagoEntry.Text))
            PagoEntry.Text = string.Empty;
    }

    private bool TryObtenerMonto(out decimal monto)
    {
        monto = 0m;
        string texto = (MontoEntry.Text ?? string.Empty).Trim().Replace(',', '.');
        if (!decimal.TryParse(texto, NumberStyles.Number, CultureInfo.InvariantCulture, out monto))
            return false;

        return monto >= 120m && monto <= 600m;
    }
}
