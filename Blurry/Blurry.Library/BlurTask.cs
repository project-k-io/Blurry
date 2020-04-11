using System;
using System.Threading;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;

namespace Blurry.Library
{
    public class BlurTask
    {
        private readonly Resources _res;
        private readonly WeakReference<Context> _contextWeakRef;
        private readonly BlurFactor _factor;
        private readonly Bitmap _bitmap;
        private readonly Action<BitmapDrawable> _callback;

        public BlurTask(View target, BlurFactor factor, Action<BitmapDrawable> callback)
        {
            _res = target.Resources;
            _factor = factor;
            _callback = callback;
            _contextWeakRef = new WeakReference<Context>(target.Context);

            target.DrawingCacheEnabled = true;
            target.DestroyDrawingCache();
            target.DrawingCacheQuality = DrawingCacheQuality.Low;
            _bitmap = target.DrawingCache;
        }

        public BlurTask(Context context, Bitmap bitmap, BlurFactor factor, Action<BitmapDrawable> callback)
        {
            _res = context.Resources;
            _factor = factor;
            _callback = callback;
            _contextWeakRef = new WeakReference<Context>(context);

            _bitmap = bitmap;
        }

        public void Execute()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                if (!_contextWeakRef.TryGetTarget(out Context context))
                    return;

                var bitmapDrawable = new BitmapDrawable(_res, Blur.Of(context, _bitmap, _factor));

                if (_callback != null)
                {
                    new Handler(Looper.MainLooper).Post(() => { _callback(bitmapDrawable); });
                }
            });
        }
    }
}
