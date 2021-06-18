//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class CampStep : ScriptableObject {

		[SerializeField, HideInInspector]
		private CampStepItem[] _Items;
		public CampStepItem[] items { get { return _Items; } }

		public CampStepItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				CampStepItem item = _Items[index];
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
	public class CampStepItem {

		[SerializeField, HideInInspector]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private float[] _Condition;
		public float[] condition { get { return _Condition; } }

		[SerializeField, HideInInspector]
		private int[] _Dialogue;
		public int[] dialogue { get { return _Dialogue; } }

		[SerializeField, HideInInspector]
		private int _NextTaskId;
		public int nextTaskId { get { return _NextTaskId; } }

		[SerializeField, HideInInspector]
		private int _BrageId;
		public int brageId { get { return _BrageId; } }

		public override string ToString() {
			return string.Format("[CampStepItem]{{id:{0}, condition:{1}, dialogue:{2}, nextTaskId:{3}, brageId:{4}}}",
				id, array2string(condition), array2string(dialogue), nextTaskId, brageId);
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
