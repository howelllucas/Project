//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class CampTasks : ScriptableObject {

		[SerializeField, HideInInspector]
		private CampTasksItem[] _Items;
		public CampTasksItem[] items { get { return _Items; } }

		public CampTasksItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				CampTasksItem item = _Items[index];
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
	public class CampTasksItem {

		[SerializeField, HideInInspector]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private int _Kind;
		public int kind { get { return _Kind; } }

		[SerializeField, HideInInspector]
		private string _Type;
		public string type { get { return _Type; } }

		[SerializeField, HideInInspector]
		private int _TaskWeight;
		public int taskWeight { get { return _TaskWeight; } }

		[SerializeField, HideInInspector]
		private int _Title;
		public int title { get { return _Title; } }

		[SerializeField, HideInInspector]
		private int _Describe;
		public int describe { get { return _Describe; } }

		[SerializeField, HideInInspector]
		private float[] _TaskCondition;
		public float[] taskCondition { get { return _TaskCondition; } }

		[SerializeField, HideInInspector]
		private string[] _Reward;
		public string[] reward { get { return _Reward; } }

		[SerializeField, HideInInspector]
		private int _DetailWeight;
		public int detailWeight { get { return _DetailWeight; } }

		[SerializeField, HideInInspector]
		private string[] _FallHeart;
		public string[] fallHeart { get { return _FallHeart; } }

		public override string ToString() {
			return string.Format("[CampTasksItem]{{id:{0}, kind:{1}, type:{2}, taskWeight:{3}, title:{4}, describe:{5}, taskCondition:{6}, reward:{7}, detailWeight:{8}, fallHeart:{9}}}",
				id, kind, type, taskWeight, title, describe, array2string(taskCondition), array2string(reward), detailWeight, array2string(fallHeart));
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
