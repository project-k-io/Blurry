using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Views.Animations;

// Copyright(C) 2018 Wasabeef
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Blurry.Library.@internal
{
 
    public class Helper
    {

        public static void SetBackground(View v, Drawable drawable)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                v.Background = drawable;
            }
            else
            {
                v.SetBackgroundDrawable(drawable);
            }
        }

        public static bool HasZero(params int[] args)
        {
            foreach (var num in args)
            {
                if (num == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static void Animate(View v, int duration)
        {
            var alpha = new AlphaAnimation(0f, 1f);
            alpha.Duration = duration;
            v.StartAnimation(alpha);
        }
    }
}
