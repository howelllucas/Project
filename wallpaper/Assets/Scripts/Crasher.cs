using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物体分裂效果组件类。
/// </summary>
public class Crasher : MonoBehaviour
{
    #region 可视变量
    [SerializeField]
    [Tooltip("Sprite对象。")]
    private Sprite sprite = null;

    [SerializeField]
    [Tooltip("碎片的层次名称，用于避碰。")]
    private string layerName = "Fragment";

    [SerializeField]
    [Tooltip("分割点的数量。")]
    private int splitPoint = 3;

    [SerializeField]
    [Tooltip("爆破力乘数。")]
    private float forceMultiply = 50F;

    [SerializeField]
    [Tooltip("碎片消失时延。")]
    private float delaySecond = 5F;
    #endregion

    #region 成员变量
    private int seed = 0;               // 随机数种子
    private float spriteWidth = 0;      // 贴图实际宽度
    private float spriteHeight = 0;     // 贴图实际高度
    private List<GameObject> fragments = new List<GameObject>();    // 碎片对象列表
    #endregion

    #region 功能方法
    /// <summary>
    /// 对对象执行粉碎特效。
    /// </summary>
    public void Crash()
    {
        // 属性初始化
        spriteWidth = sprite.texture.width;
        spriteHeight = sprite.texture.height;
        // 获取所有碎片对象
        GetFragments(sprite.texture, RandomSplits());
        // 弹射碎片对象
        for (int i = 0; i < fragments.Count; i++)
            Ejection(fragments[i]);
    }

    /// <summary>
    /// 根据割点获取所有碎片对象。
    /// </summary>
    /// <param name="texture2D">原始对象的纹理。</param>
    /// <param name="splits">割点列表。</param>
    private void GetFragments(Texture2D texture2D, Vector2[] splits)
    {
        // 分别获取x，y两个数组
        float[] splitXs = new float[splits.Length + 2];
        float[] splitYs = new float[splits.Length + 2];
        splitXs[0] = 0;
        splitXs[splitXs.Length - 1] = spriteWidth;
        splitYs[0] = 0;
        splitYs[splitYs.Length - 1] = spriteHeight;
        for (int i = 0; i < splits.Length; i++)
        {
            splitXs[i + 1] = splits[i].x;
            splitYs[i + 1] = spriteHeight - splits[i].y;    // y轴坐标系倒转
        }
        // 对数组进行升序排序
        Sort<float> sort = new Sort<float>();
        sort.QuickSort(splitXs, 0, splits.Length);
        sort.QuickSort(splitYs, 0, splits.Length);
        // 分割物体
        for (int i = 0; i < splitXs.Length - 1; i++)
        {
            for (int j = 0; j < splitYs.Length - 1; j++)
            {
                float x1 = splitXs[i];
                float y1 = splitYs[j];
                float x2 = splitXs[i + 1];
                float y2 = splitYs[j + 1];
                float centerX = gameObject.transform.position.x - gameObject.transform.localScale.x / 2 + (x1 + x2) / (2 * spriteWidth);
                float centerY = gameObject.transform.position.y - gameObject.transform.localScale.y / 2 + (y1 + y2) / (2 * spriteHeight);
                Rect rect = new Rect(x1, y1, x2 - x1, y2 - y1);
                Sprite sprite = Sprite.Create(texture2D, rect, Vector2.zero);
                Vector2 position = new Vector2(centerX, centerY);
                fragments.Add(CreateFragment(sprite, position));
            }
        }
    }

    /// <summary>
    /// 在spriteRenderer区域内获取随机分割点。
    /// </summary>
    /// <returns>分割点数组。</returns>
    private Vector2[] RandomSplits()
    {
        System.Random random;
        Vector2[] splits = new Vector2[splitPoint];
        // 为了避免割点聚集，先分割区域，再于对应区域随机取点
        float spanX = spriteWidth / (2 * splitPoint + 1);
        float spanY = spriteHeight / (2 * splitPoint + 1);
        for (int i = 0; i < splitPoint; i++)
        {
            random = new System.Random(unchecked((int)System.DateTime.Now.Ticks) + seed);
            seed++;
            double x = random.NextDouble() * spanX + 2 * (i + 1) * spanX;
            random = new System.Random(unchecked((int)System.DateTime.Now.Ticks) + seed);
            seed++;
            double y = random.NextDouble() * spanY + 2 * (i + 1) * spanY;
            splits[i] = new Vector2((float)x, (float)y);
        }
        return splits;
    }

    /// <summary>
    /// 弹射一个碎片对象。
    /// </summary>
    /// <param name="fragment">碎片对象。</param>
    private void Ejection(GameObject fragment)
    {
        Vector2 start = fragment.transform.position;
        Vector2 end = gameObject.transform.position;
        Vector2 direction = end - start;
        fragment.GetComponent<Rigidbody2D>().AddForce(direction * forceMultiply, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 创造一个碎片对象。
    /// </summary>
    /// <param name="sprite">碎片贴图。</param>
    /// <param name="position">碎片贴图位置。</param>
    /// <returns>碎片对象。</returns>
    private GameObject CreateFragment(Sprite sprite, Vector2 position)
    {
        GameObject fragment = new GameObject("Fragment");
        fragment.layer = LayerMask.NameToLayer(layerName);
        fragment.transform.position = position;
        fragment.AddComponent<SpriteRenderer>().sprite = sprite;
        // 可以将碎片视作刚体，这样会有与地形的碰撞效果
        fragment.AddComponent<Rigidbody2D>();
        fragment.AddComponent<BoxCollider2D>();
        fragment.AddComponent<FadeOut>().delaySecond = delaySecond;     // 添加淡出效果
        return fragment;
    }
    #endregion
}


