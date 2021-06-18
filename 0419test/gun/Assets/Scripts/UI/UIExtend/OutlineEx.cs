using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Game.UI
{
    [AddComponentMenu("UI/Effects/OutlineEx")]
    /// <summary>
    /// UGUI描边
    /// </summary>
    public class OutlineEx : BaseMeshEffect
    {
        private const string MATERIAL_PATH = "Assets/ArtRes/2D/UI/Material/OutlineMat.mat";
        private const string SHADER_NAME = "Custom/UI-OutlineEx";

        public Color OutlineColor = Color.black;
        [Range(0, 5)]
        public float OutlineWidth = 1;
        public Vector2 ShadowOffset = new Vector2(0, -6);
        [Range(0, 1)]
        public float ShadowGradient = 1f;

        [SerializeField]
        private bool m_UseGraphicAlpha = true;

        private static Material material;

        private static List<UIVertex> m_VetexList = new List<UIVertex>();

        private Vector3 lastLossyScale;
        private Quaternion lastGlobalRot;

        private Vector3 lastCheckScale;
        private Quaternion lastCheckRot;

        private bool needRefresh = false;

        protected override void Awake()
        {
            base.Awake();

            CheckMaterial();
        }

        protected override void Start()
        {
            base.Start();
            var canvas = GetGraphicCanvas();
            {
                if (canvas != null)
                {
                    var v1 = base.graphic.canvas.additionalShaderChannels;
                    var v2 = AdditionalCanvasShaderChannels.TexCoord1;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }
                    v2 = AdditionalCanvasShaderChannels.TexCoord2;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }
                    v2 = AdditionalCanvasShaderChannels.TexCoord3;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }
                    v2 = AdditionalCanvasShaderChannels.Tangent;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }
                    v2 = AdditionalCanvasShaderChannels.Normal;
                    if ((v1 & v2) != v2)
                    {
                        base.graphic.canvas.additionalShaderChannels |= v2;
                    }
                }
            }

            lastLossyScale = transform.lossyScale;
            lastGlobalRot = transform.rotation;

            lastCheckScale = CalcCheckScale();
            lastCheckRot = CalcCheckRot();

            NotifyRefresh();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            CheckMaterial();
            this._Refresh();
        }
#endif

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ClearMaterial();
        }

        private void CheckMaterial()
        {
            if (base.graphic == null)
                return;

            string shaderName = SHADER_NAME;
            if (base.graphic.material == null || base.graphic.material.shader.name != shaderName)
            {
                if (material == null)
                {
#if UNITY_EDITOR
                    string matPath = MATERIAL_PATH;
                    material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>(matPath);
                    if (material == null)
                    {
                        Debug.LogError("没有找到材质:" + matPath);
                    }
#else
                    var shader = Shader.Find(shaderName);
                    if (shader != null)
                        material = new Material(shader);
                    else
                    {
                        Debug.Log("没有找到Shader:" + shaderName);
                    }
#endif
                }
                base.graphic.material = material;
            }
        }

        private void ClearMaterial()
        {
            if (base.graphic == null)
                return;

            base.graphic.material = null;
        }

        private Canvas GetGraphicCanvas()
        {
            if (base.graphic == null)
                return null;
            return base.graphic.canvas;
        }

        private Canvas GetRootCanvas()
        {
            var graphicCanvas = GetGraphicCanvas();
            if (graphicCanvas == null)
                return null;
            return graphicCanvas.rootCanvas;
        }
        [ExecuteInEditMode]
        private void Update()
        {
            if (lastLossyScale != transform.lossyScale)
            {
                lastLossyScale = transform.lossyScale;
                CheckScaleRefresh();
            }

            if (lastGlobalRot != transform.rotation)
            {
                lastGlobalRot = transform.rotation;
                CheckRotRefresh();
            }

            if (needRefresh)
            {
                _Refresh();
                needRefresh = false;
            }
        }

        private void CheckScaleRefresh()
        {
            var rootCanvas = GetRootCanvas();
            if (rootCanvas == null)
                return;

            var scale = CalcCheckScale();
            if (scale != lastCheckScale)
            {
                lastCheckScale = scale;
                NotifyRefresh();
            }
        }

        private Vector3 CalcCheckScale()
        {
            Vector3 result = new Vector3();

            var rootCanvas = GetRootCanvas();
            if (rootCanvas != null)
            {
                result.x = transform.lossyScale.x / rootCanvas.transform.lossyScale.x;
                result.y = transform.lossyScale.y / rootCanvas.transform.lossyScale.y;
                result.z = transform.lossyScale.z / rootCanvas.transform.lossyScale.z;
            }
            return result;
        }

        private void CheckRotRefresh()
        {
            lastCheckRot = CalcCheckRot();
            NotifyRefresh();
        }

        private Quaternion CalcCheckRot()
        {
            Quaternion result = transform.rotation;
            var rootCanvas = GetRootCanvas();
            if (rootCanvas != null)
            {
                Quaternion canvasRot = rootCanvas.transform.rotation;
                canvasRot.w = -canvasRot.w;
                result = canvasRot * transform.rotation;
            }

            return result;
        }


        private void NotifyRefresh()
        {
            needRefresh = true;
        }

        private void _Refresh()
        {
            if (base.graphic != null)
                base.graphic.SetVerticesDirty();
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            vh.GetUIVertexStream(m_VetexList);

            this._ProcessVertices();

            vh.Clear();
            vh.AddUIVertexTriangleStream(m_VetexList);
        }


        private void _ProcessVertices()
        {
            float OutlineWidth = this.OutlineWidth;
            var ShadowOffset = this.ShadowOffset;
            var txtGraphic = base.graphic as Text;
            if (txtGraphic != null)
            {
                var fontSizeScale = txtGraphic.fontSize * 0.02f;
                OutlineWidth *= fontSizeScale;
                ShadowOffset *= fontSizeScale;
            }
            var OutlineColor = this.OutlineColor;
            if (m_UseGraphicAlpha)
            {
                OutlineColor.a *= base.graphic.color.a;
            }

            var rootCanvas = GetRootCanvas();
            if (rootCanvas != null)
            {
                ShadowOffset *= rootCanvas.scaleFactor;
                OutlineWidth *= rootCanvas.scaleFactor;
            }

            Vector2 mainTexSize = new Vector2(1f / graphic.mainTexture.width, 1f / graphic.mainTexture.height);

            for (int i = 0, count = m_VetexList.Count - 3; i <= count; i += 3)
            {
                var v1 = m_VetexList[i];
                var v2 = m_VetexList[i + 1];
                var v3 = m_VetexList[i + 2];
                // 计算原顶点坐标中心点
                //
                var minX = _Min(v1.position.x, v2.position.x, v3.position.x);
                var minY = _Min(v1.position.y, v2.position.y, v3.position.y);
                var maxX = _Max(v1.position.x, v2.position.x, v3.position.x);
                var maxY = _Max(v1.position.y, v2.position.y, v3.position.y);
                var posCenter = new Vector2(minX + maxX, minY + maxY) * 0.5f;
                // 计算原始顶点坐标和UV的方向
                //
                Vector2 triX, triY, uvX, uvY;
                Vector2 pos1 = v1.position;
                Vector2 pos2 = v2.position;
                Vector2 pos3 = v3.position;
                if (Mathf.Abs(Vector2.Dot((pos2 - pos1).normalized, Vector2.right))
                    > Mathf.Abs(Vector2.Dot((pos3 - pos2).normalized, Vector2.right)))
                {
                    triX = pos2 - pos1;
                    triY = pos3 - pos2;
                    uvX = v2.uv0 - v1.uv0;
                    uvY = v3.uv0 - v2.uv0;
                }
                else
                {
                    triX = pos3 - pos2;
                    triY = pos2 - pos1;
                    uvX = v3.uv0 - v2.uv0;
                    uvY = v2.uv0 - v1.uv0;
                }
                // 计算原始UV框
                var uvMin = _Min(v1.uv0, v2.uv0, v3.uv0);
                var uvMax = _Max(v1.uv0, v2.uv0, v3.uv0);
                //OutlineColor 和 OutlineWidth 也传入，避免出现不同的材质球
                var col_rg = new Vector2(OutlineColor.r, OutlineColor.g);       //描边颜色 用uv3 和 tangent的 zw传递
                var col_ba = new Vector4(mainTexSize.x, mainTexSize.y, OutlineColor.b, OutlineColor.a);

                // 为每个顶点设置新的Position和UV，并传入原始UV框
                v1 = _SetNewPosAndUV(v1, OutlineWidth, ShadowOffset, ShadowGradient, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
                v1.uv3 = col_rg;
                v1.tangent = col_ba;
                v2 = _SetNewPosAndUV(v2, OutlineWidth, ShadowOffset, ShadowGradient, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
                v2.uv3 = col_rg;
                v2.tangent = col_ba;
                v3 = _SetNewPosAndUV(v3, OutlineWidth, ShadowOffset, ShadowGradient, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
                v3.uv3 = col_rg;
                v3.tangent = col_ba;

                // 应用设置后的UIVertex
                //
                m_VetexList[i] = FixedScaleAndRotEffect(v1);
                m_VetexList[i + 1] = FixedScaleAndRotEffect(v2);
                m_VetexList[i + 2] = FixedScaleAndRotEffect(v3);
            }
        }


        private UIVertex FixedScaleAndRotEffect(UIVertex vt)
        {
            var tangent = vt.tangent;
            var normal = vt.normal;

            var scale = lastCheckScale;
            for (int i = 0; i < 3; i++)
            {
                var scale_i = Mathf.Abs(scale[i]);
                if (scale_i != 0)
                {
                    tangent[i] /= scale_i;
                    normal[i] /= scale_i;
                }
            }

            var rot = lastCheckRot;
            rot.w = -rot.w;
            Vector3 tangent_xyz = tangent;
            tangent_xyz = rot * tangent_xyz;
            for (int i = 0; i < 3; i++)
            {
                tangent[i] = tangent_xyz[i];
            }

            normal = rot * normal;

            vt.tangent = tangent;
            vt.normal = normal;
            return vt;
        }


        private static UIVertex _SetNewPosAndUV(UIVertex pVertex, float pOutLineWidth, Vector2 pShadowOffset, float shadowGradient,
            Vector2 pPosCenter,
            Vector2 pTriangleX, Vector2 pTriangleY,
            Vector2 pUVX, Vector2 pUVY,
            Vector2 pUVOriginMin, Vector2 pUVOriginMax)
        {
            // Position
            var pos = pVertex.position;
            var posXOffset = pos.x > pPosCenter.x ? pOutLineWidth : -pOutLineWidth;
            var posYOffset = pos.y > pPosCenter.y ? pOutLineWidth : -pOutLineWidth;
            pos.x += posXOffset;
            pos.y += posYOffset;
            // UV
            var uv = pVertex.uv0;
            uv += pUVX / pTriangleX.magnitude * posXOffset * (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1);
            uv += pUVY / pTriangleY.magnitude * posYOffset * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1);

            Vector2 shadowUVOffset = new Vector2();
            //shadowUVOffset += pUVX.normalized * pShadowOffset.x * (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1);
            //shadowUVOffset += pUVY.normalized * pShadowOffset.y * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1);

            if ((pos.x - pPosCenter.x) * pShadowOffset.x > 0)
            {
                pos.x += pShadowOffset.x;
                uv += pUVX / pTriangleX.magnitude * pShadowOffset.x * (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1);
                shadowUVOffset += pUVX.normalized * pShadowOffset.x * (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1);
            }
            else
            {
                shadowUVOffset += Vector2.Lerp(Vector2.zero, pUVX.normalized * pShadowOffset.x * (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1), shadowGradient);
            }

            if ((pos.y - pPosCenter.y) * pShadowOffset.y > 0)
            {
                pos.y += pShadowOffset.y;
                uv += pUVY / pTriangleY.magnitude * pShadowOffset.y * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1);
                shadowUVOffset += pUVY.normalized * pShadowOffset.y * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1);
            }
            else
            {
                shadowUVOffset += Vector2.Lerp(Vector2.zero, pUVY.normalized * pShadowOffset.y * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1), shadowGradient);
            }

            pVertex.position = pos;
            pVertex.uv0 = uv;

            pVertex.uv1 = pUVOriginMin;
            pVertex.uv2 = pUVOriginMax;

            pVertex.normal = new Vector3(shadowUVOffset.x, shadowUVOffset.y, pOutLineWidth);    //normal.xy存放阴影偏移 normal.z存放描边宽度 在外部处理Scale问题

            return pVertex;
        }


        private static float _Min(float pA, float pB, float pC)
        {
            return Mathf.Min(Mathf.Min(pA, pB), pC);
        }


        private static float _Max(float pA, float pB, float pC)
        {
            return Mathf.Max(Mathf.Max(pA, pB), pC);
        }


        private static Vector2 _Min(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Min(pA.x, pB.x, pC.x), _Min(pA.y, pB.y, pC.y));
        }


        private static Vector2 _Max(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Max(pA.x, pB.x, pC.x), _Max(pA.y, pB.y, pC.y));
        }
    }
}