using Android.App;
using Android.Content;
using Android.OS;

namespace MicrophoneUtil.Platforms.Android
{
    [Service(ForegroundServiceType = global::Android.Content.PM.ForegroundService.TypeMicrophone)]
    public class ForegroundMicService : Service
    {
        private LocalMicLoopback loopback;

        public override void OnCreate()
        {
            base.OnCreate();

            loopback = new LocalMicLoopback();
            loopback.Start();

            StartForeground(1, BuildNotification());
        }

        public override IBinder OnBind(Intent intent) => null;

        public override void OnDestroy()
        {
            loopback?.Stop();
            base.OnDestroy();
        }

        private Notification BuildNotification()
        {
            string channelId = "mic_loopback_channel";

            var channel = new NotificationChannel(
                channelId,
                "Microphone Loopback",
                NotificationImportance.Low);

            var manager = (NotificationManager)GetSystemService(NotificationService);
            manager.CreateNotificationChannel(channel);

            return new Notification.Builder(this, channelId)
                .SetContentTitle("Microphone running")
                .SetContentText("Bluetooth loopback active")
                .SetSmallIcon(global::Android.Resource.Drawable.IcButtonSpeakNow)
                .Build();
        }
    }
}
