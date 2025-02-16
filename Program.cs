    using SnackUpAPI.Services;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SnackUpAPI.Data;
using Microsoft.OpenApi.Models;


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
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Inserisci il token JWT con il prefisso \"Bearer \". Esempio: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
}); 
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
builder.Services.AddScoped<UserService>();  // ✅ CORRETTO
builder.Services.AddScoped<AllergenService>();  // ✅ CORRETTO
builder.Services.AddScoped<SchoolClassService>();
builder.Services.AddScoped<SchoolService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderDetailService>();
builder.Services.AddScoped<OrderTrackingService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<ProducerService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<PromotionService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<SupportRequestService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CategoryProductService>();
builder.Services.AddScoped<ProductPromotionService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<InventoryHistoryService>();
builder.Services.AddScoped<ShoppingSessionService>();
builder.Services.AddScoped<CartItemService>();
builder.Services.AddScoped<ClassDeliveryCodeService>();
builder.Services.AddScoped<ClassDeliveryLogService>();
builder.Services.AddScoped<ProductAllergenService>();
builder.Services.AddScoped<BundleProductService>();
builder.Services.AddScoped<BundleItemService>();
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<WalletTransactionService>();
builder.Services.AddScoped<LoggerClientService>();
builder.Services.AddScoped<LoggerServerService>();
builder.Services.AddScoped<ProducerUsersService>();


builder.Services.AddScoped<IDatabaseService, DatabaseService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    return new DatabaseService(connectionString);
});

builder.Services.AddScoped<RefreshTokenService>(provider =>
{
    var databaseService = provider.GetRequiredService<IDatabaseService>();
    return new RefreshTokenService(databaseService, refreshTokenExpiryInDays);
});


builder.Services.AddScoped<TokenService>(provider =>
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
