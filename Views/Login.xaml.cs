namespace mbunayExamen.Views;

public partial class Login : ContentPage
{
    private readonly string[,] usuarios = new string[4, 2]
    {
        { "recepcion", "gym2026" },
        { "entrenador", "fit_01" },
        { "uisrael", "2026" },
        { "gerente", "admin123" }
    };

    public Login()
    {
        InitializeComponent();
    }

    private async void OnIniciarSesionClicked(object? sender, EventArgs e)
    {
        string usuario = UsuarioEntry.Text?.Trim() ?? string.Empty;
        string clave = ClaveEntry.Text ?? string.Empty;

        bool valido = false;
        for (int i = 0; i < usuarios.GetLength(0); i++)
        {
            if (usuarios[i, 0] == usuario && usuarios[i, 1] == clave)
            {
                valido = true;
                break;
            }
        }

        if (!valido)
        {
            await DisplayAlertAsync("Dato incorrecto", "Usuario o contraseña incorrectos.", "OK");
            return;
        }

        await Navigation.PushAsync(new Registro(usuario));
    }

    private async void OnAcercaDeClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new AcercaDe());
    }

    public void Limpiar()
    {
        UsuarioEntry.Text = string.Empty;
        ClaveEntry.Text = string.Empty;
    }
}
