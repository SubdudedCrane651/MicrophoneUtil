using Android.Media;

public class LocalMicLoopback
{
    private AudioRecord recorder;
    private AudioTrack player;
    private bool running;

    public event Action<short[]> OnSamples;

    public void Start()
    {
        int sampleRate = 16000;

        int bufferSize = AudioRecord.GetMinBufferSize(
            sampleRate,
            ChannelIn.Mono,
            Encoding.Pcm16bit
        );

        // ✔ FIX 1: Explicit namespace for System.IO.Stream
        // ✔ FIX 2: Use ChannelConfiguration.Mono for AudioTrack
        recorder = new AudioRecord(
            AudioSource.Mic,
            sampleRate,
            ChannelIn.Mono,
            Encoding.Pcm16bit,
            bufferSize
        );

        player = new AudioTrack(
            Android.Media.Stream.Music,          // ✔ FIXED ambiguity
            sampleRate,
            ChannelConfiguration.Mono,           // ✔ FIXED ChannelOut error
            Encoding.Pcm16bit,
            bufferSize,
            AudioTrackMode.Stream
        );

        recorder.StartRecording();
        player.Play();

        running = true;

        Task.Run(() =>
        {
            byte[] buffer = new byte[bufferSize];

            while (running)
            {
                int read = recorder.Read(buffer, 0, buffer.Length);

                if (read > 0)
                {
                    player.Write(buffer, 0, read);

                    short[] samples = new short[read / 2];
                    Buffer.BlockCopy(buffer, 0, samples, 0, read);
                    OnSamples?.Invoke(samples);
                }
            }
        });
    }

    public void Stop()
    {
        running = false;

        recorder?.Stop();
        player?.Stop();

        recorder?.Release();
        player?.Release();
    }
}

