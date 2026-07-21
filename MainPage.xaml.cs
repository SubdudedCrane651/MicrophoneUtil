using Android.Media;

namespace MicrophoneUtil;

public partial class MainPage : ContentPage
{
    private AudioRecord? _audioRecord;
    private bool _isRunning;

    public MainPage()
    {
        InitializeComponent();
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

        StartMicrophone();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

#if ANDROID
        var status = await Permissions.RequestAsync<Permissions.Microphone>();
        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Error", "Microphone permission denied.", "OK");
            return;
        }
#endif

        StartMicrophone();
    }

    protected override void OnDisappearing()
    {
        StopMicrophone();
        base.OnDisappearing();
    }

    private void StartMicrophone()
    {
        int sampleRate = 44100;

        int bufferSize = AudioRecord.GetMinBufferSize(
            sampleRate,
            ChannelIn.Mono,
            Encoding.Pcm16bit);

        _audioRecord = new AudioRecord(
            AudioSource.Mic,
            sampleRate,
            ChannelIn.Mono,
            Encoding.Pcm16bit,
            bufferSize);

        _audioRecord.StartRecording();
        _isRunning = true;

        Task.Run(() =>
        {
            var buffer = new byte[bufferSize];

            while (_isRunning)
            {
                int read = _audioRecord.Read(buffer, 0, buffer.Length);

                if (read > 0)
                {
                    // Example amplitude calculation
                    int sum = 0;
                    for (int i = 0; i < read; i += 2)
                    {
                        short sample = (short)((buffer[i + 1] << 8) | buffer[i]);
                        sum += Math.Abs(sample);
                    }
                    int amplitude = sum / (read / 2);

                    // TODO: update UI or store amplitude
                }
            }
        });
    }

    private void StopMicrophone()
    {
        _isRunning = false;
        _audioRecord?.Stop();
        _audioRecord?.Release();
        _audioRecord = null;
    }
}
