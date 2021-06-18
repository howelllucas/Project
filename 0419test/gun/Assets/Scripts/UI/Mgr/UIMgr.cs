using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Game;
namespace Game.UI
{
    public enum UIType
    {
        MainUI = 1,         //主界面
        HeroUI = 2,         //卡牌界面
        ShopUI = 3,         //商店界面
        TaskUI = 4,         //任务界面
        StageUI = 5,        //关卡界面
        LabyrinthUI = 6,    //迷宫界面
        IdleRewardUI = 7,   //挂机界面
        WantedUI = 8,       //悬赏界面
        TowerUI = 9,        //爬塔界面
        QuickIdleRewardUI = 10,  //快速挂机
    }
    //界面管理器
    public class UIMgr : Singleton<UIMgr>
    {
        public static string UIRootPath = "UI/";

        public List<UIWindow> windows = new List<UIWindow>();

        private UIWindow waitNetWindow = null;

        public bool LockMain
        {
            get
            {
                Debug.Log("windows.Count " + windows.Count);
                return windows.Count > 4;
            }
        }

        public UIWindow Find(string name)
        {
            foreach (var win in windows)
            {
                if (!win.HasOpen || win.Obj == null || win.Obj.HasClose)
                    continue;

                if (win.Name == name)
                    return win;
            }
            return null;
        }
        public T FindUIObject<T>() where T : UIObject
        {
            foreach (var win in windows)
            {
                var t = win.Obj as T;
                if (t != null)
                    return t;
            }
            return null;
        }
        public T FindUIObject<T>(string name) where T : UIObject
        {
            foreach (var win in windows)
            {
                if (win.Name == name)
                    return win.Obj as T;
            }
            return null;
        }

        public IEnumerable<UIWindow> Finds(string name)
        {
            foreach (var win in windows)
            {
                if (win.Name == name)
                    yield return win;
            }
        }

        public UIObject Open(string name, object val = null, Transform root = null)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var oldUIObject = Find(name);
            if (oldUIObject != null && oldUIObject.Obj != null)
            {
                oldUIObject.Obj.Show(true);
                return oldUIObject.Obj;
            }

            UIWindow win = new UIWindow();
            win.Create(this, name, val, root);
            win.Open();
            windows.Add(win);

            return win.Obj;
        }

        public void NetOpen(string name, object val = null, Transform root = null)
        {
            if (string.IsNullOrEmpty(name))
                return;

            var oldUIObject = Find(name);
            if (oldUIObject != null && oldUIObject.Obj != null)
            {
                oldUIObject.Obj.Show(true);
                return;
            }

            UIWindow win = new UIWindow();
            win.Create(this, name, val, root);
            waitNetWindow = win;

            DateTimeMgr.singleton.FixedLocalTime();
        }

        public void Update()
        {

            for (int i = 0; i < windows.Count;)
            {
                var win = windows[i];
                if (win.HasOpen)
                    win.Update();
                else
                {
                    windows.Remove(win);
                    continue;
                }
                ++i;
            }
        }

        public void Close(UIWindow win)
        {
            if (win != null)
            {
                win.Close();
                BoradCast(UIEventType.CLOSE_UI, win.Name);
            }
        }

        public void Close(string name)
        {
            Close(Find(name));

        }

        public void BoradCast(UIEventType type, object parameter = null)
        {
            if (type == UIEventType.SERVER_TIME)
            {
                if (waitNetWindow != null)
                {
                    var success = (bool)parameter;
                    if (success)
                    {
                        waitNetWindow.Open();
                        windows.Add(waitNetWindow);
                    }
                    else
                    {
                        TipsUI.Instance.ShowTip(new TipsInfo()
                        {
                            autoClose = true,
                            duration = 1f,
                            openMask = false,
                            tipStr = LanguageMgr.GetText("Active_Award_No_Network")
                        });
                        Close(waitNetWindow);
                    }
                    waitNetWindow = null;
                }
            }

            for (int i = windows.Count - 1; i >= 0; i--)
            {
                var win = windows[i];
                win.OnEvent(type, parameter);
            }
        }

        public void Open(UIType type)
        {
            switch (type)
            {
                //case UIType.MainUI:
                //    {
                //        var cityScene = Scenes.SceneMgr.singleton.CurScene as Scenes.CityScene;
                //        if (cityScene != null)
                //        {
                //            cityScene.SetToScroll(1);
                //        }
                //    }
                //    break;
                //case UIType.HeroUI:
                //    {
                //        var cityScene = Scenes.SceneMgr.singleton.CurScene as Scenes.CityScene;
                //        if (cityScene != null)
                //        {
                //            cityScene.SetToScroll(0);
                //        }
                //    }
                //    break;
                //case UIType.ShopUI:
                //    {
                //        var cityScene = Scenes.SceneMgr.singleton.CurScene as Scenes.CityScene;
                //        if (cityScene != null)
                //        {
                //            cityScene.SetToScroll(2);
                //        }
                //    }
                //    break;
                //case UIType.TaskUI:
                //    {
                //        Open("TaskUI");
                //    }
                //    break;
                //case UIType.StageUI:
                //    {
                //        Open("StageUI");
                //    }
                //    break;
                //case UIType.IdleRewardUI:
                //    {
                //        Open("IdleRewardUI");
                //    }
                //    break;
                //case UIType.WantedUI:
                //    {
                //        Open("WantedUI");
                //    }
                //    break;
                //case UIType.TowerUI:
                //    {
                //        Open("TowerUI");
                //    }
                //    break;
                //case UIType.LabyrinthUI:
                //    {
                //        LabyrinthMgr.singleton.OpenLabyrinth(false);
                //    }
                //    break;
                //case UIType.QuickIdleRewardUI:
                //    {
                //        NetOpen("QuickIdleRewardUI");
                //    }
                //    break;
            }
        }
    }
}
