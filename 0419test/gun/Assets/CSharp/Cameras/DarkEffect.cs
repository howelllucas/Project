using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DarkEffect : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        [SerializeField]
        public Transform target;

        [SerializeField]
        public int radius;

        public Vector3 GetScreenPosition(Camera cam)
        {
            return cam.WorldToScreenPoint(target.position);
        }

        public Item(Transform t, int r)
        {
            target = t;
            radius = r;
        }
    }

    //渐变像素数量
    public int _smoothLength = 20;
    //遮罩混合颜色
    public Color _darkColor = Color.black;
    //目标物体
    public List<Item> _items = new List<Item>();

    protected Material _mainMaterial;
    protected Camera _mainCamera;

    Vector4[] _itemDatas;
    Item _tmpItem;
    Vector4 _tmpVt;
    Vector3 _tmpPos;
    int _tmpScreenHeight;

    public bool AddFocus(GameObject t, int r)
    {
        if (_items.Count > 8) return false;

        bool ret = false;
        foreach (DarkEffect.Item it in _items)
        {
            if (it.target == null)
            {
                it.target = t.transform;
                it.radius = r;
                ret = true;
                break;
            }
        }
        if (!ret)
        {
            _items.Add(new DarkEffect.Item(t.transform, r));
        }

        return true;
    }

    public bool  LostFocus(GameObject t)
    {
        foreach( DarkEffect.Item it in _items)
        {
            if (it.target.gameObject == t)
            {
                _items.Remove(it);
                return true;
            }
        }

        return false;
    }


    private void OnEnable() { }

    public void Start()
    {
        _mainMaterial = new Material(Shader.Find("Peter/DarkEffect"));
        _mainCamera = GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if ( _items.Count == 0 ) return;

        if (_mainCamera == null)
        {
            _mainCamera = GetComponent<Camera>();
        }
    
        if (_itemDatas == null || _itemDatas.Length != _items.Count)
        {
            _itemDatas = new Vector4[_items.Count];
        }

        _tmpScreenHeight = Screen.height;
        float scale = _tmpScreenHeight / 750;

	    int j = 0;
        for (int i = 0; i < _items.Count; i++)
        {
            _tmpItem = _items[i];

            if (_tmpItem == null || _tmpItem.target == null) continue;

            _tmpPos = _tmpItem.GetScreenPosition(_mainCamera);

            _tmpVt.x = _tmpPos.x;
            _tmpVt.y = _tmpPos.y;
            _tmpVt.z = _tmpItem.radius * scale;
            _tmpVt.w = 0;

            _itemDatas[j++] = _tmpVt;
        }

        if (j == 0) return;

        _mainMaterial.SetInt("_SmoothLength", (int)( _smoothLength * scale) );
        _mainMaterial.SetColor("_DarkColor", _darkColor);
        _mainMaterial.SetInt("_ItemCnt", j); // _itemDatas.Length);
        _mainMaterial.SetVectorArray("_Item", _itemDatas);

        Graphics.Blit(source, destination, _mainMaterial);
    }
}
