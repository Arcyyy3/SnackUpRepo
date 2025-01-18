using System.Collections.Generic;
using SnackUpAPI.Models;

namespace SnackUpAPI.Services
{
    public class UserService
    {
        private readonly DatabaseService _databaseService;

        public UserService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _databaseService.Query<User>(
                "SELECT * FROM Users WHERE Deleted IS NULL"
            );
        }

        public User GetUserById(int id)
        {
            return _databaseService.QuerySingle<User>(
                "SELECT * FROM Users WHERE UserID = @UserID AND Deleted IS NULL",
                new { UserID = id }
            );
        }

        public void AddUser(User user)
        {
            _databaseService.Execute(
                @"INSERT INTO Users 
                  (Name, Surname, Password, Email, Role, RegistrationDate, SchoolClassID, Created, Modified, Deleted) 
                  VALUES (@Name, @Surname, @Password, @Email, @Role, @RegistrationDate, @SchoolClassID, @Created, NULL, NULL)",
                new
                {
                    user.Name,
                    user.Surname,
                    user.Password,
                    user.Email,
                    user.Role,
                    user.RegistrationDate,
                    user.SchoolClassID,
                    Created = DateTime.UtcNow // Imposta Created come la data e ora correnti
                }
            );
        }

        public void UpdateUser(User user, int id)
        {
            _databaseService.Execute(
                @"UPDATE Users 
                  SET Name = @Name, Surname = @Surname, Password = @Password, Email = @Email, 
                      Role = @Role, RegistrationDate = @RegistrationDate, SchoolClassID = @SchoolClassID, 
                      Modified = @Modified
                  WHERE UserID = @UserID AND Deleted IS NULL",
                new
                {
                    user.Name,
                    user.Surname,
                    user.Password,
                    user.Email,
                    user.Role,
                    user.RegistrationDate,
                    user.SchoolClassID,
                    Modified = DateTime.UtcNow, // Imposta Modified come la data e ora correnti
                    UserID = id
                }
            );
        }

        public void DeleteUser(int id)
        {
            _databaseService.Execute(
                @"UPDATE Users 
                  SET Deleted = @Deleted 
                  WHERE UserID = @UserID",
                new
                {
                    Deleted = DateTime.UtcNow, // Imposta Deleted come la data e ora correnti
                    UserID = id
                }
            );
        }

        public User AuthenticateUser(string email, string password)
        {
            var user = _databaseService.QuerySingle<User>(
                "SELECT * FROM Users WHERE Email = @Email AND Deleted IS NULL",
                new { Email = email }
            );

            if (user == null)
            {
                return null; // Utente non trovato
            }

            var passwordService = new PasswordService(); // Può anche essere iniettato
            bool isPasswordValid = passwordService.VerifyPassword(password, user.Password);

            return isPasswordValid ? user : null;
        }
        public string GetUserName(int userID)
        {
            string query = @"
        SELECT 
            CONCAT(U.Name, ' ', U.Surname) AS FullName
        FROM 
            Users AS U
        WHERE 
            U.UserID = @UserID 
            AND U.Deleted IS NULL";

            return _databaseService.QuerySingle<string>(query, new { UserID = userID });
        }

        public string GetNameByEmail(string email)
        {
            var user = _databaseService.QuerySingle<User>(
                "SELECT Name FROM Users WHERE Email = @Email AND Deleted IS NULL",
                new { Email = email }
            );

            return user?.Name;
        }

        public int? GetUserIdByEmail(string email)
        {
            var user = _databaseService.QuerySingle<User>(
                "SELECT USerID FROM Users WHERE Email = @Email AND Deleted IS NULL",
                new { Email = email }
            );

            return user?.UserID;
        }
        public int? GetSchoolClassIDByEmail(string email)
        {
            var user = _databaseService.QuerySingle<User>(
                "SELECT SchoolClassID FROM Users WHERE Email = @Email AND Deleted IS NULL",
                new { Email = email }
            );

            return user?.SchoolClassID;
        }
        public bool EmailExists(string email)
        {
            return _databaseService.QuerySingleOrDefault<bool>(
                "SELECT CASE WHEN EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND Deleted IS NULL) THEN 1 ELSE 0 END",
                new { Email = email }
            );
        }

    }
}
