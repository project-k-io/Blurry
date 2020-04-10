using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Blurry.Library.@internal;

namespace Blurry.Library
{
    public class Composer
    {
        private static readonly string Tag = typeof(Composer).Name;

        private readonly View _blurredView;
        private readonly Context _context;
        private readonly BlurFactor _factor;
        private bool _async;
        private bool _animate;
        private int _duration = 300;
        private Action<BitmapDrawable> _onImageReady;

        public Composer(Context context)
        {
            _context = context;
            _blurredView = new View(context) {Tag = Tag};
            _factor = new BlurFactor();
        }


        public Composer Radius(int radius)
        {
            _factor.Radius = radius;
            return this;
        }

        public Composer Sampling(int sampling)
        {
            _factor.Sampling = sampling;
            return this;
        }

        public Composer Color(int color)
        {
            _factor.Color = new Color(color);
            return this;
        }

        public Composer Async()
        {
            _async = true;
            return this;
        }

        public Composer Async(Action<BitmapDrawable> listener)
        {
            _async = true;
            _onImageReady = listener;
            return this;
        }

        public Composer Animate()
        {
            _animate = true;
            return this;
        }

        public Composer Animate(int duration)
        {
            _animate = true;
            _duration = duration;
            return this;
        }

        public ImageComposer Capture(View capture)
        {
            return new ImageComposer(_context, capture, _factor, _async, _onImageReady);
        }

        public BitmapComposer From(Bitmap bitmap)
        {
            return new BitmapComposer(_context, bitmap, _factor, _async, _onImageReady);
        }

        public void Onto(ViewGroup target)
        {
            _factor.Width = target.MeasuredWidth;
            _factor.Height = target.MeasuredHeight;

            if (_async)
            {
                var task = new BlurTask(target, _factor, (drawable) =>
                {
                    AddView(target, drawable);
                    if (_onImageReady != null)
                    {
                        _onImageReady(drawable);
                    }
                });

                task.Execute();
            }
            else
            {
                Drawable drawable = new BitmapDrawable(_context.Resources, Blur.Of(target, _factor));
                AddView(target, drawable);
            }
        }

        private void AddView(ViewGroup target, Drawable drawable)
        {
            Helper.SetBackground(_blurredView, drawable);
            target.AddView(_blurredView);

            if (_animate)
            {
                Helper.Animate(_blurredView, _duration);
            }
        }
    }
}