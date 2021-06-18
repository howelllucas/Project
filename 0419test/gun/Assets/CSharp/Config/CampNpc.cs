//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class CampNpc : ScriptableObject {

		[SerializeField, HideInInspector]
		private CampNpcItem[] _Items;
		public CampNpcItem[] items { get { return _Items; } }

		public CampNpcItem Get(string id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				CampNpcItem item = _Items[index];
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
	public class CampNpcItem {

		[SerializeField, HideInInspector]
		private string _Id;
		public string id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private int _NotFresh;
		public int notFresh { get { return _NotFresh; } }

		[SerializeField, HideInInspector]
		private int[] _DispatchTasks;
		public int[] dispatchTasks { get { return _DispatchTasks; } }

		[SerializeField, HideInInspector]
		private string _NpcPath;
		public string NpcPath { get { return _NpcPath; } }

		[SerializeField, HideInInspector]
		private int[] _FallDurTime;
		public int[] FallDurTime { get { return _FallDurTime; } }

		[SerializeField, HideInInspector]
		private int[] _NoTask;
		public int[] NoTask { get { return _NoTask; } }

		[SerializeField, HideInInspector]
		private int[] _TaskFinished;
		public int[] TaskFinished { get { return _TaskFinished; } }

		public override string ToString() {
			return string.Format("[CampNpcItem]{{id:{0}, notFresh:{1}, dispatchTasks:{2}, NpcPath:{3}, FallDurTime:{4}, NoTask:{5}, TaskFinished:{6}}}",
				id, notFresh, array2string(dispatchTasks), NpcPath, array2string(FallDurTime), array2string(NoTask), array2string(TaskFinished));
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
