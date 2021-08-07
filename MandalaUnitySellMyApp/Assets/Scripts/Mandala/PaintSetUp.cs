/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AdvancedMobilePaint;

/// <summary>
/// <para>Scene: Gameplay</para>
/// <para>Object: PaintSetUpManager</para>
/// <para>Class for AMP SetUp. Contains methods for setting the paint texture, brushes and methods called from UI Buttons in Gameplay scene</para>
/// </summary>
public class PaintSetUp : MonoBehaviour
{

    public AdvancedMobilePaint.AdvancedMobilePaint paintEngine;
    public Slider brushSize;
    //	public Texture2D mandala;
    //	public Texture2D mask;
    //	public Sprite[] bitmaps;

    [Header("Bitmaps")]
    public Sprite[] bmp1;
    public Sprite[] bmp2;
    public Sprite[] bmp3;
    public Sprite[] bmp4;
    public Sprite[] bmp5;
    public Sprite[] bmp6;
    public Sprite[] bmp7;
    public Sprite[] bmp8;
    public Sprite[] bmp9;
    public Sprite[] bmp10;


    Texture2D currentBrush;
    Sprite[][] bmps;
    Color currentColor;
    GameObject currentShade;
    DrawMode currentDrawMode = DrawMode.Stamp;

    int bmpsIndex;
    int bmpIndex;

    bool maskForBrush;

    void Start()
    {
        bmps = new Sprite[10][];
        bmps[0] = bmp1;
        bmps[1] = bmp2;
        bmps[2] = bmp3;
        bmps[3] = bmp4;
        bmps[4] = bmp5;
        bmps[5] = bmp6;
        bmps[6] = bmp7;
        bmps[7] = bmp8;
        bmps[8] = bmp9;
        bmps[9] = bmp10;

        bmpsIndex = 0;
        bmpIndex = (int)brushSize.value;
//		currentBrush = PaintUtils.ConvertSpriteToTexture2DForBmp(bitmaps[0]);
        currentBrush = PaintUtils.ConvertSpriteToTexture2DForBmp(bmps[bmpsIndex][4 - bmpIndex]);
        currentColor = new Color32(255, 197, 197, 255);
        currentShade = GameObject.Find("Canvas/SubMenus/ColorsAndBrushes/AnimationHolder/Shades").transform.GetChild(0).gameObject;
//		SetUpQuadPaint();
        SetUpFloodFillBrush();
        SelectGameMode();
//		Invoke("EnableDrawing", 1.2f);
    }

    /// <summary>
    /// SetUp PaintEngine depending on GlobalVariables.gameMode.
    /// </summary>
    void SelectGameMode()
    {
        switch (GlobalVariables.gameMode)
        {
            case "Magic":
                paintEngine.filterMode = FilterMode.Bilinear;
                SetUpQuadPaint();
                paintEngine.SetMandalaMask(GlobalVariables.selectedTexture, FilterMode.Bilinear);
                paintEngine.useMaskLayerOnly = true;
                paintEngine.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case "Mandala":
                paintEngine.filterMode = FilterMode.Point;
                SetUpQuadPaint();
                paintEngine.SetMandalaMask(GlobalVariables.selectedTexture, FilterMode.Point);
                paintEngine.useMaskLayerOnly = true;
                paintEngine.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case "Blank":
                paintEngine.filterMode = FilterMode.Bilinear;
                SetUpQuadPaint();
                paintEngine.useMaskLayerOnly = false;
                break;
            default:
                Debug.Log("Invalid string for GameMode!");
                break;
        }
    }

    /// <summary>
    /// Sets up paint texture of AMP.
    /// </summary>
    public void SetUpQuadPaint()
    {
//		paintEngine.SetDrawingTexture(GlobalVariables.selectedTexture);
//		paintEngine.SetDrawingTexture(mandala);
//		paintEngine.SetMandalaTexture(GlobalVariables.selectedTexture);
//		paintEngine.SetMandalaTexture(mandala);
//		paintEngine.SetDrawingTexture(GenerateColoredTex(1024,1024, new Color32(229,226,174,255)));
        paintEngine.SetMandalaTexture(GenerateColoredTex(1024, 1024, new Color32(229, 226, 174, 255)), paintEngine.filterMode);
//		paintEngine.transform.GetChild(0).GetComponent<RawImage>().texture = GlobalVariables.selectedTexture;
    }

    /// <summary>
    /// SetUp FloodFill brush.
    /// </summary>
    public void SetUpFloodFillBrush()
    {
        paintEngine.SetFloodFIllBrush(currentColor, true);
        paintEngine.canDrawOnBlack = false;
    }

    /// <summary>
    /// SetUp bitmap brush.
    /// </summary>
    public void SetUpBitmapBrush()
    {
        if (GlobalVariables.gameMode == "Blank")
            maskForBrush = false;
        else
            maskForBrush = true;
		
        paintEngine.SetBitmapBrush(currentBrush, BrushProperties.Default, false, false, currentColor, maskForBrush, true, null);
        paintEngine.useLockArea = false;
//		paintEngine.drawMode = DrawMode.CustomBrush;
        paintEngine.drawMode = currentDrawMode;
    }

    /// <summary>
    /// Calls method that changes palette of colors and sets up current shade.
    /// </summary>
    /// <param name="index">Index.</param>
    public void ChangePalette(int index)
    {
        GameObject.Find("Canvas/SubMenus").GetComponent<SubMenusController>().SetPalette(index);
        ChangeColor(currentShade);
    }

    /// <summary>
    /// Changes the color of AMP.
    /// </summary>
    /// <param name="go">Go.</param>
    public void ChangeColor(GameObject go)
    {
        if (go.transform.GetChild(1).gameObject.activeInHierarchy)		//Da li je zeljena boja zakljucana?
        {
            currentColor = go.transform.parent.GetChild(0).GetComponent<Image>().color;
            paintEngine.paintColor = currentColor;
            currentShade = go.transform.parent.GetChild(0).gameObject;
            GameObject.Find("Canvas/SubMenus").GetComponent<SubMenusController>().MarkShade(0);
        }
        else
        {
            currentColor = go.GetComponent<Image>().color;
            paintEngine.paintColor = currentColor;
            currentShade = go;
        }
    }

    /// <summary>
    /// Changes the Bitmap brush image
    /// </summary>
    /// <param name="index">Index.</param>
    public void ChangeBitmap(int index)
    {
        bmpsIndex = index;
        UpdateBrushSize();

    }

    /// <summary>
    /// OnValueChange - brush size slider.
    /// </summary>
    public void UpdateBmpIndex()
    {
        bmpIndex = (int)brushSize.value;
    }

    /// <summary>
    /// Updates brush size. Called on OnPointerUp event of BrushSize slider.
    /// </summary>
    public void UpdateBrushSize()
    {
        currentBrush = PaintUtils.ConvertSpriteToTexture2DForBmp(bmps[bmpsIndex][4 - bmpIndex]);
        if (bmpsIndex > 5)
        {
            currentDrawMode = DrawMode.CustomBrush;
            SetUpBitmapBrush();
        }
        else
        {
            currentDrawMode = DrawMode.Stamp;
            SetUpBitmapBrush();			
        }
    }

    /// <summary>
    /// Helper function for generating a teksture in given color (if color is c.alpha == 0, then texture will be transparent).
    /// </summary>
    /// <returns>Teksturu.</returns>
    /// <param name="width">Sirina.</param>
    /// <param name="height">Visina.</param>
    /// <param name="c">Boja.</param>
    Texture2D GenerateColoredTex(int width, int height, Color c)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Color[] pix = tex.GetPixels();
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = c;
        }
        tex.SetPixels(pix);
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        return tex;
    }

    void EnableDrawing()
    {
        paintEngine.drawEnabled = true;
    }
}




















