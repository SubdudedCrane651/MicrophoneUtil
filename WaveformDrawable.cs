using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace MicrophoneUtil
{
    public class WaveformDrawable : IDrawable
    {
        public List<float> Samples { get; set; } = new();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Lime;
            canvas.StrokeSize = 2;

            if (Samples == null || Samples.Count == 0)
                return;

            float width = dirtyRect.Width;
            float height = dirtyRect.Height;

            float step = width / Samples.Count;
            float mid = height / 2;

            for (int i = 1; i < Samples.Count; i++)
            {
                float x1 = (i - 1) * step;
                float y1 = mid - (Samples[i - 1] * mid);

                float x2 = i * step;
                float y2 = mid - (Samples[i] * mid);

                canvas.DrawLine(x1, y1, x2, y2);
            }
        }
    }
}
