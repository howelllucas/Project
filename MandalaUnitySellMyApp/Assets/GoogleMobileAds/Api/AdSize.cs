/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace GoogleMobileAds.Api {
    public class AdSize {
        private bool isSmartBanner;
        private int width;
        private int height;

        public static readonly AdSize Banner = new AdSize(320, 50);
        public static readonly AdSize MediumRectangle = new AdSize(300, 250);
        public static readonly AdSize IABBanner = new AdSize(468, 60);
        public static readonly AdSize Leaderboard = new AdSize(728, 90);
        public static readonly AdSize SmartBanner = new AdSize(true);
        public static readonly int FullWidth = -1;

        public AdSize(int width, int height) {
            isSmartBanner = false;
            this.width = width;
            this.height = height;
        }

        private AdSize(bool isSmartBanner) : this(0, 0) {
            this.isSmartBanner = isSmartBanner;
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public bool IsSmartBanner
        {
            get
            {
                return isSmartBanner;
            }
        }
    }
}
