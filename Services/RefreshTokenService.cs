using System;
using System.Diagnostics;
using System.Security.Cryptography;
using SnackUpAPI.Services;
using SnackUpAPI.Data;

public class RefreshTokenService
{
    private readonly IDatabaseService _databaseService;
    private readonly int _refreshTokenExpiryInDays;

    public RefreshTokenService(IDatabaseService databaseService, int refreshTokenExpiryInDays)
    {
        _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        _refreshTokenExpiryInDays = refreshTokenExpiryInDays;
    }

    public string GenerateToken()
    {
        var randomBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }

    public void SaveRefreshToken(int userId, string token, DateTime expirationDate)
    {
        _databaseService.Execute(
            @"INSERT INTO RefreshTokens (UserId, Token, ExpirationDate) 
              VALUES (@UserId, @Token, @ExpirationDate)",
            new { UserId = userId, Token = token, ExpirationDate = expirationDate }
        );
    }

  public bool ValidateRefreshToken(int userId, string token)
{
    var refreshToken = _databaseService.QuerySingle<RefreshToken>(
        @"SELECT * FROM RefreshTokens 
          WHERE UserId = @UserId AND Token = @Token AND IsRevoked = 0 AND ExpirationDate > GETDATE()",
        new { UserId = userId, Token = token }
    );

    return refreshToken != null;
}



    public void RevokeRefreshToken(string token)
    {
        _databaseService.Execute(
            @"UPDATE RefreshTokens SET IsRevoked = 1 WHERE Token = @Token",
            new { Token = token }
        );
    }
}
