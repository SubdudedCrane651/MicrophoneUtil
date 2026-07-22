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
        DeviceDisplay.KeepScreenOn = true;
    }
    private void OnStartClicked(object sender, EventArgs e)
    {
#if ANDROID
        if (AudioGlobals.Loopback == null)
            AudioGlobals.Loopback = new LocalMicLoopback();

        AudioGlobals.Loopback.OnSamples += (samples) =>
        {
            waveform.Samples = samples.Select(s => s / 32768f).ToList();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                WaveformView.Invalidate();
            });
        };

        AudioGlobals.Loopback.Start();
#endif
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        waveform = new WaveformDrawable();
        WaveformView.Drawable = waveform;
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
#if ANDROID
        AudioGlobals.Loopback?.Stop();
#endif
    }
}
