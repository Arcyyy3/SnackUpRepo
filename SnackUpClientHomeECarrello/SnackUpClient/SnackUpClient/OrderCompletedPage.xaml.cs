namespace SnackUpClient;

public partial class OrderCompletedPage : ContentPage
{
    public OrderCompletedPage()
    {
        InitializeComponent();

        // Avvia le animazioni dopo il caricamento della pagina
        this.Appearing += OnPageAppearing;
    }

    private async void OnPageAppearing(object sender, EventArgs e)
    {
        // Animazione del primo label
        var snackUpLabel = this.FindByName<Label>("SnackUpLabel");
        snackUpLabel.Scale = 0; // Inizia a scala 0
        await snackUpLabel.ScaleTo(1, 500, Easing.CubicOut); // Scala da 0 a 1 in 500ms

        // Attendi 1 secondo prima di animare il secondo label e il border
        await Task.Delay(1000);

        // Animazione del secondo label
        var problemsLabel = this.FindByName<Label>("ProblemsLabel");
        problemsLabel.Scale = 0; // Inizia a scala 0
        problemsLabel.IsVisible = true;
        await problemsLabel.ScaleTo(1, 500, Easing.CubicOut); // Scala da 0 a 1 in 500ms

        // Animazione del Border
        var border = this.FindByName<Border>("AnimatedBorder");
        var initialTranslation = this.Height; // Inizia sotto lo schermo
        border.TranslationY = initialTranslation;
        border.IsVisible = true;
        await border.TranslateTo(0, 0, 500, Easing.CubicOut); // Traslazione verso l'alto
    }

    private async void OnBackToHomePage(object sender, EventArgs e)
    {
        // Navigazione alla HomePage
        await Navigation.PushAsync(new HomePage());
    }

    private async void OnNavigateToSupport(object sender, EventArgs e)
    {
        // Navigazione alla pagina di supporto
        await Navigation.PushAsync(new SupportPage());
    }
}
