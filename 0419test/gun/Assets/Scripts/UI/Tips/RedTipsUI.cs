using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{    
    public class RedTipsUI : MonoBehaviour
    {
        public GameObject redPoint;
        public List<RedTipsType> typeList = new List<RedTipsType>();
        public Text countTxt;
        public List<GameObject> removeList = new List<GameObject>();
        public bool isClickClose = false;
        // Start is called before the first frame update
        void Start()
        {
            ShowRed(false, RedTipsType.None);
            foreach (var type in typeList)
            {
                if (TipsMgr.singleton.AddRedTips(type, this))
                {
                    ShowRed(true, type);
                }
                    
            }

            if (isClickClose)
            {
                var btn = transform.parent.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.AddListener(OnClick);
                }
            }
        }

        public void UpdateRed()
        {
            ShowRed(false, RedTipsType.None);
            foreach (var type in typeList)
            {
                if (TipsMgr.singleton.CheckRedTips(type))
                {
                    ShowRed(true, type);
                    break;
                }

            }
        }

        void ShowRed(bool is_show, RedTipsType type)
        {
            redPoint.SetActive(is_show);

            if (is_show && countTxt != null && countTxt.gameObject.activeSelf)
            {
                countTxt.text = TipsMgr.singleton.GetRedTipsCount(type).ToString();
            }

            for (int i = 0; i < removeList.Count; ++i)
            {
                removeList[i].SetActive(!is_show);
            }
            
        }

        void OnClick()
        {
            if (isClickClose && redPoint.activeSelf)
            {
                
                foreach (var type in typeList)
                {
                    TipsMgr.singleton.CloseTips(type);
                    Debug.Log("isClickClose " + type);
                }
                
                //var btn = transform.parent.GetComponent<Button>();
                //if (btn != null)
                //{
                //    btn.onClick.RemoveListener(OnClick);
                //}
            }
        }
    }
}