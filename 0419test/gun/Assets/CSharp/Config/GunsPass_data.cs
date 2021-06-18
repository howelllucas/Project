//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class GunsPass_data : ScriptableObject {

		[SerializeField, HideInInspector]
		private GunsPass_dataItem[] _Items;
		public GunsPass_dataItem[] items { get { return _Items; } }

		public GunsPass_dataItem Get(int pass) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				GunsPass_dataItem item = _Items[index];
				if (item.pass == pass) { return item; }
				if (pass < item.pass) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class GunsPass_dataItem {

		[SerializeField, HideInInspector]
		private int _Pass;
		public int pass { get { return _Pass; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_howitzer;
		public double[] weapon_howitzer { get { return _Weapon_howitzer; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_mine;
		public double[] weapon_mine { get { return _Weapon_mine; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_boomerang;
		public double[] weapon_boomerang { get { return _Weapon_boomerang; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_hgrenade;
		public double[] weapon_hgrenade { get { return _Weapon_hgrenade; } }

		[SerializeField, HideInInspector]
		private double[] _S_FireTurret;
		public double[] S_FireTurret { get { return _S_FireTurret; } }

		[SerializeField, HideInInspector]
		private double[] _S_Turret002;
		public double[] S_Turret002 { get { return _S_Turret002; } }

		[SerializeField, HideInInspector]
		private double[] _S_BoomTurret;
		public double[] S_BoomTurret { get { return _S_BoomTurret; } }

		[SerializeField, HideInInspector]
		private double[] _S_Turret003;
		public double[] S_Turret003 { get { return _S_Turret003; } }

		[SerializeField, HideInInspector]
		private double[] _Carrier;
		public double[] Carrier { get { return _Carrier; } }

		[SerializeField, HideInInspector]
		private double[] _Tank_carrier;
		public double[] Tank_carrier { get { return _Tank_carrier; } }

		[SerializeField, HideInInspector]
		private double[] _Tank_turret;
		public double[] Tank_turret { get { return _Tank_turret; } }

		[SerializeField, HideInInspector]
		private double[] _Office_saw;
		public double[] Office_saw { get { return _Office_saw; } }

		[SerializeField, HideInInspector]
		private double[] _Office_prick;
		public double[] Office_prick { get { return _Office_prick; } }

		[SerializeField, HideInInspector]
		private double[] _Office_bonfire;
		public double[] Office_bonfire { get { return _Office_bonfire; } }

		[SerializeField, HideInInspector]
		private double[] _UAV;
		public double[] UAV { get { return _UAV; } }

		[SerializeField, HideInInspector]
		private double[] _UAV002;
		public double[] UAV002 { get { return _UAV002; } }

		[SerializeField, HideInInspector]
		private double[] _Dog;
		public double[] Dog { get { return _Dog; } }

		[SerializeField, HideInInspector]
		private double[] _Beetle;
		public double[] Beetle { get { return _Beetle; } }

		[SerializeField, HideInInspector]
		private double[] _Maniac;
		public double[] Maniac { get { return _Maniac; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_traps;
		public double[] weapon_traps { get { return _Weapon_traps; } }

		[SerializeField, HideInInspector]
		private double[] _Robot;
		public double[] Robot { get { return _Robot; } }

		[SerializeField, HideInInspector]
		private double[] _S_Turret005;
		public double[] S_Turret005 { get { return _S_Turret005; } }

		[SerializeField, HideInInspector]
		private double[] _S_Turret006;
		public double[] S_Turret006 { get { return _S_Turret006; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_icegrenade;
		public double[] weapon_icegrenade { get { return _Weapon_icegrenade; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_elecmine;
		public double[] weapon_elecmine { get { return _Weapon_elecmine; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_mushroom;
		public double[] weapon_mushroom { get { return _Weapon_mushroom; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_destroyer;
		public double[] weapon_destroyer { get { return _Weapon_destroyer; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_charmBomb;
		public double[] weapon_charmBomb { get { return _Weapon_charmBomb; } }

		[SerializeField, HideInInspector]
		private double[] _DeathWalker;
		public double[] DeathWalker { get { return _DeathWalker; } }

		[SerializeField, HideInInspector]
		private double[] _Office__electrom;
		public double[] Office__electrom { get { return _Office__electrom; } }

		public override string ToString() {
			return string.Format("[GunsPass_dataItem]{{pass:{0}, weapon_howitzer:{1}, weapon_mine:{2}, weapon_boomerang:{3}, weapon_hgrenade:{4}, S_FireTurret:{5}, S_Turret002:{6}, S_BoomTurret:{7}, S_Turret003:{8}, Carrier:{9}, Tank_carrier:{10}, Tank_turret:{11}, Office_saw:{12}, Office_prick:{13}, Office_bonfire:{14}, UAV:{15}, UAV002:{16}, Dog:{17}, Beetle:{18}, Maniac:{19}, weapon_traps:{20}, Robot:{21}, S_Turret005:{22}, S_Turret006:{23}, weapon_icegrenade:{24}, weapon_elecmine:{25}, weapon_mushroom:{26}, weapon_destroyer:{27}, weapon_charmBomb:{28}, DeathWalker:{29}, Office__electrom:{30}}}",
				pass, weapon_howitzer, weapon_mine, weapon_boomerang, weapon_hgrenade, S_FireTurret, S_Turret002, S_BoomTurret, S_Turret003, Carrier, Tank_carrier, Tank_turret, Office_saw, Office_prick, Office_bonfire, UAV, UAV002, Dog, Beetle, Maniac, weapon_traps, Robot, S_Turret005, S_Turret006, weapon_icegrenade, weapon_elecmine, weapon_mushroom, weapon_destroyer, weapon_charmBomb, DeathWalker, Office__electrom);
		}

	}

}
