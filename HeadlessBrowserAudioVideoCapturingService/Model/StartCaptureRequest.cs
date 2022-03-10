using System.ComponentModel;

namespace HeadlessBrowserAudioVideoCapturingService.Model;

public class StartCaptureRequest
{
    [DefaultValue(10000)]
    public int CaptureDurationMs { get; set; }
}
