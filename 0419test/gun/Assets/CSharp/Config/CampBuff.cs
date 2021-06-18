//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class CampBuff : ScriptableObject {

		[SerializeField, HideInInspector]
		private CampBuffItem[] _Items;
		public CampBuffItem[] items { get { return _Items; } }

		public CampBuffItem Get(int show_index) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				CampBuffItem item = _Items[index];
				if (item.show_index == show_index) { return item; }
				if (show_index < item.show_index) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class CampBuffItem {

		[SerializeField, HideInInspector]
		private int _Show_index;
		public int show_index { get { return _Show_index; } }

		[SerializeField, HideInInspector]
		private string _Id;
		public string id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private int _Name;
		public int name { get { return _Name; } }

		[SerializeField, HideInInspector]
		private int _Desc;
		public int desc { get { return _Desc; } }

		[SerializeField, HideInInspector]
		private int _Desc_nolearn;
		public int desc_nolearn { get { return _Desc_nolearn; } }

		[SerializeField, HideInInspector]
		private int _Max_desc;
		public int max_desc { get { return _Max_desc; } }

		[SerializeField, HideInInspector]
		private string _Icon;
		public string icon { get { return _Icon; } }

		[SerializeField, HideInInspector]
		private int _UnlockCost;
		public int unlockCost { get { return _UnlockCost; } }

		[SerializeField, HideInInspector]
		private int _CampLevel;
		public int campLevel { get { return _CampLevel; } }

		[SerializeField, HideInInspector]
		private int _InitValue;
		public int initValue { get { return _InitValue; } }

		[SerializeField, HideInInspector]
		private int _Denominator;
		public int denominator { get { return _Denominator; } }

		public override string ToString() {
			return string.Format("[CampBuffItem]{{show_index:{0}, id:{1}, name:{2}, desc:{3}, desc_nolearn:{4}, max_desc:{5}, icon:{6}, unlockCost:{7}, campLevel:{8}, initValue:{9}, denominator:{10}}}",
				show_index, id, name, desc, desc_nolearn, max_desc, icon, unlockCost, campLevel, initValue, denominator);
		}

	}

}
