using System;
using System.Runtime.InteropServices;
using UnityEngine;
namespace EZ
{
    public class EZMath
    {

        public static float SignedAngleBetween(Vector3 from_, Vector3 to_)
        {
            Vector3 v3 = Vector3.Cross(from_, to_);
            if (v3.z >= 0)
                return 360 - Vector3.Angle(from_, to_);
            else
                return Vector3.Angle(from_, to_);
        }
        public static long GetCurTime()
        {
            long currentTicks = DateTime.Now.Ticks;
            DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long currentMillis = (currentTicks - dtFrom.Ticks) / 10000;
            return currentMillis;
        }
        public static string GetMemory(object o) // 获取引用类型的内存地址方法    
        {
            GCHandle h = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);

            IntPtr addr = GCHandle.ToIntPtr(h);

            return "0x" + addr.ToString("X");
        }
        public static string FormateTime(int time)
        {
            int min = time / 60;
            int sec = time % 60;
            string showText;
            if (sec >= 10)
            {
                string text = min.ToString() + ":" + sec.ToString();
                showText = text;
            }
            else
            {
                string text = min.ToString() + ":0" + sec.ToString();
                showText = text;
            }
            return showText;
        }
        public static string FormateTimeMMSS(int time)
        {
            int min = time / 60;
            int sec = time % 60;
            string showTextPre;
            if (sec >= 10)
            {
                showTextPre = sec.ToString();
            }
            else
            {
                showTextPre =  "0" + sec.ToString();
            }
            if(min >= 10)
            {
                return min.ToString() + ":" + showTextPre;
            }
            else
            {
                return  "0" + min.ToString() + ":" + showTextPre;
            }
        }
    }
}
