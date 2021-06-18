//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Tips : ScriptableObject {

		[SerializeField, HideInInspector]
		private TipsItem[] _Items;
		private TipsItem[] items { get { return _Items; } }

		public TipsItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				TipsItem item = _Items[index];
				if (item.id == id) { return item; }
				if (id < item.id) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class TipsItem {

		[SerializeField, HideInInspector]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private string _Txtcontent;
		public string txtcontent { get { return _Txtcontent; } }

		[SerializeField, HideInInspector]
		private string _Zh;
		public string zh { get { return _Zh; } }

		[SerializeField, HideInInspector]
		private string _Zh_tw;
		public string zh_tw { get { return _Zh_tw; } }

		[SerializeField, HideInInspector]
		private string _De;
		public string de { get { return _De; } }

		[SerializeField, HideInInspector]
		private string _Ja;
		public string ja { get { return _Ja; } }

		[SerializeField, HideInInspector]
		private string _Ko;
		public string ko { get { return _Ko; } }

		[SerializeField, HideInInspector]
		private string _Fr;
		public string fr { get { return _Fr; } }

		[SerializeField, HideInInspector]
		private string _Ru;
		public string ru { get { return _Ru; } }

		public override string ToString() {
			return string.Format("[TipsItem]{{id:{0}, txtcontent:{1}, zh:{2}, zh_tw:{3}, de:{4}, ja:{5}, ko:{6}, fr:{7}, ru:{8}}}",
				id, txtcontent, zh, zh_tw, de, ja, ko, fr, ru);
		}

	}

}
