using PescaderiaAPP.Models;
using PescaderiaAPP.Services;
//using UIKit;

namespace PescaderiaAPP;

public partial class MainPage : ContentPage
{
    private List<ItemOrden> _items = new();

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnAgregar(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        await AnimarBoton(btn); //animacion de click
        var partes = btn.CommandParameter.ToString()!.Split('|');
        _items.Add(new ItemOrden
        {
            Nombre = partes[0],
            Precio = decimal.Parse(partes[1])
        });
        ActualizarResumen();
    }

    private async void OnQuitar(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        await AnimarBoton(btn); //animacion de click
        string nombre = btn.CommandParameter.ToString()!;

        // Quita solo la última ocurrencia de ese platillo
        var item = _items.LastOrDefault(i => i.Nombre == nombre);
        if (item != null)
        {
            _items.Remove(item);
            ActualizarResumen();
        }
    }

    private void ActualizarResumen()
    {
        ResumenStack.Children.Clear();

        if (_items.Count == 0)
        {
            ResumenStack.Children.Add(new Label
            {
                Text = "(Vacío)",
                TextColor = Colors.Gray,
                FontSize = 13
            });
            TotalLabel.Text = "Total: $0.00";
            return;
        }

        // Construir una fila por cada platillo agrupado
        foreach (var grupo in _items.GroupBy(i => i.Nombre))
        {
            var fila = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },   // nombre
                    new ColumnDefinition { Width = GridLength.Auto },   // cantidad y subtotal
                    new ColumnDefinition { Width = GridLength.Auto },   // botón −
                }
            };

            var lblNombre = new Label
            {
                Text = grupo.Key,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 13
            };

            var lblCantidad = new Label
            {
                Text = $"x{grupo.Count()}  —  ${grupo.Sum(i => i.Precio):0.00}",
                VerticalOptions = LayoutOptions.Center,
                TextColor = Colors.DarkSlateGray,
                FontSize = 13,
                Margin = new Thickness(8, 0)
            };

            var btnMenos = new Button
            {
                Text = "−",
                WidthRequest = 38,
                HeightRequest = 38,
                BackgroundColor = Color.FromArgb("#E53935"),
                TextColor = Colors.White,
                FontSize = 18,
                Padding = new Thickness(0),
                CommandParameter = grupo.Key
            };
            btnMenos.Clicked += OnQuitar;

            Grid.SetColumn(lblNombre, 0);
            Grid.SetColumn(lblCantidad, 1);
            Grid.SetColumn(btnMenos, 2);

            fila.Children.Add(lblNombre);
            fila.Children.Add(lblCantidad);
            fila.Children.Add(btnMenos);

            ResumenStack.Children.Add(fila);
        }

        TotalLabel.Text = $"Total: ${_items.Sum(i => i.Precio):0.00}";
    }

    private void OnLimpiarOrden(object sender, EventArgs e)
    {
        _items.Clear();
        ActualizarResumen();
    }

    private async void OnConfirmarOrden(object sender, EventArgs e)
    {
        if (MesaPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Selecciona una mesa.", "OK");
            return;
        }
        if (_items.Count == 0)
        {
            await DisplayAlert("Error", "La orden está vacía.", "OK");
            return;
        }

        string mesa = MesaPicker.SelectedItem.ToString()!;

        OrdenesManager.Instance.GuardarOrden(new OrdenMesa
        {
            Mesa = mesa,
            Items = new List<ItemOrden>(_items)
        });

        await DisplayAlert("✅ Orden guardada",
            $"{mesa} guardada con ${_items.Sum(i => i.Precio):0.00} en total.", "OK");

        _items.Clear();
        MesaPicker.SelectedIndex = -1;
        ActualizarResumen();
    }

    private async Task AnimarBoton(Button btn)
    {
        await btn.ScaleTo(0.80, 80, Easing.CubicIn);
        await btn.ScaleTo(1.0, 80, Easing.CubicOut);
    }

    private void OnMesaBorderTapped(object sender, TappedEventArgs e)
    {
        MesaPicker.Focus();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        PaginaLayout.Opacity = 0;
        PaginaLayout.TranslationY = 20;
        await Task.WhenAll(
            PaginaLayout.FadeTo(1, 300, Easing.CubicOut),
            PaginaLayout.TranslateTo(0, 0, 300, Easing.CubicOut));
    }
 
}