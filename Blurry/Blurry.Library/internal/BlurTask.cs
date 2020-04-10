
//import android.content.Context;
//import android.content.res.Resources;
//import android.graphics.Bitmap;
//import android.graphics.drawable.BitmapDrawable;
//import android.os.Handler;
//import android.os.Looper;
//import android.view.View;
//import java.lang.ref.WeakReference;
//import java.util.concurrent.ExecutorService;
//import java.util.concurrent.Executors;

using System;
using System.Threading;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Java.Util.Concurrent;

namespace Blurry.Library.@internal
{
    /**
 * Copyright (C) 2018 Wasabeef
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

    public class BlurTask {

        public interface ICallback {
            void Done(BitmapDrawable drawable);
        }

        private readonly Resources _res;
        private readonly WeakReference<Context> _contextWeakRef;
        private readonly BlurFactor _factor;
        private readonly Bitmap _bitmap;
        private readonly Action<BitmapDrawable> _callback;
        private static IExecutorService _threadPool = Executors.NewCachedThreadPool();

        public BlurTask(View target, BlurFactor factor, Action<BitmapDrawable> callback) {
            _res = target.Resources;
            _factor = factor;
            _callback = callback;
            _contextWeakRef = new WeakReference<Context>(target.Context);

            target.DrawingCacheEnabled = true;
            target.DestroyDrawingCache();
            target.DrawingCacheQuality = DrawingCacheQuality.Low;
            _bitmap = target.DrawingCache;
        }

        public BlurTask(Context context, Bitmap bitmap, BlurFactor factor, Action<BitmapDrawable> callback) {
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
