
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Game
{

    public struct FieldPair
    {
        enum ArrayType
        {
            eInt32,
            eSingle,
            eString,
        }
        public string name;
        public System.Reflection.FieldInfo filed;
        ArrayType type;
        public int fieldIndex;
        public FieldPair(System.Reflection.FieldInfo info, string name)
        {
            if (info == null)
                throw new Exception(string.Format("get FieldPair [{0}]", name));
            filed = info;
            this.name = name;
            fieldIndex = -1;
            type = ArrayType.eInt32;
        }
        public FieldPair(System.Reflection.FieldInfo info, int fieldIndex, string name)
        {
            filed = info;
            this.name = name;
            this.fieldIndex = fieldIndex;
            type = ArrayType.eInt32;
            if (filed.FieldType.FullName == "System.Int32[]")
            {
                type = ArrayType.eInt32;
            }
            else if (filed.FieldType.FullName == "System.Single[]")
            {
                type = ArrayType.eSingle;
            }
            else if (filed.FieldType.FullName == "System.String[]")
            {
                type = ArrayType.eString;
            }
        }

        public void setValue(object obj, object val)
        {
            if (fieldIndex < 0)
            {
                filed.SetValue(obj, val);
            }
            else
            {
                _setArrayValue(obj, val);
            }
        }
        public object getValue(object obj)
        {
            if (fieldIndex < 0)
            {
                return filed.GetValue(obj);
            }
            else
            {
                return _getArrayValue(obj);
            }
        }

        object _getValue(object obj)
        {
            return filed.GetValue(obj);
        }
        object _getArrayValue(object obj)
        {
            if (type == ArrayType.eInt32)
            {
                var v = (int[])_getValue(obj);
                return v[fieldIndex];
            }
            else if (type == ArrayType.eSingle)
            {
                var v = (float[])_getValue(obj);
                return v[fieldIndex];
            }
            else if (type == ArrayType.eString)
            {
                var v = (string[])_getValue(obj);
                return v[fieldIndex];
            }
            return null;
        }
        void _setArrayValue(object obj, object val)
        {
            if (type == ArrayType.eInt32)
            {
                var v = (int[])_getValue(obj);
                v[fieldIndex] = (int)val;
            }
            else if (type == ArrayType.eSingle)
            {
                var v = (float[])_getValue(obj);
                v[fieldIndex] = (float)val;
            }
            else if (type == ArrayType.eString)
            {
                var v = (string[])_getValue(obj);
                v[fieldIndex] = (string)val;
            }
        }
    }
    public class FieldPairTable
    {
        public object[] mObjects;
        public FieldPair[] mFields;
        public List<FieldPair> mListFields = new List<FieldPair>();
        public Dictionary<string, int> mFiledNameToIndex = new Dictionary<string, int>();
        public int[] mCatchColumes;
        bool mCatchFields = false;
        int mCurIndex = 0;

        public void addField(object obj, string fieldName, string name)
        {
            if (mCatchFields)
            {
                mObjects[mCurIndex] = obj;
                mCurIndex++;
            }
            else
            {
                _addField(new FieldPair(obj.GetType().GetField(fieldName
                    , System.Reflection.BindingFlags.Public
                   | System.Reflection.BindingFlags.NonPublic
                   | System.Reflection.BindingFlags.Instance
                   ), name));
            }
        }

        void _addField(FieldPair field)
        {
            mFiledNameToIndex[field.name] = mListFields.Count;
            mListFields.Add(field);
        }

        public void addArrayFileds(object obj, string fieldName, string[] names)
        {
            if (mCatchFields)
            {
                for (int i = 0; i < names.Length; ++i)
                {
                    mObjects[mCurIndex] = obj;
                    mCurIndex++;
                }
            }
            else
            {
                System.Reflection.FieldInfo field = obj.GetType().GetField(fieldName,
                    System.Reflection.BindingFlags.Public
                   | System.Reflection.BindingFlags.NonPublic
                   | System.Reflection.BindingFlags.Instance
                   );
                for (int i = 0; i < names.Length; ++i)
                {
                    _addField(new FieldPair(field, i, names[i]));
                }
            }
        }

        public void SetValue(int indexer, object vValue)
        {
            mFields[indexer].setValue(mObjects[indexer], vValue);
        }

        public object GetValue(int indexer)
        {
            return mFields[indexer].getValue(mObjects[indexer]);
        }

        public int getFiledIndexByName(string name)
        {
            int index;
            if (mFiledNameToIndex.TryGetValue(name, out index))
            {
                return index;
            }
            return -1;
        }
        public int getCatchColume(int index)
        {
            return mCatchColumes[index];
        }

        public void setCatchColume(int index, int iCatchColumeIndex)
        {
            mCatchColumes[index] = iCatchColumeIndex;
        }

        public void catchField()
        {
            mCatchFields = true;
            var mColumnCount = mListFields.Count;
            mObjects = new object[mColumnCount];
            mFields = new FieldPair[mColumnCount];
            mCatchColumes = new int[mColumnCount];
            for (int i = 0; i < mCatchColumes.Length; ++i)
            {
                mCatchColumes[i] = -1;
            }
            for (int i = 0; i < mListFields.Count; i++)
            {
                mFields[i] = mListFields[i];
            }
            mListFields = null;
        }

        public void clear()
        {
            mCurIndex = 0;
        }
        public bool isCatched()
        {
            return mCatchFields;
        }
        public bool isEmpty()
        {
            return mObjects.Length == 0;
        }
    }
    public class FieldPairTable_v2<T>
    {
        public delegate object GatParamValueFromObjFun(T obj);
        public delegate void SatParamValueFromObjFun(T obj, object val);
        public class FieldPairInfo
        {
            public GatParamValueFromObjFun mGetParamFun;
            public FieldPair mFiled;
            public SatParamValueFromObjFun mSetParamFun;
            public int mIndex;
        }
        public List<FieldPairInfo> mFields = new List<FieldPairInfo>();
        public Dictionary<string, FieldPairInfo> mFiledMap = new Dictionary<string, FieldPairInfo>();
        public List<int> mCatchColumes;
        public void addFiled(GatParamValueFromObjFun getParamfun, System.Reflection.FieldInfo filedInfo, string name)
        {
            addFiled(getParamfun, new FieldPair(filedInfo, name));
        }
        public void addFiled(GatParamValueFromObjFun getParamfun, FieldPair filed)
        {
            FieldPairInfo info = new FieldPairInfo();
            info.mGetParamFun = getParamfun;
            info.mFiled = filed;
            info.mIndex = mFields.Count;
            mFields.Add(info);
            mFiledMap[filed.name] = info;
        }
        public void addArrayFiled(GatParamValueFromObjFun getParamfun, System.Reflection.FieldInfo filedInfo, string[] names)
        {
            for (int i = 0; i < names.Length; ++i)
            {
                addFiled(getParamfun, new FieldPair(filedInfo, i, names[i]));
            }
        }

        public void SetValue(int indexer, T obj, object vValue)
        {
            mFields[indexer].mFiled.setValue(mFields[indexer].mGetParamFun(obj), vValue);
        }
        public object getValue(int indexer, T obj)
        {
            return mFields[indexer].mFiled.getValue(mFields[indexer].mGetParamFun(obj));
        }
        public int getFiledIndexByName(string name)
        {
            FieldPairInfo info;
            if (mFiledMap.TryGetValue(name, out info))
            {
                return info.mIndex;
            }
            return -1;
        }

        public int getCatchColume(int index)
        {
            return mCatchColumes[index];
        }

        public void setCatchColume(int index, int iCatchColumeIndex)
        {
            if (index >= mCatchColumes.Count)
            {
                int iCountIndex = index - mCatchColumes.Count + 1;
                for (int i = 0; i < iCountIndex; i++)
                {
                    mCatchColumes.Add(-1);
                }
            }
            mCatchColumes[index] = iCatchColumeIndex;
        }
    }
    public abstract class TableItem
    {
        public const char keyParser = '|';
        public class _DataItem
        {
            public string dataName;
            public System.Object dataValue;
        }

        public TableItem()
        {

        }

        public virtual bool FillData(_DataItem[] data)
        {
            foreach (_DataItem item in data)
            {
                System.Reflection.PropertyInfo property = GetType().GetProperty(item.dataName);
                if (property == null)
                {
                    Debug.LogWarning("read table Warning data_name:[" + this.GetType().FullName + "]" + item.dataName);
                    continue;
                }
                property.SetValue(this, item.dataValue, null);
            }
            return true;
        }

        public abstract void CollectFields(FieldPairTable fields);
        public virtual string OutPut(int colume)
        {
            return null;
        }

        public static int[] SplitToInts(string strValue, int idefualt = -1)
        {
            var _values = strValue.Split(keyParser);
            int[] vals = new int[_values.Length];
            for (int i = 0; i < _values.Length; ++i)
            {
                if (!int.TryParse(_values[i], out vals[i]))
                {
                    vals[i] = idefualt;
                }
            }
            return vals;
        }

        public static string[] SplitToString(string strValue)
        {
            return strValue.Split(keyParser);
        }

        public static int[] SpliteToInts(string strValue, string[] customParser, int idefault = -1)
        {
            var _values = strValue.Split(customParser, StringSplitOptions.RemoveEmptyEntries);
            int[] vals = new int[_values.Length];
            for (int i = 0; i < _values.Length; i++)
            {
                if(!int.TryParse(_values[i],out vals[i]))
                {
                    vals[i] = idefault;
                }
            }
            return vals;
        }

        public virtual void Init()
        {

        }

    }

}
