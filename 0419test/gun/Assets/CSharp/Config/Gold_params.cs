//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Gold_params : ScriptableObject {

		[SerializeField, HideInInspector]
		private Gold_paramsItem[] _Items;
		private Gold_paramsItem[] items { get { return _Items; } }

		public Gold_paramsItem Get(int level) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				Gold_paramsItem item = _Items[index];
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
	public class Gold_paramsItem {

		[SerializeField, HideInInspector]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField, HideInInspector]
		private double _CoinParams;
		public double coinParams { get { return _CoinParams; } }

		public override string ToString() {
			return string.Format("[Gold_paramsItem]{{level:{0}, coinParams:{1}}}",
				level, coinParams);
		}

	}

}