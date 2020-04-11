using Android.Content;
using Android.Views;

namespace Blurry.Library
{
    public class BlurryHelper
    {

        private static readonly string Tag = typeof(BlurryHelper).Name;

        public static Composer With(Context context)
        {
            return new Composer(context);
        }

        public static void Delete(ViewGroup target)
        {
            var view = target.FindViewWithTag(Tag);
            if (view != null)
            {
                target.RemoveView(view);
            }
        }
    }
}