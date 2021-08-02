/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Collections.Generic;

using UnityEngine;

namespace GoogleMobileAds.Api.Mediation
{
    public abstract class MediationExtras
    {
        public Dictionary<string, string> Extras { get; protected set; }

        public MediationExtras()
        {
            this.Extras = new Dictionary<string, string>();
        }

        public abstract string AndroidMediationExtraBuilderClassName { get; }

        public abstract string IOSMediationExtraBuilderClassName { get; }
    }
}
