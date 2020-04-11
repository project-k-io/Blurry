
using Android.Graphics;

namespace Blurry.Library
{
    public class BlurFactor {

        public static int DefaultRadius = 25;
        public static int DefaultSampling = 1;

        public int Width;
        public int Height;
        public int Radius = DefaultRadius;
        public int Sampling = DefaultSampling;
        public Color Color = Android.Graphics.Color.Transparent;
    }
}
