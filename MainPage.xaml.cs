using Android.Media;
using System.IO;

namespace MicrophoneUtil;

public partial class MainPage : ContentPage
{
    private AudioRecord? _audioRecord;
    private bool _isRunning;
    private LocalMicLoopback loopback;
    public WaveformDrawable Waveform => waveform;
    public GraphicsView WaveformViewControl => WaveformView;

    private WaveformDrawable waveform;

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnStartClicked(object sender, EventArgs e)
    {
        StartMicrophone();
    }
    protected override void OnDisappearing()
    {
        StopMicrophone();
        base.OnDisappearing();
    }

    private void StartMicrophone()
    {
#if ANDROID
        loopback = new LocalMicLoopback();

        loopback.OnSamples += (samples) =>
        {
            waveform.Samples = samples.Select(s => s / 32768f).ToList();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                WaveformView.Invalidate();
            });
        };

        loopback.Start();
#endif
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        string serverIp = "66.130.0.235";
        int port = 5002;

        waveform = new WaveformDrawable();
        WaveformView.Drawable = waveform;
    }

    private void StopMicrophone()
    {
        _isRunning = false;
        _audioRecord?.Stop();
        _audioRecord?.Release();
        _audioRecord = null;
    }
}
