//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Scene : ScriptableObject {

		[SerializeField, HideInInspector]
		private SceneItem[] _Items;
		private SceneItem[] items { get { return _Items; } }

		public SceneItem Get(int iD) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				SceneItem item = _Items[index];
				if (item.ID == iD) { return item; }
				if (iD < item.ID) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class SceneItem {

		[SerializeField, HideInInspector]
		private int _ID;
		public int ID { get { return _ID; } }

		[SerializeField, HideInInspector]
		private int _SceneNum;
		public int sceneNum { get { return _SceneNum; } }

		[SerializeField, HideInInspector]
		private int _SceneName;
		public int sceneName { get { return _SceneName; } }

		[SerializeField, HideInInspector]
		private int _PassCount;
		public int passCount { get { return _PassCount; } }

		[SerializeField, HideInInspector]
		private int[] _MissionWaveID;
		public int[] missionWaveID { get { return _MissionWaveID; } }

		[SerializeField, HideInInspector]
		private int[] _MissionWaveRate;
		public int[] missionWaveRate { get { return _MissionWaveRate; } }

		[SerializeField, HideInInspector]
		private int _MissionInterval;
		public int missionInterval { get { return _MissionInterval; } }

		[SerializeField, HideInInspector]
		private int _MissionLimit;
		public int missionLimit { get { return _MissionLimit; } }

		public override string ToString() {
			return string.Format("[SceneItem]{{ID:{0}, sceneNum:{1}, sceneName:{2}, passCount:{3}, missionWaveID:{4}, missionWaveRate:{5}, missionInterval:{6}, missionLimit:{7}}}",
				ID, sceneNum, sceneName, passCount, array2string(missionWaveID), array2string(missionWaveRate), missionInterval, missionLimit);
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
