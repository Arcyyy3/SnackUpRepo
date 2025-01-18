using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace SnackUpClient
{
    public class PresentationItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }
        public string RectangleStyle { get; set; }
        public double Width { get; set; } 
        public double Height { get; set; }
    }

    public partial class TutorialIniziale : ContentPage
    {
        private const double BackgroundShiftPerSlide = 300; // Larghezza in pixel da spostare per slide

        public TutorialIniziale()
        {
            InitializeComponent();
        }

        // Evento per il pulsante "Avanti"
        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            int currentIndex = carouselView.Position;
            int nextIndex = currentIndex + 1;

            if (nextIndex < carouselView.ItemsSource.Cast<object>().Count())
            {
                carouselView.Position = nextIndex;
            }
        }

        // Evento per il cambiamento di posizione del carosello
        private async void OnCarouselPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int currentIndex = carouselView.Position;
            int lastIndex = carouselView.ItemsSource.Cast<object>().Count() - 1;

            // Mostra o nasconde i pulsanti in base alla posizione corrente
            nextButton.IsVisible = currentIndex < lastIndex;
            startButton.IsVisible = currentIndex == lastIndex;

            // Calcola la nuova traslazione dello sfondo
            var translationX = -currentIndex * BackgroundShiftPerSlide;

            // Anima lo spostamento dello sfondo
            await BackgroundImage.TranslateTo(translationX, 0, 250, Easing.CubicInOut); // 250 ms animazione fluida
        }

        // Evento per il pulsante "Iniziamo!"
        private async void OnStartButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaIniziale());
        }

        private async Task AnimateFrame(Frame frame)
        {
            // Inizializza fuori dallo schermo (opzione scorrimento dal basso verso l'alto)
            frame.TranslationY = this.Height; // Usa l'altezza della pagina corrente

            // Esegui l'animazione (durata in millisecondi)
            await frame.TranslateTo(0, 0, 500, Easing.SpringOut);
        }
    }
}
