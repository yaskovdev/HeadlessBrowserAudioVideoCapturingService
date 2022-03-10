using CefSharp;
using CefSharp.OffScreen;

namespace HeadlessBrowserAudioVideoCapturingService.Services;

public class CefLifecycleHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<CefLifecycleHostedService> _logger;

    public CefLifecycleHostedService(IHostApplicationLifetime lifetime, ILogger<CefLifecycleHostedService> logger)
    {
        _lifetime = lifetime;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _lifetime.ApplicationStarted.Register(OnStarted);
        _lifetime.ApplicationStopping.Register(OnStopping);
        _lifetime.ApplicationStopped.Register(OnStopped);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private void OnStarted()
    {
        _logger.LogInformation($"{nameof(OnStarted)} called in thread {Environment.CurrentManagedThreadId}");
        Cef.EnableWaitForBrowsersToClose();
        var settings = new CefSettings();
        settings.CefCommandLineArgs.Add("autoplay-policy", "no-user-gesture-required");
        settings.EnableAudio();
        Cef.Initialize(settings);
    }

    private void OnStopping()
    {
        _logger.LogInformation($"{nameof(OnStopping)} called in thread {Environment.CurrentManagedThreadId}");
        Cef.WaitForBrowsersToClose();
        Cef.Shutdown();
    }

    private void OnStopped()
    {
        _logger.LogInformation($"{nameof(OnStopped)} called in thread {Environment.CurrentManagedThreadId}");
    }
}
