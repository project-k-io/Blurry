using System;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Blurry.Sample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            FindViewById<View>(Resource.Id.button).SetOnClickListener(() =>
            {
                // var startMs = System.CurrentTimeMillis
                Blurry.with(this)
                    .radius(25)
                    .sampling(1)
                    .color(ColoResource.argb(66, 0, 255, 255))
                    .async()
                    .capture(findViewById(Resource.id.right_top))
                    .into(findViewById(Resource.id.right_top));

                Blurry.with(this)
                    .radius(10)
                    .sampling(8)
                    .async()
                    .capture(findViewById(Resource.id.right_bottom))
                    .into(findViewById(Resource.id.right_bottom));

                Blurry.with(this)
                    .radius(25)
                    .sampling(1)
                    .color(ColoResource.argb(66, 255, 255, 0))
                    .async()
                    .capture(findViewById(Resource.id.left_bottom))
                    .into(findViewById(Resource.id.left_bottom));

                Log.d(getString(Resource.string.app_name), "TIME " + (System.currentTimeMillis() - startMs).toString() + "ms");
            };

            FindViewById<View>(Resource.id.button).setOnLongClickListener(OnLongClickListener (() =>
                {
        private var blurred = false

        override fun onLongClick(v: View): Boolean {
            if (blurred) {
                Blurry.delete(findViewById(Resource.id.content))
            } else {
                val startMs = System.currentTimeMillis()
                Blurry.with(this@MainActivity)
                    .radius(25)
                    .sampling(2)
                    .async()
                    .animate(500)
                    .onto(findViewById<View>(Resource.id.content) as ViewGroup)
                Log.d(getString(Resource.string.app_name),
                "TIME " + (System.currentTimeMillis() - startMs).toString() + "ms")
            }

            blurred = !blurred
            return true
        }
    })
}

                }
            )))



	}
}

