
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

namespace Game
{
    public class Table<T> : TableLoader where T : TableItem, new()
    {
        protected Dictionary<string, T> item_dic;
        public Table()
        {
            item_dic = new Dictionary<string, T>();
        }
        public Table(Dictionary<string, T> _dic)
        {
            item_dic = _dic;
        }
        protected override TableItem GenerateItem()
        {
            return new T();
        }
        protected override void Clear()
        {
            base.Clear();
            item_dic.Clear();
        }
        public T GetItemByID(int id)
        {
            return GetItemByID(id.ToString());
        }
        public T GetItemByID(string id)
        {
            T obj = null;
            if (!item_dic.TryGetValue(id, out obj))
            {
                Debug.LogFormat("Not Find id:{0} in {1}", id, fileName);
                return null;
            }
            if (obj == null)
            {
                obj = createItemByRowDatas(id) as T;
                item_dic[id] = obj;
            }
            return obj;
        }

        void fileData()
        {
            if (_file_datas == null)
                return;
            foreach (var _pair in _file_datas)
            {
                string id = _pair.Key;
                var obj = _createItemByRowDatas(id, _pair.Value);
                item_dic[id] = obj as T;
            }
            _file_datas = null;
        }

        public IEnumerable<T> getEnumeratorT()
        {
            if (_file_datas != null)
            {
                fileData();
            }

            return item_dic.Values;
        }
        public override IEnumerable<TableItem> getEnumerator()
        {
            foreach (var item in item_dic.Values)
            {
                yield return item;
            }
        }
        public IEnumerable<T> Items { get { return getEnumeratorT(); } }
        public IEnumerable<TableItem> ResItems
        {
            get
            {
                return getEnumerator();
            }
        }
        

        public int ItemCount { get { return item_dic.Count; } }

        public T this[int id]
        {
            get
            {
                T t = GetItemByID(id);
                if (t == null)
                {
                    Debug.LogWarningFormat( "[{0}] table not find [{1}] item ", fileName , id );
                }
                return t;
            }
        }

        public T this[string id]
        {
            get
            {
                T t = GetItemByID(id);
                if (t == null)
                {
                    Debug.LogWarningFormat("[{0}] table not find [{1}] item ", fileName, id);
                }
                return t;
            }
        }

        public object GetItemValueByName(int id, string name)
        {
            return GetItemValueByName(id.ToString(), name);
        }
        public object GetItemValueByName(string id, string name)
        {
            TableItem item = GetItemByID(id);
            if (item == null)
                return null;
            var _FieldPairTable = makeFieldPairTable(item);
            var FieldIndex = _FieldPairTable.getFiledIndexByName(name);
            if (FieldIndex >= 0)
            {
                return _FieldPairTable.GetValue(FieldIndex);
            }
            else
            {
                Debug.LogError("not find file [" + fileName + "]field[" + name + "]");
            }
            return null;
        }

        private bool CheckFile(string m_path)
        {
            return System.IO.File.Exists(m_path);
        }
       
        protected override bool FillData()
        {
            catchDatas = new TableItem._DataItem[_data_type.Length];
            int columeIndex = 0;
            for (int i = 0 ; i < _data_type.Length ; i++)
            {
                type e = _data_type[i];
                TableItem._DataItem item = new TableItem._DataItem();
                item.dataName = e._name;
                catchDatas[i]=item;
                nameToColume[e._name] = columeIndex;
                columeIndex++;
            }
            makeFieldPairTable(new T());
            if ( fieldPairTable!=null && !fieldPairTable.isEmpty())
            {
                if (fieldPairTable.mFields.Length != columnCount)
                {
                    Debug.Log("table[" + fileName + "]load warning:table filed[" + Convert.ToString(columnCount)
                        + "]exe filed[" + Convert.ToString(fieldPairTable.mFields.Length) + "]");
                }
            }

            foreach (var item in Items)
            {
                item.Init();
            }
            return true;
        }

        public void makeDataType()
        {
            if (fieldPairTable == null)
            {
                makeFieldPairTable(new T());
            }
            _data_type = new type[fieldPairTable.mFields.Length];
            for (int i = 0 ; i < fieldPairTable.mFields.Length;++i)
            {
                var fields = fieldPairTable.mFields[i];
                var _type = new type();
                _data_type[i] = _type;
                _type._name = fields.name;
                switch (fields.filed.FieldType.FullName)
                {
                    case "System.Int32":
                        {
                            _type._type = "int";
                            _type.mType = eColumType.eInt;
                        }break;
                    case "System.UInt32":
                        {
                            _type._type = "uint";
                            _type.mType = eColumType.eUInt;
                        }break;
                    case "System.Single":
                        {
                            _type._type = "float";
                            _type.mType = eColumType.eFloat;
                        }break;
                    case "System.String":
                        {
                            _type._type = "string";
                            _type.mType = eColumType.eString;
                        }break;
                    case "System.Double":
                        {
                            _type._type = "double";
                            _type.mType = eColumType.eDouble;
                        }
                        break;
                }
            }
        }
        public string makeHeaderString()
        {
            if (_data_type == null)
                makeDataType();
            StringBuilder strBuild = new StringBuilder();
            for (int i = 0; i < _data_type.Length - 1; i++)
            {
                strBuild.Append(getTypeBack(_data_type[i].mType));
                strBuild.Append("\t");
            }
            strBuild.Append(getTypeBack(_data_type[_data_type.Length - 1].mType));
            strBuild.Append("\n");
            for (int i = 0; i < _data_type.Length - 1; i++)
            {
                strBuild.Append(_data_type[i]._name);
                strBuild.Append("\t");
            }
            strBuild.Append(_data_type[_data_type.Length - 1]._name);
            strBuild.Append("\n");
            return strBuild.ToString();
        }

        public string makeItemString( string id )
        {
            StringBuilder strBuild = new StringBuilder();
            T item = GetItemByID(id);
            if (item == null)
            {
                return "";
            }
            for (int j = 0; j < _data_type.Length - 1; j++)
            {
                strBuild.Append(item.OutPut(j));
                strBuild.Append("\t");
            }
            strBuild.Append(item.OutPut(_data_type.Length - 1));
            strBuild.Append("\n");
            return strBuild.ToString();
        }
        public string makeTableString()
        {
            StringBuilder strBuild = new StringBuilder();
            strBuild.Append( makeHeaderString() );
            
            foreach( var pair_ in item_dic )
            {
                var item = pair_.Value;
                if (item == null)
                {
                    continue;
                }
                strBuild.Append(makeItemString(pair_.Key));
            }
            return strBuild.ToString();
        }
        public bool WriteTableToFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return false;

            string file = makeTableString();

            Encoding encod = Encoding.Default;
            StreamWriter writer = new StreamWriter(filename, false, encod);
            writer.Write(file);
            writer.Close();
            writer = null;
            return true;
        }
        string getTypeBack(eColumType _type)
        {
            switch (_type)
            {
                case eColumType.eInt: return "int";
                case eColumType.eUInt: return "uint";
                case eColumType.eFloat: return "float";
                case eColumType.eString: return "string";
                case eColumType.eDouble: return "double";
                default: return "stringlist";
            }
        }
    }

}
