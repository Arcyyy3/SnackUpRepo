using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;
using Syncfusion.Maui.TabView;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Core;
using SnackUpClient.Models;

namespace SnackUpClient;

public partial class OrderHistoryPage : ContentPage
{
    private bool isMenuVisible = false;
    private readonly Easing customEasing = new(t => 1 - Math.Pow(1 - t, 4));
    public ObservableCollection<OrdineHistory> OrdiniInCorso { get; set; } = new ObservableCollection<OrdineHistory>();
    public ObservableCollection<OrdineHistory> StoricoOrdini { get; set; } = new ObservableCollection<OrdineHistory>();
    public BadgeSettings BadgeSettings { get; set; }
    private readonly int UserID;

    public OrderHistoryPage(int userID)
    {
        UserID = userID;
        InitializeComponent();
        BindingContext = this;
        SaldoLabel.Text = "0,00";
        LoadOrders();
    }

    private async void LoadOrders()
    {
        try
        {
            using var client = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            })
            {
                BaseAddress = new Uri("https://192.168.87.188:5001")
            };

            var ordersResponse = await client.GetAsync($"api/Orders/GroupedByUser/{UserID}");
            if (!ordersResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching orders: {ordersResponse.StatusCode}");
                return;
            }

            var groupedOrders = await ordersResponse.Content.ReadFromJsonAsync<List<GroupedOrder>>();
            if (groupedOrders == null)
            {
                Console.WriteLine("No grouped orders returned.");
                return;
            }

            foreach (var group in groupedOrders)
            {
                Console.WriteLine($"Date: {group.Date}, Recreation: {group.Recreation}, Orders Count: {group.Orders.Count}");

                foreach (var order in group.Orders)
                {
                    Console.WriteLine($"  Order ID: {order.OrderID}, Total Price: {order.TotalPrice}");

                    var prodotti = order.Products.Select(p => new Prodotto
                    {
                        Nome = p.ProductName, // Usa "ProductName" dal JSON
                        Quantità = p.Quantity,
                        Prezzo = p.UnitPrice,
                        Img = p.PhotoLink // Mappa il campo PhotoLink dal JSON
                    }).ToList();

                    // Log prodotti
                    foreach (var prodotto in prodotti)
                    {
                        Console.WriteLine($"    Product: {prodotto.Nome}, Quantity: {prodotto.Quantità}, Price: {prodotto.Prezzo} , immagine{prodotto.Img}");
                    }

                    var ordine = new OrdineHistory
                    {
                        Giorno = group.Date.ToString("dddd, dd MMMM yyyy"),
                        Orario = group.Recreation switch
                        {
                            "First" => "Prima Ricreazione",
                            "Second" => "Seconda Ricreazione",
                            _ => "Altro"
                        },
                        Prodotti = prodotti
                    };

                    // Distinzione tra ordini in corso e storico ordini
                    if (group.Date >= DateTime.Today)
                        OrdiniInCorso.Add(ordine);
                    else
                        StoricoOrdini.Add(ordine);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading orders: {ex.Message}");
        }
    }



    private async void NavigateToPromoPage(object sender, EventArgs e)
    {
        InnerFramePromo.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new PromoPage());
        InnerFramePromo.BackgroundColor = Colors.Transparent;
    }
    private async void NavigateToPremiumPage(object sender, EventArgs e)
    {
        InnerFramePlus.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new PremiumPage());
        InnerFramePlus.BackgroundColor = Colors.Transparent;
    }
    private async void NavigateToSettingsPage(object sender, EventArgs e)
    {
        InnerFrameSettings.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new SettingsPage());
        InnerFrameSettings.BackgroundColor = Colors.Transparent;
    }
    private async void NavigateToSupportPage(object sender, EventArgs e)
    {
        InnerFrameSupport.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new SupportPage());
        InnerFrameSupport.BackgroundColor = Colors.Transparent;
    }
    private async void NavigateToAdminPage(object sender, EventArgs e)
    {
        InnerFrameSupport.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new SupportPage());
        InnerFrameSupport.BackgroundColor = Colors.Transparent;
    }
    private async void NavigateToProducerPage(object sender, EventArgs e)
    {
        InnerFrameSupport.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new SupportPage());
        InnerFrameSupport.BackgroundColor = Colors.Transparent;
    }
    private async void NavigateToSchoolAdminPage(object sender, EventArgs e)
    {
        InnerFrameSupport.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new SupportPage());
        InnerFrameSupport.BackgroundColor = Colors.Transparent;
    }
    private async void NavigateToCodiceRecezionePage(object sender, EventArgs e)
    {
        InnerFrameSupport.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new SupportPage());
        InnerFrameSupport.BackgroundColor = Colors.Transparent;
    }
    //RIFERIMENTO PAGINA WALLET
    private async void NavigateToWalletPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WalletPage());
    }


    private async void OnMenuClicked(object sender, EventArgs e)
    {
        if (!isMenuVisible)
        {
            Overlay.IsVisible = true;
            MenuPanel.IsVisible = true;

            var menuAnimation = MenuPanel.TranslateTo(0, 0, 400, customEasing);
            var fadeAnimation = MenuPanel.FadeTo(1, 400, customEasing);
            var contentAnimation = MainContent.TranslateTo(275, 0, 400, customEasing);

            await Task.WhenAll(menuAnimation, fadeAnimation, contentAnimation);
        }
        else
        {
            await CloseMenu();
        }

        isMenuVisible = !isMenuVisible;
    }

    private async Task CloseMenu()
    {
        var menuAnimation = MenuPanel.TranslateTo(-275, 0, 400, customEasing);
        var contentAnimation = MainContent.TranslateTo(0, 0, 400, customEasing);

        await Task.WhenAll(menuAnimation, contentAnimation);

        MenuPanel.IsVisible = false;
        Overlay.IsVisible = false;
    }

    private void OnSwipeLeft(object sender, SwipedEventArgs e)
    {
        if (isMenuVisible)
        {
            CloseMenu();
            isMenuVisible = false;
        }
    }

    private async void OnBorderTapped(object sender, EventArgs e)
    {
        if (sender is Border border)
        {
            Console.WriteLine($"BindingContext: {border.BindingContext?.GetType()}");

            if (border.BindingContext is OrdineHistory ordine)
            {
                await Navigation.PushAsync(new OrderPage(ordine));
            }
            else
            {
                Console.WriteLine("Errore: BindingContext non è di tipo Ordine.");
            }
        }
        else
        {
            Console.WriteLine("Errore: Il sender non è un Border.");
        }
    }



    private void OnOverlayTapped(object sender, EventArgs e)
    {
        // Azione da eseguire quando l'overlay viene cliccato
        if (isMenuVisible)
        {
            CloseMenu(); // Assicurati che `CloseMenu` sia definito correttamente
            isMenuVisible = false;
        }
    }


    public class GroupedOrder
    {
        public DateTime Date { get; set; }
        public string Recreation { get; set; }
        public List<OrderHistory> Orders { get; set; }
    }

    public class OrderHistory
    {
        public int OrderID { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ProductHistory> Products { get; set; }
    }

    public class ProductHistory
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public string PhotoLink { get; set; } // Cambiato per riflettere il nome del campo JSON
    }




}
