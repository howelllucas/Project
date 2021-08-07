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
using UnityEngine.EventSystems;
using AdvancedMobilePaint;

/// <summary>
/// <para>Scene: Gameplay</para>
/// <para>Object: Paint</para>
/// <para>Description: Calculates scale and position of PaintPanel object depending on tap positions. Also, contains logic that differs Zoom from painting</para>
/// </summary>
public class PinchZoom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
{
    public float zoomSpeed;
    public float scaleMax;
    public Transform objectToScale;
    public AdvancedMobilePaint.AdvancedMobilePaint paintEngine;
	
    // Pinch Zoom Variables
    float factor;
    // Scaling factor
    float prevTouchDeltaMag;
    // Touch distance in previous frame
    float touchDeltaMag;
    // Touch distance in current frame
    float deltaMagnitudeDiff;
    // Difference between touch distances in two frames
    float clamper;
    // Clamps the position
    float midX;
    // X coordinate of the midpoint between two touches
    float midY;
    // Y coordinate of the midpoint between two touches
    float magnitudeFactor;
    // Ratio between preNorm and lenghtX
    float lenghtX;
    // width of the object we are scaling
    Touch t0;
    Touch t1;
    Vector2 t0PrevPos;
    Vector2 t1PrevPos;
    Vector3 offsetVector;
    Vector3 preClampedPosition;
    Vector3 preNorm;
    Vector3 norm;
    bool touchInside;
    bool oneFingerZoom = false;
    bool inputUpdated = false;

    Vector2 pointerDownPos;
    Vector2 pointerUpPos;




    public void OnPointerDown(PointerEventData eventData)
    {
        oneFingerZoom = true;
        pointerDownPos = eventData.position;
        if (eventData.pointerId <= 0)
        {
//			Debug.Log("PointerDown");
            touchInside = true;
            offsetVector = objectToScale.localPosition - new Vector3(eventData.pressPosition.x, eventData.pressPosition.y, 0);
            if (!SubMenusController.StickerMode)
                paintEngine.drawEnabled = true;
        }

        if (eventData.pointerId == 1)
        {
//			magnitude = 0;
            midX = (Input.GetTouch(0).position.x + Input.GetTouch(1).position.x) / 2;
            midY = (Input.GetTouch(0).position.y + Input.GetTouch(1).position.y) / 2;
            offsetVector = objectToScale.localPosition - new Vector3(midX, midY, 0);

            preNorm = Camera.main.WorldToScreenPoint(objectToScale.position) - new Vector3(midX, midY, 0);
            preNorm.z = 0;
            norm = Vector3.Normalize(preNorm);
            norm.z = 0;
            magnitudeFactor = preNorm.magnitude / (objectToScale.GetComponent<RectTransform>().sizeDelta.x * objectToScale.localScale.x);

            inputUpdated = true;
        }

        CancelInvoke("DrawIsIntended");
        if (paintEngine.drawMode != DrawMode.FloodFill)
            Invoke("DrawIsIntended", 0.05f);	// delay that differs paint from zoom
		else
            paintEngine.drawIntended = true;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerUpPos = eventData.position;


        CancelInvoke("DrawIsIntended");
        if (paintEngine.drawMode == DrawMode.FloodFill && Vector2.Distance(pointerDownPos, pointerUpPos) <= 10)
        {
            paintEngine.DrawOnPointerUp();
        }

        if (eventData.pointerId <= 0)
        {
            touchInside = false;
//			paintEngine.drawEnabled = false;
            offsetVector = Vector2.zero;
        }

        inputUpdated = false;
        oneFingerZoom = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
//		Debug.Log("Drag began");
        CancelInvoke("DrawIsIntended");
        if (paintEngine.drawMode != DrawMode.FloodFill && eventData.pointerId == 0)
            paintEngine.drawIntended = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!oneFingerZoom)
            return;
//		Debug.Log("Draging u najvece...");	
        if (Input.touchCount == 1 && paintEngine.drawMode == DrawMode.FloodFill)
        {
            // Positioning the panel
            clamper = (objectToScale.GetComponent<RectTransform>().sizeDelta.x / 2) * (objectToScale.localScale.x - 1);
            preClampedPosition = new Vector3(eventData.position.x, eventData.position.y, 0) + offsetVector;
            objectToScale.localPosition = new Vector3(
                Mathf.Clamp(preClampedPosition.x, -clamper, clamper),
                Mathf.Clamp(preClampedPosition.y, -clamper, clamper),
                0);
        }
    }

    void OnApplicationPause()
    {
        touchInside = false;
    }

    void Update()
    {
        if (Input.touchCount == 2 && inputUpdated)
        {
            oneFingerZoom = false;
            t0 = Input.GetTouch(0);
            t1 = Input.GetTouch(1);
            CancelInvoke("DrawIsIntended");
            paintEngine.drawIntended = false;

            if (!touchInside)
                return;


            t0PrevPos = t0.position - t0.deltaPosition;
            t1PrevPos = t1.position - t1.deltaPosition;
			

            prevTouchDeltaMag = (t0PrevPos - t1PrevPos).magnitude;
            touchDeltaMag = (t0.position - t1.position).magnitude;
			

            deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
			
            factor = deltaMagnitudeDiff * zoomSpeed;

            objectToScale.localScale -= new Vector3(factor, factor, 0);

            if (objectToScale.localScale.x < 1)
                objectToScale.localScale = Vector3.one;
            if (objectToScale.localScale.x > scaleMax)
                objectToScale.localScale = new Vector3(scaleMax, scaleMax, 1);


            clamper = (objectToScale.GetComponent<RectTransform>().sizeDelta.x / 2) * (objectToScale.localScale.x - 1);

            midX = (t0.position.x + t1.position.x) / 2;
            midY = (t0.position.y + t1.position.y) / 2;

//			normX = (offsetVector.x - 0)/( - 0);
//			normY = (offsetVector.y - 0)/( - 0);

//			Vector3 norm = Vector3.Normalize (preNorm);
//			norm.z = 0;
            lenghtX = objectToScale.GetComponent<RectTransform>().sizeDelta.x * objectToScale.localScale.x;

//			preClampedPosition = new Vector3 (midX,midY,0) + offsetVector + magnitude*preNorm;
            preClampedPosition = new Vector3(midX, midY, 0) + offsetVector + norm * magnitudeFactor * lenghtX - preNorm;
            if (Mathf.Abs(factor) > 0)
            {
                objectToScale.localPosition = new Vector3(
                    Mathf.Clamp(preClampedPosition.x, -clamper, clamper),
                    Mathf.Clamp(preClampedPosition.y, -clamper, clamper),
                    0);
            }
				
        }

        #if UNITY_EDITOR
        if (Input.GetKey(KeyCode.UpArrow))
        {
            objectToScale.localScale += new Vector3(zoomSpeed, zoomSpeed, 0);

            if (objectToScale.localScale.x < 1)
                objectToScale.localScale = Vector3.one;
            if (objectToScale.localScale.x > scaleMax)
                objectToScale.localScale = new Vector3(scaleMax, scaleMax, 1);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            objectToScale.localScale -= new Vector3(zoomSpeed, zoomSpeed, 0);
			
            if (objectToScale.localScale.x < 1)
                objectToScale.localScale = Vector3.one;
            if (objectToScale.localScale.x > scaleMax)
                objectToScale.localScale = new Vector3(scaleMax, scaleMax, 1);
        }

        #endif

    }

    void DrawIsIntended()
    {
        paintEngine.drawIntended = true;
    }
}










































