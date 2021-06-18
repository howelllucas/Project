
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class CheckInfo
    {
        public string path;
        public string tableName;
        public virtual void CheckAtlas(string _path, string img, string msg)
        {
        }
        public virtual void CheckMp3(string _path, string msg)
        {

        }
        public virtual void CheckModel(string _path, string msg)
        {
        }
        public virtual void CheckTexture(string _path, string msg)
        {
        }
        public virtual void CheckEffect(string _path, int id, string msg)
        {
        }
    }
    public enum TableInitType
    {
        Client=1,
        Server=2
    }
    public class TableLoader 
    {
        public string fileName;
        
        public Dictionary<string, int> nameToColume = new Dictionary<string, int>();
        public FieldPairTable fieldPairTable;
        public bool hasCatchColume = false;
        protected int columnCount = 0;
        protected bool needReplaceEnter = false;
        
        public const int MAX_TABLE_ID = 0;
        protected TableItem._DataItem[] catchDatas = null;

        protected int maxId = 0;
        public int MaxId
        {
            get { return maxId; }
        }
        public enum eColumType
        {
            eUInt,
            eInt,
            eFloat,
            eString,
            eDouble,
        }
        public class type
        {
            public string _type;
            public string _name;
            public string _note;
            public eColumType mType;
        }
        protected type[] _data_type;
        protected Dictionary<string, string[]> _file_datas = new Dictionary<string, string[]>();
        protected const int Data_Begin_Line = 3;

        protected virtual bool FillData()
        {
            return true;
        }

        protected virtual TableItem GenerateItem()
        {
            return null;
        }

        public virtual IEnumerable<TableItem> getEnumerator()
        {
            return null;
        }
        public virtual void CheckRes(List<string> list)
        {

        }
        public virtual void Init(TableInitType type)
        {

        }
        public virtual void Check(CheckInfo info)
        {
            info.tableName = fileName;
        }

        protected TableItem _createItemByRowDatas(string id, string[] fileDatas)
        {
            var _item = GenerateItem();
            var _FieldPairTable = makeFieldPairTable(_item);
            //m_CatchDatas[0]._data_value = id;
            if (!GenerateDataByLine(fileDatas, catchDatas))
            {
                Debug.LogError("table[" + fileName + "] GenerateDataByLine error:id[" + id + "]");
                return null;
            }

            if (!_FieldPairTable.isEmpty())
            {
                for (int i = 0; i < _FieldPairTable.mFields.Length; ++i)
                {
                    int iColumn = _FieldPairTable.getCatchColume(i);
                    if (iColumn >= 0 && iColumn < catchDatas.Length)
                    {
                        try
                        {
                            _FieldPairTable.SetValue(i, catchDatas[iColumn].dataValue);
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError("table[" + fileName + "]load error:[" + _FieldPairTable.mFields[i].name + "]msg:" + ex.Message);
                        }
                    }
                    else
                    {
                        //Debug.LogError("表[" + _file_name + "]加载出错:[" + _FieldPairTable.mFields[i].name + "]"); ;
                    }
                }
            }
            else
            {
                if (!_item.FillData(catchDatas))
                {
                    Debug.LogError("load table[" + fileName + "]error!");
                    return null;
                }
            }
            return _item;
        }
        protected TableItem createItemByRowDatas(string id)
        {
            if (_file_datas == null)
                return null;

            string[] fileDatas;
            if (!_file_datas.TryGetValue(id, out fileDatas))
                return null;
            var _item = _createItemByRowDatas(id, fileDatas);
            _file_datas.Remove(id);
            if (_file_datas.Count == 0)
            {
                _file_datas = null;
            }
            return _item;
        }

        public string LoadTxt(string fileName)
        {
            var res = ResLoadMgr.singleton.ResourceLoad(fileName);
            if (res != null)
            {
                var text = res.GetResObject() as TextAsset;
                if (text != null)
                {
                    var str = text.text;
                    ResLoadMgr.singleton.UnLoadResource(fileName);
                    return str;
                }
            }
            return "";
        }

        public virtual bool Read(string filename, bool NeedReplaceEnter = false)
        {
            try
            {
                bool bRet = ReadFromString(filename, LoadTxt(filename), NeedReplaceEnter);
                return bRet;
            }
            catch(Exception e)
            {
                Debug.LogErrorFormat("read {0} error:{1}",filename, e);
            }
            return false;
        }

        public virtual bool ReadStrem(Stream file, string filename, bool NeedReplaceEnter = false)
        {
            var reader = new StreamReader(file, Encoding.Default);
            bool bRet = ReadFromString(filename, reader.ReadToEnd(), NeedReplaceEnter);
            reader.Close();
            reader = null;
            return bRet;
        }
        protected virtual void Clear()
        {
            needReplaceEnter = false;
            _data_type = null;
            _file_datas = new Dictionary<string, string[]>();
            nameToColume.Clear();
            hasCatchColume = false;
            fieldPairTable = null;
            maxId = 0;
            columnCount = 0;
            //_item_array = null;
            fileName = "";
        }
        public virtual bool ReadFromString(string filename, string fileContent , bool NeedReplaceEnter = false)
        {
            if (fileContent == null)
            {
                Debug.LogError("{0} is empty!");
                return false;
            }
            Clear();
            needReplaceEnter = NeedReplaceEnter;
            _data_type = null;
            _file_datas = new Dictionary<string, string[]>();
            nameToColume.Clear();
            hasCatchColume = false;
            //_item_dic.Clear();
            fieldPairTable = null;
            maxId = 0;
            columnCount = 0;
            //_item_array = null;
            fileName = filename;
            try
            {
                bool bRet = Analyse(fileContent);
                if (bRet)
                {
                    if (!FillData())
                        return false;
                }
                else
                {
                    Debug.LogErrorFormat("table[{0}]load data error !", fileName );
                }
                return bRet;
            }
            catch (Exception e)
            {
                Debug.LogError("load [" + filename + "]error!" + e.ToString());
                return false;
            }
        }
        public int getColumIndex(string name)
        {
            int colum;
            if (nameToColume.TryGetValue(name, out colum))
                return colum;
            return -1;
        }

        public FieldPairTable makeFieldPairTable(TableItem _item)
        {
            if (fieldPairTable == null)
            {
                fieldPairTable = new FieldPairTable();
                _item.CollectFields(fieldPairTable);
                fieldPairTable.catchField();
                _item.CollectFields(fieldPairTable);
            }
            else
            {
                fieldPairTable.clear();
                _item.CollectFields(fieldPairTable);
            }
            if (!hasCatchColume)
            {
                if (nameToColume.Count != 0)
                {
                    for (int i = 0; i < fieldPairTable.mFields.Length; ++i)
                    {
                        int iColumn = fieldPairTable.getCatchColume(i);
                        if (iColumn < 0)
                        {
                            iColumn = getColumIndex(fieldPairTable.mFields[i].name);
                            fieldPairTable.setCatchColume(i, iColumn);
                            if (iColumn < 0)
                            {
                                Debug.LogError("table[" + fileName + "]load error:not find [" + fieldPairTable.mFields[i].name + "]");
                            }
                        }
                    }
                    foreach (KeyValuePair<string, int> item in nameToColume)
                    {
                        int iColumn = fieldPairTable.getFiledIndexByName(item.Key);
                        if (iColumn < 0)
                        {
                            Debug.LogWarningFormat("table[{0}] column:[{1}] Has not been used!", fileName, item.Key);
                        }
                    }
                    hasCatchColume = true;
                }
            }

            return fieldPairTable;
        }

        public bool GenerateFile(string filename)
        {
            if (fileName == null)
                return false;

            string file = null;
            file = "using System;\n\n";
            file += "namespace CommonLib\n";
            file += "{\n";
            file += "\tclass " + Path.GetFileNameWithoutExtension(fileName) + " : _TableItem" + "\n";
            file += "\t{\n";

            for (int i = 0; i < _data_type.Length; i++)
            {
                string value_name = "m_" + _data_type[i]._name;
                file += "\t\t" + "private " + _data_type[i]._type + " " + value_name + ";" + "\n";
                file += "\t\t" + "public " + _data_type[i]._type + " " + _data_type[i]._name;
                file += " { get { return " + value_name + "; } set { " + value_name + " = value; } }" + "\n";
                file += "\n";
            }
            file += "\t}\n";
            file += "}\n";
            Encoding encod = Encoding.Default;
            StreamWriter writer = new StreamWriter(filename, false, encod);
            writer.Write(file);
            writer.Close();
            writer = null;
            return true;
        }

        private bool Analyse(string fileContent)
        {
            if (!ReadData(fileContent))
                return false;
            return true;
        }
        private bool ReadType(string fileContent, string str, out int read_pos)
        {
            int row_count = 0;
            read_pos = 0;
            string element = "";
            int iStart = 0;
            List<string> _file_Types = new List<string>();
            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '\t':
                        if (iStart < i)
                        {
                            element = fileContent.Substring(iStart, i - iStart);
                            //element = getSubString(str, iStart, i);
                        }
                        else
                        {
                            element = "";
                        }
                        iStart = i + 1;
                        _file_Types.Add(element);
                        element = "";
                        if (row_count == 0)
                            columnCount++;
                        break;
                    case '\n':
                    case '\r':
                        if (iStart < i)
                        {
                            element = fileContent.Substring(iStart, i - iStart);
                            //element = getSubString(str, iStart, i);
                        }
                        else
                        {
                            element = "";
                        }
                        iStart = i + 1;
                        _file_Types.Add(element);
                        if (row_count == 0)
                            columnCount++;

                        row_count++;
                        if (i + 1 < str.Length)
                        {
                            if (str[i + 1] == '\n')
                            {
                                i++;
                                iStart = i + 1;
                            }
                        }
                        if (row_count >= Data_Begin_Line)
                        {
                            read_pos = i + 1;
                            break;
                        }
                        break;
                    case '\0':
                        iStart = i + 1;
                        break;
                    default:
                        //element += str[i];
                        break;
                }
                if (row_count >= Data_Begin_Line)
                {
                    break;
                }
            }

            if (_file_Types.Count != Data_Begin_Line * columnCount)
                return false;
            return GenerateType(_file_Types);
        }
        private bool ReadData(string fileContent)
        {
            int _cur_column = 0;
            string element = "";
            int start_pos = 0;
            maxId = 0;
            //char[] str = fileContent.ToCharArray();
            var str = fileContent;
            if (!ReadType(fileContent, str, out start_pos))
            {
                Debug.LogErrorFormat( "[{0}] ReadType error!", fileName );
                return false;
            }
            if (_file_datas == null)
            {
                _file_datas = new Dictionary<string, string[]>();
            }
            int iStart = start_pos;
            string[] tempData = new string[columnCount];
            for (int i = start_pos; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '\t':
                        if (iStart < i)
                        {
                            element = fileContent.Substring(iStart, i - iStart);
                            //element = getSubString(str, iStart, i);
                        }
                        else
                        {
                            element = "";
                        }
                        tempData[_cur_column++] = element;
                        iStart = i + 1;
                        element = "";
                        break;
                    case '\n':
                    case '\r':
                        if (iStart < i)
                        {
                            element = fileContent.Substring(iStart, i - iStart);
                            //element = getSubString(str, iStart, i);
                        }
                        else
                        {
                            element = "";
                        }
                        tempData[_cur_column++] = element;
                        iStart = i + 1;
                        if (_cur_column != columnCount)
                        {
                            Debug.LogErrorFormat("table[{0}]load id data colume error !data column:{1} , title colume{2},lase element:{3}", fileName , _cur_column , columnCount , element);
                            return false;
                        }
                        try
                        {
                            string id = tempData[0];
                            try
                            {
                                _file_datas.Add(id, tempData);
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("table[" + fileName + "]load id " + id.ToString() + " error !" + e.Message);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("table[" + fileName + "]load error:index[" + _file_datas.Count + "]colume[" + _data_type[0]._name + "]error msg" + e);
                        }
                        _cur_column = 0;
                        tempData = new string[columnCount];
                        if (i + 1 < str.Length)
                        {
                            if (str[i + 1] == '\n')
                            {
                                i++;
                                iStart = i + 1;
                            }
                        }
                        break;
                    case '\0':
                        //if (_file_data.Count != column_count * row_count)
                        //    return false;
                        iStart = i + 1;
                        break;
                    default:
                        //element += str[i];
                        break;
                }
            }
            return true;
        }

        static int[] s_catch;
        static int[] s_catch_add;
        static void initCatch()
        {
            if (s_catch == null)
            {
                s_catch = new int[128];
                s_catch_add = new int[128];
                s_catch['-'] = -1; s_catch_add['-'] = 0;
                s_catch['0'] = 10; s_catch_add['0'] = 0;
                s_catch['1'] = 10; s_catch_add['1'] = 1;
                s_catch['2'] = 10; s_catch_add['2'] = 2;
                s_catch['3'] = 10; s_catch_add['3'] = 3;
                s_catch['4'] = 10; s_catch_add['4'] = 4;
                s_catch['5'] = 10; s_catch_add['5'] = 5;
                s_catch['6'] = 10; s_catch_add['6'] = 6;
                s_catch['7'] = 10; s_catch_add['7'] = 7;
                s_catch['8'] = 10; s_catch_add['8'] = 8;
                s_catch['9'] = 10; s_catch_add['9'] = 9;
            }
        }
        static int toInt(string strValue)
        {
            initCatch();
            var chars = strValue;
            int iV = 0;
            for (int i = 0; i < chars.Length; ++i)
            {
                var c = chars[i];
                int mult = s_catch[c];
                int add = s_catch_add[c];
                iV = iV * mult + add;
            }
            return iV;
        }

        private bool GenerateType(List<string> _file_Types)
        {
            _data_type = new type[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                type data = new type();
                data._name = _file_Types[i];
                string str_type = _file_Types[columnCount + i];
                switch (str_type)
                {
                    case "uint":
                        data._type = "uint";
                        data.mType = eColumType.eUInt;
                        break;
                    case "int":
                        data._type = "int";
                        data.mType = eColumType.eInt;
                        break;
                    case "float":
                        data._type = "float";
                        data.mType = eColumType.eFloat;
                        break;
                    case "double":
                        data._type = "double";
                        data.mType = eColumType.eDouble;
                        break;
                    case "string":
                    case "stringlist":
                        data.mType = eColumType.eString;
                        data._type = "string";
                        break;
                    default:
                        {
                            Debug.LogErrorFormat("not convert type:{0}", str_type);
                            return false;
                        }
                }
                
                data._note = (string)_file_Types[columnCount * 2 + i];
                _data_type[i] = data;
            }
            //GameMain.outputUseTimeInfo("ReadData GenerateType:" + _file_name);
            return true;
        }
        protected bool GenerateDataByLine(string[] strLineValues, TableItem._DataItem[] outValues)
        {
            //var objs = outValues;
            for (int j = 0; j < columnCount; j++)
            {
                var etype = _data_type[j].mType;
                switch (etype)
                {
                    case eColumType.eUInt:
                        {
                            try
                            {
                                string data = strLineValues[j];
                                if (data.Length > 0)
                                    outValues[j].dataValue = uint.Parse(data);
                                else
                                    outValues[j].dataValue = 0u;
                                //index += column_count;
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("table[" + fileName + "]load error:id[" + strLineValues[0] + "]colume[" + _data_type[j]._name + "]error msg" + e);
                            }
                        }
                        break;
                    //case "INT":
                    case eColumType.eInt:
                        {
                            try
                            {
                                string data = strLineValues[j];
                                if (data.Length > 0)
                                    outValues[j].dataValue = int.Parse(data);
                                else
                                    outValues[j].dataValue = 0;
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("table[" + fileName + "]load error:id[" + strLineValues[0] + "]colume[" + _data_type[j]._name + "]error msg" + e);
                            }
                        }
                        break;
                    //case "FLOAT":
                    case eColumType.eFloat:
                        {
                            try
                            {
                                string data = strLineValues[j];
                                if (data.Length > 0)
                                {
                                    var idx = data.IndexOf('%');
                                    if (idx > 0)
                                    {
                                        outValues[j].dataValue = float.Parse(data.Remove(idx)) / 100.0f;
                                    }
                                    else
                                        outValues[j].dataValue = float.Parse(data.Trim('\r', ' '), CultureInfo.InvariantCulture);
                                }
                                else
                                    outValues[j].dataValue = 0.0f;
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("table[" + fileName + "]load error:id[" + strLineValues[0] + "]colume[" + _data_type[j]._name + "]error msg" + e);
                            }
                        }
                        break;
                    case eColumType.eDouble:
                        {
                            try
                            {
                                string data = strLineValues[j];
                                if (data.Length > 0)
                                {
                                    var idx = data.IndexOf('%');
                                    if (idx > 0)
                                    {
                                        outValues[j].dataValue = double.Parse(data.Remove(idx)) / 100.0f;
                                    }
                                    else
                                        outValues[j].dataValue = double.Parse(data.Trim('\r', ' '), CultureInfo.InvariantCulture);
                                }
                                else
                                    outValues[j].dataValue = 0.0f;
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("table[" + fileName + "]load error:id[" + strLineValues[0] + "]colume[" + _data_type[j]._name + "]error msg" + e);
                            }
                        }
                        break;
                    //case "STRING":
                    //case "STRINGLIST":
                    case eColumType.eString:
                        {
                            if (needReplaceEnter)
                            {
                                try
                                {
                                    string data = strLineValues[j];
                                    outValues[j].dataValue = data.Replace("\\n", "\n");
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError("table[" + fileName + "]load error:id[" + strLineValues[0] + "]colume[" + _data_type[j]._name + "]error msg" + e);
                                }
                            }
                            else
                            {
                                outValues[j].dataValue = strLineValues[j];
                            }
                        }
                        break;
                    default:
                        return false;
                }
            }
            return true;
        }

        protected void ClearRowFileDataByLine(string iLine)
        {
            _file_datas.Remove(iLine);
        }
        public type[] GetTableTypes()
        {
            return _data_type;
        }
        public void SetTableTypes(type[] mList)
        {
            if (mList != null)
            {
                _data_type = mList;
            }
        }
    }
}
