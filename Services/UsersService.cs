using System.Collections.Generic;
using SnackUpAPI.Models;
using SnackUpAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace SnackUpAPI.Services
{
    public class UserService
    {
        private readonly IDatabaseService _databaseService;

        public UserService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _databaseService.Query<User>(
                "SELECT * FROM Users WHERE Deleted IS NULL"
            );
        }
        public int SchoolClassIDByUserID(int userID)
        {
            return _databaseService.QuerySingle<int>(
                "SELECT SchoolClassID FROM Users WHERE UserID = @UserID",
                new { UserID = userID }
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
            var passwordService = new PasswordService();
            string hashedPassword = passwordService.HashPassword(user.Password);

            // Genera un token di verifica univoco (GUID)
            string verificationToken = Guid.NewGuid().ToString();

            // Inserisci l'utente nel database con `Verified = 0` e salva il token
            _databaseService.Execute(
                @"INSERT INTO Users 
          (UserName, Surname, Password, Email, Verified, VerificationToken, Role, RegistrationDate, SchoolClassID, Created, Modified, Deleted) 
          VALUES (@UserName, @Surname, @Password, @Email, 0, @VerificationToken, @Role, @RegistrationDate, @SchoolClassID, @Created, NULL, NULL)",
                new
                {
                    user.UserName,
                    user.Surname,
                    Password = hashedPassword,
                    user.Email,
                    VerificationToken = verificationToken, // Salviamo il token nel DB
                    user.Role,
                    user.RegistrationDate,
                    user.SchoolClassID,
                    Created = DateTime.UtcNow
                }
            );

            // Costruisci il link di verifica con il token
            string verificationLink = $"https://192.168.249.188:5001/api/Users/VerifyEmail?token={verificationToken}";

            // Invia l'email di verifica con AWS SES
            AmazonSESHelper.SendEmailAsync(user.Email, "Conferma la tua registrazione",
                $"Ciao {user.UserName},\n\nConferma la tua registrazione cliccando su questo link: {verificationLink}\n\nGrazie!");

            Console.WriteLine($"✅ Email di verifica inviata a {user.Email}");
        }


        public void UpdateUser(User user, int id)
        {
            _databaseService.Execute(
                @"UPDATE Users 
                  SET UserName = @UserName, Surname = @Surname, Password = @Password, Email = @Email, 
                      Role = @Role, RegistrationDate = @RegistrationDate, SchoolClassID = @SchoolClassID, 
                      Modified = @Modified
                  WHERE UserID = @UserID AND Deleted IS NULL",
                new
                {
                    user.UserName,
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
        public User GetUserByVerificationToken(string token)
        {
            return _databaseService.QuerySingleOrDefault<User>(
                "SELECT * FROM Users WHERE VerificationToken = @Token",
                new { Token = token }
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

            if (user.Verified == 0)
            {
                throw new UnauthorizedAccessException("Email non verificata. Controlla la tua casella di posta.");
            }

            var passwordService = new PasswordService();
            bool isPasswordValid = passwordService.VerifyPassword(password, user.Password);

            return isPasswordValid ? user : null;
        }

        public string GetUserName(int userID)
        {
            string query = @"
        SELECT 
            CONCAT(U.UserName, ' ', U.Surname) AS FullName
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
                "SELECT UserName FROM Users WHERE Email = @Email AND Deleted IS NULL",
                new { Email = email }
            );

            return user?.UserName;
        }

        public int? GetUserIdByEmail(string email)
        {
            var user = _databaseService.QuerySingle<User>(
                "SELECT UserID FROM Users WHERE Email = @Email AND Deleted IS NULL",
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
        public bool IsFirstStudentInClass(int schoolClassId)
        {
            // Conta il numero di studenti registrati per quella classe
            int studentCount = _databaseService.QuerySingle<int>(
                @"SELECT COUNT(*) 
          FROM Users 
          WHERE SchoolClassID = @SchoolClassID AND Deleted IS NULL",
                new { SchoolClassID = schoolClassId }
            );

            // Se il conteggio è 0, significa che non ci sono studenti nella classe
            return studentCount == 0;
        }

        public UserDetailsWithSchool GetUserDetailsWithSchool(int userID)
        {
            var query = @"
        SELECT 
            U.UserName AS Nome, 
            U.Surname AS Cognome, 
            U.Email,
            S.SchoolName,
            S.Address AS SchoolAddress,
            SC.ClassYear,
            SC.ClassSection
        FROM Users U
        LEFT JOIN SchoolClasses SC ON U.SchoolClassID = SC.SchoolClassID
        LEFT JOIN Schools S ON SC.SchoolID = S.SchoolID
        WHERE U.UserID = @UserID AND U.Deleted IS NULL";

            var userDetails = _databaseService.QuerySingleOrDefault<UserDetailsWithSchool>(query, new { UserID = userID });

            if (userDetails != null)
            {
                // Recupera tutti gli incaricati della stessa classe scolastica
                var incaricatiQuery = @"
            SELECT U.UserID, U.UserName AS Nome, U.Surname AS Cognome
            FROM Users U
            WHERE U.SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID)
            AND U.Role = 'MasterStudent' 
            AND U.Deleted IS NULL";

                userDetails.Incaricati = _databaseService.Query<Incaricato>(incaricatiQuery, new { UserID = userID }).ToList();
            }

            return userDetails;
        }

        public UserDetailsWithSchool GetUserDetailsWithSchoolForProfile(int userID)
        {
            var query = @"
        SELECT 
            U.UserName AS Nome, 
            U.Surname AS Cognome, 
            U.Email,
            S.SchoolName,
            S.Address AS SchoolAddress,
            SC.ClassYear,
            SC.ClassSection
        FROM Users U
        LEFT JOIN SchoolClasses SC ON U.SchoolClassID = SC.SchoolClassID
        LEFT JOIN Schools S ON SC.SchoolID = S.SchoolID
        WHERE U.UserID = @UserID AND U.Deleted IS NULL";

            var userDetails = _databaseService.QuerySingleOrDefault<UserDetailsWithSchool>(query, new { UserID = userID });

            if (userDetails != null)
            {
                // Recupera tutti gli incaricati della stessa classe scolastica
                var incaricatiQuery = @"
            SELECT U.UserName AS Nome, U.Surname AS Cognome
            FROM Users U
            WHERE U.SchoolClassID = (SELECT SchoolClassID FROM Users WHERE UserID = @UserID)
            AND U.Role = 'MasterStudent' 
            AND U.Deleted IS NULL";

                userDetails.Incaricati = _databaseService.Query<Incaricato>(incaricatiQuery, new { UserID = userID }).ToList();
            }

            return userDetails;
        }
        public void UpdatePersonalData(int id, string nome, string cognome, string email)
        {
            _databaseService.Execute(
                @"UPDATE Users 
          SET UserName = @UserName, Surname = @Surname, Email = @Email, 
              Modified = @Modified
          WHERE UserID = @UserID AND Deleted IS NULL",
                new
                {
                    UserName = nome,
                    Surname = cognome,
                    Email = email,
                    Modified = DateTime.UtcNow,
                    UserID = id
                }
            );
        }
        public void UpdateSchoolData(int id, int classYear, string classSection)
        {
            _databaseService.Execute(
                @"UPDATE Users 
          SET SchoolClassID = (SELECT SchoolClassID FROM SchoolClasses WHERE ClassYear = @ClassYear AND ClassSection = @ClassSection),
              Modified = @Modified
          WHERE UserID = @UserID AND Deleted IS NULL",
                new
                {
                    ClassYear = classYear,
                    ClassSection = classSection,
                    Modified = DateTime.UtcNow,
                    UserID = id
                }
            );
        }
        public void VerifyUser(string email)
        {
            _databaseService.Execute(
                @"UPDATE Users 
          SET Verified = 1 
          WHERE Email = @Email",
                new { Email = email }
            );
        }


        public class UserDetailsWithSchool
        {
            public string Nome { get; set; }
            public string Cognome { get; set; }
            public string Email { get; set; }
            public string SchoolName { get; set; }
            public string SchoolAddress { get; set; }
            public int ClassYear { get; set; }
            public string ClassSection { get; set; }
            public List<Incaricato> Incaricati { get; set; }
        }

        public class Incaricato
        {
            public string Nome { get; set; }
            public string Cognome { get; set; }
        }
        public class UpdatePersonalDataRequest
        {
            public string Nome { get; set; }
            public string Cognome { get; set; }
            public string Email { get; set; }
        }
        public class UpdateSchoolDataRequest
        {
            public int ClassYear { get; set; }
            public string ClassSection { get; set; }
        }

    }
}
