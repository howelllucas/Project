using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test
{
    ///<summary>
    ///
    ///</summary>
    public class GetColor : MonoBehaviour
    {
        public Texture2D main_tex;
        public Image image;

        void Start()
        {
            image.sprite = Sprite.Create(main_tex, new Rect(0, 0, main_tex.width, main_tex.height), Vector2.zero);

        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 vOut = ReadScreenPositionTextureColor(Input.mousePosition, image.GetComponent<RectTransform>(), main_tex);
                Color colorOut = main_tex.GetPixel((int)vOut.x, (int)vOut.y);
                Debug.Log(colorOut);
            }
        }
        private Vector2 ReadScreenPositionTextureColor(Vector2 screenPosition, RectTransform rectT, Texture2D texture)
        {
            float width = rectT.rect.width * rectT.localScale.x;
            float heigth = rectT.rect.height * rectT.localScale.y;

            //UI空间坐标到屏幕坐标的转换
            //图片范围
            Vector2 v0 = new Vector2(rectT.localPosition.x + (Screen.width - width) / 2
                , rectT.localPosition.y + (Screen.height - heigth) / 2);
            Vector2 v1 = new Vector2(rectT.localPosition.x + (Screen.width + width) / 2
                , rectT.localPosition.y + (Screen.height + heigth) / 2);
            if (screenPosition.x < v1.x && screenPosition.x > v0.x && screenPosition.y < v1.y && screenPosition.y > v0.y)
            {
                //点击到图片区域
                float x = (screenPosition.x - v0.x) / width * texture.width;
                float y = (screenPosition.y - v0.y) / heigth * texture.height;
                return new Vector2(x, y);
            }
            else
            {
                Debug.Log("点击到图片之外");
                return new Vector2(0, 0);
            }
        }
    }
}
