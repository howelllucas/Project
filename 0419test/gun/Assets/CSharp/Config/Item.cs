//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Item : ScriptableObject {

		[SerializeField]
		private ItemItem[] _Items;
		public ItemItem[] items { get { return _Items; } }

		public ItemItem Get(int id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				ItemItem item = _Items[index];
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
	public class ItemItem {

		[SerializeField]
		private int _Id;
		public int id { get { return _Id; } }

		[SerializeField]
		private string _Name;
		public string name { get { return _Name; } }

		[SerializeField]
		private int _Showtype;
		public int showtype { get { return _Showtype; } }

		[SerializeField]
		private float _Dtime;
		public float dtime { get { return _Dtime; } }

		[SerializeField]
		private float _ShowParam;
		public float showParam { get { return _ShowParam; } }

		[SerializeField]
		private int _Showorder;
		public int showorder { get { return _Showorder; } }

		[SerializeField]
		private string _Gamename;
		public string gamename { get { return _Gamename; } }

		[SerializeField]
		private string _Image_grow;
		public string image_grow { get { return _Image_grow; } }

		[SerializeField]
		private string _Image_time;
		public string image_time { get { return _Image_time; } }

		[SerializeField]
		private float[] _Opencondition;
		public float[] opencondition { get { return _Opencondition; } }

		[SerializeField]
		private float[] _Supercondition;
		public float[] supercondition { get { return _Supercondition; } }

		[SerializeField]
		private int _Qualevel;
		public int qualevel { get { return _Qualevel; } }

		[SerializeField]
		private int _Morelv;
		public int morelv { get { return _Morelv; } }

		[SerializeField]
		private int _Maxlv;
		public int maxlv { get { return _Maxlv; } }

		[SerializeField]
		private int _LvParam;
		public int lvParam { get { return _LvParam; } }

		[SerializeField]
		private string _PropEffect;
		public string propEffect { get { return _PropEffect; } }

		[SerializeField]
		private string _OpenBoxImg;
		public string openBoxImg { get { return _OpenBoxImg; } }

		[SerializeField]
		private string _CloseBoxImg;
		public string closeBoxImg { get { return _CloseBoxImg; } }

		[SerializeField]
		private int _SourceLanguage;
		public int sourceLanguage { get { return _SourceLanguage; } }

		public override string ToString() {
			return string.Format("[ItemItem]{{id:{0}, name:{1}, showtype:{2}, dtime:{3}, showParam:{4}, showorder:{5}, gamename:{6}, image_grow:{7}, image_time:{8}, opencondition:{9}, supercondition:{10}, qualevel:{11}, morelv:{12}, maxlv:{13}, lvParam:{14}, propEffect:{15}, openBoxImg:{16}, closeBoxImg:{17}, sourceLanguage:{18}}}",
				id, name, showtype, dtime, showParam, showorder, gamename, image_grow, image_time, array2string(opencondition), array2string(supercondition), qualevel, morelv, maxlv, lvParam, propEffect, openBoxImg, closeBoxImg, sourceLanguage);
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
