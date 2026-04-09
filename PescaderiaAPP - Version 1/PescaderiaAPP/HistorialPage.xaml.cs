using PescaderiaAPP.Services;
//using UIKit;

namespace PescaderiaAPP;

public partial class HistorialPage : ContentPage
{
	public HistorialPage()
	{
		InitializeComponent();
		OrdenesManager.Instance.OrdenesActualizadas += Actualizar;
    }

    protected override async void OnAppearing()
	{
		base.OnAppearing();
		Actualizar();

		PaginaLayout.Opacity = 0;
		PaginaLayout.TranslationY = 20;
		await Task.WhenAll(
			PaginaLayout.FadeTo(1, 300, Easing.CubicOut),
			PaginaLayout.TranslateTo(0, 0, 300, Easing.CubicOut));
    }

	private void Actualizar()
	{
		var historial = OrdenesManager.Instance.Historial;
		HistorialCollection.ItemsSource = null;
		HistorialCollection.ItemsSource = historial;
		SinHistorialLabel.IsVisible = historial.Count == 0;
		LimpiarBtn.IsVisible = historial.Count > 0;
    }

	private async void OnLimpiarHistorial(object sender, EventArgs e)
	{
		bool confirmar = await DisplayAlert("Limpiar historial",
			"¿Borrar todo el historial del día?", "Sí, borrar", "Cancelar");

		if(confirmar)
		{
			OrdenesManager.Instance.Historial.Clear();
			Actualizar();
        }
	}
}