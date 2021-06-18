//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class GeneralConfig : ScriptableObject {

		[SerializeField, HideInInspector]
		private GeneralConfigItem[] _Items;
		private GeneralConfigItem[] items { get { return _Items; } }

		public GeneralConfigItem Get(string id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				GeneralConfigItem item = _Items[index];
				if (item.id == id) { return item; }
				if (string.Compare(id, item.id) < 0) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class GeneralConfigItem {

		[SerializeField, HideInInspector]
		private string _Id;
		public string id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private string _Content;
		public string content { get { return _Content; } }

		[SerializeField, HideInInspector]
		private string[] _Contents;
		public string[] contents { get { return _Contents; } }

		public override string ToString() {
			return string.Format("[GeneralConfigItem]{{id:{0}, content:{1}, contents:{2}}}",
				id, content, array2string(contents));
		}

		private string array2string(System.Array array) {
			int len = array.Length;
			string[] strs = new string[len];
			for (int i = 0; i < len; i++) {
				strs[i] = string.Format("{0}", array.GetValue(i));
			}
			return string.Concat("[", string.Join(", ", strs), "]");
		}

	}

}
