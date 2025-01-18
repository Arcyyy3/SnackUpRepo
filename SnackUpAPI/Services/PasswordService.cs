using BCrypt.Net;

namespace SnackUpAPI.Services
{
    public class PasswordService
    {
        // Metodo per generare l'hash della password
        public string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        // Metodo per verificare la password inserita con l'hash salvato
        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
