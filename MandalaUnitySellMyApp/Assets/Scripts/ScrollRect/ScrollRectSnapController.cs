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
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Scene: CharacterSelect
/// Object: Characters
/// Description: Used for character scrolling with snap at every character
/// </summary>

public class ScrollRectSnapController : MonoBehaviour
{

    // Snapping positions list
    public List<float> snapPositions;

    // List element size
    public float cellSizeX;
    public float cellSizeY;
    public float spacing;

    // Used for checking characters snap point
    private float currentCharCheckTemp;

    public Vector3 newLerpPosition;

    // Lerping variables
    private bool lerping;
    public float lerpingSpeed;

    // true if we are interacting with scroll rect
    private bool holdingRect;

    public float focusedElementScale;
    public float unfocusedElementsScale;

    public List<GameObject> listOfCharacters;

    public bool horizontalList;

    public bool useButtons;
    public GameObject backwardButton;
    public GameObject forwardButton;

    public bool useIndicators;
    public GameObject indicatorStart;
    public GameObject indicatorEnd;

    private bool buttonPressed;

    public int currentCharacter;

    //	[Header ("Native Ads")]
    //	public FacebookNativeAd[] nativeAdsInScroll;
    //	public int currentNativeAd = 0;
    //	public bool[] adShown;

    public void Awake()
    {
//		if (GlobalVariables.removeAds) {
//			for (int i = 0; i < nativeAdsInScroll.Length; i++) {
//				if (nativeAdsInScroll [i] != null)
//					DestroyImmediate (nativeAdsInScroll [i].gameObject);	
//
//			}
//			nativeAdsInScroll = null;
//			adShown = null;
//		}

        // If we are coming from another scene and we want focus to be on some
        // other character than first one
        //		if (GlobalVariables.selectedCharacterIndex != -1)
        //			currentCharacter = GlobalVariables.selectedCharacterIndex;
        //		else
//		currentCharacter = 0;

        lerping = false;
        buttonPressed = false;

        // Set size of the cell
        if (GetComponent<GridLayoutGroup>().cellSize == Vector2.zero)
        {
            Vector2 cellSize = new Vector2(cellSizeX, cellSizeY);
            GetComponent<GridLayoutGroup>().cellSize = cellSize;
        }
        else
        {
            cellSizeX = GetComponent<GridLayoutGroup>().cellSize.x;
            cellSizeY = GetComponent<GridLayoutGroup>().cellSize.y;
        }

        // Set size delta of parent scroll rect so elements wouldn't be jumpy
        transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSizeX, cellSizeY);

        if (horizontalList)
        {
            transform.parent.GetComponent<ScrollRect>().horizontal = true;
            transform.parent.GetComponent<ScrollRect>().vertical = false;

            // Check if layout spacing differes from zero vector
            if (GetComponent<GridLayoutGroup>().spacing == Vector2.zero)
            {
                Vector2 spacingVector = new Vector2(spacing, 0);
                GetComponent<GridLayoutGroup>().spacing = spacingVector;
            }
            else
            {
                if (GetComponent<GridLayoutGroup>().spacing.x != 0)
                    spacing = GetComponent<GridLayoutGroup>().spacing.x;

                Vector2 spacingVector = new Vector2(spacing, 0);
            }

            GetComponent<GridLayoutGroup>().startAxis = GridLayoutGroup.Axis.Vertical;
            GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
            GetComponent<GridLayoutGroup>().constraintCount = 1;
            currentCharCheckTemp = (cellSizeX + spacing) / 2;
        }
        else
        {
            transform.parent.GetComponent<ScrollRect>().horizontal = false;
            transform.parent.GetComponent<ScrollRect>().vertical = true;

            if (GetComponent<GridLayoutGroup>().spacing == Vector2.zero)
            {
                Vector2 spacingVector = new Vector2(0, spacing);
                GetComponent<GridLayoutGroup>().spacing = spacingVector;
            }
            else
            {
                if (GetComponent<GridLayoutGroup>().spacing.y != 0)
                    spacing = GetComponent<GridLayoutGroup>().spacing.y;
				
                Vector2 spacingVector = new Vector2(0, spacing);
            }

            GetComponent<GridLayoutGroup>().startAxis = GridLayoutGroup.Axis.Horizontal;
            GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            GetComponent<GridLayoutGroup>().constraintCount = 1;
            currentCharCheckTemp = (cellSizeY + spacing) / 2;
        }

        snapPositions.Clear();
        snapPositions = new List<float>();

//		if (currentCharacter == 0)
//			leftArrow.SetActive(false);
//		else if (currentCharacter == 5)
//			rightArrow.SetActive(false);

        // Get all characters and put then into list
        foreach (Transform t in transform)
            listOfCharacters.Add(t.gameObject);

        // Set transform rect position and size depending of number of characters and spacing
        if (horizontalList)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(listOfCharacters.Count * cellSizeX + (listOfCharacters.Count - 1) * spacing, cellSizeY);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().sizeDelta.x - 2 * spacing, GetComponent<RectTransform>().anchoredPosition.y);

            float startSnapPosition = GetComponent<RectTransform>().sizeDelta.x / 2 - cellSizeX / 2;
            snapPositions.Add(startSnapPosition);

            // Set fist character to be of focused scale
            listOfCharacters[0].transform.localScale = new Vector3(focusedElementScale, focusedElementScale, 1);
			
            for (int i = 1; i < listOfCharacters.Count; i++)
            {
                startSnapPosition -= cellSizeX + spacing;
                snapPositions.Add(startSnapPosition);
				
                // Set scale for not focused elements to be scale
                listOfCharacters[i].transform.localScale = new Vector3(unfocusedElementsScale, unfocusedElementsScale, 1);
            }
        }
        else
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(cellSizeX, listOfCharacters.Count * cellSizeY + (listOfCharacters.Count - 1) * spacing);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, -(GetComponent<RectTransform>().sizeDelta.y - 2 * spacing));

            float startSnapPosition = GetComponent<RectTransform>().sizeDelta.y / 2 - cellSizeY / 2;
            snapPositions.Add(startSnapPosition);

            // Set fist character to be of focused scale
            listOfCharacters[0].transform.localScale = new Vector3(focusedElementScale, focusedElementScale, 1);
			
            for (int i = 1; i < listOfCharacters.Count; i++)
            {
                startSnapPosition -= cellSizeY + spacing;
                snapPositions.Add(startSnapPosition);
				
                // Set scale for not focused elements to be scale
                listOfCharacters[i].transform.localScale = new Vector3(unfocusedElementsScale, unfocusedElementsScale, 1);
            }
        }

        if (Application.loadedLevelName == "MainScene")
        {
            newLerpPosition = new Vector3(snapPositions[1], 0, 0);
            currentCharacter = 1;
            lerping = true;
        }
    }

    //	void OnEnable ()
    //	{
    //		if (nativeAdsInScroll != null)
    //			adShown = new bool[nativeAdsInScroll.Length];
    ////		CancelAllAds();
    //	}

    //	void Start()
    //	{
    //		for(int i = 0; i<adShown.Length; i++)
    //		{
    //			adShown[i] = false;
    //		}
    //		if(Application.loadedLevelName == "MainScene")
    //		{
    //			if(GlobalVariables.removeAds)
    //			{
    //				newLerpPosition = new Vector3(snapPositions[1],0,0);
    //				currentCharacter = 1;
    //			}
    //			else
    //			{
    //				newLerpPosition = new Vector3(snapPositions[2],0,0);
    //				currentCharacter = 2;
    //			}
    //			lerping = true;
    //		}
    //	}

    // Determining closesst snap point -349 is half distance - 1 and 350 is half distance
    void SetLerpPositionToClosestSnapPoint()
    {
        for (int i = 0; i < snapPositions.Count; i++)
        {
            if (horizontalList)
            {
                if (transform.localPosition.x > snapPositions[i] - currentCharCheckTemp - 1 && transform.localPosition.x <= snapPositions[i] + currentCharCheckTemp)
                {
                    newLerpPosition = new Vector3(snapPositions[i], 0, 0);
                    lerping = true;
                    currentCharacter = i;
                    break;
                }
            }
            else
            {
                if (transform.localPosition.y > snapPositions[i] - currentCharCheckTemp - 1 && transform.localPosition.y <= snapPositions[i] + currentCharCheckTemp)
                {
                    newLerpPosition = new Vector3(0, snapPositions[i], 0);
                    lerping = true;
                    currentCharacter = listOfCharacters.Count - i - 1;
                    break;
                }
            }
        }
    }

    void SetCurrentCharacter()
    {
        for (int i = 0; i < snapPositions.Count; i++)
        {
            if (horizontalList)
            {
                if (transform.localPosition.x > snapPositions[i] - currentCharCheckTemp - 1 && transform.localPosition.x <= snapPositions[i] + currentCharCheckTemp)
                {
                    currentCharacter = i;
                    break;
                }
            }
            else
            {
                if (transform.localPosition.y > snapPositions[i] - currentCharCheckTemp - 1 && transform.localPosition.y <= snapPositions[i] + currentCharCheckTemp)
                {
                    currentCharacter = listOfCharacters.Count - i - 1;
                    break;
                }
            }
        }

        //numberOfDressedCharacter.text= (currentCharacter + 1).ToString() + "/6";
    }

    // This function purpouse is to wait a little before pressing button again
    IEnumerator ButtonPressed()
    {
        yield return new WaitForSeconds(0.4f);
        buttonPressed = false;
    }

    public void BackwardButtonPressed()
    {
        if (horizontalList)
        {
            if (currentCharacter > 0 && !buttonPressed)
            {
                // Button pressed
                buttonPressed = true;

                currentCharacter -= 1;
                newLerpPosition = new Vector3(snapPositions[currentCharacter], transform.localPosition.y, 0);
                lerping = true;

                StartCoroutine(ButtonPressed());
            }
        }
        else
        {
            if (currentCharacter > 0 && !buttonPressed)
            {
                // Button pressed
                buttonPressed = true;
				
                currentCharacter -= 1;
                newLerpPosition = new Vector3(transform.localPosition.x, snapPositions[listOfCharacters.Count - currentCharacter - 1], 0);
                lerping = true;
				
                StartCoroutine(ButtonPressed());
            }
        }
    }

    public void ForwardButtonPressed()
    {
        if (horizontalList)
        {
            if (currentCharacter < snapPositions.Count - 1 && !buttonPressed)
            {
                // Button pressed
                buttonPressed = true;

                currentCharacter += 1;
                newLerpPosition = new Vector3(snapPositions[currentCharacter], transform.localPosition.y, 0);
                lerping = true;

                StartCoroutine(ButtonPressed());
            }
        }
        else
        {
            if (currentCharacter < listOfCharacters.Count - 1 && !buttonPressed)
            {
                // Button pressed
                buttonPressed = true;
				
                currentCharacter += 2;
                newLerpPosition = new Vector3(transform.localPosition.x, snapPositions[listOfCharacters.Count - currentCharacter], 0);
                lerping = true;
				
                StartCoroutine(ButtonPressed());
            }
        }
    }

    public void SetButtonActive(GameObject button)
    {
        if (!useButtons)
            return;
        Color c = button.GetComponent<Image>().color;
        c = new Color(1, 1, 1, 1);
        button.GetComponent<Image>().color = c;
        button.transform.GetChild(0).GetComponent<Image>().color = c;

        button.GetComponent<Button>().interactable = true;

    }

    public void SetButtonInactive(GameObject button)
    {
        if (!useButtons)
            return;
        Color c = button.GetComponent<Image>().color;
        c = new Color(1, 1, 1, 0.3f);
        button.GetComponent<Image>().color = c;
        button.transform.GetChild(0).GetComponent<Image>().color = c;
		
        button.GetComponent<Button>().interactable = false;
    }

    public void SetIndicatorActive(GameObject indicator)
    {
        if (!useIndicators)
            return;
        indicator.SetActive(false);
    }

    public void SetIndicatorInactive(GameObject indicator)
    {
        if (!useIndicators)
            return;
        indicator.SetActive(true);
    }

    void Update()
    {
//		// Deo koji hendluje NativeAd u zavisnosti od pozicije na ekranu
//		if (!GlobalVariables.removeAds) {
//			for (int i = 0; i < nativeAdsInScroll.Length; i++) {
////				Debug.Log(Camera.main.WorldToScreenPoint(nativeAdsInScroll[i].transform.position).x + " " + Camera.main.WorldToScreenPoint(nativeAdsInScroll[i].transform.position).y + " " + adShown[i] + " " + Screen.width);
//				if (Camera.main.WorldToScreenPoint (nativeAdsInScroll [i].transform.position).x < -50 || Camera.main.WorldToScreenPoint (nativeAdsInScroll [i].transform.position).x > Screen.width + 50) {
//					if (adShown [i]) {
////						Debug.Log("Native ad hidden x");
//						adShown [i] = false;		
//						nativeAdsInScroll [i].CancelLoading ();
//						nativeAdsInScroll [i].HideAd ();
//					}
//				} else if ((!horizontalList && Camera.main.WorldToScreenPoint (nativeAdsInScroll [i].transform.position).y < -80) || Camera.main.WorldToScreenPoint (nativeAdsInScroll [i].transform.position).y > Screen.height - 200) {
//					if (adShown [i]) {
////						Debug.Log("Native ad hidden y");
//						adShown [i] = false;				
//						nativeAdsInScroll [i].CancelLoading ();
//						nativeAdsInScroll [i].HideAd ();
//					}
//				} else if (!adShown [i]) {
////					Debug.Log("Native ad loaded");
//					adShown [i] = true;
//					nativeAdsInScroll [i].CancelLoading ();
//					nativeAdsInScroll [i].LoadAd ();
//					currentNativeAd = i;
//				}			
//			}
//		}

        // If we are holding button than do not lerp
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && !buttonPressed)
        {
            holdingRect = true;
            SetCurrentCharacter();
            newLerpPosition = transform.localPosition;
        }

        if (Input.GetMouseButtonUp(0))
            holdingRect = false;

        // If not lerping and velocityis small enough find closest snap point and lerp to it
        if (horizontalList)
        {
            if (!lerping && !holdingRect && Mathf.Abs(transform.parent.GetComponent<ScrollRect>().velocity.x) >= 0f && Mathf.Abs(transform.parent.GetComponent<ScrollRect>().velocity.x) < 100f)
            {
                SetLerpPositionToClosestSnapPoint();
            }
            else
            {
                SetCurrentCharacter();
            }
        }
        else
        {
            if (!lerping && !holdingRect && Mathf.Abs(transform.parent.GetComponent<ScrollRect>().velocity.y) >= 0f && Mathf.Abs(transform.parent.GetComponent<ScrollRect>().velocity.y) < 100f)
            {
                SetLerpPositionToClosestSnapPoint();
            }
            else
            {
                SetCurrentCharacter();
            }
        }

        // Set appropriate for elements in list according to distance from current snap point
        if (horizontalList)
        {
            if (currentCharacter == 0)
            {
                float sb = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] - transform.localPosition.x - currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] - transform.localPosition.x) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

                if (s <= unfocusedElementsScale || s > focusedElementScale)
                    s = unfocusedElementsScale;

                if (sb <= unfocusedElementsScale || sb > focusedElementScale)
                    sb = unfocusedElementsScale;

                listOfCharacters[currentCharacter].transform.localScale = new Vector3(s, s, 1);

                listOfCharacters[currentCharacter + 1].transform.localScale = new Vector3(sb, sb, 1);
            }
            else if (currentCharacter == listOfCharacters.Count - 1)
            {
                float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] - transform.localPosition.x) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float sf = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] - transform.localPosition.x + currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

                if (s <= unfocusedElementsScale || s > focusedElementScale)
                    s = unfocusedElementsScale;

                if (sf <= unfocusedElementsScale || sf > focusedElementScale)
                    sf = unfocusedElementsScale;

                listOfCharacters[currentCharacter - 1].transform.localScale = new Vector3(sf, sf, 1);
                listOfCharacters[currentCharacter].transform.localScale = new Vector3(s, s, 1);
            }
            else
            {
                float sb = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] - transform.localPosition.x - currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] - transform.localPosition.x) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float sf = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] - transform.localPosition.x + currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

                if (s <= unfocusedElementsScale || s > focusedElementScale)
                    s = unfocusedElementsScale;
				
                if (sb <= unfocusedElementsScale || sb > focusedElementScale)
                    sb = unfocusedElementsScale;

                if (sf <= unfocusedElementsScale || sf > focusedElementScale)
                    sf = unfocusedElementsScale;

                listOfCharacters[currentCharacter - 1].transform.localScale = new Vector3(sf, sf, 1);
                listOfCharacters[currentCharacter].transform.localScale = new Vector3(s, s, 1);
                listOfCharacters[currentCharacter + 1].transform.localScale = new Vector3(sb, sb, 1);
            }
        }
        else
        {
            if (currentCharacter == 0)
            {
                float sb = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] + transform.localPosition.y - currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] + transform.localPosition.y) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

                if (s <= unfocusedElementsScale || s > focusedElementScale)
                    s = unfocusedElementsScale;
				
                if (sb <= unfocusedElementsScale || sb > focusedElementScale)
                    sb = unfocusedElementsScale;

                listOfCharacters[currentCharacter].transform.localScale = new Vector3(s, s, 1);
                listOfCharacters[currentCharacter + 1].transform.localScale = new Vector3(sb, sb, 1);
            }
            else if (currentCharacter == listOfCharacters.Count - 1)
            {
                float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] + transform.localPosition.y) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float sf = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] + transform.localPosition.y + currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

                if (s <= unfocusedElementsScale || s > focusedElementScale)
                    s = unfocusedElementsScale;
				
                if (sf <= unfocusedElementsScale || sf > focusedElementScale)
                    sf = unfocusedElementsScale;

                listOfCharacters[currentCharacter - 1].transform.localScale = new Vector3(sf, sf, 1);
                listOfCharacters[currentCharacter].transform.localScale = new Vector3(s, s, 1);
            }
            else
            {
                float sb = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] + transform.localPosition.y - currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float s = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] + transform.localPosition.y) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);
                float sf = Mathf.Abs(Mathf.Abs(snapPositions[currentCharacter] + transform.localPosition.y + currentCharCheckTemp * 2) * (focusedElementScale - unfocusedElementsScale) / Mathf.Abs(currentCharCheckTemp * 2) - focusedElementScale);

                if (s <= unfocusedElementsScale || s > focusedElementScale)
                    s = unfocusedElementsScale;
				
                if (sb <= unfocusedElementsScale || sb > focusedElementScale)
                    sb = unfocusedElementsScale;
				
                if (sf <= unfocusedElementsScale || sf > focusedElementScale)
                    sf = unfocusedElementsScale;

                listOfCharacters[currentCharacter - 1].transform.localScale = new Vector3(sf, sf, 1);
                listOfCharacters[currentCharacter].transform.localScale = new Vector3(s, s, 1);
                listOfCharacters[currentCharacter + 1].transform.localScale = new Vector3(sb, sb, 1);
            }
        }

        // If we let the mouse button and velocity small enough
        if (lerping)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newLerpPosition, lerpingSpeed);

            if (Vector3.Distance(transform.localPosition, newLerpPosition) < 1f)
            {
                transform.localPosition = newLerpPosition;
                transform.parent.GetComponent<ScrollRect>().velocity = new Vector3(0, 0, 0);
                lerping = false;

                for (int i = 0; i < listOfCharacters.Count; i++)
                {
                    if (i != currentCharacter)
                        listOfCharacters[i].transform.localScale = new Vector3(unfocusedElementsScale, unfocusedElementsScale, 1);
                }

            }
        }

        if (horizontalList)
        {
            // Updating arrow buttons
            if (transform.localPosition.x > snapPositions[snapPositions.Count - 1] + spacing / 2)
            {
                SetButtonActive(forwardButton);
                SetIndicatorActive(indicatorEnd);
            }
            else
            {
                SetButtonInactive(forwardButton);
                SetIndicatorInactive(indicatorEnd);
            }

            if (transform.localPosition.x < snapPositions[0] - spacing / 2)
            {
                SetButtonActive(backwardButton);
                SetIndicatorActive(indicatorStart);
            }
            else
            {
                SetButtonInactive(backwardButton);
                SetIndicatorInactive(indicatorStart);
            }
        }
        else
        {
            // Updating arrow buttons
            if (transform.localPosition.y > snapPositions[snapPositions.Count - 1] + spacing / 2)
            {
                SetButtonActive(backwardButton);
                SetIndicatorActive(indicatorStart);
            }
            else
            {
                SetButtonInactive(backwardButton);
                SetIndicatorInactive(indicatorStart);
            }
			
            if (transform.localPosition.y < snapPositions[0] - spacing / 2)
            {
                SetButtonActive(forwardButton);
                SetIndicatorActive(indicatorEnd);
            }
            else
            {
                SetButtonInactive(forwardButton);
                SetIndicatorInactive(indicatorEnd);
            }
        }
    }

    //	public void LoadCurrentAdInScroll (float delay)
    //	{
    //		if (nativeAdsInScroll == null)
    //			return;
    //		CancelAllAds ();
    //
    //		nativeAdsInScroll [currentNativeAd].LoadAdWithDelay (delay);
    //		adShown [currentNativeAd] = true;
    //		currentNativeAd = currentNativeAd;
    //	}
    //
    //	public void CancelAllAds ()
    //	{
    //		if (nativeAdsInScroll == null)
    //			return;
    //		for (int i = 0; i < nativeAdsInScroll.Length; i++) {
    ////			Debug.Log("Index " + i + "AdShown " + adShown.Length);
    //			adShown [i] = false;
    //			nativeAdsInScroll [i].CancelLoading ();
    //			nativeAdsInScroll [i].HideAd ();
    //		}
    //	}

}





















