
using Android.Graphics;

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

    public class BlurFactor {

        public static int DefaultRadius = 25;
        public static int DefaultSampling = 1;

        public int Width;
        public int Height;
        public int Radius = DefaultRadius;
        public int Sampling = DefaultSampling;
        public Color Color = Android.Graphics.Color.Transparent;
    }
}
