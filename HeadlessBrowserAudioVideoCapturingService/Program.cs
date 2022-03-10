using CefSharp;
using CefSharp.OffScreen;
using HeadlessBrowserAudioVideoCapturingService.Services;

Console.WriteLine($"Before run by thread {Environment.CurrentManagedThreadId}");
Cef.EnableWaitForBrowsersToClose();
var settings = new CefSettings();
settings.CefCommandLineArgs.Add("autoplay-policy", "no-user-gesture-required");
settings.EnableAudio();
Cef.Initialize(settings);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

Console.WriteLine($"After shutdown by thread {Environment.CurrentManagedThreadId}");

Cef.WaitForBrowsersToClose();
Cef.Shutdown();
