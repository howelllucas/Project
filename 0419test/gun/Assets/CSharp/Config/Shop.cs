//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Shop : ScriptableObject {

		[SerializeField, HideInInspector]
		private ShopItem[] _Items;
		public ShopItem[] items { get { return _Items; } }

		public ShopItem Get(string id) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				ShopItem item = _Items[index];
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
	public class ShopItem {

		[SerializeField, HideInInspector]
		private string _Id;
		public string id { get { return _Id; } }

		[SerializeField, HideInInspector]
		private int _GoodsType;
		public int goodsType { get { return _GoodsType; } }

		[SerializeField, HideInInspector]
		private float _GoodsNum;
		public float goodsNum { get { return _GoodsNum; } }

		[SerializeField, HideInInspector]
		private int _Desc;
		public int desc { get { return _Desc; } }

		[SerializeField, HideInInspector]
		private string _ItemIcon;
		public string itemIcon { get { return _ItemIcon; } }

		[SerializeField, HideInInspector]
		private int _ConsumeType;
		public int consumeType { get { return _ConsumeType; } }

		[SerializeField, HideInInspector]
		private float _Price;
		public float price { get { return _Price; } }

		[SerializeField, HideInInspector]
		private int _Type;
		public int type { get { return _Type; } }

		[SerializeField, HideInInspector]
		private int _Platform;
		public int platform { get { return _Platform; } }

		[SerializeField, HideInInspector]
		private int _EffectNum;
		public int effectNum { get { return _EffectNum; } }

		[SerializeField, HideInInspector]
		private string _BgLight;
		public string bgLight { get { return _BgLight; } }

		public override string ToString() {
			return string.Format("[ShopItem]{{id:{0}, goodsType:{1}, goodsNum:{2}, desc:{3}, itemIcon:{4}, consumeType:{5}, price:{6}, type:{7}, platform:{8}, effectNum:{9}, bgLight:{10}}}",
				id, goodsType, goodsNum, desc, itemIcon, consumeType, price, type, platform, effectNum, bgLight);
		}

	}

}
