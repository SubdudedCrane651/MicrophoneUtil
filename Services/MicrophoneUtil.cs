using MicrophoneUtil.Platforms.Android.Services;

namespace MicrophoneUtil.Services;

public interface IMicrophoneUtil
{
    void Start();
    void Stop();
}

public class MicrophoneUtil : IMicrophoneUtil
{
    public void Start()
    {
#if ANDROID
        var context = Android.App.Application.Context;
        var intent = new Android.Content.Intent(context, typeof(MicrophoneUtilService));
        context.StartForegroundService(intent);
#endif
    }

    public void Stop()
    {
#if ANDROID
        var context = Android.App.Application.Context;
        var intent = new Android.Content.Intent(context, typeof(MicrophoneUtilService));
        context.StopService(intent);
#endif
    }
}
