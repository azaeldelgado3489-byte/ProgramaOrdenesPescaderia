using PescaderiaAPP.Services;

namespace PescaderiaAPP;

public partial class MesasPage : ContentPage
{
    public MesasPage()
    {
        InitializeComponent();
        OrdenesManager.Instance.OrdenesActualizadas += Actualizar;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Actualizar();

        // Animación
        PaginaLayout.Opacity = 0;
        PaginaLayout.TranslationY = 20;
        await Task.WhenAll(
            PaginaLayout.FadeTo(1, 300, Easing.CubicOut),
            PaginaLayout.TranslateTo(0, 0, 300, Easing.CubicOut));
    }

    private void Actualizar()
    {
        var ordenes = OrdenesManager.Instance.OrdenesActivas;
        MesasCollection.ItemsSource = null;
        MesasCollection.ItemsSource = ordenes;
        SinOrdenesLabel.IsVisible = ordenes.Count == 0;
    }

    private async void OnCerrarMesa(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        string mesa = btn.CommandParameter.ToString()!;
        bool confirmar = await DisplayAlert("Cerrar Mesa",
            $"¿Cerrar la orden de {mesa}?", "Sí", "No");
        if (confirmar)
            OrdenesManager.Instance.CerrarOrden(mesa);
    }
}