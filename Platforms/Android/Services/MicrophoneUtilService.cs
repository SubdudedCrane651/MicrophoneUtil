using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using System.Threading.Tasks;

namespace MicrophoneUtil.Platforms.Android.Services;

[Service(Exported = true)]
public class MicrophoneUtilService : Service
{
    private AudioRecord? _audioRecord;
    private bool _isRunning;

    public override IBinder? OnBind(Intent? intent) => null;

    public override void OnCreate()
    {
        base.OnCreate();

        StartForeground(1, BuildNotification());
        StartMicrophone();
    }

    private Notification BuildNotification()
    {
        const string channelId = "mic_channel";

        var channel = new NotificationChannel(
            channelId,
            "Microphone Util",
            NotificationImportance.Low);

        var manager = (NotificationManager)GetSystemService(NotificationService);
        manager.CreateNotificationChannel(channel);

        return new Notification.Builder(this, channelId)
            .SetContentTitle("MicrophoneUtil")
            .SetContentText("Using microphone in the background…")
            .SetSmallIcon(Resource.Drawable.ic_mic)
            .Build();
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
                    // optional amplitude calc
                }
            }
        });
    }

    public override void OnDestroy()
    {
        _isRunning = false;
        _audioRecord?.Stop();
        _audioRecord?.Release();
        base.OnDestroy();
    }
}
