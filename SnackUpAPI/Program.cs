using SnackUpAPI.Services;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// **Connessione al database**
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// **Certificato HTTPS**
var certPath = builder.Configuration["Certificate:Path"];
var certPassword = builder.Configuration["Certificate:Password"];

if (string.IsNullOrEmpty(certPath) || string.IsNullOrEmpty(certPassword))
{
    throw new InvalidOperationException("Certificato HTTPS non configurato correttamente.");
}

try
{
    var certificate = new X509Certificate2(certPath, certPassword);
    Debug.WriteLine($"Certificato caricato correttamente: {certificate.Subject}");
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ConfigureHttpsDefaults(httpsOptions =>
        {
            httpsOptions.ServerCertificate = certificate;
            httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
        });

        options.ListenAnyIP(5001, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    });
}
catch (Exception ex)
{
    throw new InvalidOperationException("Errore durante il caricamento del certificato HTTPS.", ex);
}

// **JWT Configuration**
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException("JWT Secret Key is not configured.");
}
var jwtKey = Encoding.UTF8.GetBytes(jwtSecretKey);

// Validazione RefreshTokenExpiryInDays
var refreshTokenExpiryInDays = int.TryParse(builder.Configuration["Jwt:RefreshTokenExpiryInDays"], out var expiryDays) ? expiryDays : 60;

// Configura l'autenticazione JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "SnackUpAPI",
        ValidAudience = "SnackUpClients",
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
    };
});

// **Aggiunta dei servizi**
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// **Dependency Injection (DI)**
builder.Services.AddSingleton(_ => new DatabaseService(connectionString));
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<SchoolClassService>();
builder.Services.AddSingleton<SchoolService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<OrderDetailService>();
builder.Services.AddSingleton<OrderTrackingService>();
builder.Services.AddSingleton<PaymentService>();
builder.Services.AddSingleton<ProducerService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<ReviewService>();
builder.Services.AddSingleton<PromotionService>();
builder.Services.AddSingleton<SubscriptionService>();
builder.Services.AddSingleton<SupportRequestService>();
builder.Services.AddSingleton<PasswordService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<CategoryProductService>();
builder.Services.AddSingleton<ProductPromotionService>();
builder.Services.AddSingleton<InventoryService>();
builder.Services.AddSingleton<InventoryHistoryService>();
builder.Services.AddSingleton<ShoppingSessionService>();
builder.Services.AddSingleton<CartItemService>();
builder.Services.AddSingleton<ClassDeliveryCodeService>();
builder.Services.AddSingleton<ClassDeliveryLogService>();
builder.Services.AddSingleton<AllergenService>();
builder.Services.AddSingleton<ProductAllergenService>();


builder.Services.AddSingleton<RefreshTokenService>(provider =>
{
    var databaseService = provider.GetRequiredService<DatabaseService>();
    return new RefreshTokenService(databaseService, refreshTokenExpiryInDays);
});
builder.Services.AddSingleton<TokenService>(provider =>
{
    var tokenExpiryInMinutes = int.TryParse(builder.Configuration["Jwt:TokenExpiryInMinutes"], out var expiry) ? expiry : 60;
    return new TokenService(jwtSecretKey, tokenExpiryInMinutes);
});

// **Applicazione**
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigins");
app.UseStaticFiles(); // Serve i file statici da wwwroot
app.MapControllers();

// Endpoint di test
app.MapGet("/", () => Results.Json(new { Message = "Welcome to SnackUp API" }));

// Avvio dell'applicazione
app.Run();
