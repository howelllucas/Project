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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdvancedMobilePaint.Tools;

namespace AdvancedMobilePaint
{
    public class PaintUndoManager : MonoBehaviour
    {
	
        /// <summary>
        /// Pokazivac na AMP skriptu.
        /// Setuje se automatski ako je ova skripta pridruzena AMP objektu.
        /// </summary>
        public AdvancedMobilePaint paintEngine;
        /// <summary>
        /// The doing work flag.Used internaly.
        /// </summary>
        public bool doingWork = false;
        //stek za undo korake
        Stack<UStep> steps;
        //stek za redo korake
        Stack<UStep> redoSteps;
        /// <summary>
        /// Dubina steka u broju koraka koja moze da se pamti.
        /// </summary>
        public int stackDepth = int.MaxValue;
        /// <summary>
        /// Flag koji oznacava da li je stek popunjen tj da li je moguce dodati undo korake;
        /// </summary>
        public bool stackFull = false;

        [Header("Stickers")]
        //Stickers
		public GameObject stickerPrefab;
        public Transform stickersHolder;
        public Transform stickerButtonsHolder;
		
		
        Stack<string> undoSnapshots;
		
        public int snapshotFrequency = 10;
		
        public int totalSnaps = 0;
		
        public int snapCounter = 0;
		
        public int numberOfStepsSinceLastSnapshot = 0;
        // Use this for initialization
        void Awake()
        {
            steps = new Stack<UStep>();
            redoSteps = new Stack<UStep>();
            undoSnapshots = new Stack<string>();
            if (paintEngine == null)
            {
                if (gameObject.GetComponent<AdvancedMobilePaint>() != null)
                {
                    paintEngine = gameObject.GetComponent<AdvancedMobilePaint>();
                    paintEngine.undoEnabled = true;
                    paintEngine.undoController = gameObject.GetComponent<PaintUndoManager>();
                }
                else
                    Debug.LogError("AMP: PaintUndoManger Cant find paint engine!");
				
            }
            if (!Directory.Exists(Application.persistentDataPath + "/AMP_cashe"))
                Directory.CreateDirectory(Application.persistentDataPath + "/AMP_cashe");
            else
            {
                string[] files = Directory.GetFiles(Application.persistentDataPath + "/AMP_cashe"); 
                for (int i = 0; i < files.Length; i++)
                {
                    File.Delete(files[i]);
                }
            }
        }

        void MakeSnapshot()
        {
            totalSnaps++;
            File.WriteAllBytes(Application.persistentDataPath + "/AMP_cashe/" + totalSnaps.ToString() + ".bytes", paintEngine.pixels);
            undoSnapshots.Push(totalSnaps.ToString());
            #if UNITY_EDITOR
//		Debug.Log ("SNAP ADDED, total snaps "+totalSnaps);
            #endif
        }

        void LoadSnapshot()
        {
            paintEngine.pixels = File.ReadAllBytes(Application.persistentDataPath + "/AMP_cashe/" + undoSnapshots.Peek().ToString() + ".bytes");
            #if UNITY_EDITOR
//			Debug.Log ("SNAP retrived, total snaps "+totalSnaps);
            #endif
        }

        void ClearSnapshot()
        {

            undoSnapshots.Pop();
            paintEngine.pixels = File.ReadAllBytes(Application.persistentDataPath + "/AMP_cashe/" + totalSnaps.ToString() + ".bytes");
            File.Delete(Application.persistentDataPath + "/AMP_cashe/" + totalSnaps.ToString() + ".bytes");
            totalSnaps--;
            #if UNITY_EDITOR
//			Debug.Log ("SNAP destroyed, total snaps "+totalSnaps);
            #endif
        }

        public void ClearSnapshots()
        {
            totalSnaps = 0;
            snapCounter = 0;
            numberOfStepsSinceLastSnapshot = 0;
            undoSnapshots.Clear();
            if (!Directory.Exists(Application.persistentDataPath + "/AMP_cashe"))
                Directory.CreateDirectory(Application.persistentDataPath + "/AMP_cashe");
            else
            {
                string[] files = Directory.GetFiles(Application.persistentDataPath + "/AMP_cashe"); 
                for (int i = 0; i < files.Length; i++)
                {
                    File.Delete(files[i]);
                }
            }
        }
        // Use this for initialization (ovde mozete da dodate neku custom inicijalizaciju)
	
        //	void Start () {
        //
        //	}

        /// <summary>
        /// Dodaje nov korak u stek ako stek nije pun.
        /// </summary>
        /// <param name="step">Step.</param>
        public void AddStep(UStep step)
        {

            if (step != null && steps != null)
            {
                if (steps.Count < stackDepth)
                {
                    steps.Push(step);
                    if (steps.Count % snapshotFrequency == 0)
                    {
                        MakeSnapshot();
//				snapCounter++;
                    }
                    #if UNITY_EDITOR
                    Debug.Log("AMP: NEW STEP ADDED!");
                    #endif
                }
                else
                {
                    //#if UNITY_EDITOR
                    stackFull = true;
                    Debug.Log("AMP: StackFull");
                    //#endif
                }
            }
            else
            {
                //#if UNITY_EDITOR
                //normalno nikad ne bi trebalo da se izvrsi ova grana
                Debug.Log("AMP: ERROR IN ADDING NEW STEP!");
                //#endif
            }	
        }

        /// <summary>
        /// Brise sve stekove (sve korake).
        /// </summary>
        public void ClearSteps()
        {
            steps.Clear();
            redoSteps.Clear();
            stackFull = false;
            ClearSnapshots();
		
        }
	
        //	public void RetriveCachedState()
        //	{
        //
        //	}
        /// <summary>
        /// Reiscrtavanje svih koraka.
        /// Ne ekstenduj ovu funkciju [INTERNAL]; 
        /// </summary>
        void UndoRedrawSteps()
        {
            Stack <UStep> tmpStack = new Stack<UStep>();
            //save previous settings
            UStep settings = new UStep();
            settings.SetStepPropertiesFromEngine(paintEngine);
            //get all steps from stack bottom to top
//		while(steps.Count!=0)
//		{
//			tmpStack.Push(steps.Pop());
//		}
//		numberOfStepsSinceLastSnapshot=0;
//		numberOfStepsSinceLastSnapshot=(steps.Count+1)%snapshotFrequency;
//			if(totalSnaps>0 && numberOfStepsSinceLastSnapshot!=0)
//			{
////				Debug.Log ("sdfsdfsdfsdfdsfd");
//			
//				LoadSnapshot();
//			}
//			else
//			{
////				Debug.Log ("asdsadsa");
//				if(undoSnapshots.Count>0)
//					ClearSnapshot();
//				if(totalSnaps>0)
//					LoadSnapshot();
//				else
//					paintEngine.CopyUndoPixels(paintEngine.pixels);//<---?
//					
//				numberOfStepsSinceLastSnapshot=(steps.Count<=snapshotFrequency-1)?(steps.Count):snapshotFrequency-1;
////				Debug.Log ("NUM OF SNAPS "+numberOfStepsSinceLastSnapshot);
//				
//			}
            if (numberOfStepsSinceLastSnapshot != 0)
            {
//			numberOfStepsSinceLastSnapshot--;
			
                while (numberOfStepsSinceLastSnapshot > 0 && steps.Count > 0)
                {
                    tmpStack.Push(steps.Pop());
                    numberOfStepsSinceLastSnapshot--;
                    Debug.Log("Add to tmp stack!");
                }
                Debug.Log("UNDO WITH SC");
            }
            else
            {
                Debug.Log("NO UNDO WITH SC");
                if (steps.Count > 0)
                {
                    tmpStack.Push(steps.Pop());
                }
                if (undoSnapshots.Count > 0)
                {
                    ClearSnapshot();
				
                }
                if (undoSnapshots.Count > 0)
                {
                    LoadSnapshot();
					
                }
                numberOfStepsSinceLastSnapshot = (steps.Count <= snapshotFrequency - 1) ? steps.Count : snapshotFrequency - 1;
                while (numberOfStepsSinceLastSnapshot > 0 && steps.Count > 0)
                {
                    tmpStack.Push(steps.Pop());
                    numberOfStepsSinceLastSnapshot--;
                }	

            }
		
            //redraw all the steps in order of drawing
            Debug.Log("TMP COUNT " + tmpStack.Count);
            while (tmpStack.Count != 0)
            {
                UStep tmp = tmpStack.Pop();
                //skip possible non-drawing steps
                if (tmp.type >= 0 || tmp.type < 5)
                {
//				Debug.Log ("UNDO STEP EXEC");
                    //setup engine in order to read step
                    //TODO  compare current and step settings, if they are same than read current custom brush load can be avoided
                    bool sameBrush = false;
                    if (tmp.brushTexture == paintEngine.customBrush)
                        sameBrush = true;
                    #if UNITY_EDITOR
                    Debug.Log("Same brush in step " + sameBrush);
                    #endif
                    tmp.SetPropertiesFromStep(paintEngine);
				
                    //
                    //bitmap based brushes redraw
                    if (tmp.type == 0)
                    {
                        switch (tmp.brushMode)
                        {
                            case BrushProperties.Default:
//						Debug.Log ("DEF MODE");
                                if (!sameBrush)
                                    paintEngine.ReadCurrentCustomBrush();
                                for (int i = 0; i < tmp.drawCoordinates.Count; i++)
                                {
                                    BitmapBrushesTools.DrawCustomBrush2((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                                    if (i > 0)
                                    {
                                        BitmapBrushesTools.DrawLineWithBrush(new Vector2((int)tmp.drawCoordinates[i - 1].x, (int)tmp.drawCoordinates[i - 1].y), new Vector2((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y), paintEngine);
                                    }
                                }
                                break;
                            case BrushProperties.Simple:
							//FIXME UNDO
                                if (!sameBrush)
                                    paintEngine.ReadCurrentCustomBrush();
                                for (int i = 0; i < tmp.drawCoordinates.Count; i++)
                                {
                                    BitmapBrushesTools.DrawCustomBrush2((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                                    if (i >= 0)
                                    {
                                        BitmapBrushesTools.DrawLineWithBrush(new Vector2((int)tmp.drawCoordinates[i - 1].x, (int)tmp.drawCoordinates[i - 1].y), new Vector2((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y), paintEngine);
                                    }
                                }
                                break;
                            case BrushProperties.Pattern:
                                if (!sameBrush)
                                    paintEngine.ReadCurrentCustomBrush();
                                paintEngine.ReadCurrentCustomPattern(tmp.patternTexture);
                                for (int i = 0; i < tmp.drawCoordinates.Count; i++)
                                    BitmapBrushesTools.DrawCustomBrush2((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                                break;
                            default:
                                break;
				
                        }
                    }
                    else if (tmp.type == 2)
                    {
                        for (int i = 0; i < tmp.drawCoordinates.Count; i++)
                            if (paintEngine.useSmartFloodFill)
                            {
                                FloodFillTools.FloodFillAutoMaskWithThreshold((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                            }
                            else
                            {
                                if (tmp.useTreshold)
                                {	
                                    if (tmp.useMaskLayerOnly)
                                    {
                                        FloodFillTools.FloodFillMaskOnlyWithThreshold((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                                    }
                                    else
                                    {
                                        FloodFillTools.FloodFillWithTreshold((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                                    }
                                }
                                else
                                {
                                    if (tmp.useMaskLayerOnly)
                                    {
                                        FloodFillTools.FloodFillMaskOnly((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                                    }
                                    else
                                    {
                                        FloodFillTools.FloodFill((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                                    }
                                }
                            }
                    }
                    else if (tmp.type == 1)
                    {
						
						
                        if (tmp.brushMode == BrushProperties.Default)
                        {
                            if (tmp.vectorBrushType == VectorBrush.Circle)
                            {
                                for (int i = 0; i < tmp.drawCoordinates.Count; i++)
                                    VectorBrushesTools.DrawCircle((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                            }
                            else
                            {
                                for (int i = 0; i < tmp.drawCoordinates.Count; i++)
                                    VectorBrushesTools.DrawRectangle((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                            }
							
                        }
                        else if (tmp.brushMode == BrushProperties.Pattern)
                        {
                            paintEngine.ReadCurrentCustomPattern(tmp.patternTexture);
                            if (tmp.vectorBrushType == VectorBrush.Circle)
                            {
                                for (int i = 0; i < tmp.drawCoordinates.Count; i++)
                                    VectorBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                            }
                            else
                            {
                                for (int i = 0; i < tmp.drawCoordinates.Count; i++)
                                    VectorBrushesTools.DrawPatternRectangle((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                            }
                        }
						
						
                    }
                    else if (tmp.type == 4)
                    {
                        if (paintEngine.multitouchEnabled)
                            DrawMultiTouchLine(tmp);
                        else
                            DrawSingleTouchLine(tmp);
							
                    }
                    else if (tmp.type == 3) // FIXME Cackano! Privremeno za StampBrush
                    {
//						Debug.Log ("STAMP MODE");
                        if (!sameBrush)
                            paintEngine.ReadCurrentCustomBrush();
                        for (int i = 0; i < tmp.drawCoordinates.Count; i++)
                        {
                            BitmapBrushesTools.DrawCustomBrush2((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                            if (i > 0)
                            {
                                BitmapBrushesTools.DrawStampLineWithBrush(new Vector2((int)tmp.drawCoordinates[i - 1].x, (int)tmp.drawCoordinates[i - 1].y), new Vector2((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y), paintEngine);
                            }
                        }
                    }
                    //return it to stack
                    steps.Push(tmp);
                }
                else
                    steps.Push(tmp);
            }
            //load and apply changes
            paintEngine.tex.LoadRawTextureData(paintEngine.pixels);
            paintEngine.tex.Apply();
            //restore modes
            bool sameBrushUsed = false;
            if (settings.brushTexture == paintEngine.customBrush)
                sameBrushUsed = true;
            settings.SetPropertiesFromStep(paintEngine);
            if (paintEngine.drawMode == DrawMode.Stamp || paintEngine.drawMode == DrawMode.CustomBrush) //FIXME Cackano!
			if (!sameBrushUsed)
                paintEngine.ReadCurrentCustomBrush();
        }

        public void DrawMultiTouchLine(UStep tmp)
        {
			
            if (tmp.lineEgdeSize == 0)
            {
//				Debug.Log("DRAW MULTI TOUCH LINE UNDO MANAGER!");
                if (tmp.isPatternLine)
                {
                    for (int j = 0; j < tmp.touchCoordinates.Count; j++)
                    {
                        VectorBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].y, paintEngine);
                        for (int i = 1; i < tmp.touchCoordinates[j].coordinatesIndex.Count; i++)
                        {
                            BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 1]], tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]], paintEngine.brushSize * 2, true, paintEngine.patternBrushBytes, paintEngine);
                            VectorBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]].y, paintEngine);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < tmp.touchCoordinates.Count; j++)
                    {
                        VectorBrushesTools.DrawCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].y, paintEngine);
                        for (int i = 1; i < tmp.touchCoordinates[j].coordinatesIndex.Count; i++)
                        {
                            BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 1]], tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]], paintEngine.brushSize * 2, false, null, paintEngine);
                            VectorBrushesTools.DrawCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]].y, paintEngine);
                        }
                    }
                }
            }
            else
            {
                if (tmp.isPatternLine)
                {
                    for (int j = 0; j < tmp.touchCoordinates.Count; j++)
                    {
                        BitmapBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].y, paintEngine.customBrushBytes, paintEngine.brushSize + paintEngine.lineEdgeSize, paintEngine);
                        VectorBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].y, paintEngine);
                        for (int i = 1; i < tmp.touchCoordinates[j].coordinatesIndex.Count; i++)
                        {
                            BitmapBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]].y, paintEngine.customBrushBytes, paintEngine.brushSize + paintEngine.lineEdgeSize, paintEngine);
                            BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 1]], tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]], paintEngine.brushSize * 2 + paintEngine.lineEdgeSize * 2, true, paintEngine.customBrushBytes, paintEngine);
                            //
                            if (i > 1)
                            {
                                BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 2]], tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 1]], paintEngine.brushSize * 2 + paintEngine.lineEdgeSize * 2, true, paintEngine.customBrushBytes, paintEngine);
                                BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 2]], tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 1]], paintEngine.brushSize * 2, true, paintEngine.patternBrushBytes, paintEngine);
                                VectorBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 2]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 2]].y, paintEngine);
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < tmp.touchCoordinates.Count; j++)
                    {
                        VectorBrushesTools.DrawCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[0]].y, paintEngine);
                        for (int i = 1; i < tmp.touchCoordinates[j].coordinatesIndex.Count; i++)
                        {
                            BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i - 1]], tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]], paintEngine.brushSize * 2, false, null, paintEngine);
                            VectorBrushesTools.DrawCircle((int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]].x, (int)tmp.drawCoordinates[tmp.touchCoordinates[j].coordinatesIndex[i]].y, paintEngine);
                        }
                    }
                }
            }
        }

        public void DrawSingleTouchLine(UStep tmp)
        {
            if (tmp.lineEgdeSize == 0)
            {
                if (tmp.isPatternLine)
                {
                    VectorBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[0].x, (int)tmp.drawCoordinates[0].y, paintEngine);
                    for (int i = 1; i < tmp.drawCoordinates.Count; i++)
                    {
                        BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[i - 1], tmp.drawCoordinates[i], paintEngine.brushSize * 2, true, paintEngine.patternBrushBytes, paintEngine);
                        VectorBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                    }
                }
                else
                {
                    VectorBrushesTools.DrawCircle((int)tmp.drawCoordinates[0].x, (int)tmp.drawCoordinates[0].y, paintEngine);
                    for (int i = 1; i < tmp.drawCoordinates.Count; i++)
                    {
                        BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[i - 1], tmp.drawCoordinates[i], paintEngine.brushSize * 2, false, null, paintEngine);
                        VectorBrushesTools.DrawCircle((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                    }
                }
            }
            else
            {
                if (tmp.isPatternLine)
                {
                    BitmapBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[0].x, (int)tmp.drawCoordinates[0].y, paintEngine.customBrushBytes, paintEngine.brushSize + paintEngine.lineEdgeSize, paintEngine);
                    VectorBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[0].x, (int)tmp.drawCoordinates[0].y, paintEngine);
                    for (int i = 1; i < tmp.drawCoordinates.Count; i++)
                    {
                        //									paintEngine.DrawLineBrush(tmp.drawCoordinates[i-1],tmp.drawCoordinates[i],paintEngine.brushSize*2,true,paintEngine.patternBrushBytes);
                        //									paintEngine.DrawPatternCircle((int)tmp.drawCoordinates[i].x,(int)tmp.drawCoordinates[i].y);
                        BitmapBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine.customBrushBytes, paintEngine.brushSize + paintEngine.lineEdgeSize, paintEngine);
                        BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[i - 1], tmp.drawCoordinates[i], paintEngine.brushSize * 2 + paintEngine.lineEdgeSize * 2, true, paintEngine.customBrushBytes, paintEngine);
                        //
                        if (i > 1)
                        {
                            BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[i - 2], tmp.drawCoordinates[i - 1], paintEngine.brushSize * 2 + paintEngine.lineEdgeSize * 2, true, paintEngine.customBrushBytes, paintEngine);
                            BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[i - 2], tmp.drawCoordinates[i - 1], paintEngine.brushSize * 2, true, paintEngine.patternBrushBytes, paintEngine);
                            VectorBrushesTools.DrawPatternCircle((int)tmp.drawCoordinates[i - 2].x, (int)tmp.drawCoordinates[i - 2].y, paintEngine);
                        }
                    }
                }
                else
                {
                    VectorBrushesTools.DrawCircle((int)tmp.drawCoordinates[0].x, (int)tmp.drawCoordinates[0].y, paintEngine);
                    for (int i = 1; i < tmp.drawCoordinates.Count; i++)
                    {
                        BitmapBrushesTools.DrawLineBrush(tmp.drawCoordinates[i - 1], tmp.drawCoordinates[i], paintEngine.brushSize * 2, false, null, paintEngine);
                        VectorBrushesTools.DrawCircle((int)tmp.drawCoordinates[i].x, (int)tmp.drawCoordinates[i].y, paintEngine);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the sticker transform.
        /// </summary>
        void UpdateStickerTransform(UStep tmp)
        {
//			Debug.Log("RepositionStickerUndo");
            tmp.sticker.transform.localScale = tmp.stickerScale;
            tmp.sticker.transform.localPosition = tmp.stickerLocalPos;
            tmp.sticker.transform.localRotation = tmp.stickerRotation;
        }

        /// <summary>
        /// Instantiates the sticker.
        /// </summary>
        void InstantiateSticker(UStep tmp)
        {
//			Debug.Log("InstantiateStickerUndo");
//			GameObject clone = (GameObject) Instantiate (stickerPrefab,Vector3.zero,Quaternion.identity);
//			clone.transform.SetParent(stickersHolder);
//			clone.transform.localScale = tmp.stickerScale;
//			clone.transform.localPosition = tmp.stickerLocalPos;
//			clone.transform.localRotation = tmp.stickerRotation;
//			Sprite s = stickerButtonsHolder.GetChild(tmp.stickerDeleted).GetComponent<Image>().sprite;
//			clone.GetComponent<Image>().sprite = s;
//			StickersController.SelectSticker(clone);
            tmp.sticker.SetActive(true);
            StickersController.SelectSticker(tmp.sticker);

        }

        /// <summary>
        /// Deletes the sticker.
        /// </summary>
        void DeleteSticker(UStep tmp)
        {
//			Debug.Log("DeleteStickerUndo");
//			Destroy(tmp.sticker);
            tmp.sticker.SetActive(false);
            StickersController.SelectSticker(null);
        }

        /// <summary>
        /// Undo operacija.
        /// </summary>
        public void UndoLastStep()
        {
            {
//				Debug.Log ("Undo step" + steps.Count.ToString ());
                if (!doingWork && steps.Count > 0)
                {
                    doingWork = true;
                    //pop top of stack and push it to redo stack
                    //we dont need it for undo draw
                    UStep step = steps.Pop();
                    //cashe snapshot retrival
                    numberOfStepsSinceLastSnapshot = 0;
                    numberOfStepsSinceLastSnapshot = (steps.Count + 1) % snapshotFrequency;
                    if (totalSnaps > 0 && numberOfStepsSinceLastSnapshot != 0)
                    {
                        Debug.Log("sdfsdfsdfsdfdsfd");
						
                        LoadSnapshot();
                    }
                    else
                    {
                        Debug.Log("asdsadsa");
                        if (undoSnapshots.Count > 0)
                            ClearSnapshot();
                        if (totalSnaps > 0)
                            LoadSnapshot();
                        else
                            paintEngine.CopyUndoPixels(paintEngine.pixels);//<---?
						
                        numberOfStepsSinceLastSnapshot = (steps.Count <= snapshotFrequency - 1) ? (steps.Count) : snapshotFrequency - 1;
                        Debug.Log("NUM OF SNAPS " + numberOfStepsSinceLastSnapshot);
						
                    }
					
                    //
                    switch (step.type)
                    {
                        case -3:
//						Debug.Log("Undo -3");
                            InstantiateSticker(step);
                            doingWork = false;
                            break;
                        case -2:
//						Debug.Log("Undo -2");
                            UpdateStickerTransform(step);
                            doingWork = false;
                            break;
                        case -1:
//						Debug.Log("Undo -1");
                            DeleteSticker(step);
                            doingWork = false;
                            break;
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
						//restore initial texture state
//						paintEngine.CopyUndoPixels(paintEngine.pixels);//<---
						//redraw all steps from bottom to top
                            UndoRedrawSteps();
                            doingWork = false;
                            break;
                        default:
                            break;
                    }
                    //push this step on undo stack
                    redoSteps.Push(step);
					
                }
            }
			
        }

        /// <summary>
        /// Redo operacija.
        /// </summary>
        public void RedoLastStep()
        {
            {
                //Debug.Log ("Redo step" + redoSteps.Count.ToString ());
                if (!doingWork && redoSteps.Count > 0)
                {
                    doingWork = true;
                    UStep step = redoSteps.Pop();
					
                    switch (step.type)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            {//BitmapDraw
                                UStep settings = new UStep();
                                settings.SetStepPropertiesFromEngine(paintEngine);
						
						
                                if (step.type > -1 || step.type < 5)
                                {
                                    Debug.Log("Redo STEP EXEC");
                                    step.SetPropertiesFromStep(paintEngine);
                                    if (step.type == 0)
                                    {
                                        switch (step.brushMode)
                                        {
                                            case BrushProperties.Default:
                                                Debug.Log("DEF-R MODE");
                                                paintEngine.ReadCurrentCustomBrush();
                                                for (int i = 0; i < step.drawCoordinates.Count; i++)
                                                    BitmapBrushesTools.DrawCustomBrush2((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                                break;
                                            case BrushProperties.Simple:
									//FIX REDO
                                                paintEngine.ReadCurrentCustomBrush();
                                                for (int i = 0; i < step.drawCoordinates.Count; i++)
                                                    BitmapBrushesTools.DrawCustomBrush2((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                                break;
                                            case BrushProperties.Pattern:
                                                paintEngine.ReadCurrentCustomBrush();
                                                paintEngine.ReadCurrentCustomPattern(step.patternTexture);
                                                for (int i = 0; i < step.drawCoordinates.Count; i++)
                                                    BitmapBrushesTools.DrawCustomBrush2((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                                break;
                                            default:
                                                break;
									
                                        }
                                    }
                                    else if (step.type == 2)
                                    {
                                        for (int i = 0; i < step.drawCoordinates.Count; i++)
                                            if (paintEngine.useSmartFloodFill)
                                            {
                                                FloodFillTools.FloodFillAutoMaskWithThreshold((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                            }
                                            else
                                            {
                                                if (step.useTreshold)
                                                {
                                                    if (step.useMaskLayerOnly)
                                                    {
                                                        FloodFillTools.FloodFillMaskOnlyWithThreshold((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                                    }
                                                    else
                                                    {
                                                        FloodFillTools.FloodFillWithTreshold((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                                    }
                                                }
                                                else
                                                {
                                                    if (step.useMaskLayerOnly)
                                                    {
                                                        FloodFillTools.FloodFillMaskOnly((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                                    }
                                                    else
                                                    {
                                                        FloodFillTools.FloodFill((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                                    }
                                                }
                                            }
                                    }
                                    else if (step.type == 1)
                                    {
								
								
                                        if (step.brushMode == BrushProperties.Default)
                                        {
                                            if (step.vectorBrushType == VectorBrush.Circle)
                                            {
                                                for (int i = 0; i < step.drawCoordinates.Count; i++)
                                                    VectorBrushesTools.DrawCircle((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                            }
                                            else
                                            {
                                                for (int i = 0; i < step.drawCoordinates.Count; i++)
                                                    VectorBrushesTools.DrawRectangle((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);	
                                            }
								
                                        }
                                        else if (step.brushMode == BrushProperties.Pattern)
                                        {
                                            paintEngine.ReadCurrentCustomPattern(step.patternTexture);
                                            if (step.vectorBrushType == VectorBrush.Circle)
                                            {
                                                for (int i = 0; i < step.drawCoordinates.Count; i++)
                                                    VectorBrushesTools.DrawPatternCircle((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                            }
                                            else
                                            {
                                                for (int i = 0; i < step.drawCoordinates.Count; i++)
                                                    VectorBrushesTools.DrawPatternRectangle((int)step.drawCoordinates[i].x, (int)step.drawCoordinates[i].y, paintEngine);
                                            }
                                        }
								
								
                                    }
                                    else if (step.type == 4)
                                    {

                                        if (paintEngine.multitouchEnabled)
                                            DrawMultiTouchLine(step);
                                        else
                                            DrawSingleTouchLine(step);
                                    }
							
                                    paintEngine.tex.LoadRawTextureData(paintEngine.pixels);
                                    paintEngine.tex.Apply();
                                    //return it to stack
                                    steps.Push(step);
                                }
                                else
                                    steps.Push(step);
						
                                settings.SetPropertiesFromStep(paintEngine);
                            }
                            break;
                        default:
                            break;
                    }
					
					
                    //steps.Push (step);
                    doingWork = false;
                }
				
            }
        }
		

    }

    /// <summary>
    /// UStep klasa za cuvanje podesavanja AMP-a za UNDO i druge potrebe.Interno se koristi.
    /// </summary>
    public class UStep
    {
        public int type;
        //step type ,if extending this class use negative integers for added custom types
        //-3 sticker deleted - call Instantiate
        //-2 sticker transform edited
        //-1 sticker instantiated - call Delete
        //0 draw bitmap - bitmap brush
        //1 draw bitmap - vector brush
        //2 draw bitmap - floodfill brush
        //3 ? draw bitmap - Stamp //FIXME cackano! privremeno
        //4 draw line
        public BrushProperties brushMode;
        public DrawMode drawMode;
        public int brushSize;
        // only for vector brushes
        public Texture2D brushTexture;
        public Texture2D patternTexture;
        public bool useAdditiveColors;
        public bool canDrawOnBlack;
        public Color paintColor;
        public bool useLockArea;
        public bool useCustomBrushAlpha;
        public int vectorBrushIndex;
        public bool isFloodFill;
        public bool connectBrushStrokes;
        //public bool interpolation;
        public bool useMaskLayerOnly;
        public bool useTreshold;
        public bool treshold;
        public bool useMaskImage;
        public float brushAlphaStrength;
        public List<Vector2> drawCoordinates;
        //
        public VectorBrush vectorBrushType;
        public int brushWidth;
        public int brushHeight;
        public bool isLine;
        public int lineEgdeSize;
        public bool isPatternLine;
        public List<TouchCoordinates> touchCoordinates;

        //Sticker variables
        public Vector2 stickerLocalPos;
        public Vector3 stickerScale;
        public Quaternion stickerRotation;
        //		public int stickerDeleted;
        public GameObject sticker;

        /// <summary>
        /// Sets the engine properties from step.
        /// </summary>
        /// <param name="paintEngine">Paint engine.</param>
        public void SetPropertiesFromStep(AdvancedMobilePaint paintEngine)
        {
            paintEngine.brushMode = this.brushMode;
            paintEngine.drawMode = this.drawMode;
            paintEngine.brushSize = this.brushSize;
            paintEngine.customBrush = this.brushTexture;
            paintEngine.pattenTexture = this.patternTexture;
            paintEngine.useAdditiveColors = this.useAdditiveColors;
            paintEngine.canDrawOnBlack = this.canDrawOnBlack;
            paintEngine.paintColor = this.paintColor;
            paintEngine.useLockArea = this.useLockArea;
            paintEngine.useCustomBrushAlpha = this.useCustomBrushAlpha;
            paintEngine.connectBrushStokes = this.connectBrushStrokes;
            //paintEngine.doInterpolation=this.interpolation;
            paintEngine.useMaskLayerOnly = this.useMaskLayerOnly;
            paintEngine.useThreshold = this.useTreshold;
            paintEngine.useMaskImage = this.useMaskImage;
            paintEngine.brushAlphaStrength = this.brushAlphaStrength;
            paintEngine.vectorBrushType = this.vectorBrushType;
            paintEngine.customBrushWidth = this.brushWidth;
            paintEngine.customBrushHeight = this.brushHeight;
            paintEngine.isLinePaint = this.isLine;
            paintEngine.lineEdgeSize = this.lineEgdeSize;
            paintEngine.isPatternLine = this.isPatternLine;
            //
		
        }

        /// <summary>
        /// Sets the step properties from engine.
        /// </summary>
        /// <param name="paintEngine">Paint engine.</param>
        public void SetStepPropertiesFromEngine(AdvancedMobilePaint paintEngine)
        {
            this.brushMode = paintEngine.brushMode;
            this.drawMode = paintEngine.drawMode;
            this.brushSize = paintEngine.brushSize;
            this.brushTexture = paintEngine.customBrush;
            this.patternTexture = paintEngine.pattenTexture;
            this.useAdditiveColors = paintEngine.useAdditiveColors;
            this.canDrawOnBlack = paintEngine.canDrawOnBlack;
            this.paintColor = paintEngine.paintColor;
            this.useLockArea = paintEngine.useLockArea;
            this.useCustomBrushAlpha = paintEngine.useCustomBrushAlpha;
            this.connectBrushStrokes = paintEngine.connectBrushStokes;
            //this.interpolation=paintEngine.doInterpolation;
            this.useMaskLayerOnly = paintEngine.useMaskLayerOnly;
            this.useTreshold = paintEngine.useThreshold;
            this.useMaskImage = paintEngine.useMaskImage;
            this.brushAlphaStrength = paintEngine.brushAlphaStrength;
            this.vectorBrushType = paintEngine.vectorBrushType;
            this.brushHeight = paintEngine.customBrushHeight;
            this.brushWidth = paintEngine.customBrushWidth;
            this.isLine = paintEngine.isLinePaint;
            this.isPatternLine = paintEngine.isPatternLine;
            this.lineEgdeSize = paintEngine.lineEdgeSize;
        }

        public void AddTouchCoordinate(int fId)
        {
            Debug.Log("ADD TOUCH COORDINATE");
            if (touchCoordinates == null)
            {
                touchCoordinates = new List<TouchCoordinates>();
            }
            if (touchCoordinates.Count < 1)
            {
                Debug.Log("ADD TOUCH COORDINATE FIRST TIME");
                TouchCoordinates tc = new TouchCoordinates();
                tc.coordinatesIndex = new List<int>();
                tc.fingerId = fId;
                tc.coordinatesIndex.Add(drawCoordinates.Count - 1);
                touchCoordinates.Add(tc);
				
            }
            else
            {
                //
                bool yes = false;
                for (int i = 0; i < touchCoordinates.Count; i++)
                {
                    if (touchCoordinates[i].fingerId == fId)
                    {
                        Debug.Log("ADD TOUCH COORDINATE EXISTING");
                        touchCoordinates[i].coordinatesIndex.Add(drawCoordinates.Count - 1);
						
                        yes = true;
                        break;
                    }
                }
                if (!yes)
                {
                    Debug.Log("ADD TOUCH NEW");
                    TouchCoordinates tc = new TouchCoordinates();
                    tc.coordinatesIndex = new List<int>();
                    tc.fingerId = fId;
                    tc.coordinatesIndex.Add(drawCoordinates.Count - 1);
                    touchCoordinates.Add(tc);
                }
            }
			
			
        }
    }

    public class TouchCoordinates
    {
        public int fingerId;
        public List<int> coordinatesIndex;
    }

}
