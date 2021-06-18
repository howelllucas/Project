//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Drop : ScriptableObject {

		[SerializeField]
		private DropItem[] _Items;
		public DropItem[] items { get { return _Items; } }

		public DropItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				DropItem item = _Items[index];
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
	public class DropItem {

		[SerializeField]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField]
		private string[] _Prop;
		public string[] prop { get { return _Prop; } }

		[SerializeField]
		private int[] _Rate;
		public int[] rate { get { return _Rate; } }

		public override string ToString() {
			return string.Format("[DropItem]{{id:{0}, prop:{1}, rate:{2}}}",
				id, array2string(prop), array2string(rate));
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
