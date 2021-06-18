using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battles
{
    [System.Serializable]
    public class PropertyBlockColorSetter
    {
        //Setter的name
        public string name;
        public string[] setColorsKeyArr = new string[] { };
        //Block没有缺省值 需要手动设置
        public Color defaultColor = Color.white;
        private int[] setColorsIDArr;
        private List<MaterialData> matDatas = new List<MaterialData>();
        private Color color;

        public void Init()
        {
            setColorsIDArr = new int[setColorsKeyArr.Length];
            for (int i = 0; i < setColorsKeyArr.Length; i++)
            {
                setColorsIDArr[i] = Shader.PropertyToID(setColorsKeyArr[i]);
            }
            color = defaultColor;
        }

        public void CollectGameObjectMaterial(GameObject go, bool collectChildren = true)
        {
            if (setColorsIDArr == null || setColorsIDArr.Length <= 0)
                return;

            var rendererList = go.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < rendererList.Length; i++)
            {
                var renderer = rendererList[i];
                if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
                {
                    if (matDatas.Exists(m => m.Renderer == renderer))
                        continue;

                    MaterialData data = new MaterialData(renderer);
                    for (int k = 0; k < setColorsIDArr.Length; k++)
                    {
                        data.AddColorParam(setColorsIDArr[k]);
                    }
                    if (!data.IsEmpty)
                        matDatas.Add(data);
                }
            }

        }

        public void SetColor(Color color, bool doKill = true)
        {
            if (doKill)
                DOKill();
            this.color = color;
            for (int i = 0; i < matDatas.Count; i++)
            {
                matDatas[i].SetColor(color);
            }
        }

        public void SetStone(bool isStone)
        {
            for (int i = 0; i < matDatas.Count; i++)
            {
                matDatas[i].SetStone(isStone);
            }
        }

        public Color GetColor()
        {
            return color;
        }

        public void ResetAll()
        {
            SetColor(defaultColor);
        }

        public void ClearAll()
        {
            ResetAll();
            matDatas.Clear();
        }

        private class MaterialData
        {
            public Renderer Renderer { get; private set; }
            private MaterialPropertyBlock block;
            private List<ColorParam> paramList = new List<ColorParam>();
            public bool IsEmpty { get { return paramList.Count <= 0; } }

            public MaterialData(Renderer renderer)
            {
                this.Renderer = renderer;
                block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
            }

            public void AddColorParam(int colorID)
            {
                if (paramList.Exists(p => p.ColorID == colorID))
                    return;

                var param = new ColorParam();
                param.Init(block, colorID);
                paramList.Add(param);
            }

            public void SetColor(Color color)
            {
                if (Renderer == null)
                    return;

                Renderer.GetPropertyBlock(block);
                for (int i = 0; i < paramList.Count; i++)
                {
                    paramList[i].SetColor(block, color);
                }
                Renderer.SetPropertyBlock(block);
            }

            public void SetStone(bool isStone)
            {
                Renderer.GetPropertyBlock(block);
                block.SetFloat("_shihua", isStone ? 1.0f : 0.0f);
                Renderer.SetPropertyBlock(block);
            }
        }

        private class ColorParam
        {
            public int ColorID { get; private set; }

            public void Init(MaterialPropertyBlock block, int colorID)
            {
                this.ColorID = colorID;
            }

            public void SetColor(MaterialPropertyBlock block, Color color)
            {
                block.SetColor(ColorID, color);
            }
        }

        public TweenerCore<Color, Color, ColorOptions> DOColor(Color endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> tweenerCore = DOTween.To(this.GetColor, (val) => this.SetColor(val, false), endValue, duration);
            tweenerCore.SetTarget(this);
            return tweenerCore;
        }

        public void DOKill(bool complete = false)
        {
            DOTween.Kill(this, complete);
        }
    }

    [System.Serializable]
    public class MaterialColorSetter
    {
        public string[] setColorsKeyArr = new string[] { };
        private int[] setColorsIDArr;
        private List<MaterialData> matDatas = new List<MaterialData>();

        public void Init()
        {
            setColorsIDArr = new int[setColorsKeyArr.Length];
            for (int i = 0; i < setColorsKeyArr.Length; i++)
            {
                setColorsIDArr[i] = Shader.PropertyToID(setColorsKeyArr[i]);
            }
        }

        public void CollectGameObjectMaterial(GameObject go, bool collectChildren = true)
        {
            if (setColorsIDArr == null || setColorsIDArr.Length <= 0)
                return;

            var rendererList = go.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < rendererList.Length; i++)
            {
                var mats = rendererList[i].materials;
                for (int j = 0; j < mats.Length; j++)
                {
                    var mat = mats[j];
                    if (matDatas.Exists(m => m.Mat == mat))
                        continue;

                    MaterialData data = new MaterialData(mat);
                    for (int k = 0; k < setColorsIDArr.Length; k++)
                    {
                        data.AddColorParam(setColorsIDArr[k]);
                    }
                    if (!data.IsEmpty)
                        matDatas.Add(data);
                }
            }

        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < matDatas.Count; i++)
            {
                matDatas[i].SetColor(color);
            }
        }

        public void ResetAll()
        {
            for (int i = 0; i < matDatas.Count; i++)
            {
                matDatas[i].ResetColor();
            }
        }

        public void ClearAll()
        {
            ResetAll();
            matDatas.Clear();
        }

        private class MaterialData
        {
            public Material Mat { get; private set; }
            private List<ColorParam> paramList = new List<ColorParam>();
            public bool IsEmpty { get { return paramList.Count <= 0; } }

            public MaterialData(Material mat)
            {
                this.Mat = mat;
            }

            public void AddColorParam(int colorID)
            {
                if (paramList.Exists(p => p.ColorID == colorID))
                    return;

                if (ColorParam.ValidParam(Mat, colorID))
                {
                    var param = new ColorParam();
                    param.Init(Mat, colorID);
                    paramList.Add(param);
                }
            }

            public void SetColor(Color color)
            {
                for (int i = 0; i < paramList.Count; i++)
                {
                    paramList[i].SetColor(Mat, color);
                }
            }

            public void ResetColor()
            {
                for (int i = 0; i < paramList.Count; i++)
                {
                    paramList[i].Reset(Mat);
                }
            }
        }

        private class ColorParam
        {
            public int ColorID { get; private set; }
            private Color oriColor;

            public static bool ValidParam(Material mat, int colorID) { return mat.HasProperty(colorID); }

            public void Init(Material mat, int colorID)
            {
                this.ColorID = colorID;
                this.oriColor = mat.GetColor(colorID);
            }

            public void SetColor(Material mat, Color color)
            {
                mat.SetColor(ColorID, color);
            }

            public void Reset(Material mat)
            {
                mat.SetColor(ColorID, oriColor);
            }
        }
    }
}
