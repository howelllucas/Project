using LitJson;
using System;
using System.Text;
using UnityEngine;

namespace EZ.DataMgr
{
    public class BaseDataMgr<T>
    {
        private string m_FilePath;
        private char[] m_Key;
        protected T m_Data;
        byte[] m_Ascll = new byte[5] {
            0X45,0X5a,0X44,0X51,0X58
        };
        protected virtual void Init(string filePath)
        {
            m_FilePath = IoUtils.GetDocumentPath() + filePath;

            string newStr = null;
            for (int i = 0; i < m_Ascll.Length; i++)
            {
                newStr += ((char)m_Ascll[i]).ToString();
            }
            m_Key = (filePath + newStr).ToCharArray();
            for (int i = 0; i < m_Key.Length; i++)
            {
                m_Key[i] ^= m_Key[m_Key.Length - i - 1];
            }
            ReadData();
        }

        public virtual void OnInit()
        {

        }

        public T GetData()
        {
            return m_Data;
        }

        protected void ReadData()
        {
            if (System.IO.File.Exists(m_FilePath))
            {
                string str = System.IO.File.ReadAllText(m_FilePath, Encoding.Default);
                char[] data = str.ToCharArray();
                int keyLength = m_Key.Length;
#if (!UNITY_EDITOR)
            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= m_Key[i % keyLength];
            }
#endif
                m_Data = JsonMapper.ToObject<T>(new string(data));
            }
        }

        public void SaveData()
        {
            string str = JsonMapper.ToJson(m_Data);
            char[] data = str.ToCharArray();
            int keyLength = m_Key.Length;
#if (!UNITY_EDITOR)
            for (int i = 0; i < data.Length; i++)
            {
                data[i] ^= m_Key[i % keyLength];
            }
#endif
            System.IO.File.WriteAllText(m_FilePath, new string(data));
        }
		public void ClearData()
        {
            System.IO.File.Delete(m_FilePath);
            m_Data = default(T);
            OnInit();
        }
		public string GetDataStr()
        {
            return JsonMapper.ToJson(m_Data);
        }
    }
}
