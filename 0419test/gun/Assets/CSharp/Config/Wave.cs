//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Wave : ScriptableObject {

		[SerializeField]
		private WaveItem[] _Items;
		private WaveItem[] items { get { return _Items; } }

		public WaveItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				WaveItem item = _Items[index];
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
	public class WaveItem {

		[SerializeField]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField]
		private int[] _TrigMode;
		public int[] trigMode { get { return _TrigMode; } }

		[SerializeField]
		private string[] _NodeID;
		public string[] nodeID { get { return _NodeID; } }

		[SerializeField]
		private int[] _EnemyID;
		public int[] enemyID { get { return _EnemyID; } }

		[SerializeField]
		private int[] _EnemyNum;
		public int[] enemyNum { get { return _EnemyNum; } }

		[SerializeField]
		private float _DtTime;
		public float dtTime { get { return _DtTime; } }

		[SerializeField]
		private int _NumTime;
		public int NumTime { get { return _NumTime; } }

		[SerializeField]
		private float _DelayTime;
		public float delayTime { get { return _DelayTime; } }

		[SerializeField]
		private int _Warning;
		public int warning { get { return _Warning; } }

		[SerializeField]
		private float _HpParam;
		public float HpParam { get { return _HpParam; } }

		public override string ToString() {
			return string.Format("[WaveItem]{{id:{0}, trigMode:{1}, nodeID:{2}, enemyID:{3}, enemyNum:{4}, dtTime:{5}, NumTime:{6}, delayTime:{7}, warning:{8}, HpParam:{9}}}",
				id, array2string(trigMode), array2string(nodeID), array2string(enemyID), array2string(enemyNum), dtTime, NumTime, delayTime, warning, HpParam);
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
