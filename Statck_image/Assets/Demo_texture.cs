using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Demo_texture : MonoBehaviour
{
    //计算鼠标点击位置 对应的像素位置，一个是image的左下角，一个是图片的右上角
    public Transform textureOrigin;
    public Transform textureUPEnd;
    //存储点击的图片的texture2D getpixel() 使用
    private Texture2D clickTexture2D;
    //存储鼠标点击位置的像素值
    private Color testColor;
    //存储计算出来的像素点的位置
    private Vector2 colorPos;
    //存储图片定位点的屏幕坐标
    private Vector3 textureOriginScreenPosition;
    private Vector3 textureEndUPScreenPosition;
    //测试用的显示颜色的图片
    public Image image;
    private void Start()
    {
        textureOriginScreenPosition = Camera.main.WorldToScreenPoint(textureOrigin.position);
        textureEndUPScreenPosition = Camera.main.WorldToScreenPoint(textureUPEnd.position);
    }
    private void Update()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(r, out hit))
            {
                HitColorChooseImage(hit);
            }
        }
    }
    private void HitColorChooseImage(RaycastHit hit)
    {
        // print(hit.collider.name);
        if (hit.collider.name == "Image")
        {
            //  print(3);
            clickTexture2D = hit.collider.gameObject.GetComponent<Image>().sprite.texture;
            CaculateVector2();

        }
    }
    public Stack<int> stackX = new Stack<int>();
    public Stack<int> stackY = new Stack<int>();
    private void CaculateVector2()
    {
        colorPos.x = (Input.mousePosition.x - textureOriginScreenPosition.x) / (textureEndUPScreenPosition.x - textureOriginScreenPosition.x) * clickTexture2D.width;
        colorPos.y = (Input.mousePosition.y - textureOriginScreenPosition.y) / (textureEndUPScreenPosition.y - textureOriginScreenPosition.y) * clickTexture2D.height;
      //  print("x:--" + colorPos.x + "--y:---" + colorPos.y);

        if (!(getColor((int)colorPos.x, (int)colorPos.y) == Color.white))//再加一层判断，判断当前选择的颜色和单色图的颜色是否对应，不对应也return
        {
            return;
        }
        int[] dx = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1 };
        int[] dy = new int[8] { -1, -1, 0, 1, 1, 1, 0, -1 };

        stackX.Push((int)colorPos.x);
        stackY.Push((int)colorPos.y);
        int x;
        int y;
        int xx;
        int yy;
        print(clickTexture2D.width + "_" + clickTexture2D.height);
        while (stackX.Count > 0)
        {
            x = stackX.Pop();
            y = stackY.Pop();
            //setColor(x, y, Color.red);
            setColor(x, y, Color.clear);
            for (int i = 0; i < 8; i++)
            {
                xx = x + dx[i];
                yy = y + dy[i];
                if (xx > 0 && xx < clickTexture2D.width && yy > 0 && yy < clickTexture2D.height && getColor(xx, yy) == Color.white)
                {

                    stackX.Push(xx);
                    stackY.Push(yy);
                }
            }
        }
        clickTexture2D.Apply();
    }
    public Color getColor(int x, int y)
    {
        return clickTexture2D.GetPixel(x, y);
    }
    public void setColor(int x, int y, Color c)
    {
        clickTexture2D.SetPixel(x, y, c);
    }
}

