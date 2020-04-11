using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;

namespace Blurry.Library
{
    public class BitmapComposer
    {

        private readonly Context _context;
        private readonly Bitmap _bitmap;
        private readonly BlurFactor _factor;
        private readonly bool _async;
        private readonly Action<BitmapDrawable> _onImageReady;

        public BitmapComposer(Context context, Bitmap bitmap, BlurFactor factor, bool async, Action<BitmapDrawable> onImageReady)
        {
            _context = context;
            _bitmap = bitmap;
            _factor = factor;
            _async = async;
            _onImageReady = onImageReady;
        }

        public void Into(ImageView target)
        {
            _factor.Width = _bitmap.Width;
            _factor.Height = _bitmap.Height;

            if (_async)
            {
                var task = new BlurTask(target.Context, _bitmap, _factor, (drawable) =>
                {
                    if (_onImageReady == null)
                    {
                        target.SetImageDrawable(drawable);
                    }
                    else
                    {
                        _onImageReady(drawable);
                    }

                });
                task.Execute();
            }
            else
            {
                Drawable drawable = new BitmapDrawable(_context.Resources, Blur.Of(target.Context, _bitmap, _factor));
                target.SetImageDrawable(drawable);
            }
        }
    }
}