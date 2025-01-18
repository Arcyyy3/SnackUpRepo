using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace SnackUpClient
{
    public partial class PaginaLogin : ContentPage
    {
        private readonly HttpClient _httpClient;
        private bool _isPasswordHidden = true;
        private string _email;
        private string _password;
        private bool _isEmailErrorVisible;
        private bool _isPasswordErrorVisible;
        private readonly string BaseSource = "192.168.87.188";

        public PaginaLogin()
        {
            InitializeComponent();

            // Configura HttpClient per ignorare certificati non validi (solo per sviluppo!).
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001") // IP del server
            };

            BindingContext = this; // Imposta il contesto dati della pagina.
        }

        // Proprietà per la visibilità della password.
        public bool IsPasswordHidden
        {
            get => _isPasswordHidden;
            set
            {
                _isPasswordHidden = value;
                OnPropertyChanged(nameof(IsPasswordHidden));
                OnPropertyChanged(nameof(EyeIcon));
            }
        }

        // Proprietà per cambiare l'icona dell'occhio.
        public string EyeIcon => IsPasswordHidden ? "eye_closed_icon.svg" : "eye_open_icon.svg";

        // Proprietà per il campo Email.
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        // Proprietà per il campo Password.
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        // Visibilità del messaggio di errore per Email.
        public bool IsEmailErrorVisible
        {
            get => _isEmailErrorVisible;
            set
            {
                _isEmailErrorVisible = value;
                OnPropertyChanged(nameof(IsEmailErrorVisible));
                OnPropertyChanged(nameof(EmailFrameColor));
            }
        }

        // Visibilità del messaggio di errore per Password.
        public bool IsPasswordErrorVisible
        {
            get => _isPasswordErrorVisible;
            set
            {
                _isPasswordErrorVisible = value;
                OnPropertyChanged(nameof(IsPasswordErrorVisible));
                OnPropertyChanged(nameof(PasswordFrameColor));
            }
        }

        // Colore del Frame per Email (rosso se errore, trasparente altrimenti)
        public string EmailFrameColor => IsEmailErrorVisible ? "Transparent" : "Transparent";

        // Colore del Frame per Password (rosso se errore, trasparente altrimenti)
        public string PasswordFrameColor => IsPasswordErrorVisible ? "Transparent" : "Transparent";


        // Comando per il toggle della password.
        public Command TogglePasswordCommand => new Command(() =>
        {
            IsPasswordHidden = !IsPasswordHidden;
        });

        // Comando per la validazione dei campi e invio delle credenziali.
        public Command ValidateCommand => new Command(async () =>
        {
            // Validazione del campo Email.
            IsEmailErrorVisible = string.IsNullOrWhiteSpace(Email);

            // Validazione del campo Password.
            IsPasswordErrorVisible = string.IsNullOrWhiteSpace(Password);

            if (IsEmailErrorVisible || IsPasswordErrorVisible)
            {
                Debug.WriteLine("Errore nei campi di input.");
                return;
            }

            // Costruzione del modello per l'API.
            var loginRequest = new
            {
                Email = Email,
                Password = Password
            };
            try
            {
                Debug.WriteLine($"Inviando la richiesta di login per l'email: {Email}");
                var response = await _httpClient.PostAsJsonAsync("api/Users/Login", loginRequest);

                Debug.WriteLine($"Risultato della risposta: {response.StatusCode}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Corpo della risposta: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    string token = null, refreshToken = null;

                    try
                    {
                        var jsonDocument = JsonDocument.Parse(responseContent);
                        var root = jsonDocument.RootElement;

                        token = root.GetProperty("token").GetString();
                        refreshToken = root.GetProperty("refreshToken").GetString();

                        Console.WriteLine($"Token: {token}");
                        Console.WriteLine($"RefreshToken: {refreshToken}");

                        var userElement = root.GetProperty("user");
                        var userId = userElement.GetProperty("userID").GetInt32();
                        var name = userElement.GetProperty("name").GetString();

                        Console.WriteLine($"UserID: {userId}, Name: {name}");
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"Errore durante l'accesso ai campi JSON: {jsonEx.Message}");
                        await DisplayAlert("Errore", "Risposta del server non valida. Riprova.", "OK");
                        return;
                    }

                    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
                    {
                        Debug.WriteLine("Token o RefreshToken vuoti.");
                        await DisplayAlert("Errore", "Impossibile salvare i token. Riprova.", "OK");
                        return;
                    }

                    await SecureStorage.SetAsync("accessToken", token);
                    await SecureStorage.SetAsync("refreshToken", refreshToken);

                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    Debug.WriteLine($"Errore del server: {response.StatusCode}, Dettagli: {responseContent}");
                    await DisplayAlert("Errore", "Credenziali non valide. Riprova.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore durante il login: {ex.Message}");
                await DisplayAlert("Errore", "Errore di connessione. Controlla la tua rete.", "OK");
            }
        });

        private async void OnNavigateToRegister(object sender, EventArgs e)
        {
            // Navigazione alla pagina di registrazione.
            await Navigation.PushAsync(new PaginaRegistrazione());
        }
    }

    // Modello per la risposta del login
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public UserResponse User { get; set; }
    }

    // Modello per l'utente nella risposta del login
    public class UserResponse
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
