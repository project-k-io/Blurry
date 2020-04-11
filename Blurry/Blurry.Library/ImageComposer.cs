using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

namespace Blurry.Library
{
    public class ImageComposer
    {
        private readonly bool _async;
        private readonly View _capture;
        private readonly Context _context;
        private readonly BlurFactor _factor;
        private readonly Action<BitmapDrawable> _imageReady;

        public ImageComposer(Context context, View capture, BlurFactor factor, bool async, Action<BitmapDrawable> imageReady)
        {
            _context = context;
            _capture = capture;
            _factor = factor;
            _async = async;
            _imageReady = imageReady;
        }

        public void Into(ImageView target)
        {
            _factor.Width = _capture.MeasuredWidth;
            _factor.Height = _capture.MeasuredHeight;

            if (_async)
            {
                var task = new BlurTask(_capture, _factor, drawable =>
                {
                    {
                        if (_imageReady == null)
                            target.SetImageDrawable(drawable);
                        else
                            _imageReady(drawable);
                    }
                });
                task.Execute();
            }
            else
            {
                Drawable drawable = new BitmapDrawable(_context.Resources, Blur.Of(_capture, _factor));
                target.SetImageDrawable(drawable);
            }
        }
    }
}