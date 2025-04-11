using Microsoft.IdentityModel.Tokens;
using prjFullStack;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .Build();

var connectionString = config.GetConnectionString("ClientesDB");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapGet("/clientes", () =>
{
    //var data = new Data(connectionString: connectionString);
    var clientes = Data.GetClientes(connectionString);

    /*
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    */
    return clientes;
})
.WithName("GetClientes");

app.MapPost("/login", (LoginModel login) => {
    if (login.Usuario == "admin" && login.Senha == "1234")
    {
        var claims = new[] { new Claim(ClaimTypes.Name, login.Usuario) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-mega-ultra-super-secret@@2025!"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: creds);
        return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
    return Results.Unauthorized();
});

app.Run();

class LoginModel
{
    public required string Usuario { get; set; }
    public required string Senha { get; set; }
}