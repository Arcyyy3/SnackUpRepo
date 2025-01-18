using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.ProgressBar;
using SnackUpClient.Models;
using System.Linq;

namespace SnackUpClient
{
    public partial class OrderPage : ContentPage
    {
        public ObservableCollection<StepProgressBarItem> StepProgressItems { get; set; }
        public ObservableCollection<ProdottoOrdine> ListaOrdini { get; set; } = new ObservableCollection<ProdottoOrdine>();
        public string DataConsegna { get; set; }
        public string Orario { get; set; }
        public string DataPagamento { get; set; }
        public decimal Totale => ListaOrdini.Sum(p => p.Prezzo * p.Quantita);
        private int currentStepIndex = 0;

        public OrderPage(OrdineHistory ordine)
        {
            InitializeComponent();

            // Imposta la data e l'orario dell'ordine
            DataConsegna = ordine.Giorno;
            Orario = ordine.Orario;
            DataPagamento = DateTime.Now.ToString("dd/MM/yyyy");

            // Imposta i prodotti
            ListaOrdini = new ObservableCollection<ProdottoOrdine>(
                ordine.Prodotti.Select(p => new ProdottoOrdine
                {
                    Nome = p.Nome,
                    Prezzo = p.Prezzo,
                    Quantita = p.Quantità,
                    Immagine = p.Img ?? "placeholder.png"
                })
            );

            // Configura gli step della barra di avanzamento
            StepProgressItems = new ObservableCollection<StepProgressBarItem>
            {
                new StepProgressBarItem
                {
                    PrimaryFormattedText = CreateFormattedText("Ordine Registrato", "L'ordine è stato registrato con successo."),
                    SecondaryFormattedText = CreateFormattedText("")
                },
                new StepProgressBarItem
                {
                    PrimaryFormattedText = CreateFormattedText("Preparazione in Corso", "Il fornitore sta preparando il tuo ordine."),
                    SecondaryFormattedText = CreateFormattedText("")
                },
                new StepProgressBarItem
                {
                    PrimaryFormattedText = CreateFormattedText("Consegna in Corso", "Il tuo ordine è in viaggio verso la scuola."),
                    SecondaryFormattedText = CreateFormattedText("")
                },
                new StepProgressBarItem
                {
                    PrimaryFormattedText = CreateFormattedText("Consegna Completata", "Il tuo ordine è stato consegnato."),
                    SecondaryFormattedText = CreateFormattedText("")
                }
            };

            // Imposta il contesto del binding
            BindingContext = this;

            // Simula l'avanzamento degli step
            NextStep();
            NextStep();
        }

        private FormattedString CreateFormattedText(string boldText, string line1 = null)
        {
            var formattedString = new FormattedString();
            formattedString.Spans.Add(new Span { Text = boldText, FontSize = 16, FontAttributes = FontAttributes.Bold });
            if (!string.IsNullOrEmpty(line1))
                formattedString.Spans.Add(new Span { Text = $"\n{line1}", FontSize = 12, TextColor = Color.FromArgb("#171717") });
            return formattedString;
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public void UpdateStepProgress(int stepIndex)
        {
            if (stepIndex >= 0 && stepIndex < StepProgressItems.Count)
            {
                currentStepIndex = stepIndex;
                stepProgress.ActiveStepIndex = stepIndex;
                stepProgress.ActiveStepProgressValue = stepIndex == StepProgressItems.Count - 1 ? 100 : 50;
            }
            else
            {
                Console.WriteLine("Step index fuori dai limiti.");
            }
        }

        public void NextStep()
        {
            UpdateStepProgress(currentStepIndex + 1);
        }

        public void PreviousStep()
        {
            UpdateStepProgress(currentStepIndex - 1);
        }
    }

    public class ProdottoOrdine
    {
        public string Nome { get; set; }
        public decimal Prezzo { get; set; }
        public int Quantita { get; set; }
        public string Immagine { get; set; }
    }
}
