﻿using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Blurry.Library;
using Java.Lang;

namespace Blurry.Sample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            FindViewById<View>(Resource.Id.button).Click += (s, e) =>
            {
                // var startMs = System.CurrentTimeMillis
                Blurry2.With(this)
                    .Radius(25)
                    .Sampling(1)
                    .Color(Color.Argb(66, 0, 255, 255))
                    .Async()
                    .Capture(FindViewById(Resource.Id.right_top))
                    .Into(FindViewById<ImageView>(Resource.Id.right_top));

                Blurry2.With(this)
                    .Radius(10)
                    .Sampling(8)
                    .Async()
                    .Capture(FindViewById(Resource.Id.right_bottom))
                    .Into(FindViewById<ImageView>(Resource.Id.right_bottom));

                Blurry2.With(this)
                    .Radius(25)
                    .Sampling(1)
                    .Color(Color.Argb(66, 255, 255, 0))
                    .Async()
                    .Capture(FindViewById(Resource.Id.left_bottom))
                    .Into(FindViewById<ImageView>(Resource.Id.left_bottom));

                // Log.Debug(GetString(Resource.string.app_name), "TIME " + (System.currentTimeMillis() - startMs).toString() + "ms"));
            };

            var blurred = false;

            FindViewById<View>(Resource.Id.button).LongClick += (sender, args) =>
            {
                if (blurred)
                {
                    Blurry2.Delete(FindViewById<ViewGroup>(Resource.Id.content));
                }
                else
                {
                    var startMs = JavaSystem.CurrentTimeMillis();
                    Blurry2.With(this)
                        .Radius(25)
                        .Sampling(2)
                        .Async()
                        .Animate(500)
                        .Onto(FindViewById<ViewGroup>(Resource.Id.content));
                    Log.Debug(GetString(Resource.String.app_name),
                        "TIME " + (JavaSystem.CurrentTimeMillis() - startMs).ToString() + "ms");
                }

                blurred = !blurred;
                args.Handled = true;
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}