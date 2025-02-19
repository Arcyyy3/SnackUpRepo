using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize]  // ✅ Protegge tutti gli endpoint di questo controller

[Route("api/Users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;
    private readonly RefreshTokenService _refreshTokenService;

    public UserController(UserService userService, TokenService tokenService, RefreshTokenService refreshTokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
    }
    [HttpGet("GetUserName/{userID}")]
    public IActionResult GetUserName(int userID)
    {
        try
        {
            var fullName = _userService.GetUserName(userID);
            return Ok(fullName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the user name.", Details = ex.Message });
        }
    }
    [HttpGet("GetSchoolClassIDFromUserID/{userID}")]
    public int SchoolClassIDByUserID(int userID)
    {
        try
        {
            int SchoolID = _userService.SchoolClassIDByUserID(userID);
            return SchoolID;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        try
        {
            var users = _userService.GetAllUsers();
            return Ok(users); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching users: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        try
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound($"User with ID {id} not found."); // HTTP 404
            return Ok(user); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("NameEmail/{email}")]
    public IActionResult GetNameByEmail(string email)
    {
        try
        {
            var name = _userService.GetNameByEmail(email);
            if (name == null) return NotFound($"No user found with email '{email}'."); // HTTP 404
            return Ok(name); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user name by email '{email}': {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [AllowAnonymous]
    [HttpGet("UserIDEmail/{email}")]
    public IActionResult GetUserIdByEmail(string email)
    {
        try
        {
            var name = _userService.GetUserIdByEmail(email);
            if (name == null) return NotFound($"No user found with email '{email}'."); // HTTP 404
            return Ok(name); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user UserID by email '{email}': {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [AllowAnonymous]
    [HttpGet("IsFirstStudentByClass/{schoolClassId}")]
    public IActionResult IsFirstStudentInClass(int schoolClassId)
    {
        try
        {
            bool isFirstStudent = _userService.IsFirstStudentInClass(schoolClassId);
            return Ok(isFirstStudent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while checking if the class is empty.", Details = ex.Message });
        }
    }



    [HttpGet("SchoolClassIDEmail/{email}")]
    public IActionResult GetSchoolClassIDByEmail(string email)
    {
        try
        {
            var schoolClassID = _userService.GetSchoolClassIDByEmail(email);
            if (schoolClassID == null) return NotFound($"No school class ID found for email '{email}'."); // HTTP 404
            return Ok(schoolClassID); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching school class ID by email '{email}': {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost]
    [AllowAnonymous] // ✅ Permette l'accesso senza token
    public IActionResult AddUser([FromBody] User user, [FromServices] PasswordService passwordService)
    {
        try
        {
            if (user.Role == "Student" && user.SchoolClassID == null)
            {
                return BadRequest("SchoolClassID is required for students.");
            }

            _userService.AddUser(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserID }, user); // HTTP 201
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding user: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser([FromBody] User user, int id)
    {
        try
        {
            user.UserID = id;
            _userService.UpdateUser(user, id);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        try
        {
            _userService.DeleteUser(id);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting user ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("Login")]
    [AllowAnonymous] // ✅ Permette l'accesso senza token
    public IActionResult Login([FromBody] LoginRequest loginRequest, [FromServices] TokenService tokenService, [FromServices] RefreshTokenService refreshTokenService)
    {
        try
        {
            var user = _userService.AuthenticateUser(loginRequest.Email, loginRequest.Password);
            if (user == null) return Unauthorized("Invalid email or password."); // HTTP 401

            var jwtToken = tokenService.GenerateToken(user.UserID, user.Email, user.Role);
            var refreshToken = refreshTokenService.GenerateToken();
            refreshTokenService.SaveRefreshToken(user.UserID, refreshToken, DateTime.UtcNow.AddMonths(2));

            return Ok(new
            {
                Token = jwtToken,
                RefreshToken = refreshToken,
                User = new
                {
                    user.UserID,
                    user.UserName,
                    user.Surname,
                    user.Email,
                    user.Role,
                    user.RegistrationDate
                }
            }); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("RefreshToken")]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            Console.WriteLine($"🔄 Tentativo di refresh con AccessToken: {request.AccessToken}");
            Debug.WriteLine($"🔄 Tentativo di refresh con RefreshToken: {request.RefreshToken}");

            if (string.IsNullOrEmpty(request.AccessToken) || string.IsNullOrEmpty(request.RefreshToken))
            {
                Debug.WriteLine("❌ Entrambi i token sono richiesti.");
                return BadRequest("Both Token and RefreshToken are required."); // HTTP 400
            }

            // Estrai UserID dal token scaduto
            var userId = _tokenService.GetUserIdFromExpiredToken(request.AccessToken);
            if (userId == null)
            {
               Debug.WriteLine("❌ Token non valido o non decodificabile.");
                return Unauthorized("Invalid token."); // HTTP 401
            }
            Console.WriteLine($"✅ UserID estratto dal Token: {userId}");

            // Controlla se il RefreshToken è valido
            var isValid = _refreshTokenService.ValidateRefreshToken(userId.Value, request.RefreshToken);
            if (!isValid)
            {
                Debug.WriteLine("❌ RefreshToken non valido o scaduto.");
                return Unauthorized("Invalid or expired refresh token."); // HTTP 401
            }
            Debug.WriteLine("✅ RefreshToken valido.");

            // Recupera l'utente dal database
            var user = _userService.GetUserById(userId.Value);
            if (user == null)
            {
                Console.WriteLine("❌ Utente non trovato.");
                return Unauthorized("User not found."); // HTTP 401
            }
            Debug.WriteLine($"✅ Utente trovato: {user.Email}");

            // Genera nuovo AccessToken
            var newAccessToken = _tokenService.GenerateToken(user.UserID, user.Email, user.Role);
            Debug.WriteLine($"✅ Nuovo AccessToken generato: {newAccessToken}");

            // Genera nuovo RefreshToken
            var newRefreshToken = _refreshTokenService.GenerateToken();
            Debug.WriteLine($"✅ Nuovo RefreshToken generato: {newRefreshToken}");

            // Salva il nuovo RefreshToken nel database
            _refreshTokenService.SaveRefreshToken(userId.Value, newRefreshToken, DateTime.UtcNow.AddMonths(2));
            Debug.WriteLine("✅ Nuovo RefreshToken salvato nel database.");

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            }); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Errore durante il refresh del token: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("UserWithSchool/{userID}")]
    public IActionResult GetUserDetailsWithSchoolForProfile(int userID)
    {
        try
        {
            var userDetails = _userService.GetUserDetailsWithSchoolForProfile(userID);
            if (userDetails == null)
                return NotFound(new { Message = "Utente non trovato o nessuna scuola associata." });

            return Ok(userDetails);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante il recupero delle informazioni utente: {ex.Message}");
            return StatusCode(500, "Errore interno del server.");
        }
    }
    [HttpPut("UpdatePersonalData/{id}")]
    public IActionResult UpdatePersonalData(int id, [FromBody] UpdatePersonalDataRequest request)
    {
        try
        {
            _userService.UpdatePersonalData(id, request.Nome, request.Cognome, request.Email);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating personal data for user ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    [HttpPut("UpdateSchoolData/{id}")]
    public IActionResult UpdateSchoolData(int id, [FromBody] UpdateSchoolDataRequest request)
    {
        try
        {
            _userService.UpdateSchoolData(id, request.ClassYear, request.ClassSection);
            return NoContent(); // HTTP 204
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating school data for user ID {id}: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }


    public class NomeCompleto()
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
