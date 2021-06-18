using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EZ
{
    public partial class CampsitePointUI_DataChangeItem
    {
        public void Init(string lastStr, string curStr, bool isUp)
        {
            LastValTxt.text.text = lastStr;
            CurValTxt.text.text = curStr;
            CurValTxt.text.color = isUp ? Color.green : Color.red;
            UpFlag.gameObject.SetActive(isUp);
            DownFlag.gameObject.SetActive(!isUp);
        }
    }
}