using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Views.Animations;

namespace jp.wasabeef.blurry
{
    public class Helper
    {
        public static void SetBackground(View v, Drawable drawable)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
                v.Background = drawable;
            else
                v.SetBackgroundDrawable(drawable);
        }

        public static bool HasZero(params int[] args)
        {
            foreach (var num in args)
                if (num == 0)
                    return true;

            return false;
        }

        public static void Animate(View v, int duration)
        {
            var alpha = new AlphaAnimation(0f, 1f);
            alpha.Duration = duration;
            v.StartAnimation(alpha);
        }
    }
}