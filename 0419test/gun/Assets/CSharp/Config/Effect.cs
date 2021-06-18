//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Effect : ScriptableObject {

		[SerializeField, HideInInspector]
		private EffectItem[] _Items;
		private EffectItem[] items { get { return _Items; } }

		public EffectItem Get(string id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				EffectItem item = _Items[index];
				if (item.id == id) { return item; }
				if (string.Compare(id, item.id) < 0) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class EffectItem {

		[SerializeField, HideInInspector]
		private string _Id;
		public string id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private string _Path;
		public string path { get { return _Path; } }

		public override string ToString() {
			return string.Format("[EffectItem]{{id:{0}, path:{1}}}",
				id, path);
		}

	}

}
