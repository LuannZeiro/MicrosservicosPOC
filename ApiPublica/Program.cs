using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona HttpClient para consumir API pública
builder.Services.AddHttpClient("PublicApi", client =>
{
    client.BaseAddress = new Uri("https://api.publica.com/"); // substitua pelo endpoint real
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Adiciona serviços ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiPublica", Version = "v1" });
});

var app = builder.Build();

// Configura pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPublica v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
