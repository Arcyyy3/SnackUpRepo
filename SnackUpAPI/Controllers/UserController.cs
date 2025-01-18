using Microsoft.AspNetCore.Mvc;
using SnackUpAPI.Models;
using SnackUpAPI.Services;
using System.Diagnostics;

[ApiController]
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
    public IActionResult AddUser([FromBody] User user, [FromServices] PasswordService passwordService)
    {
        try
        {
            if (user.Role == "Student" && user.SchoolClassID == null)
            {
                return BadRequest("SchoolClassID is required for students.");
            }

            user.Password = passwordService.HashPassword(user.Password); // Hash the password
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
    public IActionResult Login([FromBody] LoginRequest loginRequest, [FromServices] TokenService tokenService, [FromServices] RefreshTokenService refreshTokenService)
    {
        try
        {
            var user = _userService.AuthenticateUser(loginRequest.Email, loginRequest.Password);
            if (user == null) return Unauthorized("Invalid email or password."); // HTTP 401

            var jwtToken = tokenService.GenerateToken(user.Email, user.Role);
            var refreshToken = refreshTokenService.GenerateToken();
            refreshTokenService.SaveRefreshToken(user.UserID, refreshToken, DateTime.UtcNow.AddMonths(2));

            return Ok(new
            {
                Token = jwtToken,
                RefreshToken = refreshToken,
                User = new
                {
                    user.UserID,
                    user.Name,
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
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest("Both Token and RefreshToken are required."); // HTTP 400
            }

            var userId = _tokenService.GetUserIdFromExpiredToken(request.Token);
            if (userId == null) return Unauthorized("Invalid token."); // HTTP 401

            var isValid = _refreshTokenService.ValidateRefreshToken(userId.Value, request.RefreshToken);
            if (!isValid) return Unauthorized("Invalid or expired refresh token."); // HTTP 401

            var user = _userService.GetUserById(userId.Value);
            if (user == null) return Unauthorized("User not found."); // HTTP 401

            var newAccessToken = _tokenService.GenerateToken(user.Email, user.Role);
            var newRefreshToken = _refreshTokenService.GenerateToken();
            _refreshTokenService.SaveRefreshToken(userId.Value, newRefreshToken, DateTime.UtcNow.AddMonths(2));

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            }); // HTTP 200
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during token refresh: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }
    public class NomeCompleto()
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
    }

}
