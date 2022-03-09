using CefSharp;
using CefSharp.Handler;
using CefSharp.Structs;

namespace HeadlessBrowserAudioVideoCapturingService.Services;

public class CustomAudioHandler : AudioHandler
{
    private const bool ProceedWithAudioStreamCapture = true;

    private int _numberOfChannels;

    private readonly FileStream _rawCapturedAudio;

    public CustomAudioHandler(string workingDirectory)
    {
        var dest = Path.Combine(workingDirectory, $"audio_{DateTime.UtcNow.Ticks}.pcm");
        _rawCapturedAudio = new FileStream(dest, FileMode.Create, FileAccess.Write);
    }

    protected override bool GetAudioParameters(IWebBrowser chromiumWebBrowser, IBrowser browser, ref AudioParameters parameters) =>
        ProceedWithAudioStreamCapture;

    protected override void OnAudioStreamStarted(IWebBrowser chromiumWebBrowser, IBrowser browser, AudioParameters parameters, int channels)
    {
        Console.WriteLine($"Audio stream started with channel layout {parameters.ChannelLayout}, number of channels {channels}");
        _numberOfChannels = channels;
    }

    protected override void OnAudioStreamPacket(IWebBrowser chromiumWebBrowser, IBrowser browser, IntPtr data, int numberOfFramesPerChannel, long pts)
    {
        unsafe
        {
            var channelData = (float**)data.ToPointer();
            var size = _numberOfChannels * numberOfFramesPerChannel * 4;
            var samples = new byte[size];
            fixed (byte* destByte = samples)
            {
                var dest = (float*)destByte;
                for (var i = 0; i < numberOfFramesPerChannel; i++)
                {
                    for (var c = 0; c < _numberOfChannels; c++)
                    {
                        *dest++ = channelData[c][i];
                    }
                }
            }
            _rawCapturedAudio.Write(samples, 0, samples.Length);
        }
    }

    protected override void Dispose(bool disposing)
    {
        _rawCapturedAudio.Close();
        base.Dispose(disposing);
    }
}
