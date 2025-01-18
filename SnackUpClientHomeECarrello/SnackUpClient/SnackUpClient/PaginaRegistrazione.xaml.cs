using Microsoft.Maui.Controls.Compatibility;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Net.Http;
using System.Windows.Input;
using System.Text.Json;


namespace SnackUpClient
{
    public partial class PaginaRegistrazione : ContentPage, INotifyPropertyChanged
    {private readonly HttpClient _httpClient;
        private readonly string BaseSource = "192.168.87.188";
        private const string ErrorColor = "#FF7B21"; // Costante per il colore degli errori
        private const string TransparentColor = "Transparent"; // Colore senza errori

        // Variabili private per i campi
        private bool _isPasswordHidden = true;
        private bool _isConfirmPasswordHidden = true;
        private string _name, _surname, _email, _confirmEmail, _password, _confirmPassword;
        private string _selectedCity, _selectedSchool, _selectedYear, _selectedSection;
        private List<string> _cities, _schools, _years, _sections;
        private bool _isFormValidated = false; // Indica se il modulo è stato convalidato

        // Proprietà calcolate per gli errori
        public bool IsNameErrorVisible => _isFormValidated && string.IsNullOrWhiteSpace(Name);
        public bool IsSurnameErrorVisible => _isFormValidated && string.IsNullOrWhiteSpace(Surname);
        public bool IsEmailErrorVisible => _isFormValidated && string.IsNullOrWhiteSpace(Email);
        public bool IsConfirmEmailErrorVisible => _isFormValidated && (string.IsNullOrWhiteSpace(ConfirmEmail) || Email != ConfirmEmail);
        public bool IsCityErrorVisible => _isFormValidated && string.IsNullOrWhiteSpace(SelectedCity);
        public bool IsSchoolErrorVisible => _isFormValidated && string.IsNullOrWhiteSpace(SelectedSchool);
        public bool IsYearErrorVisible => _isFormValidated && string.IsNullOrWhiteSpace(SelectedYear);
        public bool IsSectionErrorVisible => _isFormValidated && string.IsNullOrWhiteSpace(SelectedSection);
        public bool IsPasswordErrorVisible => _isFormValidated && string.IsNullOrWhiteSpace(Password);
        public bool IsConfirmPasswordErrorVisible => _isFormValidated && (string.IsNullOrWhiteSpace(ConfirmPassword) || Password != ConfirmPassword);


        public PaginaRegistrazione()
        {

            InitializeComponent();

            // Configura HttpClient per ignorare certificati non validi (solo per sviluppo!).
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001") // Modifica con l'indirizzo corretto
            };

            BindingContext = this; // Imposta il contesto dati della pagina
            LoadCities(); // Carica le città inizialmente
            LoadYear(); // Carica gli anni inizialmente
        }
        // Implementazione dell'evento PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Proprietà per i campi di input
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string ConfirmEmail
        {
            get => _confirmEmail;
            set
            {
                _confirmEmail = value;
                OnPropertyChanged(nameof(ConfirmEmail));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

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

        public bool IsConfirmPasswordHidden
        {
            get => _isConfirmPasswordHidden;
            set
            {
                _isConfirmPasswordHidden = value;
                OnPropertyChanged(nameof(IsConfirmPasswordHidden));
                OnPropertyChanged(nameof(ConfirmEyeIcon));
            }
        }

        // Icone per mostrare/nascondere la password
        public string EyeIcon => IsPasswordHidden ? "eye_closed_icon.svg" : "eye_open_icon.svg";
        public string ConfirmEyeIcon => IsConfirmPasswordHidden ? "eye_closed_icon.svg" : "eye_open_icon.svg";

        public ICommand TogglePasswordCommand => new Command(() =>
        {
            IsPasswordHidden = !IsPasswordHidden; // Alterna la visibilità della password
        });

        public ICommand ToggleConfirmPasswordCommand => new Command(() =>
        {
            IsConfirmPasswordHidden = !IsConfirmPasswordHidden; // Alterna la visibilità della conferma password
        });


        // Picker dinamici per Città, Scuola, Classe e Aula
        public List<string> Cities
        {
            get => _cities;
            set
            {
                _cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        public List<string> Schools
        {
            get => _schools;
            set
            {
                _schools = value;
                OnPropertyChanged(nameof(Schools));
            }
        }

              public List<string> Years { get; set; } = new List<string> { "1", "2", "3", "4", "5" }; // Lista statica per gli anni


        public List<string> Sections
        {
            get => _sections;
            set
            {
                _sections = value;
                OnPropertyChanged(nameof(Sections));
            }
        }

        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Debug.WriteLine("Città selezionata:", value);
                    _ = LoadSchools(Uri.EscapeDataString(value)); // Carica le scuole in base alla città selezionata
                }
            }
        }

        public string SelectedSchool
        {
            get => _selectedSchool;
            set
            {
                _selectedSchool = value;
                OnPropertyChanged(nameof(SelectedSchool));
                LoadYear();
            }
        }

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                if (int.TryParse(value, out var year) && !string.IsNullOrWhiteSpace(SelectedSchool))
                {
                    _ = LoadSections(year); // Aggiorna le stanze
                }
            }
        }

        public string SelectedSection
        {
            get => _selectedSection;
            set
            {
                _selectedSection = value;
                OnPropertyChanged(nameof(SelectedSection));
            }
        }

        public ICommand ValidateCommand => new Command(async () => await ValidateAndNavigate());


        // Validazione dei campi
        private async Task ValidateAndNavigate()
        {
            // Verifica che le caselle di controllo siano spuntate
            if (!PrivacyCheckBox.IsChecked || !TermsCheckBox.IsChecked)
            {
                await Application.Current.MainPage.DisplayAlert("Errore", "Devi accettare la Privacy Policy e i Termini e Condizioni per continuare.", "OK");
                return; // Interrompe l'esecuzione se le caselle non sono spuntate
            }

            _isFormValidated = true;

            // Notifica il cambiamento delle proprietà di errore
            OnPropertyChanged(nameof(IsPasswordErrorVisible));
            OnPropertyChanged(nameof(IsConfirmPasswordErrorVisible));
            OnPropertyChanged(nameof(IsSectionErrorVisible));
            OnPropertyChanged(nameof(IsNameErrorVisible));
            OnPropertyChanged(nameof(IsSurnameErrorVisible));
            OnPropertyChanged(nameof(IsEmailErrorVisible));
            OnPropertyChanged(nameof(IsConfirmEmailErrorVisible));
            OnPropertyChanged(nameof(IsCityErrorVisible));
            OnPropertyChanged(nameof(IsSchoolErrorVisible));
            OnPropertyChanged(nameof(IsYearErrorVisible));

            // Esegui la validazione globale
            if (IsPasswordErrorVisible || IsConfirmPasswordErrorVisible || IsSectionErrorVisible ||
                IsNameErrorVisible || IsSurnameErrorVisible || IsEmailErrorVisible ||
                IsConfirmEmailErrorVisible || IsCityErrorVisible || IsSchoolErrorVisible ||
                IsYearErrorVisible)
            {
                Debug.WriteLine("Errore nella validazione. Rivedere i campi.");
                await Application.Current.MainPage.DisplayAlert("Errore", "Rivedi i campi evidenziati.", "OK");
                return; // Interrompe l'esecuzione se ci sono errori
            }
            bool isRegistered = await RegisterUser();
            if (isRegistered)
            {
                Debug.WriteLine("Registrazione completata con successo!");
                await NavigateToLogin();
            }
            else
            {
                Debug.WriteLine("Errore durante la registrazione.");
                await Application.Current.MainPage.DisplayAlert("Errore", "Registrazione non riuscita. Riprova.", "OK");
            }
            // Se tutto è valido, vai al login
            Debug.WriteLine("Validazione completata con successo!");
          //  await NavigateToLogin();
        }

        // Logica per caricare i dati
        private async Task LoadCities()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Schools/CityList");
                if (response.IsSuccessStatusCode)
                {
                    var cities = await response.Content.ReadFromJsonAsync<List<string>>();
                    Cities = cities ?? new List<string>();
                }
                else
                {
                    Cities = new List<string>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore durante il caricamento delle città: {ex.Message}");
                Cities = new List<string>();
            }
            OnPropertyChanged(nameof(Cities));
        }


        private async Task LoadSchools(string city)
        {
            ResetPicker(2);
            if (string.IsNullOrEmpty(city)) return;

            try
            {
                var url = $"api/Schools/schools-in-city/{city}";
                Debug.WriteLine($"URL richiesto: {url}");

                var response = await _httpClient.GetAsync(url);

                Debug.WriteLine($"Stato della risposta: {response.StatusCode}");
                Debug.WriteLine($"Contenuto della risposta: {await response.Content.ReadAsStringAsync()}");

                if (response.IsSuccessStatusCode)
                {
                    var schools = await response.Content.ReadFromJsonAsync<List<string>>();
                    Schools = schools ?? new List<string>();
                }
                else
                {
                    Schools = new List<string>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore durante il caricamento delle scuole: {ex.Message}");
                Schools = new List<string>();
            }
            OnPropertyChanged(nameof(Schools));
        }
        // Proprietà per l'icona di Google (già esistente)
        public string GoogleIcon { get; set; } = "google_icon.svg";

        private void LoadYear()
        {
           // ResetPicker(3);
            

            Years = new List<string> { "1", "2", "3", "4", "5" };
            OnPropertyChanged(nameof(Years));


        }



        private async Task LoadSections(int year)
        {
            ResetPicker(4);

            if (string.IsNullOrWhiteSpace(SelectedCity) || string.IsNullOrWhiteSpace(SelectedSchool))
            {
                Debug.WriteLine("SelectedCity o SelectedSchool non validi.");
                Sections = new List<string>();
                OnPropertyChanged(nameof(Sections));
                return;
            }

            try
            {
                Debug.WriteLine($"SelectedYear: {SelectedYear}, SelectedSchool: {SelectedSchool}");

                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                using var client = new HttpClient(handler)
                {
                    BaseAddress = new Uri($"https://{BaseSource}:5001")
                };

                var schoolID = await client.GetFromJsonAsync<int>(
                    $"https://{BaseSource}:5001/api/Schools/address/{Uri.EscapeDataString(SelectedCity)}/name/{Uri.EscapeDataString(SelectedSchool)}");
                Debug.WriteLine($"Caricamento stanze per schoolID: {schoolID}, year: {year}");


                var response = await _httpClient.GetAsync($"api/SchoolClasses/{schoolID},{year}");
                Debug.WriteLine($"Caricamento rooms: {response}");
                if (response.IsSuccessStatusCode)
                {
                    var rooms = await response.Content.ReadFromJsonAsync<List<string>>();
                    Sections = rooms ?? new List<string>();
                }
                else
                {
                    Debug.WriteLine($"Errore durante il caricamento delle stanze: {response.StatusCode}");
                    Sections = new List<string>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore durante il caricamento delle stanze: {ex.Message}");
                Sections = new List<string>();
            }

            OnPropertyChanged(nameof(Sections));
        }
        private async Task<bool> RegisterUser()
        {
            Debug.WriteLine($"parametri {SelectedCity}{SelectedSchool}{SelectedYear}{SelectedSection}{Name}{Surname}{Password}{Email}");
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            using var client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001")
            };
            var SchoolID = await client.GetFromJsonAsync<int>(

                         $"https://{BaseSource}:5001/api/Schools/address/{Uri.EscapeDataString(SelectedCity)}/name/{Uri.EscapeDataString(SelectedSchool)}");
            Debug.WriteLine($"School id:{SchoolID}");
            HttpClientHandler handler1 = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            using var client1 = new HttpClient(handler1)
            {
                BaseAddress = new Uri($"https://{BaseSource}:5001")
            };
            var classID = await client1.GetFromJsonAsync<int>($"https://{BaseSource}:5001/api/SchoolClasses/register/{SchoolID}/{Uri.EscapeDataString(SelectedYear)}/{Uri.EscapeDataString(SelectedSection)}");
            Debug.WriteLine($"Class ID:{classID}");
            try
            {
                Debug.WriteLine($"parametri {Name}{Surname}{Password}{Email}{classID}");
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
                Debug.WriteLine($"Password hashata: {hashedPassword}");


                var newUser = new
                {
                    Name,
                    Surname,
                    Password= hashedPassword,
                    Email,
                    Role = "Student",
                    RegistrationDate = DateTime.UtcNow,
                    SchoolClassID = classID,
                };

                Debug.WriteLine(JsonSerializer.Serialize(newUser));

                Debug.WriteLine("=== DEBUG DEI CAMPI ===");
                Debug.WriteLine($"Name: {Name}");
                Debug.WriteLine($"Surname: {Surname}");
                Debug.WriteLine($"Email: {Email}");
                Debug.WriteLine($"Password: {Password}");
                Debug.WriteLine($"SchoolClassID: {SchoolID}");

                Debug.WriteLine("========================");
                Debug.WriteLine("Inviando i dati di registrazione...");
                Debug.WriteLine($"Received User: {JsonSerializer.Serialize(newUser)}");
                var response = await _httpClient.PostAsJsonAsync("api/Users", newUser);

                Debug.WriteLine($"Risultato della risposta: {response}");

                Debug.WriteLine($"Risultato della risposta: {response.StatusCode}");
                Debug.WriteLine($"Corpo della risposta: {await response.Content.ReadAsStringAsync()}");

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Successo", "Registrazione completata!", "OK");
                    return true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Errore", "Devi accettare la Privacy Policy e i Termini e Condizioni per continuare.", "OK");
                    return false; // Interrompe l'esecuzione se le caselle non sono spuntate
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Errore durante la registrazione: {ex.Message}");
                await DisplayAlert("Errore", "Errore di connessione. Controlla la tua rete.", "OK");
                return false;
            }
        }


        private void ResetAllPickers()
        {
            SelectedSchool = null;
            SelectedYear = null;
            SelectedSection = null;

            Schools = null; // Questo imposta automaticamente IsSchoolPickerEnabled su false
            Years = null;   // Questo imposta automaticamente IsClassPickerEnabled su false
            Sections = null; // Questo imposta automaticamente IsRoomPickerEnabled su false

            // Notifica i cambiamenti delle proprietà calcolate
            OnPropertyChanged(nameof(SelectedSchool));
            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(SelectedSection));
            OnPropertyChanged(nameof(Schools));
            OnPropertyChanged(nameof(Years));
            OnPropertyChanged(nameof(Sections));
        }

        private void ResetPicker(int level)
        {
            if (level <= 2)
            {
                SelectedSchool = null;
                Schools = null; // Questo disabilita automaticamente IsSchoolPickerEnabled
                OnPropertyChanged(nameof(SelectedSchool));
                OnPropertyChanged(nameof(Schools));
            }
            if (level <= 3)
            {
                SelectedYear = null;
                Years = null; // Questo disabilita automaticamente IsClassPickerEnabled
                OnPropertyChanged(nameof(SelectedYear));
                OnPropertyChanged(nameof(Years));
            }
            if (level <= 4)
            {
                SelectedSection = null;
                Sections = null; // Questo disabilita automaticamente IsRoomPickerEnabled
                OnPropertyChanged(nameof(SelectedSection));
                OnPropertyChanged(nameof(Sections));
            }
        }

        private async void OnNavigateToLogin(object sender, EventArgs e)
        {
            await NavigateToLogin();
        }
        private async Task NavigateToLogin()
        {
            await Navigation.PushAsync(new PaginaLogin());
        }


    }
}