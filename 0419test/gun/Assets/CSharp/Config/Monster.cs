//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Monster : ScriptableObject {

		[SerializeField, HideInInspector]
		private MonsterItem[] _Items;
		public MonsterItem[] items { get { return _Items; } }

		public MonsterItem Get(int tag) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				MonsterItem item = _Items[index];
				if (item.tag == tag) { return item; }
				if (tag < item.tag) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class MonsterItem {

		[SerializeField, HideInInspector]
		private int _Tag;
		public int tag { get { return _Tag; } }

		[SerializeField, HideInInspector]
		private string _Name;
		public string name { get { return _Name; } }

		[SerializeField, HideInInspector]
		private string _Path;
		public string path { get { return _Path; } }

		[SerializeField, HideInInspector]
		private int _Type;
		public int type { get { return _Type; } }

		[SerializeField, HideInInspector]
		private double _BaseHp;
		public double baseHp { get { return _BaseHp; } }

		[SerializeField, HideInInspector]
		private float _BaseSpeed;
		public float baseSpeed { get { return _BaseSpeed; } }

		[SerializeField, HideInInspector]
		private int _HasHpSlider;
		public int hasHpSlider { get { return _HasHpSlider; } }

		[SerializeField, HideInInspector]
		private int[] _DropId;
		public int[] dropId { get { return _DropId; } }

		[SerializeField, HideInInspector]
		private int _DropCoinCount;
		public int dropCoinCount { get { return _DropCoinCount; } }

		[SerializeField, HideInInspector]
		private int _HasShowAct;
		public int hasShowAct { get { return _HasShowAct; } }

		[SerializeField, HideInInspector]
		private float _AniSpeed;
		public float aniSpeed { get { return _AniSpeed; } }

		public override string ToString() {
			return string.Format("[MonsterItem]{{tag:{0}, name:{1}, path:{2}, type:{3}, baseHp:{4}, baseSpeed:{5}, hasHpSlider:{6}, dropId:{7}, dropCoinCount:{8}, hasShowAct:{9}, aniSpeed:{10}}}",
				tag, name, path, type, baseHp, baseSpeed, hasHpSlider, array2string(dropId), dropCoinCount, hasShowAct, aniSpeed);
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
