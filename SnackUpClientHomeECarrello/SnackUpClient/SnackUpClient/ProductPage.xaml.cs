using SnackUpClient.Models;
using System.Net.Http.Json;

namespace SnackUpClient;

public partial class ProductPage : ContentPage
{
    private const double InitialScale = 1.0; // 100%
    private const double FinalScale = 1.5;   // 150%
    private int _remainingItems; // Numero massimo di pezzi rimanenti
    private int _currentQuantity = 1; // Quantità iniziale
    private const double BottomSheetHeight = 300; // Altezza del Bottom Sheet
    private double _startY; // Posizione iniziale dello swipe
    private TimeSpan remainingTime = TimeSpan.FromMinutes(30); // Imposta il tempo iniziale
    private System.Timers.Timer timer; // Timer per aggiornare il testo
    private readonly ProductPageData _productData; // Dati del prodotto
    private readonly HttpClient _httpClient;
    private readonly string BaseSource = "192.168.87.188";
    private int UserID;
    public ProductPage(ProductPageData productData,int userID)
    {
        InitializeComponent();
        _productData = productData;
        UserID=userID;
        _remainingItems = productData.RemainingItems;

        // Caricamento dinamico dei contenuti
        LoadContent();
        StartTimer();
        LoadProductDetails();
        LoadCategoriesAndAllergens();
        LoadTextDetails();

        // Aggiungi il gestore per l'evento Scrolled
        var scrollView = this.FindByName<ScrollView>("MainScrollView");
        if (scrollView != null)
        {
            scrollView.Scrolled += OnScrollViewScrolled;
        }

        BottomSheet.TranslationY = BottomSheetHeight;
    }
    private void OnToggleRecommendationsClicked(object sender, EventArgs e)
    {
        if (RecommendationsSection != null)
        {
            RecommendationsSection.IsVisible = !RecommendationsSection.IsVisible;

            // Scorri verso il basso se la sezione diventa visibile
            if (RecommendationsSection.IsVisible && MainScrollView != null)
            {
                MainScrollView.ScrollToAsync(RecommendationsSection, ScrollToPosition.Start, true);
            }

            // Ruota l'ImageButton
            if (sender is ImageButton button)
            {
                button.Rotation = RecommendationsSection.IsVisible ? 180 : 0; // Rotazione della freccia
                button.WidthRequest = 30; // Sostituisci con il valore specifico dal tuo XAML
                button.HeightRequest = 30;
                button.Padding = new Thickness(6); // Specifica il padding
            }
        }
    }
    private void LoadContent()
    {
        // Set the store image
        StoreImage.Source = _productData.StoreImage ?? "default_store_image.jpg";
        ProductTitleLabel.Text = _productData.ProductName;
        StoreTitleLabel.Text = _productData.StoreName;
        BottomSheetProductTitleLabel.Text = _productData.ProductName;
        // Set the product image
        ProductImage.Source = _productData.ProductImage ?? "default_product_image.jpg";
    }

    private void LoadProductDetails()
    {
        // Imposta il numero di pezzi rimanenti
        RemainingItemsLabel.Text = $"{_productData.RemainingItems} rimanenti";
        BottomSheetRemainingItemsLabel.Text = $"{_productData.RemainingItems} rimanenti";
        if (_productData.DiscountedPrice.HasValue && _productData.DiscountedPrice.Value < _productData.OriginalPrice)
        {
            // Mostra il prezzo originale e il prezzo scontato
            OriginalPriceLabel.IsVisible = true;
            OriginalPriceLabel.Text = $"{_productData.OriginalPrice:C}";
            OriginalPriceLabel.TextDecorations = TextDecorations.Strikethrough;

            DiscountedPriceLabel.IsVisible = true;
            DiscountedPriceLabel.Text = $"{_productData.DiscountedPrice.Value:C}";


            BottomSheetDiscountedPriceLabel.IsVisible = true;
            BottomSheetDiscountedPriceLabel.Text = $"{_productData.DiscountedPrice.Value:C}";
            
        }
        else
        {
            // Mostra solo il prezzo originale come prezzo effettivo
            OriginalPriceLabel.IsVisible = false;

            DiscountedPriceLabel.IsVisible = true;
            DiscountedPriceLabel.Text = $"{_productData.OriginalPrice:C}";
            // Mostra solo il prezzo originale come prezzo effettivo
            BottomSheetDiscountedPriceLabel.IsVisible = false;

            BottomSheetDiscountedPriceLabel.IsVisible = true;
            BottomSheetDiscountedPriceLabel.Text = $"{_productData.OriginalPrice:C}";
        }
    }

    private void LoadCategoriesAndAllergens()
    {
        // Categorie
        foreach (var category in _productData.Categories)
        {
            var newFrame = new Frame
            {
                CornerRadius = CategoryFrameTemplate.CornerRadius,
                BackgroundColor = CategoryFrameTemplate.BackgroundColor,
                BorderColor = CategoryFrameTemplate.BorderColor,
                Padding = CategoryFrameTemplate.Padding,
                Margin = CategoryFrameTemplate.Margin,
                HorizontalOptions = CategoryFrameTemplate.HorizontalOptions,
                Content = new Label
                {
                    Text = category,
                    TextColor = CategoryLabelTemplate.TextColor,
                    FontSize = CategoryLabelTemplate.FontSize,
                    HorizontalTextAlignment = CategoryLabelTemplate.HorizontalTextAlignment
                }
            };

            // Aggiunge il nuovo frame al FlexLayout
            CategoryFlexLayout.Children.Add(newFrame);
        }

        // Allergenici
        foreach (var allergen in _productData.Allergens)
        {
            var newFrame = new Frame
            {
                CornerRadius = AllergenFrameTemplate.CornerRadius,
                BackgroundColor = AllergenFrameTemplate.BackgroundColor,
                BorderColor = AllergenFrameTemplate.BorderColor,
                Padding = AllergenFrameTemplate.Padding,
                Margin = AllergenFrameTemplate.Margin,
                HorizontalOptions = AllergenFrameTemplate.HorizontalOptions,
                Content = new Label
                {
                    Text = allergen,
                    TextColor = AllergenLabelTemplate.TextColor,
                    FontSize = AllergenLabelTemplate.FontSize,
                    HorizontalTextAlignment = AllergenLabelTemplate.HorizontalTextAlignment
                }
            };

            // Aggiunge il nuovo frame al FlexLayout
            AllergensFlexLayout.Children.Add(newFrame);
        }

        // Nasconde i modelli di base
        CategoryFrameTemplate.IsVisible = false;
        AllergenFrameTemplate.IsVisible = false;
    }

    private void LoadTextDetails()
    {
        // Imposta il testo della descrizione del prodotto
        ProductDescriptionTextLabel.Text = _productData.Description;

        // Imposta il testo delle specifiche del prodotto
        SpecificationsTextLabel.Text = _productData.Details;

        // Imposta le raccomandazioni
        if (!string.IsNullOrWhiteSpace(_productData.Raccomandation))
        {
            RecommendationsSection.IsVisible = true;
            RecommendationsTextLabel.Text = _productData.Raccomandation;
        }
        else
        {
            RecommendationsSection.IsVisible = false;
        }
    }


    private void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
    {
        if (e.ScrollY >= 0)
        {
            double progress = Math.Min(1, e.ScrollY / 250); // Normalizza tra 0 e 1
            double scale = InitialScale - (progress * (InitialScale - FinalScale));

            var blackOverlay = this.FindByName<StackLayout>("BlackOverlay");
            if (blackOverlay != null)
            {
                blackOverlay.Scale = scale;
            }
        }
    }
    private async void OnAddToCartClicked(object sender, EventArgs e)
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

            // Ottieni o crea la sessione attiva
            int sessionId = await GetOrCreateActiveSessionId();

            if (sessionId == -1)
            {
                Console.WriteLine("Errore: Nessuna sessione attiva trovata o creata.");
                return;
            }

            // Prepara i dati del prodotto da aggiungere
            var payload = new
            {
                SessionID = sessionId,
                ProductID = _productData.ProductID,
                Quantity = _currentQuantity,
                Price = _productData.DiscountedPrice ?? _productData.OriginalPrice
            };

            // Effettua la richiesta per aggiungere il prodotto al carrello
            var response = await client.PostAsJsonAsync("/api/CartItems", payload);

            if (response.IsSuccessStatusCode)
            {
                // Mostra conferma visiva
                AddToCartButton.Text = "Aggiunto!";
                AddToCartButton.BackgroundColor = Color.FromArgb("#E6E6E6");
                AddToCartButton.BorderColor = Color.FromArgb("#E6E6E6");
                AddToCartButton.TextColor = Color.FromArgb("#FFFFFF");

                GoToCartButton.IsVisible = true;

                await Task.Delay(400);
                await AddToCartButton.FadeTo(0, 300);
                AddToCartButton.IsVisible = false;
                QuantitySelector.Opacity = 0;
                QuantitySelector.IsVisible = true;
                await QuantitySelector.FadeTo(1, 300);

                BottomSheet.IsVisible = true;
                await BottomSheet.TranslateTo(0, 0, 400, Easing.CubicOut);
            }
            else
            {
                Console.WriteLine($"Errore nell'aggiunta al carrello: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante l'aggiunta al carrello: {ex.Message}");
        }
    }

    private async void OnIncreaseQuantityClicked(object sender, EventArgs e)
    {
        if (_currentQuantity < _remainingItems)
        {
            _currentQuantity++;
            QuantityLabel.Text = _currentQuantity.ToString();

            // Aggiorna la quantità nel carrello
            await UpdateCartItemQuantity(_productData.ProductID, _currentQuantity);
        }
    }

    private async void OnDecreaseQuantityClicked(object sender, EventArgs e)
    {
        if (_currentQuantity > 1)
        {
            _currentQuantity--;
            QuantityLabel.Text = _currentQuantity.ToString();

            // Aggiorna la quantità nel carrello
            await UpdateCartItemQuantity(_productData.ProductID, _currentQuantity);
        }
        else
        {
            Console.WriteLine("La quantità non può essere inferiore a 1.");
        }
    }

    private async Task UpdateCartItemQuantity(int productId, int newQuantity)
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

            // Ottieni o crea la sessione attiva
            int sessionId = await GetOrCreateActiveSessionId();

            if (sessionId == -1)
            {
                Console.WriteLine("Errore: Nessuna sessione attiva trovata o creata.");
                return;
            }

            // Prepara i dati per l'aggiornamento
            var payload = new
            {
                SessionID = sessionId,
                ProductID = productId,
                Quantity = newQuantity
            };

            // Effettua la richiesta per aggiornare la quantità
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

    private async Task<int> GetOrCreateActiveSessionId()
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

            // Ottieni la sessione attiva per l'utente corrente
            var response = await client.GetAsync($"/api/ShoppingSessions/user/{UserID}");

            if (response.IsSuccessStatusCode)
            {
                var session = await response.Content.ReadFromJsonAsync<ShoppingSession>();
                if (session != null)
                {
                    return session.SessionID;
                }
            }

            Console.WriteLine($"Errore nel recupero o nella creazione della sessione: {response.StatusCode}");
            return -1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero o la creazione della sessione: {ex.Message}");
            return -1;
        }
    }

    // Metodo per ottenere l'ID utente corrente (da implementare secondo la tua logica)
    

    // Metodo chiamato quando si preme il pulsante di chiusura
    private async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        await CloseBottomSheetAsync();
    }

    // Metodo per chiudere il Bottom Sheet
    private async Task CloseBottomSheetAsync()
    {
        // Esegui animazione per nascondere il Bottom Sheet
        await BottomSheet.TranslateTo(0, 300, 400, Easing.SpringIn);
        BottomSheet.IsVisible = false; // Nascondi il Bottom Sheet
    }

    private async void OnNavigateToHomePage(object sender, EventArgs e)
    {
        // Navigazione alla HomePage
        await Navigation.PushAsync(new HomePage());
    }
    private async void OnNavigateToCartPage(object sender, EventArgs e)
    {
        // Navigazione alla HomePage
        await Navigation.PushAsync(new CartPage(UserID));
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Shell.SetTabBarIsVisible(this, false);
        CheckAndStartTimer(); // Controlla e avvia il timer se necessario
    }

    private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // Riduci il tempo rimanente di 1 secondo
        remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));

        // Aggiorna il testo della Label sul thread principale
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (remainingTime.TotalSeconds > 0)
            {
                TimerLabel.Text = $"Affrettati! Hai solo {remainingTime.Minutes:D2}:{remainingTime.Seconds:D2} minuti per ordinare!";
            }
            else
            {
                TimerLabel.Text = "Il tempo per ordinare è scaduto!";
                timer.Stop(); // Ferma il timer quando raggiunge 0
            }
        });
    }

    private void StartTimer()
    {
        // Inizializza il timer con un intervallo di 1 secondo
        timer = new System.Timers.Timer(1000);
        timer.Elapsed += OnTimerElapsed;
        timer.AutoReset = true;
        timer.Enabled = true;
    }
    private void CheckAndStartTimer()
    {
        // Definisci gli orari critici
        TimeSpan ricreazione1 = new TimeSpan(10, 0, 0); // 10:00 AM
        TimeSpan ricreazione2 = new TimeSpan(12, 0, 0); // 12:00 PM

        // Calcola i limiti di chiusura ordinazioni
        TimeSpan chiusuraOrdinazioni1 = ricreazione1.Subtract(TimeSpan.FromHours(1)); // 9:00 AM
        TimeSpan chiusuraOrdinazioni2 = ricreazione2.Subtract(TimeSpan.FromHours(1)); // 11:00 AM

        // Calcola il tempo attuale
        TimeSpan oraAttuale = DateTime.Now.TimeOfDay;
        Console.WriteLine($"Entrato{oraAttuale}");
        // Calcola il tempo rimanente
        if (oraAttuale >= chiusuraOrdinazioni1.Subtract(TimeSpan.FromMinutes(30)) && oraAttuale <= chiusuraOrdinazioni1)
        {
            // Siamo nella finestra per la prima ricreazione
            remainingTime = chiusuraOrdinazioni1.Subtract(oraAttuale);
            TimerLabel.IsVisible = true; // Mostra il timer
            FrameScadenza.IsVisible = true; // Nascondi il timer
            StartTimer(); // Avvia il timer
        }
        else if (oraAttuale >= chiusuraOrdinazioni2.Subtract(TimeSpan.FromMinutes(30)) && oraAttuale <= chiusuraOrdinazioni2)
        {
            // Siamo nella finestra per la seconda ricreazione
            remainingTime = chiusuraOrdinazioni2.Subtract(oraAttuale);
            TimerLabel.IsVisible = true; // Mostra il timer
            FrameScadenza.IsVisible = true; // Nascondi il timer
            StartTimer(); // Avvia il timer
        }
        else
        {
            Console.WriteLine("Entrato nell'else Timer");
            // Fuori dalle finestre critiche
            TimerLabel.IsVisible = false; // Nascondi il timer
            FrameScadenza.IsVisible = false; // Nascondi il timer
            if (timer != null)
            {
                timer.Stop();
            }
        }
    }

}
