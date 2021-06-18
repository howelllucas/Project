//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class CampRecycle : ScriptableObject {

		[SerializeField, HideInInspector]
		private CampRecycleItem[] _Items;
		public CampRecycleItem[] items { get { return _Items; } }

		public CampRecycleItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				CampRecycleItem item = _Items[index];
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
	public class CampRecycleItem {

		[SerializeField, HideInInspector]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private string _Name;
		public string name { get { return _Name; } }

		[SerializeField, HideInInspector]
		private float _Price;
		public float price { get { return _Price; } }

		[SerializeField, HideInInspector]
		private int _Dec;
		public int dec { get { return _Dec; } }

		public override string ToString() {
			return string.Format("[CampRecycleItem]{{id:{0}, name:{1}, price:{2}, dec:{3}}}",
				id, name, price, dec);
		}

	}

}
