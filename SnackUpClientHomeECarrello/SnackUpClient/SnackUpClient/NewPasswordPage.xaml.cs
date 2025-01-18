using System.ComponentModel;
using System.Windows.Input;

namespace SnackUpClient
{
    public partial class NewPasswordPage : ContentPage, INotifyPropertyChanged
    {
        private const string ErrorColor = "#FF7B21"; // Colore per i bordi in caso di errore
        private const string TransparentColor = "Transparent"; // Colore predefinito per i bordi

        // Variabili per password e conferma
        private string _password;
        private string _confirmPassword;
        private bool _isPasswordHidden = true;
        private bool _isConfirmPasswordHidden = true;

        // Stati degli errori
        private bool _isPasswordErrorVisible = false;
        private bool _isConfirmPasswordErrorVisible = false;

        // Implementazione dell'evento PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Proprietà per i campi di input
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

        // Proprietà per la visibilità degli errori
        public bool IsPasswordErrorVisible
        {
            get => _isPasswordErrorVisible;
            set
            {
                _isPasswordErrorVisible = value;
                OnPropertyChanged(nameof(IsPasswordErrorVisible));
            }
        }

        public bool IsConfirmPasswordErrorVisible
        {
            get => _isConfirmPasswordErrorVisible;
            set
            {
                _isConfirmPasswordErrorVisible = value;
                OnPropertyChanged(nameof(IsConfirmPasswordErrorVisible));
            }
        }

        // Colori per il bordo dei campi

        // Icone per mostrare/nascondere la password
        public string EyeIcon => IsPasswordHidden ? "eye_closed_icon.svg" : "eye_open_icon.svg";
        public string ConfirmEyeIcon => IsConfirmPasswordHidden ? "eye_closed_icon.svg" : "eye_open_icon.svg";

        // Comandi per alternare la visibilità delle password
        public ICommand TogglePasswordCommand => new Command(() =>
        {
            IsPasswordHidden = !IsPasswordHidden;
        });

        public ICommand ToggleConfirmPasswordCommand => new Command(() =>
        {
            IsConfirmPasswordHidden = !IsConfirmPasswordHidden;
        });

        // Comando per validare e aggiornare la password
        public ICommand ValidateCommand => new Command(async () =>
        {
            // Validazione dei campi
            IsPasswordErrorVisible = string.IsNullOrWhiteSpace(Password);
            IsConfirmPasswordErrorVisible = string.IsNullOrWhiteSpace(ConfirmPassword) || Password != ConfirmPassword;

            // Se i campi sono validi, naviga alla pagina di login
            if (!IsPasswordErrorVisible && !IsConfirmPasswordErrorVisible)
            {
                await Navigation.PushAsync(new PaginaLogin());
            }
        });

        // Costruttore
        public NewPasswordPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
