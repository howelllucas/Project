//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class PassSpecial_data : ScriptableObject {

		[SerializeField, HideInInspector]
		private PassSpecial_dataItem[] _Items;
		public PassSpecial_dataItem[] items { get { return _Items; } }

		public PassSpecial_dataItem Get(int level) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				PassSpecial_dataItem item = _Items[index];
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
	public class PassSpecial_dataItem {

		[SerializeField, HideInInspector]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField, HideInInspector]
		private float _Coinparam;
		public float coinparam { get { return _Coinparam; } }

		public override string ToString() {
			return string.Format("[PassSpecial_dataItem]{{level:{0}, coinparam:{1}}}",
				level, coinparam);
		}

	}

}
