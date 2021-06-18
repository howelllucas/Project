//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Pass : ScriptableObject {

		[SerializeField, HideInInspector]
		private PassItem[] _Items;
		public PassItem[] items { get { return _Items; } }

		public PassItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				PassItem item = _Items[index];
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
	public class PassItem {

		[SerializeField, HideInInspector]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private int _SceneID;
		public int sceneID { get { return _SceneID; } }

		[SerializeField, HideInInspector]
		private string _Scene;
		public string scene { get { return _Scene; } }

		[SerializeField, HideInInspector]
		private string _Collider;
		public string collider { get { return _Collider; } }

		[SerializeField, HideInInspector]
		private int[] _WaveID;
		public int[] waveID { get { return _WaveID; } }

		[SerializeField, HideInInspector]
		private int _SceneType;
		public int sceneType { get { return _SceneType; } }

		[SerializeField, HideInInspector]
		private int _TimeLmt;
		public int timeLmt { get { return _TimeLmt; } }

		[SerializeField, HideInInspector]
		private double _HpParam;
		public double hpParam { get { return _HpParam; } }

		[SerializeField, HideInInspector]
		private float _GoldParam;
		public float goldParam { get { return _GoldParam; } }

		[SerializeField, HideInInspector]
		private int _NextID;
		public int nextID { get { return _NextID; } }

		[SerializeField, HideInInspector]
		private int[] _DropID;
		public int[] dropID { get { return _DropID; } }

		[SerializeField, HideInInspector]
		private float[] _DropRate;
		public float[] dropRate { get { return _DropRate; } }

		[SerializeField, HideInInspector]
		private int _LimitView;
		public int limitView { get { return _LimitView; } }

		[SerializeField, HideInInspector]
		private int _EnableVIT;
		public int enableVIT { get { return _EnableVIT; } }

		[SerializeField, HideInInspector]
		private string _MainUIbg;
		public string mainUIbg { get { return _MainUIbg; } }

		[SerializeField, HideInInspector]
		private string _MainUIeffect;
		public string mainUIeffect { get { return _MainUIeffect; } }

		[SerializeField, HideInInspector]
		private float _LevelEXP;
		public float levelEXP { get { return _LevelEXP; } }

		[SerializeField, HideInInspector]
		private int _BossLevel;
		public int bossLevel { get { return _BossLevel; } }

		[SerializeField, HideInInspector]
		private int _PassType;
		public int passType { get { return _PassType; } }

		[SerializeField, HideInInspector]
		private int _RewardType;
		public int rewardType { get { return _RewardType; } }

		[SerializeField, HideInInspector]
		private int _RewardCount;
		public int rewardCount { get { return _RewardCount; } }

		[SerializeField, HideInInspector]
		private int _MissionName;
		public int missionName { get { return _MissionName; } }

		[SerializeField, HideInInspector]
		private int _CoinNum;
		public int coinNum { get { return _CoinNum; } }

		[SerializeField, HideInInspector]
		private int _DtType;
		public int dtType { get { return _DtType; } }

		[SerializeField, HideInInspector]
		private int[] _ChipParam;
		public int[] chipParam { get { return _ChipParam; } }

		[SerializeField, HideInInspector]
		private int[] _SourceParam;
		public int[] sourceParam { get { return _SourceParam; } }

		[SerializeField, HideInInspector]
		private int[] _SourceParam1;
		public int[] sourceParam1 { get { return _SourceParam1; } }

		public override string ToString() {
			return string.Format("[PassItem]{{id:{0}, sceneID:{1}, scene:{2}, collider:{3}, waveID:{4}, sceneType:{5}, timeLmt:{6}, hpParam:{7}, goldParam:{8}, nextID:{9}, dropID:{10}, dropRate:{11}, limitView:{12}, enableVIT:{13}, mainUIbg:{14}, mainUIeffect:{15}, levelEXP:{16}, bossLevel:{17}, passType:{18}, rewardType:{19}, rewardCount:{20}, missionName:{21}, coinNum:{22}, dtType:{23}, chipParam:{24}, sourceParam:{25}, sourceParam1:{26}}}",
				id, sceneID, scene, collider, array2string(waveID), sceneType, timeLmt, hpParam, goldParam, nextID, array2string(dropID), array2string(dropRate), limitView, enableVIT, mainUIbg, mainUIeffect, levelEXP, bossLevel, passType, rewardType, rewardCount, missionName, coinNum, dtType, array2string(chipParam), array2string(sourceParam), array2string(sourceParam1));
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
