namespace SnackUpClient;

public partial class PaginaIniziale : ContentPage
{
    public PaginaIniziale()
    {
        InitializeComponent();
    }
    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaLogin());
    }
    private async void OnStartButtonClicked2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaginaRegistrazione());
    }
}