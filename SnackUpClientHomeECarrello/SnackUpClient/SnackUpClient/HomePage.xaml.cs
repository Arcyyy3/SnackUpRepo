using System.Collections.ObjectModel;
using Syncfusion.Maui.TabView;
using System.Text.Json;
using System.Text;
using Microsoft.Maui.Storage;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Collections.Immutable;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Windows.Input;
using SnackUpClient.Models;

namespace SnackUpClient;

public partial class HomePage : ContentPage
{
    private readonly HttpClient _httpClient;
    private readonly string BaseSource = "192.168.87.188";
    private bool isMenuVisible = false;
    private bool isExpanded = false;
    int MainUserID;
    public string UserName { get; set; }
    public ObservableCollection<FoodItem> FoodItems { get; set; }
    public ObservableCollection<FoodItem> FavoriteItems { get; set; }
    public ObservableCollection<FoodItem> NewFoodItems { get; set; }

    private readonly Easing customEasing = new Easing(t => 1 - Math.Pow(1 - t, 4)); // Easing personalizzato per rallentamento
    public ICommand AddToCartCommand { get; }
    public HomePage()
    {
        InitializeComponent();

        // Configura HttpClient per ignorare certificati non validi (solo per sviluppo)
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri($"https://{BaseSource}:5001") // Sostituisci con l'indirizzo corretto
        };
        //SALDO DISPONIBILE

        SaldoLabel.Text = "0,00";


        AddToCartCommand = new Command<FoodItemCart>(async (foodItemCart) =>
        {
            if (foodItemCart == null)
            {
                Debug.WriteLine("CommandParameter è null.");
            }
            else
            {
                Debug.WriteLine($"CommandParameter: {foodItemCart.ProductID}");
            }

            await OnAddToCart(foodItemCart);
        });

        BindingContext = this;
        Debug.WriteLine($"BindingContext corrente: {BindingContext}");


        // Avvia l'animazione quando la pagina è completamente caricata
        Loaded += async (s, e) =>
        {
            foreach (var child in FoodItemsContainer.Children)
            {
                if (child is View block)
                {
                    block.IsVisible = false; // Nascondi i blocchi
                    block.TranslationX = 300; // Posiziona fuori schermo a destra
                    block.Opacity = 0; // Rendi invisibile
                }
            }

            await CheckTokenAndRedirect();
            await LoadInitialProducts();
            await ConfigureMenu();
            // Avvia l'animazione
            await AnimateBlocks(FoodItemsContainer, 200, 800, 100);
            MainUserID = await GetUserIDAsync();
        };

    }

    //COLLEGAMENTI MENU A ALTRE PAGINE COLLEGAMENTI MENU A ALTRE PAGINE COLLEGAMENTI MENU A ALTRE PAGINE COLLEGAMENTI MENU A ALTRE PAGINE
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
        await Navigation.PushAsync(new HomePage());
        InnerFrameSupport.BackgroundColor = Colors.Transparent;
    }
    private async void NavigateToSchoolAdminPage(object sender, EventArgs e)
    {
        InnerFrameSupport.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new SupportPage());
        InnerFrameSupport.BackgroundColor = Colors.Transparent;
    }
  private async void NavigateToCodiceRecezionePage1(object sender, EventArgs e)
    {
        Debug.WriteLine("NavigateToCodiceRecezionePage chiamato");
        InnerFrameSupport.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new OrderCollectionPage(MainUserID));// 
        var page = Navigation.NavigationStack.LastOrDefault() as OrderCollectionPage;
       
        InnerFrameSupport.BackgroundColor = Colors.Transparent;
    }

    //RIFERIMENTO PAGINA WALLET
    private async void NavigateToWalletPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WalletPage());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Avvia animazione dei blocchetti all'apertura della pagina
        await AnimateBlocks(FoodItemsContainer, 400, 800, 100); // Durata totale 400ms per ogni blocco
    }
    string AccessToken;

    private bool isAnimating = false; // Flag per evitare doppie animazioni
    private async Task AnimateBlocks(StackLayout container, uint durationPerBlock, int initialDelay = 800, int slightDelay = 50)
    {
        if (isAnimating) return; // Evita di avviare l'animazione se è già in corso
        isAnimating = true;

        // Aspetta prima di iniziare l'animazione
        await Task.Delay(initialDelay);

        // Lista di task per gestire le animazioni in parallelo
        var animationTasks = new List<Task>();

        for (int i = 0; i < container.Children.Count; i++)
        {
            if (container.Children[i] is View block)
            {
                block.IsVisible = false;
                block.TranslationX = 300; // Posiziona fuori schermo a destra
                block.Opacity = 0; // Rendi invisibile

                // Mostra il blocco con un piccolo ritardo
                block.IsVisible = true;

                // Avvia l'animazione con un ritardo leggero
                var slideIn = block.TranslateTo(-3, 0, (uint)(durationPerBlock * 0.7), Easing.CubicOut);
                var fadeIn = block.FadeTo(1, (uint)(durationPerBlock * 0.7), Easing.CubicIn);

                var animationTask = Task.WhenAll(slideIn, fadeIn)
                    .ContinueWith(async _ =>
                    {
                        // Fase 2: Rimbalza indietro
                        await block.TranslateTo(0, 0, (uint)(durationPerBlock * 0.3), Easing.CubicInOut);
                    });

                animationTasks.Add(animationTask);

                // Piccolo ritardo tra i blocchi
                await Task.Delay(slightDelay);
            }
        }

        // Aspetta il completamento di tutte le animazioni
        await Task.WhenAll(animationTasks);

        isAnimating = false; // Animazione completata
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


    private async void OnOverlayTapped(object sender, EventArgs e)
    {
        if (isMenuVisible)
        {
            await CloseMenu();
            isMenuVisible = false;
        }
    }

    private async Task CloseMenu()
    {
        var menuAnimation = MenuPanel.TranslateTo(-275, 0, 400, customEasing);
        var contentAnimation = MainContent.TranslateTo(0, 0, 400, customEasing);

        await Task.WhenAll(menuAnimation, contentAnimation);

        MenuPanel.IsVisible = false;
        Overlay.IsVisible = false;
    }

    private async void OnSwipeLeft(object sender, SwipedEventArgs e)
    {
        if (isMenuVisible)
        {
            // Chiudi il menu
            await CloseMenu();
            isMenuVisible = false;
        }
    }

    private void OnExpandClicked(object sender, EventArgs e)
    {
        isExpanded = !isExpanded;

        double expandedHeight = 250;
        double collapsedHeight = 0;
        uint animationDuration = 400;

        if (isExpanded)
        {
            FavoritesContent.IsVisible = true;
            AnimateHeight(FavoritesContent, collapsedHeight, expandedHeight, animationDuration);
        }
        else
        {
            AnimateHeight(FavoritesContent, expandedHeight, collapsedHeight, animationDuration);
            FavoritesContent.IsVisible = false;
        }

        if (sender is ImageButton button)
        {
            button.Rotation = isExpanded ? 180 : 0; // Rotazione senza animazione
        }
    }

    private void AnimateHeight(View view, double fromHeight, double toHeight, uint duration)
    {
        var animation = new Animation(progress =>
        {
            double currentHeight = fromHeight + (toHeight - fromHeight) * progress;
            view.HeightRequest = currentHeight;
        });

        animation.Commit(view, "HeightAnimation", length: duration, easing: Easing.CubicOut);
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            // Cambia il testo del bottone
            button.Text = "Aggiunto ✔";

            button.BackgroundColor = Colors.White;

            button.TextColor = Colors.Black;

            button.BorderColor = Colors.Black;

            button.BorderWidth = 1.5;
        }
    }
    private async Task CheckTokenAndRedirect()
    {
        try
        {
            // Recupera i token salvati
            var accessToken = await SecureStorage.GetAsync("accessToken");
            var refreshToken = await SecureStorage.GetAsync("refreshToken");

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                Console.WriteLine("Token mancanti. Reindirizzamento al login.");
                await Shell.Current.GoToAsync("//StartPage");
                return;
            }

            Console.WriteLine($" entrato con AccessToken: {accessToken}");
            AccessToken = accessToken;
            Console.WriteLine($"RefreshToken: {refreshToken}");

            if (IsJwtExpired(accessToken))
            {
                Console.WriteLine("AccessToken scaduto. Tentativo di rinnovo.");
                var isRefreshed = await RefreshAccessToken(refreshToken, accessToken);
                if (!isRefreshed)
                {
                    Console.WriteLine("Rinnovo del token fallito. Reindirizzamento al login.");
                    await Shell.Current.GoToAsync("//StartPage");
                }
            }

            var email = GetEmailFromToken(accessToken);

            if (!string.IsNullOrEmpty(email))
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

                    // Invia la richiesta GET per ottenere il nome dell'utente
                    var response = await client.GetAsync($"api/Users/NameEmail/{Uri.EscapeDataString(email)}");

                    // Verifica se la risposta è OK (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // Leggi il contenuto come stringa (assicurati che la risposta sia nel formato atteso)
                        string userName = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Nome utente recuperato: {userName}"); // Debug

                        // Imposta il nome utente
                        UserName = userName;
                        Console.WriteLine($"Nome utente aggiornato: {UserName}"); // Debug
                        OnPropertyChanged(nameof(UserName));  // Aggiungi questa riga

                        // Ora il nome utente è disponibile per l'interfaccia utente
                    }
                    else
                    {
                        // Gestisci gli errori del server
                        Console.WriteLine($"Errore nel recuperare il nome: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore durante la richiesta al server: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Email non trovata nel token.");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il controllo dei token: {ex.Message}");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
    private async Task LoadInitialProducts()
    {
        // Carica i prodotti della categoria "Salato"
        var salatoItems = await LoadProductsByCategory("Salato");
        var newFoodItemsContainer = this.FindByName<StackLayout>("NewFoodItemsContainer");
        if (newFoodItemsContainer != null)
        {
            BindableLayout.SetItemsSource(newFoodItemsContainer, salatoItems);
        }

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
            Console.WriteLine($"AccessToken: {AccessToken}");
            Console.WriteLine(GetEmailFromToken(AccessToken));

            // Richiesta per ottenere l'ID dell'utente
            var response = await client.GetAsync($"api/Users/UserIDEmail/{Uri.EscapeDataString(GetEmailFromToken(AccessToken))}");

            if (response.IsSuccessStatusCode)
            {
                // Leggi l'ID utente dalla risposta
                int userID = await response.Content.ReadFromJsonAsync<int>();

                // Carica i prodotti preferiti
                var favouritesItems = await LoadFavoritesProducts(userID);
                if (favouritesItems != null)
                {
                    FavoriteItems = new ObservableCollection<FoodItem>(favouritesItems);
                    OnPropertyChanged(nameof(FavoriteItems)); // Notifica il binding alla UI
                    Console.WriteLine($"Dati caricati nei preferiti: {FavoriteItems.Count}");
                }
            }
            else
            {
                Console.WriteLine($"Errore nel recuperare l'ID utente: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il caricamento dei prodotti: {ex.Message}");
        }
        var EvidenceItem = await LoadEvidenceProduct();
        if (EvidenceItem != null)
        {
            FoodItems = new ObservableCollection<FoodItem>(EvidenceItem);
            OnPropertyChanged(nameof(FoodItems)); // Notifica il binding alla UI
            Console.WriteLine($"Dati caricati nei Evidenza: {FoodItems.Count}");
        }
    }

    private async void OnTabSelectionChanged(object sender, EventArgs e)
    {
        if (sender is SfTabView tabView)
        {
            // Ottieni l'indice della scheda selezionata
            int selectedIndex = (int)tabView.SelectedIndex;
            Console.WriteLine($"Indice selezionato: {selectedIndex}");

            // Ottieni la scheda selezionata
            if (selectedIndex >= 0 && selectedIndex < tabView.Items.Count)
            {
                var selectedTab = tabView.Items[selectedIndex] as SfTabItem;
                if (selectedTab != null)
                {
                    Console.WriteLine($"Sezione cambiata in {selectedTab.Header}");

                    // Aggiorna il contenuto in base all'indice
                    await UpdateTabContent(selectedIndex);
                }
            }
        }
    }

    // Modifica del metodo per supportare async/await
    private async Task UpdateTabContent(int selectedIndex)
    {
        var newFoodItemsContainer = this.FindByName<StackLayout>("NewFoodItemsContainer");
        if (newFoodItemsContainer != null)
        {
            ObservableCollection<FoodItem> items = selectedIndex switch
            {
                0 => await GetSalatoItems(),
                1 => await GetDolceItems(),
                2 => await GetFitItems(),
                3 => await GetVeganItems(),
                4 => await GetGlutenFreeItems(),
                5 => await GetBevandeItems(),
                6 => await GetAltroItems(),
                _ => new ObservableCollection<FoodItem>(),
            };

            // Aggiorna la sorgente di elementi
            BindableLayout.SetItemsSource(newFoodItemsContainer, items);
        }
    }

    // Funzioni per ottenere i dati da API per ogni categoria
    private async Task<ObservableCollection<FoodItem>> GetSalatoItems()
    {
        return await LoadProductsByCategory("Salato");
    }

    private async Task<ObservableCollection<FoodItem>> GetDolceItems()
    {
        return await LoadProductsByCategory("Dolce");
    }

    private async Task<ObservableCollection<FoodItem>> GetFitItems()
    {
        return await LoadProductsByCategory("Fit");
    }

    private async Task<ObservableCollection<FoodItem>> GetVeganItems()
    {
        return await LoadProductsByCategory("Vegan");
    }

    private async Task<ObservableCollection<FoodItem>> GetGlutenFreeItems()
    {
        return await LoadProductsByCategory("Gluten Free");
    }

    private async Task<ObservableCollection<FoodItem>> GetBevandeItems()
    {
        return await LoadProductsByCategory("Bevande");
    }

    private async Task<ObservableCollection<FoodItem>> GetAltroItems()
    {
        return await LoadProductsByCategory("Altro");
    }

    // Metodo per caricare i prodotti dalla categoria API
    private async Task<ObservableCollection<FoodItem>> LoadProductsByCategory(string category)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Products/with-stock-byCategory?category={Uri.EscapeDataString(category)}");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
                if (products != null)
                {
                    Console.WriteLine("Entrato");
                    return new ObservableCollection<FoodItem>(
                        products.Select(p => new FoodItem
                        {
                            Image = p.PhotoLink,
                            Title = p.Name,
                            Description = $"{p.Description} - €{p.Price:F2}",
                            IsLowStock = p.IsLowStockBool // Usa la proprietà calcolata
                        }));
                }
            }
            Console.WriteLine($"Errore nel caricamento dei prodotti per categoria {category}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il caricamento dei prodotti per categoria {category}: {ex.Message}");
        }

        return new ObservableCollection<FoodItem>();
    }
    private async Task<ObservableCollection<FoodItem>> LoadFavoritesProducts(int UserID)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Products/preferred-products/{UserID}");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductDTOForFavoritesAndMostPurcased>>();
                if (products != null)
                {
                    Console.WriteLine("Entrato");
                    return new ObservableCollection<FoodItem>(
                        products.Select(p => new FoodItem
                        {
                            Image = p.PhotoLink,
                            Title = p.Name,
                            Description = $"€{p.Price:F2}",
                            IsLowStock = p.IsLowStockBool // Usa la proprietà calcolata
                        }));
                }
            }
            Console.WriteLine($"Errore nel caricamento dei prodotti per User {UserID}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il caricamento dei prodotti per USer {UserID}: {ex.Message}");
        }

        return new ObservableCollection<FoodItem>();
    }

    private async Task<ObservableCollection<FoodItem>> LoadEvidenceProduct()
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Products/most-purchased-products");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductDTOForFavoritesAndMostPurcased>>();
                if (products != null)
                {
                    Console.WriteLine("Entrato1");
                    return new ObservableCollection<FoodItem>(
                        products.Select(p => new FoodItem
                        {
                            Image = p.PhotoLink,
                            Title = p.Name,
                            Description = $"€{p.Price:F2}",
                            IsLowStock = p.IsLowStockBool // Usa la proprietà calcolata
                        }));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il caricamento dei prodotti in evidenza: {ex.Message}");
        }

        return new ObservableCollection<FoodItem>();
    }

    private string GetEmailFromToken(string token)
    {
        try
        {
            // Estrai il payload dal token
            var payload = token.Split('.')[1];
            var jsonPayload = Base64Decode(payload);

            // Decodifica il payload e ottieni l'email
            var email = JsonSerializer.Deserialize<JwtPayload>(jsonPayload)?.email;

            return email ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la decodifica del token: {ex.Message}");
            return string.Empty; // In caso di errore, restituisce una stringa vuota
        }
    }

    private bool IsJwtExpired(string token)
    {
        try
        {
            var payload = token.Split('.')[1];
            var jsonPayload = Base64Decode(payload);
            Console.WriteLine($"payload: {payload}");
            Console.WriteLine($"jsonPayload: {jsonPayload}");
            var exp = JsonSerializer.Deserialize<JwtPayload>(jsonPayload)?.exp;

            if (exp == null)
            {
                Console.WriteLine("entrato");
                return true;
            }

            var expiryDate = DateTimeOffset.FromUnixTimeSeconds((long)exp);
            Console.WriteLine($"Token scadenza: {expiryDate}, Ora corrente: {DateTimeOffset.UtcNow}");
            return expiryDate <= DateTimeOffset.UtcNow;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la verifica del token JWT: {ex.Message}");
            return true; // Se c'è un errore nel parsing, considera il token scaduto
        }
    }


    private async Task<bool> RefreshAccessToken(string refreshToken, string accessToken)
    {
        try
        {
            // Invia sia il refresh token che l'access token scaduto
            var requestData = new
            {
                Token = accessToken, // Include il token scaduto
                RefreshToken = refreshToken
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Users/RefreshToken", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Risultato del refresh token: {response.StatusCode}");
            Console.WriteLine($"Corpo della risposta: {responseContent}");

            if (response.IsSuccessStatusCode)
            {
                var tokenData = JsonSerializer.Deserialize<TokenResponse>(responseContent);

                if (tokenData != null && !string.IsNullOrEmpty(tokenData.AccessToken) && !string.IsNullOrEmpty(tokenData.RefreshToken))
                {
                    // Salva i nuovi token
                    await SecureStorage.SetAsync("accessToken", tokenData.AccessToken);
                    await SecureStorage.SetAsync("refreshToken", tokenData.RefreshToken);

                    Console.WriteLine("Token rinnovati con successo.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Errore: risposta del server non valida o token mancanti.");
                }
            }
            else
            {
                Console.WriteLine($"Errore del server durante il refresh: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il refresh del token: {ex.Message}");
        }

        return false;
    }

    private string Base64Decode(string base64Url)
    {
        try
        {
            // Converti Base64URL in Base64 standard
            string base64 = base64Url.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            var base64EncodedBytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Errore durante la decodifica Base64: {ex.Message}");
            return string.Empty;
        }
    }
    private string GetRoleFromToken(string token)
    {
        try
        {
            // Estrai il payload dal token
            var payload = token.Split('.')[1];
            var jsonPayload = Base64Decode(payload);

            // Decodifica il payload e ottieni il ruolo
            var role = JsonSerializer.Deserialize<JwtPayload>(jsonPayload)?.role;

            return role ?? string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la decodifica del token: {ex.Message}");
            return string.Empty; // In caso di errore, restituisce un ruolo vuoto
        }
    }
    private async Task ConfigureMenu()
    {
        try
        {
            // Recupera il token JWT
            var token = await SecureStorage.GetAsync("accessToken");
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token non trovato. Reindirizzamento al login.");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            // Ottieni il ruolo dal token
            var role = GetRoleFromToken(token);
            Console.WriteLine($"Ruolo utente: {role}");

            // Nascondi tutte le pagine specifiche all'inizio
            InnerFrameAdminEX.IsVisible = false;
            InnerFrameProducerEX.IsVisible = false;
            InnerFrameSchoolAdminEX.IsVisible = false;
            InnerFrameCodiceRecezioneEX.IsVisible = false;

            // Configura visibilità in base al ruolo
            switch (role)
            {
                case "Admin":
                    InnerFrameAdminEX.IsVisible = true;
                    InnerFrameProducerEX.IsVisible = true;
                    InnerFrameSchoolAdminEX.IsVisible = true;
                    InnerFrameCodiceRecezioneEX.IsVisible = true;
                    break;
                case "Producer":
                    InnerFrameProducerEX.IsVisible = true;
                    InnerFrameSchoolAdminEX.IsVisible = false;
                    InnerFrameAdminEX.IsVisible = false;
                    InnerFrameCodiceRecezioneEX.IsVisible = false;
                    break;
                case "SchoolAdmin":
                    InnerFrameSchoolAdminEX.IsVisible = true;
                    InnerFrameAdminEX.IsVisible = false;
                    InnerFrameProducerEX.IsVisible = false;
                    InnerFrameCodiceRecezioneEX.IsVisible = false;
                    break;
                case "MasterStudent":
                    InnerFrameCodiceRecezioneEX.IsVisible = true;
                    InnerFrameAdminEX.IsVisible = false;
                    InnerFrameProducerEX.IsVisible = false;
                    InnerFrameSchoolAdminEX.IsVisible = false;
                    break;
                default:
                    Console.WriteLine("Ruolo sconosciuto. Nessuna pagina supplementare visibile.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la configurazione del menu: {ex.Message}");
        }
    }

    private async void NavigateTo(string page)
    {
        try
        {
            await Shell.Current.GoToAsync($"//{page}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la navigazione: {ex.Message}");
        }
    }
    private async void OnAddToCartButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            var foodItem = button.BindingContext as FoodItem;

            if (foodItem != null)
            {
                try
                {
                    // Recupera ProductID e Price
                    var (productId, price) = await GetProductDetailsByName(foodItem.Title);

                    // Usa ProductID e Price per creare FoodItemCart
                    var foodItemCart = foodItem.ToFoodItemCart(productId, price);

                    // Aggiungi al carrello
                    await OnAddToCart(foodItemCart);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Errore: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Il BindingContext del pulsante non è un FoodItem valido.");
            }
        }
    }

    private async Task OnAddToCart(FoodItemCart foodItemCart)
    {
        // Log per verificare che il metodo venga chiamato
        Console.WriteLine("OnAddToCart invocato");

        // Verifica che il prodotto non sia nullo
        if (foodItemCart == null)
        {
            Console.WriteLine("Elemento FoodItemCart nullo.");
            return;
        }

        Console.WriteLine($"Aggiunta al carrello: ProductID={foodItemCart.ProductID}, Title={foodItemCart.Title}");

        try
        {
            // Recupera l'ID della sessione attiva
            var sessionId = await GetActiveSessionID();
            Console.WriteLine($"Session ID: {sessionId}");

            // Prepara la richiesta per il server
            var addToCartRequest = new AddToCartRequest
            {
                SessionID = sessionId,
                ProductID = foodItemCart.ProductID,
                Quantity = 1,
                Price = foodItemCart.Price // Assicurati che questo sia il prezzo corretto
            };

            // Invia la richiesta POST al server
            var response = await _httpClient.PostAsJsonAsync("api/Cart/add", addToCartRequest);

            if (response.IsSuccessStatusCode)
            {
                // Conferma l'aggiunta al carrello
                await DisplayAlert("Successo", "Prodotto aggiunto al carrello!", "OK");
            }
            else
            {
                // Gestione errori del server
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Errore nell'aggiunta al carrello: {errorMessage}");
                await DisplayAlert("Errore", "Non è stato possibile aggiungere il prodotto al carrello.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Gestione di eventuali eccezioni
            Console.WriteLine($"Errore durante l'aggiunta al carrello: {ex.Message}");
            await DisplayAlert("Errore", "Si è verificato un errore imprevisto.", "OK");
        }
    }

    private async Task<int> GetActiveSessionID()
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
            var response12 = await client.GetAsync($"api/Users/UserIDEmail/{Uri.EscapeDataString(GetEmailFromToken(AccessToken))}");
            int userID = await response12.Content.ReadFromJsonAsync<int>();
            var response = await _httpClient.GetAsync($"api/ShoppingSessions/active/{userID}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            else
            {
                throw new Exception("Impossibile recuperare l'ID della sessione.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero della sessione: {ex.Message}");
            throw;
        }
    }

    // Modello per i dati dei blocchi
    private async Task<(int ProductID, double Price)> GetProductDetailsByName(string productName)
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

            // Effettua la richiesta GET per ottenere i dettagli del prodotto
            var response = await client.GetAsync($"api/Products/details-by-name?name={Uri.EscapeDataString(productName)}");

            if (response.IsSuccessStatusCode)
            {
                // Leggi i dati restituiti come un oggetto anonimo
                var productDetails = await response.Content.ReadFromJsonAsync<ProductDetailsDto>();

                if (productDetails != null)
                {
                    return (productDetails.ProductID, productDetails.Price);
                }
                else
                {
                    throw new Exception("Dettagli del prodotto non trovati.");
                }
            }
            else
            {
                throw new Exception($"Errore nella richiesta: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero dei dettagli del prodotto: {ex.Message}");
            throw;
        }
    }
    int userIDExport;
    private async void OnProductTapped(object sender, EventArgs e)
    {
        Console.WriteLine("Entrato nel tapped");

        // Recupera l'oggetto FoodItem dal CommandParameter
        var tappedElement = sender as Element;
        var foodItem = tappedElement?.BindingContext as FoodItem;

        if (foodItem != null)
        {
            try
            {
                HttpClientHandler handler1 = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using var client1 = new HttpClient(handler1)
                {
                    BaseAddress = new Uri($"https://{BaseSource}:5001")
                };

                var response = await client1.GetAsync($"/api/Products/details-by-name?name={Uri.EscapeDataString(foodItem.Title)}");

                if (response.IsSuccessStatusCode)
                {
                    var productDetails = await response.Content.ReadFromJsonAsync<ProductDetailsDto>();
                    if (productDetails == null)
                    {
                        Console.WriteLine("Errore: Dettagli del prodotto nulli.");
                        return;
                    }

                    int productId = productDetails.ProductID;

                    HttpClientHandler handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                    };

                    using var client = new HttpClient(handler)
                    {
                        BaseAddress = new Uri($"https://{BaseSource}:5001")
                    };

                    var response1 = await client.GetAsync($"/api/Products/product-page/{productId}");

                    if (response1.IsSuccessStatusCode)
                    {
                        var productPageData = await response1.Content.ReadFromJsonAsync<ProductPageData>();
                        if (productPageData != null)
                        {
                            try
                            {
                                HttpClientHandler handler2 = new HttpClientHandler
                                {
                                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                                };

                                using var client2 = new HttpClient(handler2)
                                {
                                    BaseAddress = new Uri($"https://{BaseSource}:5001")
                                };
                                Console.WriteLine($"AccessToken: {AccessToken}");
                                Console.WriteLine(GetEmailFromToken(AccessToken));

                                // Richiesta per ottenere l'ID dell'utente
                                var response2 = await client2.GetAsync($"api/Users/UserIDEmail/{Uri.EscapeDataString(GetEmailFromToken(AccessToken))}");

                                if (response2.IsSuccessStatusCode)
                                {
                                    // Leggi l'ID utente dalla risposta
                                    userIDExport = await response2.Content.ReadFromJsonAsync<int>();

                                    // Carica i prodotti preferiti
                                    var favouritesItems = await LoadFavoritesProducts(userIDExport);
                                    if (favouritesItems != null)
                                    {
                                        FavoriteItems = new ObservableCollection<FoodItem>(favouritesItems);
                                        OnPropertyChanged(nameof(FavoriteItems)); // Notifica il binding alla UI
                                        Console.WriteLine($"Dati caricati nei preferiti: {FavoriteItems.Count}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Errore nel recuperare l'ID utente: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Errore durante il caricamento dei prodotti: {ex.Message}");
                            }
                            Console.WriteLine($" USER ID:{userIDExport}");
                            await Navigation.PushAsync(new ProductPage(productPageData, userIDExport));
                        }
                        else
                        {
                            Console.WriteLine("Errore: Dati del prodotto nulli.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Errore nella richiesta API: {response1.StatusCode}, {await response1.Content.ReadAsStringAsync()}");
                    }
                }
                else
                {
                    Console.WriteLine($"Errore nella richiesta API: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il caricamento del prodotto: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Errore: FoodItem null.");
        }
    }
    public async Task<int> GetUserIDAsync()
    {
        int userIDOrder = 0;

        HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        using var client = new HttpClient(handler)
        {
            BaseAddress = new Uri($"https://{BaseSource}:5001")
        };

        try
        {
            Console.WriteLine($"AccessToken: {AccessToken}");
            Console.WriteLine(GetEmailFromToken(AccessToken));

            // Richiesta per ottenere l'ID dell'utente
            var response = await client.GetAsync($"api/Users/UserIDEmail/{Uri.EscapeDataString(GetEmailFromToken(AccessToken))}");

            // Verifica se la risposta è OK (status code 200)
            if (response.IsSuccessStatusCode)
            {
                userIDOrder = await response.Content.ReadFromJsonAsync<int>();
            }
            else
            {
                Console.WriteLine($"Errore nella richiesta: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la richiesta per ottenere UserID: {ex.Message}");
        }

        return userIDOrder;
    }

    private async void NavigateToOrderHistoryPage(object sender, EventArgs e)
    {
        InnerFrameHistory.BackgroundColor = Color.FromArgb("#FF7B21");
        await Navigation.PushAsync(new OrderHistoryPage(await GetUserIDAsync()));
        InnerFrameHistory.BackgroundColor = Colors.Transparent;
    }

    // Classe DTO per mappare la risposta dell'API
    public class ProductDetailsDto
    {
        public int ProductID { get; set; }
        public double Price { get; set; }
    }




    public class JwtPayload
    {
        public long? exp { get; set; }
        public string role { get; set; } // Aggiungi questa proprietà
        public string email { get; set; } // Aggiungi questa proprietà

    }

    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }


    // Classe per il modello restituito dall'API
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoLink { get; set; }
        public double Price { get; set; }
        public string CategoryName { get; set; }
        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }
        public int IsLowStock { get; set; } // Cambiato da bool a int

        // Proprietà calcolata per ottenere un valore booleano
        public bool IsLowStockBool => IsLowStock == 1;
    }

    public class ProductDTOForFavoritesAndMostPurcased
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoLink { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }
        public int IsLowStock { get; set; } // Cambiato da bool a int

        // Proprietà calcolata per ottenere un valore booleano
        public bool IsLowStockBool => IsLowStock == 1;
        public int TotalQuantity { get; set; } // Cambiato da bool a int

    }
    public class AddToCartRequest
    {
        public int SessionID { get; set; } // ID della sessione attiva
        public int ProductID { get; set; } // ID del prodotto
        public int Quantity { get; set; }  // Quantità del prodotto
        public double Price { get; set; } // Prezzo unitario del prodotto
    }


}
