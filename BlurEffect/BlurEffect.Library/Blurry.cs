using Android.Content;
using Android.Views;

namespace BlurEffect.Library
{
    public class Blurry
    {
        private static readonly string Tag = typeof(Blurry).Name;

        public static Composer With(Context context)
        {
            return new Composer(context);
        }

        public static void Delete(ViewGroup target)
        {
            var view = target.FindViewWithTag(Tag);
            if (view != null) target.RemoveView(view);
        }
    }
}