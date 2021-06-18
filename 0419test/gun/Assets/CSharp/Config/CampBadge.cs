//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class CampBadge : ScriptableObject {

		[SerializeField, HideInInspector]
		private CampBadgeItem[] _Items;
		public CampBadgeItem[] items { get { return _Items; } }

		public CampBadgeItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				CampBadgeItem item = _Items[index];
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
	public class CampBadgeItem {

		[SerializeField, HideInInspector]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private string _ActiveIcon;
		public string activeIcon { get { return _ActiveIcon; } }

		[SerializeField, HideInInspector]
		private string _NoActiveIcon;
		public string noActiveIcon { get { return _NoActiveIcon; } }

		[SerializeField, HideInInspector]
		private int _Title;
		public int title { get { return _Title; } }

		[SerializeField, HideInInspector]
		private int _Detail;
		public int detail { get { return _Detail; } }

		public override string ToString() {
			return string.Format("[CampBadgeItem]{{id:{0}, activeIcon:{1}, noActiveIcon:{2}, title:{3}, detail:{4}}}",
				id, activeIcon, noActiveIcon, title, detail);
		}

	}

}
