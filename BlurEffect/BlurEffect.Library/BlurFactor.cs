using Android.Graphics;

namespace BlurEffect.Library
{
    public class BlurFactor
    {
        public static int DefaultRadius = 25;
        public static int DefaultSampling = 1;
        public Color Color = Color.Transparent;
        public int Height;
        public int Radius = DefaultRadius;
        public int Sampling = DefaultSampling;

        public int Width;
    }
}