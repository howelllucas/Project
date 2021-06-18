//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class CampBuff_data : ScriptableObject {

		[SerializeField, HideInInspector]
		private CampBuff_dataItem[] _Items;
		public CampBuff_dataItem[] items { get { return _Items; } }

		public CampBuff_dataItem Get(int level) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				CampBuff_dataItem item = _Items[index];
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
	public class CampBuff_dataItem {

		[SerializeField, HideInInspector]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_atkNpc;
		public float[] buff_atkNpc { get { return _Buff_atkNpc; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_atkNpc_cost;
		public float[] buff_atkNpc_cost { get { return _Buff_atkNpc_cost; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_atk;
		public float[] buff_atk { get { return _Buff_atk; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_atk_cost;
		public float[] buff_atk_cost { get { return _Buff_atk_cost; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_fireSpeed;
		public float[] buff_fireSpeed { get { return _Buff_fireSpeed; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_fireSpeed_cost;
		public float[] buff_fireSpeed_cost { get { return _Buff_fireSpeed_cost; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_Beheading;
		public float[] buff_Beheading { get { return _Buff_Beheading; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_Beheading_cost;
		public float[] buff_Beheading_cost { get { return _Buff_Beheading_cost; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_second;
		public float[] buff_second { get { return _Buff_second; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_second_cost;
		public float[] buff_second_cost { get { return _Buff_second_cost; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_robot;
		public float[] buff_robot { get { return _Buff_robot; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_robot_cost;
		public float[] buff_robot_cost { get { return _Buff_robot_cost; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_seckill;
		public float[] buff_seckill { get { return _Buff_seckill; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_seckill_cost;
		public float[] buff_seckill_cost { get { return _Buff_seckill_cost; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_critBoom;
		public float[] buff_critBoom { get { return _Buff_critBoom; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_critBoom_cost;
		public float[] buff_critBoom_cost { get { return _Buff_critBoom_cost; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_critIgnition;
		public float[] buff_critIgnition { get { return _Buff_critIgnition; } }

		[SerializeField, HideInInspector]
		private float[] _Buff_critIgnition_cost;
		public float[] buff_critIgnition_cost { get { return _Buff_critIgnition_cost; } }

		public override string ToString() {
			return string.Format("[CampBuff_dataItem]{{level:{0}, buff_atkNpc:{1}, buff_atkNpc_cost:{2}, buff_atk:{3}, buff_atk_cost:{4}, buff_fireSpeed:{5}, buff_fireSpeed_cost:{6}, buff_Beheading:{7}, buff_Beheading_cost:{8}, buff_second:{9}, buff_second_cost:{10}, buff_robot:{11}, buff_robot_cost:{12}, buff_seckill:{13}, buff_seckill_cost:{14}, buff_critBoom:{15}, buff_critBoom_cost:{16}, buff_critIgnition:{17}, buff_critIgnition_cost:{18}}}",
				level, array2string(buff_atkNpc), array2string(buff_atkNpc_cost), array2string(buff_atk), array2string(buff_atk_cost), array2string(buff_fireSpeed), array2string(buff_fireSpeed_cost), array2string(buff_Beheading), array2string(buff_Beheading_cost), array2string(buff_second), array2string(buff_second_cost), array2string(buff_robot), array2string(buff_robot_cost), array2string(buff_seckill), array2string(buff_seckill_cost), array2string(buff_critBoom), array2string(buff_critBoom_cost), array2string(buff_critIgnition), array2string(buff_critIgnition_cost));
		}

		private string array2string(System.Array array) {
			int len = array.Length;
			string[] strs = new string[len];
			for (int i = 0; i < len; i++) {
				strs[i] = string.Format("{0}", array.GetValue(i));
			}
			return string.Concat("[", string.Join(", ", strs), "]");
		}

	}

}
