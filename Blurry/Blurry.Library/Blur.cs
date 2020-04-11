using System;
using Android.Annotation;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Renderscripts;
using Android.Views;

namespace Blurry.Library
{ 

    public class Blur
    {

        public static Bitmap Of(View view, BlurFactor factor)
        {
            view.DrawingCacheEnabled = true;
            view.DestroyDrawingCache();
            view.DrawingCacheQuality = DrawingCacheQuality.Low;
            var cache = view.DrawingCache;
            var bitmap = Of(view.Context, cache, factor);
            cache.Recycle();
            return bitmap;
        }

        public static Bitmap Of(Context context, Bitmap source, BlurFactor factor)
        {
            var width = factor.Width / factor.Sampling;
            var height = factor.Height / factor.Sampling;

            if (Helper.HasZero(width, height))
            {
                return null;
            }

            var bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);

            var canvas = new Canvas(bitmap);
            canvas.Scale(1 / (float) factor.Sampling, 1 / (float) factor.Sampling);
            var paint = new Paint();
            paint.Flags = PaintFlags.FilterBitmap | PaintFlags.AntiAlias;
            var filter = new PorterDuffColorFilter(factor.Color, PorterDuff.Mode.SrcAtop);
            paint.SetColorFilter(filter);
            canvas.DrawBitmap(source, 0, 0, paint);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                try
                {
                    bitmap = Rs(context, bitmap, factor.Radius);
                }
                catch (RSRuntimeException e)
                {
                    bitmap = Stack(bitmap, factor.Radius, true);
                }
            }
            else
            {
                bitmap = Stack(bitmap, factor.Radius, true);
            }

            if (factor.Sampling == BlurFactor.DefaultSampling)
            {
                return bitmap;
            }
            else
            {
                var scaled = Bitmap.CreateScaledBitmap(bitmap, factor.Width, factor.Height, true);
                bitmap.Recycle();
                return scaled;
            }
        }

        [TargetApi(Value = (int) BuildVersionCodes.JellyBeanMr2)]
        private static Bitmap Rs(Context context, Bitmap bitmap, int radius) //AK throws RSRuntimeException 
        {
            RenderScript rs = null;
            Allocation input = null;
            Allocation output = null;
            ScriptIntrinsicBlur blur = null;
            try
            {
                rs = RenderScript.Create(context);
                rs.MessageHandler = new RenderScript.RSMessageHandler();
                input = Allocation.CreateFromBitmap(rs, bitmap, Allocation.MipmapControl.MipmapNone, AllocationUsage.Script);
                output = Allocation.CreateTyped(rs, input.Type);
                blur = ScriptIntrinsicBlur.Create(rs, Element.U8_4(rs));

                blur.SetInput(input);
                blur.SetRadius(radius);
                blur.ForEach(output);
                output.CopyTo(bitmap);
            }
            finally
            {
                rs?.Destroy();

                input?.Destroy();

                output?.Destroy();

                blur?.Destroy();
            }

            return bitmap;
        }


        private static Bitmap Stack(Bitmap sentBitmap, int radius, bool canReuseInBitmap)
        {

            // Stack Blur v1.0 from
            // http://www.quasimondo.com/StackBlurForCanvas/StackBlurDemo.html
            //
            // Java Author: Mario Klingemann <mario at quasimondo.com>
            // http://incubator.quasimondo.com
            // created Feburary 29, 2004
            // Android port : Yahel Bouaziz <yahel at kayenko.com>
            // http://www.kayenko.com
            // ported april 5th, 2012

            // This is a compromise between Gaussian Blur and Box blur
            // It creates much better looking blurs than Box Blur, but is
            // 7x faster than my Gaussian Blur implementation.
            //
            // I called it Stack Blur because this describes best how this
            // filter works internally: it creates a kind of moving stack
            // of colors whilst scanning through the image. Thereby it
            // just has to add one new block of color to the right side
            // of the stack and remove the leftmost color. The remaining
            // colors on the topmost layer of the stack are either added on
            // or reduced by one, depending on if they are on the right or
            // on the left side of the stack.
            //
            // If you are using this algorithm in your code please add
            // the following line:
            //
            // Stack Blur Algorithm by Mario Klingemann <mario@quasimondo.com>

            Bitmap bitmap;
            if (canReuseInBitmap)
            {
                bitmap = sentBitmap;
            }
            else
            {
                bitmap = sentBitmap.Copy(sentBitmap.GetConfig(), true);
            }

            if (radius < 1)
            {
                return (null);
            }

            int w = bitmap.Width;
            int h = bitmap.Height;

            var pix = new int[w * h];
            bitmap.GetPixels(pix, 0, w, 0, 0, w, h);

            var wm = w - 1;
            var hm = h - 1;
            var wh = w * h;
            var div = radius + radius + 1;

            var r = new int[wh];
            var g = new int[wh];
            var b = new int[wh];
            int rsum, gsum, bsum, x, y, i, p, yp, yi, yw;
            var vmin = new int[Math.Max(w, h)];

            var divsum = (div + 1) >> 1;
            divsum *= divsum;
            var dv = new int[256 * divsum];
            for (i = 0; i < 256 * divsum; i++)
            {
                dv[i] = (i / divsum);
            }

            yw = yi = 0;

            var stack = new int[div][];
            for (var j = 0; j < stack.Length; j++)
                stack[i] = new int[3];

            int stackpointer;
            int stackstart;
            int[] sir;
            int rbs;
            var r1 = radius + 1;
            int routsum, goutsum, boutsum;
            int rinsum, ginsum, binsum;

            for (y = 0; y < h; y++)
            {
                rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
                for (i = -radius; i <= radius; i++)
                {
                    p = pix[yi + Math.Min(wm, Math.Max(i, 0))];
                    sir = stack[i + radius];
                    sir[0] = (p & 0xff0000) >> 16;
                    sir[1] = (p & 0x00ff00) >> 8;
                    sir[2] = (p & 0x0000ff);
                    rbs = r1 - Math.Abs(i);
                    rsum += sir[0] * rbs;
                    gsum += sir[1] * rbs;
                    bsum += sir[2] * rbs;
                    if (i > 0)
                    {
                        rinsum += sir[0];
                        ginsum += sir[1];
                        binsum += sir[2];
                    }
                    else
                    {
                        routsum += sir[0];
                        goutsum += sir[1];
                        boutsum += sir[2];
                    }
                }

                stackpointer = radius;

                for (x = 0; x < w; x++)
                {

                    r[yi] = dv[rsum];
                    g[yi] = dv[gsum];
                    b[yi] = dv[bsum];

                    rsum -= routsum;
                    gsum -= goutsum;
                    bsum -= boutsum;

                    stackstart = stackpointer - radius + div;
                    sir = stack[stackstart % div];

                    routsum -= sir[0];
                    goutsum -= sir[1];
                    boutsum -= sir[2];

                    if (y == 0)
                    {
                        vmin[x] = Math.Min(x + radius + 1, wm);
                    }

                    p = pix[yw + vmin[x]];

                    sir[0] = (p & 0xff0000) >> 16;
                    sir[1] = (p & 0x00ff00) >> 8;
                    sir[2] = (p & 0x0000ff);

                    rinsum += sir[0];
                    ginsum += sir[1];
                    binsum += sir[2];

                    rsum += rinsum;
                    gsum += ginsum;
                    bsum += binsum;

                    stackpointer = (stackpointer + 1) % div;
                    sir = stack[(stackpointer) % div];

                    routsum += sir[0];
                    goutsum += sir[1];
                    boutsum += sir[2];

                    rinsum -= sir[0];
                    ginsum -= sir[1];
                    binsum -= sir[2];

                    yi++;
                }

                yw += w;
            }

            for (x = 0; x < w; x++)
            {
                rinsum = ginsum = binsum = routsum = goutsum = boutsum = rsum = gsum = bsum = 0;
                yp = -radius * w;
                for (i = -radius; i <= radius; i++)
                {
                    yi = Math.Max(0, yp) + x;

                    sir = stack[i + radius];

                    sir[0] = r[yi];
                    sir[1] = g[yi];
                    sir[2] = b[yi];

                    rbs = r1 - Math.Abs(i);

                    rsum += r[yi] * rbs;
                    gsum += g[yi] * rbs;
                    bsum += b[yi] * rbs;

                    if (i > 0)
                    {
                        rinsum += sir[0];
                        ginsum += sir[1];
                        binsum += sir[2];
                    }
                    else
                    {
                        routsum += sir[0];
                        goutsum += sir[1];
                        boutsum += sir[2];
                    }

                    if (i < hm)
                    {
                        yp += w;
                    }
                }

                yi = x;
                stackpointer = radius;
                for (y = 0; y < h; y++)
                {
                    // Preserve alpha channel: ( 0xff000000 & pix[yi] )
                    pix[yi] = (int)(0xff000000 & pix[yi]) | (dv[rsum] << 16) | (dv[gsum] << 8) | dv[bsum];

                    rsum -= routsum;
                    gsum -= goutsum;
                    bsum -= boutsum;

                    stackstart = stackpointer - radius + div;
                    sir = stack[stackstart % div];

                    routsum -= sir[0];
                    goutsum -= sir[1];
                    boutsum -= sir[2];

                    if (x == 0)
                    {
                        vmin[y] = Math.Min(y + r1, hm) * w;
                    }

                    p = x + vmin[y];

                    sir[0] = r[p];
                    sir[1] = g[p];
                    sir[2] = b[p];

                    rinsum += sir[0];
                    ginsum += sir[1];
                    binsum += sir[2];

                    rsum += rinsum;
                    gsum += ginsum;
                    bsum += binsum;

                    stackpointer = (stackpointer + 1) % div;
                    sir = stack[stackpointer];

                    routsum += sir[0];
                    goutsum += sir[1];
                    boutsum += sir[2];

                    rinsum -= sir[0];
                    ginsum -= sir[1];
                    binsum -= sir[2];

                    yi += w;
                }
            }

            bitmap.SetPixels(pix, 0, w, 0, 0, w, h);
            return (bitmap);

        }
    }
}

