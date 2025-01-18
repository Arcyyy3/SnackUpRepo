using System;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Microsoft.Maui.Controls;
using SnackUpClient.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SnackUpClient;

public partial class OrderCollectionPage : ContentPage
{
    private const double BottomSheetHeight = 300; // Altezza del Bottom Sheet
    public ObservableCollection<Ordine> OrdiniInCorso { get; set; }
    private readonly string BaseSource = "192.168.87.188";
    private int UserID;
    private string _class;


    public string Class
    {
        get => _class;
        set
        {
            _class = value;
            OnPropertyChanged(); // Notifica la UI
        }
    }

    private string _section;
    public string Section
    {
        get => _section;
        set
        {
            _section = value;
            OnPropertyChanged(); // Notifica la UI
        }
    }
    public string ClassSection => $"{Class}{Section}";
    public OrderCollectionPage(int userID)
    {

            InitializeComponent();
            // Aggiorna il pulsante in base all'orario

        
        Console.WriteLine("Costruttore chiamato");
        UserID = userID;
        OrdiniInCorso = new ObservableCollection<Ordine>();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        Debug.WriteLine("OnAppearing chiamato");
        base.OnAppearing();
        try
        {
            await SetDefaultValues();
            Debug.WriteLine("SetDefaultValues completato");
            await LoadOrdersAsync();
            Debug.WriteLine("LoadOrdersAsync completato");
            await GetCode();
            Debug.WriteLine("GetCode completato");
            await CheckCode();
            Debug.WriteLine("CheckCodeCOmpletato");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Errore in OnAppearing: {ex.Message}");
        }
    }

    public async Task CheckCode()
    {
        try
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001")
            };

            // Effettua la chiamata API
            var checkResponse = await client.GetAsync($"/api/ClassDeliveryCodes/IsCodeAlreadyRetrievedWithName/{UserID}");
            if (!checkResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Errore nella risposta API: {checkResponse.StatusCode}");
                return;
            }

            // Leggi il contenuto come stringa
            var responseCheck = await checkResponse.Content.ReadFromJsonAsync<ResponseCheck>();

            // Log della risposta
            Console.WriteLine($"Risposta API: {responseCheck.Ritirato}");

            // Se la risposta è in formato JSON e vuoi deserializzarla
         
            if (responseCheck.Ritirato == "invalid moment")
            {
               //ciao arci

                AddToCartButton.BackgroundColor = Colors.Gray;
                AddToCartButton.BorderColor = Colors.Gray;
                AddToCartButton.Text = $"Codice non disponibile";
                AddToCartButton.IsEnabled = false;
            }else if (responseCheck.Ritirato == "non ritirato")
            {
                AddToCartButton.BackgroundColor = Color.FromHex("#FF7B21"); // Imposta un colore abilitato (esempio)
                AddToCartButton.BorderColor = Color.FromHex("#FF7B21"); // Imposta lo sfondo grigio
                AddToCartButton.Text = "Ottieni il codice"; // Testo del pulsante abilitato
                AddToCartButton.IsEnabled = true; // Abilita il pulsante
                GetCode();
            }
            else
            {
                Console.WriteLine("ENTRATO");
                AddToCartButton.BackgroundColor = Colors.Gray;
                AddToCartButton.BorderColor = Colors.Gray;
                AddToCartButton.Text = $"Codice già ritirato da: {responseCheck.Ritirato}";
                AddToCartButton.IsEnabled = false;
                CodiceRecezione.Text = "Codice già confermato";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore in CheckCode: {ex.Message}");
        }
    }


    public async Task GetCode()
    {

        try
        {


            // Prepara l'handler HTTP
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            // Configura l'HTTP client
            using var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001")
            };

            // Effettua la richiesta al server
            var response = await client.GetAsync($"/api/ClassDeliveryCodes/GetCode/{UserID}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DeliveryCodeResponse>();
                AddToCartButton.BackgroundColor = Color.FromHex("#FF7B21"); // Imposta un colore abilitato (esempio)
                AddToCartButton.BorderColor = Color.FromHex("#FF7B21"); // Imposta lo sfondo grigio
                AddToCartButton.Text = "Ottieni il codice"; // Testo del pulsante abilitato
                AddToCartButton.IsEnabled = true; // Abilita il pulsante
                CodiceRecezione.Text = result?.Code ?? "Codice non trovato";
                if (AddToCartButton == null)
                {
                    Console.WriteLine("prima parte  è null. Controlla il binding XAML.");
                    return;
                }

                else
                {
                    Console.WriteLine("prima non ènull. Controlla il binding XAML.");

                }

            }
            else if ((int)response.StatusCode == 400)
            {
                CodiceRecezione.Text = "Codice non disponibile al momento";
                AddToCartButton.BackgroundColor = Colors.Gray; // Imposta lo sfondo grigio
                AddToCartButton.BorderColor = Colors.Gray; // Imposta lo sfondo grigio
                AddToCartButton.Text = "Ordine non disponibile"; // Testo del pulsante
                AddToCartButton.IsEnabled = false; // Disabilita il pulsante
                if (AddToCartButton == null)
                {
                    Console.WriteLine("Seconda è null. Controlla il binding XAML.");
                    return;
                }

                else
                {
                    Console.WriteLine("Seconda non ènull. Controlla il binding XAML.");

                }

                return;
            }

            else
            {
                CodiceRecezione.Text = "Errore nel recupero del codice";
                Console.WriteLine($"Errore nella risposta del server: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            CodiceRecezione.Text = "Errore nel caricamento del codice";
            Console.WriteLine($"Errore durante il recupero del codice: {ex.Message}");
        }
    }
   
  

    private async Task LoadOrdersAsync()
    {
        var orders = await FetchOrdersAsync();

        // Svuota la collezione esistente
        OrdiniInCorso.Clear();

        foreach (var order in orders)
        {
            OrdiniInCorso.Add(new Ordine
            {
                Studente = $"{order.StudentName} {order.StudentSurname}",
                Prodotti = order.Items.Select(item => new Prodotto
                {
                    Nome = item.ProductName,
                    Quantità = item.Quantity
                }).ToList()
            });
        }
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    private async void OnAddToCartClicked(object sender, EventArgs e)
    {
        // Modifica il bottone "Aggiungi al Carrello"
        AddToCartButton.Text = "Conferma e ritira";
        AddToCartButton.Clicked -= OnAddToCartClicked; // Rimuovi vecchio handler
        AddToCartButton.Clicked += OnConfirmClicked;   // Aggiungi nuovo handler
        BottomSheet.IsVisible = true;
        // Animazione di apertura
        await BottomSheet.TranslateTo(0, 0, 400, Easing.CubicOut);
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        try
        {
            // Prepara l'handler HTTP
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            // Configura l'HTTP client
            using var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001")
            };

            // Effettua la richiesta per confermare il ritiro del codice
            var response = await client.PostAsync($"/api/ClassDeliveryCodes/ConfirmRetrieval/{UserID}", null);

            if (response.IsSuccessStatusCode)
            {
                // Supponiamo che il server restituisca il nome della persona che ha ritirato il codice
                var result = await response.Content.ReadFromJsonAsync<DeliveryCodeConfirmationResponse>();

                HttpClientHandler handler1 = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                // Configura l'HTTP client
                using var client1 = new HttpClient(handler)
                {
                    BaseAddress = new Uri($"https://{BaseSource}:5001")
                };
                var response1 = await client.GetAsync($"/api/ClassDeliveryCodes/GetCodeInfo/{UserID}");
                var CodeInfoStorage = await response1.Content.ReadFromJsonAsync<CodeInfo>();
                var payload = new
                {
                    ClassDeliveryCodeID= CodeInfoStorage.CodeID,
                    UserID=UserID,
                    CodeType= CodeInfoStorage.Moment,
                };

                // Effettua la richiesta per aggiungere il prodotto al carrello
                var response2 = await client.PostAsJsonAsync("/api/ClassDeliveryCodes", payload);
                
                // Nascondi il codice
                await Navigation.PushAsync(new OrderCompletedPage(), false);

            }
            else
            {
                // Gestione degli errori
                CodiceRecezione.Text = "Errore nel confermare il ritiro del codice";
                Console.WriteLine($"Errore nella risposta del server: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            CodiceRecezione.Text = "Errore nel ritiro del codice";
            Console.WriteLine($"Errore durante il ritiro del codice: {ex.Message}");
        }
    }

    private async void OnNavigateToCartPage(object sender, EventArgs e)
    {
        // Navigazione alla HomePage
        await Navigation.PushAsync(new HomePage());
    }
    // Metodo per andare alla pagina OrderCompletedPage



    private async Task<List<OrderCollection>> FetchOrdersAsync()
    {
        try
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001")
            };

            var response = await client.GetAsync($"/api/Orders/ClassOrders/User/{UserID}");
            if (response.IsSuccessStatusCode)
            {
                var orders = await response.Content.ReadFromJsonAsync<List<OrderCollection>>();
                return orders ?? new List<OrderCollection>();
            }
            else
            {
                Console.WriteLine($"Errore nella risposta: {response.StatusCode}");
                return new List<OrderCollection>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero degli ordini: {ex.Message}");
            return new List<OrderCollection>();
        }
    }


    private async Task SetDefaultValues()
    {
        try
        {
            Console.WriteLine($"ENTRATO DENTRO USER ID {UserID}");
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001")
            };

            var response = await client.GetAsync($"/api/SchoolClasses/ByUser/{UserID}");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"JSON Response: {jsonResponse}");

            if (response.IsSuccessStatusCode)
            {
                var dataList = await response.Content.ReadFromJsonAsync<List<ClassYearSection>>();
                if (dataList != null && dataList.Any())
                {
                    var data = dataList.First();
                    Console.WriteLine($"BASE VALUE {data.classSection}, {data.classYear}");
                    Class = data.classYear.ToString();
                    Section = data.classSection;

                    // Forza l'aggiornamento manuale
                    OnPropertyChanged(nameof(Class));
                    OnPropertyChanged(nameof(Section));
                    OnPropertyChanged(nameof(ClassSection)); // Se utilizzata nella UI
                }
                else
                {
                    Console.WriteLine("No data returned from API.");
                }
            }
            else
            {
                Console.WriteLine($"Errore nella risposta: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero della sessione: {ex.Message}");
        }
    }
    public class Ordine
    {
        public string Studente { get; set; }
        public List<Prodotto> Prodotti { get; set; }

    }

    public class Prodotto
    {
        public string Nome { get; set; }
        public int Quantità { get; set; }

        // Proprietà per il testo formattato
        public string QuantitàFormattata => $"   x{Quantità}";
    }


    public class ClassYearSection
    {
        public int classYear { get; set; }
        public string classSection { get; set; }
    }

    public class OrderCollection
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public List<OrderDetailCollection> Items { get; set; }
    }

    public class OrderDetailCollection
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }


    public class OrderItemCollection
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class DeliveryCodeResponse
    {
        public string Code { get; set; }
    }

    public class NomeCompleto
    {
        public string Nome { get; set; }
    }
    public class ResponseCheck
    {
        public string Ritirato { get; set; }
    }
    public class DeliveryCodeConfirmationResponse
    {
        public string RetrievedBy { get; set; } // Nome della persona che ha ritirato
        public string Message { get; set; }    // Eventuale messaggio di conferma
    }

public class CodeInfo
{
    public int CodeID { get; set; } // Nome della persona che ha ritirato
  public string Moment { get; set; } // Nome della persona che ha ritirato

    }
    public class LogInfo
    {
        public int logID { get; set; } // Nome della persona che ha ritirato
        public int classDeliveryCodeID { get; set; } // Nome della persona che ha ritirato
        public int userID { get; set; } // Nome della persona che ha ritirato
        public string codeType { get; set; } // Nome della persona che ha ritirato
        public string timestamp { get; set; } // Nome della persona che ha ritirato
  

}
}