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

    private void OnStartClicked(object sender, EventArgs e)
    {
        _microphoneUtil.Start();
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
        _microphoneUtil.Stop();
    }
}
