//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class GunsSub_data : ScriptableObject {

		[SerializeField, HideInInspector]
		private GunsSub_dataItem[] _Items;
		private GunsSub_dataItem[] items { get { return _Items; } }

		public GunsSub_dataItem Get(int level) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				GunsSub_dataItem item = _Items[index];
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
	public class GunsSub_dataItem {

		[SerializeField, HideInInspector]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField, HideInInspector]
		private double[] _Base_cost_1;
		public double[] base_cost_1 { get { return _Base_cost_1; } }

		[SerializeField, HideInInspector]
		private double[] _Base_params_1;
		public double[] base_params_1 { get { return _Base_params_1; } }

		[SerializeField, HideInInspector]
		private double[] _Base_cost_2;
		public double[] base_cost_2 { get { return _Base_cost_2; } }

		[SerializeField, HideInInspector]
		private double[] _Base_params_2;
		public double[] base_params_2 { get { return _Base_params_2; } }

		public override string ToString() {
			return string.Format("[GunsSub_dataItem]{{level:{0}, base_cost_1:{1}, base_params_1:{2}, base_cost_2:{3}, base_params_2:{4}}}",
				level, base_cost_1, base_params_1, base_cost_2, base_params_2);
		}

	}

}
