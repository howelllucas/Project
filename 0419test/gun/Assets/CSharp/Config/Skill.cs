//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Skill : ScriptableObject {

		[SerializeField, HideInInspector]
		private SkillItem[] _Items;
		public SkillItem[] items { get { return _Items; } }

		public SkillItem Get(string id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				SkillItem item = _Items[index];
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
	public class SkillItem {

		[SerializeField, HideInInspector]
		private string _Id;
		public string id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private int _Name;
		public int name { get { return _Name; } }

		[SerializeField, HideInInspector]
		private int _Desc;
		public int desc { get { return _Desc; } }

		[SerializeField, HideInInspector]
		private int _Desc_nolearn;
		public int desc_nolearn { get { return _Desc_nolearn; } }

		[SerializeField, HideInInspector]
		private string _Icon;
		public string icon { get { return _Icon; } }

		[SerializeField, HideInInspector]
		private int _Location;
		public int location { get { return _Location; } }

		[SerializeField, HideInInspector]
		private string _Bg;
		public string bg { get { return _Bg; } }

		[SerializeField, HideInInspector]
		private string _Bgs;
		public string bgs { get { return _Bgs; } }

		[SerializeField, HideInInspector]
		private string _Bgn;
		public string bgn { get { return _Bgn; } }

		[SerializeField, HideInInspector]
		private int _Weight;
		public int weight { get { return _Weight; } }

		[SerializeField, HideInInspector]
		private int _Init;
		public int init { get { return _Init; } }

		[SerializeField, HideInInspector]
		private int _Percentage;
		public int percentage { get { return _Percentage; } }

		[SerializeField, HideInInspector]
		private int _Max_desc;
		public int max_desc { get { return _Max_desc; } }

		public override string ToString() {
			return string.Format("[SkillItem]{{id:{0}, name:{1}, desc:{2}, desc_nolearn:{3}, icon:{4}, location:{5}, bg:{6}, bgs:{7}, bgn:{8}, weight:{9}, init:{10}, percentage:{11}, max_desc:{12}}}",
				id, name, desc, desc_nolearn, icon, location, bg, bgs, bgn, weight, init, percentage, max_desc);
		}

	}

}
