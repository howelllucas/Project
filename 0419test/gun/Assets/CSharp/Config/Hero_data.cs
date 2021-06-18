//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Hero_data : ScriptableObject {

		[SerializeField, HideInInspector]
		private Hero_dataItem[] _Items;
		public Hero_dataItem[] items { get { return _Items; } }

		public Hero_dataItem Get(int level) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				Hero_dataItem item = _Items[index];
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
	public class Hero_dataItem {

		[SerializeField, HideInInspector]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField, HideInInspector]
		private float _ExpRequire;
		public float expRequire { get { return _ExpRequire; } }

		[SerializeField, HideInInspector]
		private float _AtkParams;
		public float atkParams { get { return _AtkParams; } }

		public override string ToString() {
			return string.Format("[Hero_dataItem]{{level:{0}, expRequire:{1}, atkParams:{2}}}",
				level, expRequire, atkParams);
		}

	}

}
