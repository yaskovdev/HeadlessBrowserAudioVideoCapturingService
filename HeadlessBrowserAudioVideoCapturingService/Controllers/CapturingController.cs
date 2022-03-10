using CefSharp;
using CefSharp.DevTools.Page;
using CefSharp.OffScreen;
using HeadlessBrowserAudioVideoCapturingService.Model;
using HeadlessBrowserAudioVideoCapturingService.Services;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;

namespace HeadlessBrowserAudioVideoCapturingService.Controllers;

[ApiController]
[Route("/capturing")]
public class CapturingController
{
    private readonly string _workingDirectory;
    private readonly ILogger<CapturingController> _logger;

    public CapturingController(ILogger<CapturingController> logger)
    {
        _workingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "media");
        _logger = logger;
    }

    [HttpPost("/start")]
    public void Start([FromBody] StartCaptureRequest request)
    {
        AsyncContext.Run(async delegate
        {
            _logger.LogInformation("Started capturing");
            using var browser = new ChromiumWebBrowser("https://www.youtube.com/embed/WPTxkU38BKg?autoplay=1");
            browser.AudioHandler = new CustomAudioHandler(_workingDirectory);
            var initialLoadResponse = await browser.WaitForInitialLoadAsync();
            AssertSuccess(initialLoadResponse.Success,
                $"Page load failed with ErrorCode:{initialLoadResponse.ErrorCode}, HttpStatusCode:{initialLoadResponse.HttpStatusCode}");
            var devToolsClient = browser.GetDevToolsClient();
            var page = devToolsClient.Page;
            page.ScreencastFrame += async (_, args) =>
            {
                await page.ScreencastFrameAckAsync(args.SessionId);
                var path = Path.Combine(_workingDirectory, $"frame_{DateTime.UtcNow.Ticks}.png");
                await File.WriteAllBytesAsync(path, args.Data);
            };
            var response = await page.StartScreencastAsync(StartScreencastFormat.Png);
            AssertSuccess(response.Success, $"Cannot start screencast, DevTools response is {response.ResponseAsJsonString}");
            await Task.Delay(request.CaptureDurationMs);
            _logger.LogInformation("Finished capturing");
        });
    }

    private static void AssertSuccess(bool success, string errorMessage)
    {
        if (!success) throw new Exception(errorMessage);
    }
}
