/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;

/// <summary>
/// <para>Scene: Gameplay</para>
/// <para>Object: ButtonLeft, ButtonRight</para>
/// <para>Description: Moving scrollRect with colors when clicked on arrows</para>
/// </summary>
public class ColorsPositionUpdate : MonoBehaviour
{

    public static bool targetUpdated = false;
    public static Vector3 target;

    void Update()
    {
        if (targetUpdated)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, 0.5f);

            if (Vector3.Distance(transform.localPosition, target) < 0.2f)
            {
                transform.localPosition = target;
                targetUpdated = false;
            }
        }
    }
}
