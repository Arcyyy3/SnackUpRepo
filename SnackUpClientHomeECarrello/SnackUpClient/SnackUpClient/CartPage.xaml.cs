using System;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Calendar;
using SnackUpClient.Models;
using Microsoft.Maui.ApplicationModel.Communication;

namespace SnackUpClient
{
    public partial class CartPage : ContentPage
    {
        public ObservableCollection<CartItemModel> CartItemCollection { get; set; }
        private readonly string BaseSource = "192.168.87.188";
        private int UserID;

        public CartPage(int userID)
        {
            InitializeComponent();
            UserID = userID;

            CartItemCollection = new ObservableCollection<CartItemModel>();

            BindingContext = this;
            LoadCartItems();
        }
        private async void LoadCartItems()
        {
            try
            {
                int sessionID = await GetActiveSessionId();
                if (sessionID == -1)
                {
                    await DisplayAlert("Errore", "Impossibile recuperare la sessione attiva.", "OK");
                    return;
                }
                var cartItems = await GetCartItems(sessionID);
                foreach (var item in cartItems)
                {
                    var productDetails = await GetProductDetails(item.ProductID);
                    if (productDetails != null)
                    {
                        var cartItem = new CartItemModel(UserID)
                        {
                            Name = productDetails.Name,
                            Price = item.Price,
                            Image = productDetails.PhotoLink,
                            Quantity = item.Quantity
                        };

                        // Collega l'evento OnQuantityChanged
                        cartItem.OnQuantityChanged += RecalculateCartTotal;

                        // Aggiungi l'oggetto alla collezione
                        CartItemCollection.Add(cartItem);
                    }
                }

                // Calcola il totale iniziale
                RecalculateCartTotal();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il caricamento del carrello: {ex.Message}");
                await DisplayAlert("Errore", "Si è verificato un problema durante il caricamento del carrello.", "OK");
            }
        }

        private void RecalculateCartTotal()
        {
            decimal total = 0;

            foreach (var item in CartItemCollection)
            {
                total += item.Price * item.Quantity; // Calcola il totale per ogni elemento
            }

            OriginalPriceLabelTOTALE.Text = $"{total:C}"; // Mostra il totale calcolato
        }

        public async Task GetTotal()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001"  )
            };

            var response = await client.GetAsync($"/api/CartItems/Total/{UserID}");
            if (response.IsSuccessStatusCode)
            {
                var session = await response.Content.ReadFromJsonAsync<Total>();
                OriginalPriceLabelTOTALE.Text = $"{session.items}";
            }
        }
        private async Task<int> GetActiveSessionId()
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

                var response = await client.GetAsync($"/api/ShoppingSessions/user/{UserID}");
                if (response.IsSuccessStatusCode)
                {
                    var session = await response.Content.ReadFromJsonAsync<ShoppingSession>();
                    return session?.SessionID ?? -1;
                }

                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero della sessione: {ex.Message}");
                return -1;
            }
        }

        private async Task<CartItemModelInside[]> GetCartItems(int sessionID)
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

                var response = await client.GetAsync($"/api/CartItems/{sessionID}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemModelInside[]>();
                }

                return Array.Empty<CartItemModelInside>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero degli elementi del carrello: {ex.Message}");
                return Array.Empty<CartItemModelInside>();
            }
        }

        private async Task<ProductDetails> GetProductDetails(int productID)
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

                var response = await client.GetAsync($"/api/Products/{productID}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ProductDetails>();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dei dettagli del prodotto: {ex.Message}");
                return null;
            }
        }

        private async void OnTrashClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.BindingContext is CartItemModel item)
            {
                // Rimuovi l'elemento dalla collezione locale
                CartItemCollection.Remove(item);
                RecalculateCartTotal();

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
                    // Presuppongo che il tuo endpoint rimuova l'elemento con un DELETE
                    var response = await client.DeleteAsync($"/api/CartItems/{Uri.EscapeDataString(item.Name)}/{UserID}");
                    if (!response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Errore", "Non è stato possibile rimuovere il prodotto dal carrello.", "OK");

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore durante la rimozione dell'elemento: {ex.Message}");
                    await DisplayAlert("Errore", "Si è verificato un problema durante la rimozione del prodotto.", "OK");
                }
            }
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    public class CartItemModel : BindableObject
    {
        private int _quantity;
        private bool _isParametersVisible;
        private bool _isDateErrorVisible;
        private bool _isTimeErrorVisible;
        private DateTime? _selectedDate;
        private readonly string BaseSource = "192.168.87.188";
        private int _userID; // Memorizza l'UserID
        public event Action OnQuantityChanged;

        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public bool IsParametersVisible
        {
            get => _isParametersVisible;
            set
            {
                _isParametersVisible = value;
                OnPropertyChanged();
            }
        }

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedDateText));
            }
        }

        public string SelectedDateText => SelectedDate?.ToString("dddd dd MMMM") ?? "Seleziona una data dal calendario";

        public bool IsDateErrorVisible
        {
            get => _isDateErrorVisible;
            set
            {
                _isDateErrorVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsTimeErrorVisible
        {
            get => _isTimeErrorVisible;
            set
            {
                _isTimeErrorVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToggleParametersCommand => new Command(() => IsParametersVisible = !IsParametersVisible);

        public ICommand ConfirmCommand => new Command(() =>
        {
            IsDateErrorVisible = SelectedDate == null;
            IsTimeErrorVisible = false; // Aggiorna la logica in base alla selezione dell'orario

            if (!IsDateErrorVisible && !IsTimeErrorVisible)
            {
                IsParametersVisible = false;
            }
        });
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }

        // Costruttore per accettare l'UserID
        public CartItemModel(int userID)
        {
            _userID = userID; // Memorizza l'UserID
            IncreaseQuantityCommand = new Command(async () => await IncreaseQuantity());
            DecreaseQuantityCommand = new Command(async () => await DecreaseQuantity());
        }
        public ICommand CancelCommand => new Command(() =>
        {
            IsParametersVisible = false;
            IsDateErrorVisible = false;
            IsTimeErrorVisible = false;
            SelectedDate = null;
        });


        public async Task<int> GetProductIDAsync(string name)
        {
            try
            {
                Console.WriteLine($"NOME:{name}");
                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using var client = new HttpClient(handler)
                {
                    BaseAddress = new Uri($"https://{BaseSource}:5001")
                };

                // Effettua la richiesta GET per ottenere l'ID del prodotto
                var response = await client.GetAsync($"/api/Products/ProductIDByName/{Uri.EscapeDataString(name)}");
                if (response.IsSuccessStatusCode)
                {
                    // Leggi l'ID del prodotto dalla risposta
                    var productId = await response.Content.ReadFromJsonAsync<int>();
                    Console.WriteLine(productId);
                    return productId;
                }
                else
                {
                    Console.WriteLine("Entrato nel primo errore");
                    Console.WriteLine($"Errore nella richiesta: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dell'ID del prodotto per '{name}': {ex.Message}");
                return 0;
            }
        }
        
        private async Task IncreaseQuantity()
        {
            Quantity++;
            OnQuantityChanged?.Invoke(); // Notifica il cambiamento della quantità
            await UpdateCartItemQuantity();
        }

        private async Task DecreaseQuantity()
        {
            if (Quantity > 1)
            {
                Quantity--;
                OnQuantityChanged?.Invoke(); // Notifica il cambiamento della quantità
                await UpdateCartItemQuantity();
            }
            else
            {
                Console.WriteLine("La quantità non può essere inferiore a 1.");
            }
        }


        private async Task UpdateCartItemQuantity()
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
                var response1 = await client.GetAsync($"/api/CartItems/SessionIDByUserID{_userID}");

                int ProductID =await GetProductIDAsync(Name);
                Console.WriteLine($"Product ID: {ProductID}");
                Console.WriteLine($"USer ID: {_userID}");
                Console.WriteLine($"Quantiti ID: {Quantity}");
                int sessionID=await response1.Content.ReadFromJsonAsync<int>();
                var payload = new
                {
                    SessionID =sessionID ,// Passa l'UserID
                    ProductID,
                    Quantity
                };

                var response = await client.PutAsJsonAsync("/api/Cart/update-quantity", payload);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Quantità aggiornata con successo nel carrello.");
                }
                else
                {
                    Console.WriteLine($"Errore nell'aggiornamento della quantità: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'aggiornamento della quantità: {ex.Message}");
            }
        }
    
   


    }
    public class CartItemModelInside
    {
        public int CartItemID { get; set; }
        public int SessionID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ProductDetails
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Raccomandation { get; set; }
        public decimal Price { get; set; }
        public int ProducerID { get; set; }
        public string PhotoLink { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
    public class Total
    {
        public double items { get; set; }
    }
}
