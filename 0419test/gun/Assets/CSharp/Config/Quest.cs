//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Quest : ScriptableObject {

		[SerializeField, HideInInspector]
		private QuestItem[] _Items;
		public QuestItem[] items { get { return _Items; } }

		public QuestItem Get(int quest_id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				QuestItem item = _Items[index];
				if (item.quest_id == quest_id) { return item; }
				if (quest_id < item.quest_id) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class QuestItem {

		[SerializeField, HideInInspector]
		private int _Quest_id;
		public int quest_id { get { return _Quest_id; } }

		[SerializeField, HideInInspector]
		private int _Type;
		public int type { get { return _Type; } }

		[SerializeField, HideInInspector]
		private int _Title;
		public int title { get { return _Title; } }

		[SerializeField, HideInInspector]
		private int _Desc;
		public int desc { get { return _Desc; } }

		[SerializeField, HideInInspector]
		private float[] _Condition;
		public float[] condition { get { return _Condition; } }

		[SerializeField, HideInInspector]
		private float[] _Award;
		public float[] award { get { return _Award; } }

		[SerializeField, HideInInspector]
		private int _Pre_id;
		public int pre_id { get { return _Pre_id; } }

		[SerializeField, HideInInspector]
		private int _Post_id;
		public int post_id { get { return _Post_id; } }

		[SerializeField, HideInInspector]
		private int _Root_id;
		public int root_id { get { return _Root_id; } }

		[SerializeField, HideInInspector]
		private int _Open;
		public int open { get { return _Open; } }

		[SerializeField, HideInInspector]
		private string _AwardIcon;
		public string awardIcon { get { return _AwardIcon; } }

		[SerializeField, HideInInspector]
		private string _AwardOpen;
		public string awardOpen { get { return _AwardOpen; } }

		public override string ToString() {
			return string.Format("[QuestItem]{{quest_id:{0}, type:{1}, title:{2}, desc:{3}, condition:{4}, award:{5}, pre_id:{6}, post_id:{7}, root_id:{8}, open:{9}, awardIcon:{10}, awardOpen:{11}}}",
				quest_id, type, title, desc, array2string(condition), array2string(award), pre_id, post_id, root_id, open, awardIcon, awardOpen);
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
