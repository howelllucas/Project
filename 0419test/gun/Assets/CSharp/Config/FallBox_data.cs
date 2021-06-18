//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class FallBox_data : ScriptableObject {

		[SerializeField, HideInInspector]
		private FallBox_dataItem[] _Items;
		private FallBox_dataItem[] items { get { return _Items; } }

		public FallBox_dataItem Get(int level) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				FallBox_dataItem item = _Items[index];
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
	public class FallBox_dataItem {

		[SerializeField, HideInInspector]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField, HideInInspector]
		private float _Coin;
		public float coin { get { return _Coin; } }

		public override string ToString() {
			return string.Format("[FallBox_dataItem]{{level:{0}, coin:{1}}}",
				level, coin);
		}

	}

}
