//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Shop_data : ScriptableObject {

		[SerializeField, HideInInspector]
		private Shop_dataItem[] _Items;
		public Shop_dataItem[] items { get { return _Items; } }

		public Shop_dataItem Get(int level) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				Shop_dataItem item = _Items[index];
				if (item.level == level) { return item; }
				if (level < item.level) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class Shop_dataItem {

		[SerializeField, HideInInspector]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField, HideInInspector]
		private float _StageGet_1;
		public float stageGet_1 { get { return _StageGet_1; } }

		[SerializeField, HideInInspector]
		private float _StageGet_2;
		public float stageGet_2 { get { return _StageGet_2; } }

		[SerializeField, HideInInspector]
		private float _StageGet_3;
		public float stageGet_3 { get { return _StageGet_3; } }

		public override string ToString() {
			return string.Format("[Shop_dataItem]{{level:{0}, stageGet_1:{1}, stageGet_2:{2}, stageGet_3:{3}}}",
				level, stageGet_1, stageGet_2, stageGet_3);
		}

	}

}
