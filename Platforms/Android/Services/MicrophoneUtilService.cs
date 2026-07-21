using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using System.Threading.Tasks;

namespace MicrophoneUtil.Platforms.Android.Services;

[Service(ForegroundServiceType = ForegroundService.TypeMicrophone, Exported = true)]
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

        // Change Android.Resource.Drawable.ic_mic to Resource.Drawable.IcMic
        return new Notification.Builder(this, channelId)
            .SetContentTitle("MicrophoneUtil")
            .SetContentText("Using microphone in the background…")
            .SetSmallIcon(Resource.Drawable.ic_mic)
            .Build();
    }

    private void StartMicrophone()
    {
        int sampleRate = 16000;
        int bufferSize = AudioRecord.GetMinBufferSize(
            sampleRate,
            ChannelIn.Mono,
            Encoding.Pcm16bit);

        _audioRecord = new AudioRecord(
            AudioSource.VoiceRecognition,
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

                    // TODO: send amplitude to MAUI
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
