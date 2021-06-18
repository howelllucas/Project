using UnityEngine;
using System.Collections.Generic;
using Game;
using System.Linq;

namespace EZ
{
    public class NetOpenBuffer
    {
        public string panelName;
        public UIInfo info;
        public object args;
    }

    public class UiMgr
    {
        private Dictionary<string, BaseUi> m_Panels;
        private Dictionary<string, BaseUi> m_CachePanel;
        private HashSet<string> m_CachePanelNames;
        private Dictionary<string, BaseUi> m_LastingPanel;
        private HashSet<string> m_LastingPanelNames;
        private Transform m_RootNodeTsf;
        private Transform m_UiCanvasTsf;
        private Transform m_TopCanvasTsf;
        private bool m_HasInit = false;
        private Queue<NetOpenBuffer> m_NetOpenBufferQueue = new Queue<NetOpenBuffer>();
        private Dictionary<int, List<BaseUi>> m_LayerDic = new Dictionary<int, List<BaseUi>>();
        private int m_TopLayer;
        private bool m_NeedRefreshTopLayer;
        public UiMgr()
        {
            m_Panels = new Dictionary<string, BaseUi>();
            m_CachePanelNames = new HashSet<string>();
            m_CachePanel = new Dictionary<string, BaseUi>();
            m_LastingPanelNames = new HashSet<string>();
            m_LastingPanel = new Dictionary<string, BaseUi>();
            InitRootNode();
            RegisterListeners();
        }

        public void OpenPanel<T>(string panelName, T args)
        {
            InitRootNode();
            ClosePanel(panelName);
            UIInfo uiInfo;
            if (Wndid.gWndInfo.TryGetValue(panelName, out uiInfo))
            {
                if (uiInfo.NetOpen)
                {
                    AddNetOpenBuffer(panelName, uiInfo, args);
                }
                else
                {
                    OpenPanelImp(uiInfo, panelName, args);
                }
            }
            else
            {
                Debug.Log("error ====== ui not exit ==== " + panelName);
            }
        }
        public void OpenPanel(string panelName)
        {

            InitRootNode();
            ClosePanel(panelName);
            UIInfo uiInfo;
            if (Wndid.gWndInfo.TryGetValue(panelName, out uiInfo))
            {
                if (uiInfo.NetOpen)
                {
                    AddNetOpenBuffer(panelName, uiInfo, 0);
                }
                else
                {
                    OpenPanelImp(uiInfo, panelName, 0);
                }
            }
            else
            {
                Debug.Log("error ====== ui not exit ==== " + panelName);
            }
        }

        private void AddNetOpenBuffer(string panelName, UIInfo uiInfo, object args)
        {
            if (InternetMgr.singleton.IsInternetConnect())
            {
                NetOpenBuffer buffer = new NetOpenBuffer()
                {
                    panelName = panelName,
                    info = uiInfo,
                    args = args
                };
                m_NetOpenBufferQueue.Enqueue(buffer);
                if (uiInfo.NetOpenShowLoading && !CheckPanelExit(Wndid.LoadingUI))
                {
                    OpenPanel(Wndid.LoadingUI);
                }
                DateTimeMgr.singleton.FixedLocalTime();
            }
            else
            {
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, LanguageMgr.GetText("Active_Award_No_Network"));
            }
        }

        public void CachePanel(string panelName)
        {
            UIInfo uiInfo;
            if (Wndid.gWndInfo.TryGetValue(panelName, out uiInfo))
            {
                if (m_CachePanelNames.Contains(panelName))
                {
                    return;
                }
                GameObject uiPanel = Global.gApp.gResMgr.InstantiateObj(uiInfo.ResPath);
                m_CachePanel.Add(panelName, uiPanel.GetComponent<BaseUi>());
                m_CachePanelNames.Add(panelName);
                uiPanel.transform.localScale = new Vector3(1, 1, 1);
                uiPanel.transform.SetParent(m_RootNodeTsf, true);
                uiPanel.gameObject.SetActive(false);
            }
        }

        public void LastingPanel(string panelName)
        {
            UIInfo uiInfo;
            if (Wndid.gWndInfo.TryGetValue(panelName, out uiInfo) && !m_LastingPanel.ContainsKey(panelName))
            {
                if (m_LastingPanelNames.Contains(panelName))
                {
                    return;
                }
                GameObject uiPanel = Global.gApp.gResMgr.InstantiateObj(uiInfo.ResPath);
                m_LastingPanel.Add(panelName, uiPanel.GetComponent<BaseUi>());
                m_LastingPanelNames.Add(panelName);
                uiPanel.transform.localScale = new Vector3(1, 1, 1);
                uiPanel.transform.SetParent(m_RootNodeTsf, true);
                uiPanel.gameObject.SetActive(false);
            }
        }
        public BaseUi GetPanel(UIInfo uiInfo, string name)
        {
            BaseUi basePanel;
            if (m_CachePanel.TryGetValue(name, out basePanel))
            {
                m_CachePanel.Remove(name);
                basePanel.transform.localPosition = Vector3.zero;
                basePanel.transform.localScale = new Vector3(1, 1, 1);
                basePanel.gameObject.SetActive(true);
                return basePanel;
            }
            else if (m_LastingPanel.TryGetValue(name, out basePanel))
            {
                m_LastingPanel.Remove(name);
                basePanel.transform.localPosition = Vector3.zero;
                basePanel.transform.localScale = new Vector3(1, 1, 1);
                basePanel.gameObject.SetActive(true);
                return basePanel;
            }
            else
            {
                GameObject uiPanel = Global.gApp.gResMgr.InstantiateObj(uiInfo.ResPath);
                basePanel = uiPanel.GetComponent<BaseUi>();
                return basePanel;
            }
        }
        public void OpenPanelImp<T>(UIInfo uiInfo, string name, T arg)
        {
            string assetName = uiInfo.ResPath;
            BaseUi baseUi = GetPanel(uiInfo, name);
            if (uiInfo.AddRoot)
            {
                Canvas[] canvas = baseUi.gameObject.GetComponentsInChildren<Canvas>();
                if (canvas.Length > 0)
                {
                    baseUi.transform.SetParent(m_RootNodeTsf, false);
                    foreach (Canvas mCanvas in canvas)
                    {
                        mCanvas.worldCamera = Global.gApp.gUICameraCmpt;
                        //if (uiInfo.Order > 0)
                        {
                            mCanvas.sortingOrder = uiInfo.Order;
                        }
                        GameAdapterUtils.AdaptCanvas(mCanvas);
                    }
                }
                else
                {
                    Transform canvasTsf = CreateCanvas(name, uiInfo.Order);
                    baseUi.transform.SetParent(canvasTsf, false);
                }
            }
            else
            {
                baseUi.transform.SetParent(m_UiCanvasTsf, false);
            }
            baseUi.Init(name, uiInfo, arg);
            //if (name != Wndid.RewardEffectUi)
            //{
            m_Panels.Add(name, baseUi);
            //}
            //if (!m_LastingPanelNames.Contains(name) && !name.Equals(Wndid.HomeUI))
            //{
            //    //Debug.Log(name + DateTimeUtil.GetMills(System.DateTime.Now));
            //    NewbieGuideButton ngb = baseUi.GetComponentInChildren<NewbieGuideButton>();
            //    //Debug.Log(name + (ngb == null));
            //    if (ngb == null)
            //    {
            //        //Debug.Log("open " + baseUi.name);
            //        Global.gApp.gMsgDispatcher.Broadcast<bool>(MsgIds.HideGameGuideAD, false);
            //    }
            //    //Debug.Log(name + DateTimeUtil.GetMills(System.DateTime.Now));
            //}
            if (uiInfo.Layer > 0)
            {
                List<BaseUi> uiList;
                if (!m_LayerDic.TryGetValue(uiInfo.Layer, out uiList))
                {
                    uiList = new List<BaseUi>();
                    m_LayerDic[uiInfo.Layer] = uiList;
                }
                if (!uiList.Contains(baseUi))
                {
                    uiList.Add(baseUi);
                    m_NeedRefreshTopLayer = true;
                }
            }
            Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIPanelOpen, name);
        }

        private void BeforeClose(BaseUi baseUi, string panelName)
        {
            if (!m_LastingPanelNames.Contains(panelName) && !panelName.Equals(Wndid.HomeUI))
            {
                //Debug.Log(panelName + DateTimeUtil.GetMills(System.DateTime.Now));
                NewbieGuideButton ngb = baseUi.GetComponentInChildren<NewbieGuideButton>();
                //Debug.Log(panelName + (ngb == null));
                if (ngb == null)
                {
                    //Debug.Log("close " + baseUi.name);
                    Global.gApp.gMsgDispatcher.Broadcast<bool>(MsgIds.HideGameGuideAD, true);
                }
                //Debug.Log(panelName + DateTimeUtil.GetMills(System.DateTime.Now));
            }
        }

        public void ClosePanel(string panelName)
        {
            BaseUi baseUi;
            if (m_Panels.TryGetValue(panelName, out baseUi))
            {
                int uiLayer = baseUi.UILayer;
                if (uiLayer > 0)
                {
                    List<BaseUi> uiList;
                    if (m_LayerDic.TryGetValue(uiLayer, out uiList))
                    {
                        uiList.Remove(baseUi);
                        if (uiList.Count <= 0)
                        {
                            m_LayerDic.Remove(uiLayer);
                            m_NeedRefreshTopLayer = true;
                        }
                    }
                }

                BeforeClose(baseUi, panelName);
                if (m_CachePanelNames.Contains(panelName))
                {
                    baseUi.gameObject.SetActive(false);
                    baseUi.transform.SetParent(m_RootNodeTsf, false);
                    baseUi.Recycle();
                    m_Panels.Remove(panelName);
                    m_CachePanel.Add(panelName, baseUi);
                }
                else if (m_LastingPanelNames.Contains(panelName))
                {
                    baseUi.gameObject.SetActive(false);
                    baseUi.transform.SetParent(m_RootNodeTsf, false);
                    baseUi.Recycle();
                    m_Panels.Remove(panelName);
                    m_LastingPanel.Add(panelName, baseUi);
                }
                else
                {
                    baseUi.Release();
                }
                m_Panels.Remove(panelName);
                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIPanelClose, panelName);
            }
        }
        private void InitRootNode()
        {
            if (!m_HasInit)
            {
                m_HasInit = true;
                GameObject rootNode = new GameObject("UiRootNode");
                m_RootNodeTsf = rootNode.transform;
                m_RootNodeTsf.SetParent(Global.gApp.gKeepNode.transform, false);
                m_UiCanvasTsf = CreateCanvas("UiCanvas", 30);
                m_TopCanvasTsf = CreateCanvas("TopCanvas", 300);
            }
        }

        private Transform CreateCanvas(string name, int order)
        {
            string path = EffectConfig.CanvasPath;
            GameObject canvasGo = Global.gApp.gResMgr.InstantiateObj(path);
            canvasGo.name = name;
            canvasGo.transform.SetParent(m_RootNodeTsf, false);
            Canvas cvs = canvasGo.GetComponent<Canvas>();

            GameAdapterUtils.AdaptCanvas(cvs);

            cvs.worldCamera = Global.gApp.gUICameraCmpt;
            cvs.sortingOrder = order;
            return canvasGo.transform;
        }

        public void ClossAllPanel(bool absClear = false)
        {
            foreach (KeyValuePair<string, BaseUi> kv in m_Panels)
            {
                BaseUi panel = kv.Value;
                BeforeClose(panel, kv.Key);
                panel.gameObject.SetActive(false);
                if (m_LastingPanelNames.Contains(panel.GetName()))
                {
                    panel.transform.SetParent(m_RootNodeTsf, false);
                    panel.Recycle();
                    m_LastingPanel.Add(panel.GetName(), panel);
                }
                else
                {
                    panel.Release();
                }

            }
            foreach (BaseUi panel in m_CachePanel.Values)
            {
                panel.gameObject.SetActive(false);
                if (m_LastingPanelNames.Contains(panel.GetName()))
                {
                    panel.transform.SetParent(m_RootNodeTsf, false);
                    panel.Recycle();
                    m_LastingPanel.Add(panel.GetName(), panel);
                }
                else
                {
                    panel.Release();
                }
            }
            if (absClear)
            {
                foreach (BaseUi panel in m_LastingPanel.Values)
                {
                    panel.gameObject.SetActive(false);
                    panel.Release();
                }
                m_LastingPanel.Clear();
                m_LastingPanelNames.Clear();
            }
            m_Panels.Clear();
            m_CachePanel.Clear();
            m_CachePanelNames.Clear();
            m_TopLayer = 0;
            m_NeedRefreshTopLayer = false;
            m_LayerDic.Clear();
        }

        public bool CheckPanelExit(string panelName)
        {
            return m_Panels.ContainsKey(panelName);
        }
        public BaseUi GetPanelCompent(string panelName)
        {
            BaseUi panel;
            if (m_Panels.TryGetValue(panelName, out panel))
            {
                return panel.gameObject.GetComponent<BaseUi>();
            }
            if (m_CachePanel.TryGetValue(panelName, out panel))
            {
                return panel.gameObject.GetComponent<BaseUi>();
            }
            if (m_LastingPanel.TryGetValue(panelName, out panel))
            {
                return panel.gameObject.GetComponent<BaseUi>();
            }
            return default(BaseUi);
        }
        public T GetPanelCompent<T>(string panelName)
        {
            BaseUi panel;
            if (m_Panels.TryGetValue(panelName, out panel))
            {
                return panel.gameObject.GetComponent<T>();
            }
            if (m_CachePanel.TryGetValue(panelName, out panel))
            {
                return panel.gameObject.GetComponent<T>();
            }
            if (m_LastingPanel.TryGetValue(panelName, out panel))
            {
                return panel.gameObject.GetComponent<T>();
            }
            return default(T);
        }
        public Transform GetUiCanvasTsf()
        {
            return m_UiCanvasTsf;
        }
        public Transform GetTopCanvasTsf()
        {
            return m_TopCanvasTsf;
        }
        public Transform GetRootNodeTsf()
        {
            return m_RootNodeTsf;
        }

        public bool IsOnTop(string panelName)
        {
            if (CheckPanelExit(panelName))
            {
                var ui = GetPanelCompent(panelName);
                return ui.UILayer >= m_TopLayer;
            }
            return false;
        }

        public void Destroy()
        {

        }

        private void OnServerTimeFixed(bool success)
        {
            var panels = m_Panels.Values.ToArray();
            foreach (var p in panels)
            {
                p.OnServerTimeFixed(success);
            }

            if (success)
            {
                NetOpenBuffer buffer;
                while (m_NetOpenBufferQueue.Count > 0)
                {
                    if ((buffer = m_NetOpenBufferQueue.Dequeue()) != null)
                        OpenPanelImp(buffer.info, buffer.panelName, buffer.args);
                }
            }
            else
            {
                if (m_NetOpenBufferQueue.Count > 0)
                {
                    Global.gApp.gMsgDispatcher.Broadcast(MsgIds.ShowGameTipsByStr, LanguageMgr.GetText("Active_Award_No_Network"));
                    m_NetOpenBufferQueue.Clear();
                }
            }
            ClosePanel(Wndid.LoadingUI);

        }

        private void OnDateTimeRefresh(DateTimeRefreshType type)
        {
            var panels = m_Panels.Values.ToArray();
            foreach (var p in panels)
            {
                p.OnDateTimeRefresh(type);
            }
        }

        private void RegisterListeners()
        {
            Global.gApp.gMsgDispatcher.AddListener<bool>(MsgIds.ServerTimeFixed, OnServerTimeFixed);
            Global.gApp.gMsgDispatcher.AddListener<DateTimeRefreshType>(MsgIds.DateTimeRefresh, OnDateTimeRefresh);

            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.ServerTimeFixed);
            Global.gApp.gMsgDispatcher.MarkAsPermanent(MsgIds.DateTimeRefresh);
        }

        public void Update()
        {
            if (m_NeedRefreshTopLayer)
            {
                int newTopLayer = 0;
                foreach (var layer in m_LayerDic.Keys)
                {
                    newTopLayer = Mathf.Max(newTopLayer, layer);
                }

                if (newTopLayer < m_TopLayer)
                {
                    List<BaseUi> list;
                    if (m_LayerDic.TryGetValue(newTopLayer, out list))
                    {
                        foreach (var ui in list)
                        {
                            if (ui != null)
                                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.UIPanelBackToTop, ui.GetName());
                        }
                    }
                }
                else if (newTopLayer > m_TopLayer)
                {
                    List<BaseUi> list;
                    if (m_LayerDic.TryGetValue(m_TopLayer, out list))
                    {
                        foreach (var ui in list)
                        {
                            if (ui != null)
                                Global.gApp.gMsgDispatcher.Broadcast(MsgIds.TopUIBeCovered, ui.GetName());
                        }
                    }
                }
                m_TopLayer = newTopLayer;

                m_NeedRefreshTopLayer = false;
            }
        }
    }
}
