//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Level : ScriptableObject {

		[SerializeField]
		private LevelItem[] _Items;
		public LevelItem[] items { get { return _Items; } }

		public LevelItem Get(int level) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				LevelItem item = _Items[index];
				if (item.level == level) { return item; }
				if (level < item.level) {
					max = index;
				} else {
					min = index + 1;
				}
			}
			return null;
		}

	}

	[System.Serializable]
	public class LevelItem {

		[SerializeField]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField]
		private int _PresentExp;
		public int presentExp { get { return _PresentExp; } }

		[SerializeField]
		private float _AtkParam;
		public float atkParam { get { return _AtkParam; } }

		[SerializeField]
		private float[] _Weapon_ak47_cost;
		public float[] weapon_ak47_cost { get { return _Weapon_ak47_cost; } }

		[SerializeField]
		private float[] _Weapon_ak47;
		public float[] weapon_ak47 { get { return _Weapon_ak47; } }

		[SerializeField]
		private float[] _Weapon_laser_cost;
		public float[] weapon_laser_cost { get { return _Weapon_laser_cost; } }

		[SerializeField]
		private float[] _Weapon_laser;
		public float[] weapon_laser { get { return _Weapon_laser; } }

		[SerializeField]
		private float[] _Weapon_rpg_cost;
		public float[] weapon_rpg_cost { get { return _Weapon_rpg_cost; } }

		[SerializeField]
		private float[] _Weapon_rpg;
		public float[] weapon_rpg { get { return _Weapon_rpg; } }

		[SerializeField]
		private float[] _Weapon_mg_cost;
		public float[] weapon_mg_cost { get { return _Weapon_mg_cost; } }

		[SerializeField]
		private float[] _Weapon_mg;
		public float[] weapon_mg { get { return _Weapon_mg; } }

		[SerializeField]
		private float[] _Weapon_scatter_cost;
		public float[] weapon_scatter_cost { get { return _Weapon_scatter_cost; } }

		[SerializeField]
		private float[] _Weapon_scatter;
		public float[] weapon_scatter { get { return _Weapon_scatter; } }

		[SerializeField]
		private float[] _Weapon_firegun_cost;
		public float[] weapon_firegun_cost { get { return _Weapon_firegun_cost; } }

		[SerializeField]
		private float[] _Weapon_firegun;
		public float[] weapon_firegun { get { return _Weapon_firegun; } }

		[SerializeField]
		private float[] _S_FireTurret_cost;
		public float[] S_FireTurret_cost { get { return _S_FireTurret_cost; } }

		[SerializeField]
		private float[] _S_FireTurret;
		public float[] S_FireTurret { get { return _S_FireTurret; } }

		[SerializeField]
		private float[] _Weapon_laserX_cost;
		public float[] weapon_laserX_cost { get { return _Weapon_laserX_cost; } }

		[SerializeField]
		private float[] _Weapon_laserX;
		public float[] weapon_laserX { get { return _Weapon_laserX; } }

		[SerializeField]
		private float[] _Weapon_awp_cost;
		public float[] weapon_awp_cost { get { return _Weapon_awp_cost; } }

		[SerializeField]
		private float[] _Weapon_awp;
		public float[] weapon_awp { get { return _Weapon_awp; } }

		[SerializeField]
		private float[] _Weapon_elecgun_cost;
		public float[] weapon_elecgun_cost { get { return _Weapon_elecgun_cost; } }

		[SerializeField]
		private float[] _Weapon_elecgun;
		public float[] weapon_elecgun { get { return _Weapon_elecgun; } }

		[SerializeField]
		private float[] _Weapon_shotgun_cost;
		public float[] weapon_shotgun_cost { get { return _Weapon_shotgun_cost; } }

		[SerializeField]
		private float[] _Weapon_shotgun;
		public float[] weapon_shotgun { get { return _Weapon_shotgun; } }

		[SerializeField]
		private float[] _Weapon_bouncegun_cost;
		public float[] weapon_bouncegun_cost { get { return _Weapon_bouncegun_cost; } }

		[SerializeField]
		private float[] _Weapon_bouncegun;
		public float[] weapon_bouncegun { get { return _Weapon_bouncegun; } }

		[SerializeField]
		private float[] _Weapon_firecrossbow_cost;
		public float[] weapon_firecrossbow_cost { get { return _Weapon_firecrossbow_cost; } }

		[SerializeField]
		private float[] _Weapon_firecrossbow;
		public float[] weapon_firecrossbow { get { return _Weapon_firecrossbow; } }

		[SerializeField]
		private float[] _Weapon_icecrossbow_cost;
		public float[] weapon_icecrossbow_cost { get { return _Weapon_icecrossbow_cost; } }

		[SerializeField]
		private float[] _Weapon_icecrossbow;
		public float[] weapon_icecrossbow { get { return _Weapon_icecrossbow; } }

		[SerializeField]
		private float[] _Weapon_elecrossbow_cost;
		public float[] weapon_elecrossbow_cost { get { return _Weapon_elecrossbow_cost; } }

		[SerializeField]
		private float[] _Weapon_elecrossbow;
		public float[] weapon_elecrossbow { get { return _Weapon_elecrossbow; } }

		[SerializeField]
		private float[] _Weapon_crossbow_cost;
		public float[] weapon_crossbow_cost { get { return _Weapon_crossbow_cost; } }

		[SerializeField]
		private float[] _Weapon_crossbow;
		public float[] weapon_crossbow { get { return _Weapon_crossbow; } }

		[SerializeField]
		private float[] _Car_001_cost;
		public float[] Car_001_cost { get { return _Car_001_cost; } }

		[SerializeField]
		private float[] _Car_001;
		public float[] Car_001 { get { return _Car_001; } }

		[SerializeField]
		private float[] _Skill_exatk;
		public float[] skill_exatk { get { return _Skill_exatk; } }

		[SerializeField]
		private float[] _Skill_exatk_cost;
		public float[] skill_exatk_cost { get { return _Skill_exatk_cost; } }

		[SerializeField]
		private float[] _Skill_exgold;
		public float[] skill_exgold { get { return _Skill_exgold; } }

		[SerializeField]
		private float[] _Skill_exgold_cost;
		public float[] skill_exgold_cost { get { return _Skill_exgold_cost; } }

		[SerializeField]
		private float[] _Skill_exhp;
		public float[] skill_exhp { get { return _Skill_exhp; } }

		[SerializeField]
		private float[] _Skill_exhp_cost;
		public float[] skill_exhp_cost { get { return _Skill_exhp_cost; } }

		[SerializeField]
		private float[] _Skill_exspeed;
		public float[] skill_exspeed { get { return _Skill_exspeed; } }

		[SerializeField]
		private float[] _Skill_exspeed_cost;
		public float[] skill_exspeed_cost { get { return _Skill_exspeed_cost; } }

		[SerializeField]
		private float[] _Skill_excrit;
		public float[] skill_excrit { get { return _Skill_excrit; } }

		[SerializeField]
		private float[] _Skill_excrit_cost;
		public float[] skill_excrit_cost { get { return _Skill_excrit_cost; } }

		[SerializeField]
		private float[] _Skill_exbufftime;
		public float[] skill_exbufftime { get { return _Skill_exbufftime; } }

		[SerializeField]
		private float[] _Skill_exbufftime_cost;
		public float[] skill_exbufftime_cost { get { return _Skill_exbufftime_cost; } }

		[SerializeField]
		private float[] _Skill_exdodge;
		public float[] skill_exdodge { get { return _Skill_exdodge; } }

		[SerializeField]
		private float[] _Skill_exdodge_cost;
		public float[] skill_exdodge_cost { get { return _Skill_exdodge_cost; } }

		[SerializeField]
		private float[] _Skill_exbossharm;
		public float[] skill_exbossharm { get { return _Skill_exbossharm; } }

		[SerializeField]
		private float[] _Skill_exbossharm_cost;
		public float[] skill_exbossharm_cost { get { return _Skill_exbossharm_cost; } }

		public override string ToString() {
			return string.Format("[LevelItem]{{level:{0}, presentExp:{1}, atkParam:{2}, weapon_ak47_cost:{3}, weapon_ak47:{4}, weapon_laser_cost:{5}, weapon_laser:{6}, weapon_rpg_cost:{7}, weapon_rpg:{8}, weapon_mg_cost:{9}, weapon_mg:{10}, weapon_scatter_cost:{11}, weapon_scatter:{12}, weapon_firegun_cost:{13}, weapon_firegun:{14}, S_FireTurret_cost:{15}, S_FireTurret:{16}, weapon_laserX_cost:{17}, weapon_laserX:{18}, weapon_awp_cost:{19}, weapon_awp:{20}, weapon_elecgun_cost:{21}, weapon_elecgun:{22}, weapon_shotgun_cost:{23}, weapon_shotgun:{24}, weapon_bouncegun_cost:{25}, weapon_bouncegun:{26}, weapon_firecrossbow_cost:{27}, weapon_firecrossbow:{28}, weapon_icecrossbow_cost:{29}, weapon_icecrossbow:{30}, weapon_elecrossbow_cost:{31}, weapon_elecrossbow:{32}, weapon_crossbow_cost:{33}, weapon_crossbow:{34}, Car_001_cost:{35}, Car_001:{36}, skill_exatk:{37}, skill_exatk_cost:{38}, skill_exgold:{39}, skill_exgold_cost:{40}, skill_exhp:{41}, skill_exhp_cost:{42}, skill_exspeed:{43}, skill_exspeed_cost:{44}, skill_excrit:{45}, skill_excrit_cost:{46}, skill_exbufftime:{47}, skill_exbufftime_cost:{48}, skill_exdodge:{49}, skill_exdodge_cost:{50}, skill_exbossharm:{51}, skill_exbossharm_cost:{52}}}",
				level, presentExp, atkParam, array2string(weapon_ak47_cost), array2string(weapon_ak47), array2string(weapon_laser_cost), array2string(weapon_laser), array2string(weapon_rpg_cost), array2string(weapon_rpg), array2string(weapon_mg_cost), array2string(weapon_mg), array2string(weapon_scatter_cost), array2string(weapon_scatter), array2string(weapon_firegun_cost), array2string(weapon_firegun), array2string(S_FireTurret_cost), array2string(S_FireTurret), array2string(weapon_laserX_cost), array2string(weapon_laserX), array2string(weapon_awp_cost), array2string(weapon_awp), array2string(weapon_elecgun_cost), array2string(weapon_elecgun), array2string(weapon_shotgun_cost), array2string(weapon_shotgun), array2string(weapon_bouncegun_cost), array2string(weapon_bouncegun), array2string(weapon_firecrossbow_cost), array2string(weapon_firecrossbow), array2string(weapon_icecrossbow_cost), array2string(weapon_icecrossbow), array2string(weapon_elecrossbow_cost), array2string(weapon_elecrossbow), array2string(weapon_crossbow_cost), array2string(weapon_crossbow), array2string(Car_001_cost), array2string(Car_001), array2string(skill_exatk), array2string(skill_exatk_cost), array2string(skill_exgold), array2string(skill_exgold_cost), array2string(skill_exhp), array2string(skill_exhp_cost), array2string(skill_exspeed), array2string(skill_exspeed_cost), array2string(skill_excrit), array2string(skill_excrit_cost), array2string(skill_exbufftime), array2string(skill_exbufftime_cost), array2string(skill_exdodge), array2string(skill_exdodge_cost), array2string(skill_exbossharm), array2string(skill_exbossharm_cost));
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
