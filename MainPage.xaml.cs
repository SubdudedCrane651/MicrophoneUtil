using MicrophoneUtil.Services;

namespace MicrophoneUtil;

public partial class MainPage : ContentPage
{
    private readonly IMicrophoneUtil _microphoneUtil;

    public MainPage(IMicrophoneUtil microphoneUtil)
    {
        InitializeComponent();
        _microphoneUtil = microphoneUtil;
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
#if ANDROID
        var status = await Permissions.RequestAsync<Permissions.Microphone>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Error", "Microphone permission denied.", "OK");
            return;
        }
#endif

        _microphoneUtil.Start();
    }


    private void OnStopClicked(object sender, EventArgs e)
    {
        _microphoneUtil.Stop();
    }
}
