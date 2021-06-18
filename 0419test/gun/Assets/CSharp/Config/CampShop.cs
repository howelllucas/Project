//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class CampShop : ScriptableObject {

		[SerializeField, HideInInspector]
		private CampShopItem[] _Items;
		public CampShopItem[] items { get { return _Items; } }

		public CampShopItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				CampShopItem item = _Items[index];
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
	public class CampShopItem {

		[SerializeField, HideInInspector]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private int _PropId;
		public int propId { get { return _PropId; } }

		[SerializeField, HideInInspector]
		private string _PropName;
		public string propName { get { return _PropName; } }

		[SerializeField, HideInInspector]
		private int _PropNum;
		public int propNum { get { return _PropNum; } }

		[SerializeField, HideInInspector]
		private int _HeartNum;
		public int heartNum { get { return _HeartNum; } }

		[SerializeField, HideInInspector]
		private int _LimitButTimes;
		public int limitButTimes { get { return _LimitButTimes; } }

		public override string ToString() {
			return string.Format("[CampShopItem]{{id:{0}, propId:{1}, propName:{2}, propNum:{3}, heartNum:{4}, limitButTimes:{5}}}",
				id, propId, propName, propNum, heartNum, limitButTimes);
		}

	}

}
