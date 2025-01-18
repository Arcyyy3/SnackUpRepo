using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace SnackUpClient
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
            BindingContext = new ForgotPasswordViewModel();
        }
    }

    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private string _phoneNumber;
        private string _errorMessage;
        private string _codeErrorMessage;
        private bool _isFrameVisible;
        private bool _isButtonEnabled = true;
        private string _buttonText = "Manda richiesta";
        private string _buttonColor = "#FF7B21";
        private string _verificationCode;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsErrorVisible));
            }
        }

        public string CodeErrorMessage
        {
            get => _codeErrorMessage;
            set
            {
                _codeErrorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCodeErrorVisible));
            }
        }

        public bool IsErrorVisible => !string.IsNullOrEmpty(ErrorMessage);
        public bool IsCodeErrorVisible => !string.IsNullOrEmpty(CodeErrorMessage);

        public bool IsFrameVisible
        {
            get => _isFrameVisible;
            set
            {
                _isFrameVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                _isButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        public string ButtonColor
        {
            get => _buttonColor;
            set
            {
                _buttonColor = value;
                OnPropertyChanged();
            }
        }

        public string VerificationCode
        {
            get => _verificationCode;
            set
            {
                _verificationCode = value;
                OnPropertyChanged();
            }
        }

        public ICommand ValidateCommand { get; }
        public ICommand ConfirmCodeCommand { get; }

        public ForgotPasswordViewModel()
        {
            ValidateCommand = new Command(OnValidate);
            ConfirmCodeCommand = new Command(OnConfirmCode);

            // Precompilazione automatica con prefisso internazionale
            PhoneNumber = "+39"; // Prefisso italiano predefinito
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                ErrorMessage = "*Questo campo è obbligatorio.";
                return;
            }

            if (!IsValidPhoneNumber(PhoneNumber))
            {
                ErrorMessage = "*Formato non valido. Inserire un numero valido.";
                return;
            }

            ErrorMessage = null;

            // Disabilita il pulsante e cambia il testo
            IsButtonEnabled = false;
            ButtonText = "Controlla i tuoi messaggi";
            ButtonColor = "#E6E6E6";

            // Mostra il frame per l'inserimento del codice
            ShowCodeEntryFrame();
        }

        private void ShowCodeEntryFrame()
        {
            IsFrameVisible = true;
        }

        private async void OnConfirmCode()
        {
            if (string.IsNullOrWhiteSpace(VerificationCode) || VerificationCode.Length != 6)
            {
                CodeErrorMessage = "*Il codice deve essere di 6 cifre.";
                return;
            }

            // Logica per confermare il codice inserito
            CodeErrorMessage = null;

            // Navigazione alla pagina NewPasswordPage
            await Application.Current.MainPage.Navigation.PushAsync(new NewPasswordPage(), false);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Controlla se il numero è valido con prefisso internazionale
            return Regex.IsMatch(phoneNumber, "^\\+\\d{2,3}\\d{10}$");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
