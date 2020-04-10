//package jp.wasabeef.blurry;

//import android.content.Context;
//import android.graphics.Bitmap;
//import android.graphics.drawable.BitmapDrawable;
//import android.graphics.drawable.Drawable;
//import android.view.View;
//import android.view.ViewGroup;
//import android.widget.ImageView;
//import jp.wasabeef.blurry.Blurry.ImageComposer.ImageComposerListener;
//import jp.wasabeef.blurry.internal.Blur;
//import jp.wasabeef.blurry.internal.BlurFactor;
//import jp.wasabeef.blurry.internal.BlurTask;
//import jp.wasabeef.blurry.internal.Helper;

using Android.Content;
using Android.Views;
using Blurry.Library;

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

public class Blurry2 {

  private static readonly string Tag =   typeof(Blurry2).Name;

  public static Composer With2(Context context) {
    return new Composer(context);
  }

  public static void Delete(ViewGroup target) {
    View view = target.FindViewWithTag(Tag);
    if (view != null) {
      target.RemoveView(view);
    }
  }
}