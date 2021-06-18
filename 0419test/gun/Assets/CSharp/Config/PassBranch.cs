//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class PassBranch : ScriptableObject {

		[SerializeField, HideInInspector]
		private PassBranchItem[] _Items;
		public PassBranchItem[] items { get { return _Items; } }

		public PassBranchItem Get(int num) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				PassBranchItem item = _Items[index];
				if (item.num == num) { return item; }
				if (num < item.num) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class PassBranchItem {

		[SerializeField, HideInInspector]
		private int _Num;
		public int num { get { return _Num; } }

		[SerializeField, HideInInspector]
		private int _MainPassId;
		public int mainPassId { get { return _MainPassId; } }

		[SerializeField, HideInInspector]
		private int _PassId;
		public int passId { get { return _PassId; } }

		[SerializeField, HideInInspector]
		private int _TipsId;
		public int tipsId { get { return _TipsId; } }

		[SerializeField, HideInInspector]
		private int _IsMust;
		public int isMust { get { return _IsMust; } }

		[SerializeField, HideInInspector]
		private int _IsRank;
		public int isRank { get { return _IsRank; } }

		public override string ToString() {
			return string.Format("[PassBranchItem]{{num:{0}, mainPassId:{1}, passId:{2}, tipsId:{3}, isMust:{4}, isRank:{5}}}",
				num, mainPassId, passId, tipsId, isMust, isRank);
		}

	}

}
