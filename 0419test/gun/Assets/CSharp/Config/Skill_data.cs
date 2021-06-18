//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Skill_data : ScriptableObject {

		[SerializeField, HideInInspector]
		private Skill_dataItem[] _Items;
		public Skill_dataItem[] items { get { return _Items; } }

		public Skill_dataItem Get(int mainLevel) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				Skill_dataItem item = _Items[index];
				if (item.mainLevel == mainLevel) { return item; }
				if (mainLevel < item.mainLevel) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class Skill_dataItem {

		[SerializeField, HideInInspector]
		private int _MainLevel;
		public int mainLevel { get { return _MainLevel; } }

		[SerializeField, HideInInspector]
		private float _CoinCost;
		public float coinCost { get { return _CoinCost; } }

		[SerializeField, HideInInspector]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField, HideInInspector]
		private float[] _Skill_exatk;
		public float[] skill_exatk { get { return _Skill_exatk; } }

		[SerializeField, HideInInspector]
		private float[] _Skill_exgold;
		public float[] skill_exgold { get { return _Skill_exgold; } }

		[SerializeField, HideInInspector]
		private float[] _Skill_exhp;
		public float[] skill_exhp { get { return _Skill_exhp; } }

		[SerializeField, HideInInspector]
		private float[] _Skill_exspeed;
		public float[] skill_exspeed { get { return _Skill_exspeed; } }

		[SerializeField, HideInInspector]
		private float[] _Skill_excrit;
		public float[] skill_excrit { get { return _Skill_excrit; } }

		[SerializeField, HideInInspector]
		private float[] _Skill_exbufftime;
		public float[] skill_exbufftime { get { return _Skill_exbufftime; } }

		[SerializeField, HideInInspector]
		private float[] _Skill_exdodge;
		public float[] skill_exdodge { get { return _Skill_exdodge; } }

		[SerializeField, HideInInspector]
		private float[] _Skill_exbossharm;
		public float[] skill_exbossharm { get { return _Skill_exbossharm; } }

		[SerializeField, HideInInspector]
		private float[] _Skill_exhittime;
		public float[] skill_exhittime { get { return _Skill_exhittime; } }

		public override string ToString() {
			return string.Format("[Skill_dataItem]{{mainLevel:{0}, coinCost:{1}, level:{2}, skill_exatk:{3}, skill_exgold:{4}, skill_exhp:{5}, skill_exspeed:{6}, skill_excrit:{7}, skill_exbufftime:{8}, skill_exdodge:{9}, skill_exbossharm:{10}, skill_exhittime:{11}}}",
				mainLevel, coinCost, level, array2string(skill_exatk), array2string(skill_exgold), array2string(skill_exhp), array2string(skill_exspeed), array2string(skill_excrit), array2string(skill_exbufftime), array2string(skill_exdodge), array2string(skill_exbossharm), array2string(skill_exhittime));
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
