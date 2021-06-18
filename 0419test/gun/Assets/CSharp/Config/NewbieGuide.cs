//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class NewbieGuide : ScriptableObject {

		[SerializeField, HideInInspector]
		private NewbieGuideItem[] _Items;
		public NewbieGuideItem[] items { get { return _Items; } }

		public NewbieGuideItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				NewbieGuideItem item = _Items[index];
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
	public class NewbieGuideItem {

		[SerializeField, HideInInspector]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private int _Tips;
		public int tips { get { return _Tips; } }

		[SerializeField, HideInInspector]
		private float _Tips_y;
		public float tips_y { get { return _Tips_y; } }

		[SerializeField, HideInInspector]
		private float[] _Condition;
		public float[] condition { get { return _Condition; } }

		[SerializeField, HideInInspector]
		private float[] _Condition1;
		public float[] condition1 { get { return _Condition1; } }

		[SerializeField, HideInInspector]
		private int _Pre_id;
		public int pre_id { get { return _Pre_id; } }

		[SerializeField, HideInInspector]
		private int _Post_id;
		public int post_id { get { return _Post_id; } }

		[SerializeField, HideInInspector]
		private int _Root_id;
		public int root_id { get { return _Root_id; } }

		[SerializeField, HideInInspector]
		private int _Quit;
		public int quit { get { return _Quit; } }

		[SerializeField, HideInInspector]
		private int _OnlyText;
		public int onlyText { get { return _OnlyText; } }

		[SerializeField, HideInInspector]
		private string _Param;
		public string param { get { return _Param; } }

		[SerializeField, HideInInspector]
		private float _Delay;
		public float delay { get { return _Delay; } }

		public override string ToString() {
			return string.Format("[NewbieGuideItem]{{id:{0}, tips:{1}, tips_y:{2}, condition:{3}, condition1:{4}, pre_id:{5}, post_id:{6}, root_id:{7}, quit:{8}, onlyText:{9}, param:{10}, delay:{11}}}",
				id, tips, tips_y, array2string(condition), array2string(condition1), pre_id, post_id, root_id, quit, onlyText, param, delay);
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
