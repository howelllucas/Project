//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

using UnityEngine;

namespace EZ.Data {

	public partial class Guns_data : ScriptableObject {

		[SerializeField, HideInInspector]
		private Guns_dataItem[] _Items;
		public Guns_dataItem[] items { get { return _Items; } }

		public Guns_dataItem Get(int level) {
			int min = 0;
			int max = items.Length;
			while (min < max) {
				int index = (min + max) >> 1;
				Guns_dataItem item = _Items[index];
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
	public class Guns_dataItem {

		[SerializeField, HideInInspector]
		private int _Level;
		public int level { get { return _Level; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_ak47_cost;
		public double[] weapon_ak47_cost { get { return _Weapon_ak47_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_ak47;
		public double[] weapon_ak47 { get { return _Weapon_ak47; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_ak47_super;
		public double[] weapon_ak47_super { get { return _Weapon_ak47_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_laser_cost;
		public double[] weapon_laser_cost { get { return _Weapon_laser_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_laser;
		public double[] weapon_laser { get { return _Weapon_laser; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_laser_super;
		public double[] weapon_laser_super { get { return _Weapon_laser_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_rpg_cost;
		public double[] weapon_rpg_cost { get { return _Weapon_rpg_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_rpg;
		public double[] weapon_rpg { get { return _Weapon_rpg; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_rpg_super;
		public double[] weapon_rpg_super { get { return _Weapon_rpg_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_mg_cost;
		public double[] weapon_mg_cost { get { return _Weapon_mg_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_mg;
		public double[] weapon_mg { get { return _Weapon_mg; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_mg_super;
		public double[] weapon_mg_super { get { return _Weapon_mg_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_scatter_cost;
		public double[] weapon_scatter_cost { get { return _Weapon_scatter_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_scatter;
		public double[] weapon_scatter { get { return _Weapon_scatter; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_scatter_super;
		public double[] weapon_scatter_super { get { return _Weapon_scatter_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_firegun_cost;
		public double[] weapon_firegun_cost { get { return _Weapon_firegun_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_firegun;
		public double[] weapon_firegun { get { return _Weapon_firegun; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_firegun_super;
		public double[] weapon_firegun_super { get { return _Weapon_firegun_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_laserX_cost;
		public double[] weapon_laserX_cost { get { return _Weapon_laserX_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_laserX;
		public double[] weapon_laserX { get { return _Weapon_laserX; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_laserX_super;
		public double[] weapon_laserX_super { get { return _Weapon_laserX_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_awp_cost;
		public double[] weapon_awp_cost { get { return _Weapon_awp_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_awp;
		public double[] weapon_awp { get { return _Weapon_awp; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_awp_super;
		public double[] weapon_awp_super { get { return _Weapon_awp_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_elecgun_cost;
		public double[] weapon_elecgun_cost { get { return _Weapon_elecgun_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_elecgun;
		public double[] weapon_elecgun { get { return _Weapon_elecgun; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_elecgun_super;
		public double[] weapon_elecgun_super { get { return _Weapon_elecgun_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_shotgun_cost;
		public double[] weapon_shotgun_cost { get { return _Weapon_shotgun_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_shotgun;
		public double[] weapon_shotgun { get { return _Weapon_shotgun; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_shotgun_super;
		public double[] weapon_shotgun_super { get { return _Weapon_shotgun_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_bouncegun_cost;
		public double[] weapon_bouncegun_cost { get { return _Weapon_bouncegun_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_bouncegun;
		public double[] weapon_bouncegun { get { return _Weapon_bouncegun; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_bouncegun_super;
		public double[] weapon_bouncegun_super { get { return _Weapon_bouncegun_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_firecrossbow_cost;
		public double[] weapon_firecrossbow_cost { get { return _Weapon_firecrossbow_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_firecrossbow;
		public double[] weapon_firecrossbow { get { return _Weapon_firecrossbow; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_firecrossbow_super;
		public double[] weapon_firecrossbow_super { get { return _Weapon_firecrossbow_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_icecrossbow_cost;
		public double[] weapon_icecrossbow_cost { get { return _Weapon_icecrossbow_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_icecrossbow;
		public double[] weapon_icecrossbow { get { return _Weapon_icecrossbow; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_icecrossbow_super;
		public double[] weapon_icecrossbow_super { get { return _Weapon_icecrossbow_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_elecrossbow_cost;
		public double[] weapon_elecrossbow_cost { get { return _Weapon_elecrossbow_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_elecrossbow;
		public double[] weapon_elecrossbow { get { return _Weapon_elecrossbow; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_elecrossbow_super;
		public double[] weapon_elecrossbow_super { get { return _Weapon_elecrossbow_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_crossbow_cost;
		public double[] weapon_crossbow_cost { get { return _Weapon_crossbow_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_crossbow;
		public double[] weapon_crossbow { get { return _Weapon_crossbow; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_crossbow_super;
		public double[] weapon_crossbow_super { get { return _Weapon_crossbow_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_patriot_cost;
		public double[] weapon_patriot_cost { get { return _Weapon_patriot_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_patriot;
		public double[] weapon_patriot { get { return _Weapon_patriot; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_patriot_super;
		public double[] weapon_patriot_super { get { return _Weapon_patriot_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_tornado_cost;
		public double[] weapon_tornado_cost { get { return _Weapon_tornado_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_tornado;
		public double[] weapon_tornado { get { return _Weapon_tornado; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_tornado_super;
		public double[] weapon_tornado_super { get { return _Weapon_tornado_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_boomcrossbow_cost;
		public double[] weapon_boomcrossbow_cost { get { return _Weapon_boomcrossbow_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_boomcrossbow;
		public double[] weapon_boomcrossbow { get { return _Weapon_boomcrossbow; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_boomcrossbow_super;
		public double[] weapon_boomcrossbow_super { get { return _Weapon_boomcrossbow_super; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_bubble_cost;
		public double[] weapon_bubble_cost { get { return _Weapon_bubble_cost; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_bubble;
		public double[] weapon_bubble { get { return _Weapon_bubble; } }

		[SerializeField, HideInInspector]
		private double[] _Weapon_bubble_super;
		public double[] weapon_bubble_super { get { return _Weapon_bubble_super; } }

		public override string ToString() {
			return string.Format("[Guns_dataItem]{{level:{0}, weapon_ak47_cost:{1}, weapon_ak47:{2}, weapon_ak47_super:{3}, weapon_laser_cost:{4}, weapon_laser:{5}, weapon_laser_super:{6}, weapon_rpg_cost:{7}, weapon_rpg:{8}, weapon_rpg_super:{9}, weapon_mg_cost:{10}, weapon_mg:{11}, weapon_mg_super:{12}, weapon_scatter_cost:{13}, weapon_scatter:{14}, weapon_scatter_super:{15}, weapon_firegun_cost:{16}, weapon_firegun:{17}, weapon_firegun_super:{18}, weapon_laserX_cost:{19}, weapon_laserX:{20}, weapon_laserX_super:{21}, weapon_awp_cost:{22}, weapon_awp:{23}, weapon_awp_super:{24}, weapon_elecgun_cost:{25}, weapon_elecgun:{26}, weapon_elecgun_super:{27}, weapon_shotgun_cost:{28}, weapon_shotgun:{29}, weapon_shotgun_super:{30}, weapon_bouncegun_cost:{31}, weapon_bouncegun:{32}, weapon_bouncegun_super:{33}, weapon_firecrossbow_cost:{34}, weapon_firecrossbow:{35}, weapon_firecrossbow_super:{36}, weapon_icecrossbow_cost:{37}, weapon_icecrossbow:{38}, weapon_icecrossbow_super:{39}, weapon_elecrossbow_cost:{40}, weapon_elecrossbow:{41}, weapon_elecrossbow_super:{42}, weapon_crossbow_cost:{43}, weapon_crossbow:{44}, weapon_crossbow_super:{45}, weapon_patriot_cost:{46}, weapon_patriot:{47}, weapon_patriot_super:{48}, weapon_tornado_cost:{49}, weapon_tornado:{50}, weapon_tornado_super:{51}, weapon_boomcrossbow_cost:{52}, weapon_boomcrossbow:{53}, weapon_boomcrossbow_super:{54}, weapon_bubble_cost:{55}, weapon_bubble:{56}, weapon_bubble_super:{57}}}",
				level, weapon_ak47_cost, weapon_ak47, weapon_ak47_super, weapon_laser_cost, weapon_laser, weapon_laser_super, weapon_rpg_cost, weapon_rpg, weapon_rpg_super, weapon_mg_cost, weapon_mg, weapon_mg_super, weapon_scatter_cost, weapon_scatter, weapon_scatter_super, weapon_firegun_cost, weapon_firegun, weapon_firegun_super, weapon_laserX_cost, weapon_laserX, weapon_laserX_super, weapon_awp_cost, weapon_awp, weapon_awp_super, weapon_elecgun_cost, weapon_elecgun, weapon_elecgun_super, weapon_shotgun_cost, weapon_shotgun, weapon_shotgun_super, weapon_bouncegun_cost, weapon_bouncegun, weapon_bouncegun_super, weapon_firecrossbow_cost, weapon_firecrossbow, weapon_firecrossbow_super, weapon_icecrossbow_cost, weapon_icecrossbow, weapon_icecrossbow_super, weapon_elecrossbow_cost, weapon_elecrossbow, weapon_elecrossbow_super, weapon_crossbow_cost, weapon_crossbow, weapon_crossbow_super, weapon_patriot_cost, weapon_patriot, weapon_patriot_super, weapon_tornado_cost, weapon_tornado, weapon_tornado_super, weapon_boomcrossbow_cost, weapon_boomcrossbow, weapon_boomcrossbow_super, weapon_bubble_cost, weapon_bubble, weapon_bubble_super);
		}

	}

}
