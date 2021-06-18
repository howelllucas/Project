using System.Collections.Generic;

namespace EZ
{
    public class BulletConfig
    {
        public static string BulletCampExplode = "bullet_camp_explode";
        public static string Bullet_Extern = "bullet_extern";
        public static string Bullet_RobotSkill02 = "bullet_RobotSkill02";
        private static string Bullet_UWA = "bullet_uwa";
        private static string Bullet_Ak47 = "bullet_ak47";
        private static string Bullet_Ak47_S = "bullet_ak47_s";
        private static string Bullet_Mg = "bullet_mg";
        private static string Bullet_Mg_S = "bullet_mg_s";
        private static string Bullet_Scatter = "bullet_scatter";
        private static string Bullet_Scatter_S = "bullet_scatter_s";
        private static string Bullet_Tornado = "bullet_tornado";
        private static string Bullet_Tornado_S = "bullet_tornado_s";
        private static string Bullet_Dog = "bullet_dog";
        private static string Bullet_Dubble = "bullet_bubble";
        public static Dictionary<string, string> BulletPath = new Dictionary<string, string>
        {
            { Bullet_Ak47_S,"Prefabs/Bullet/RoleBullet/bullet_ak47_s"},
            { Bullet_Ak47,"Prefabs/Bullet/RoleBullet/bullet_ak47"},
            { Bullet_Mg,"Prefabs/Bullet/RoleBullet/bullet_mg"},
            { Bullet_Mg_S,"Prefabs/Bullet/RoleBullet/bullet_mg_s"},
            { Bullet_Scatter,"Prefabs/Bullet/RoleBullet/bullet_scatter"},
            { Bullet_Scatter_S,"Prefabs/Bullet/RoleBullet/bullet_scatter_s"},
            { Bullet_Extern,"Prefabs/Bullet/RoleBullet/bullet_extern"},
            { Bullet_RobotSkill02,"Prefabs/Bullet/RoleBullet/bullet_RobotSkill02"},
            { Bullet_UWA,"Prefabs/Bullet/RoleBullet/bullet_uwa"},
            { Bullet_Tornado,"Prefabs/Bullet/RoleBullet/bullet_tornado"},
            { Bullet_Tornado_S,"Prefabs/Bullet/RoleBullet/bullet_tornado_s"},
            { Bullet_Dog,"Prefabs/Bullet/RoleBullet/bullet_dog"},
            { Bullet_Dubble,"Prefabs/Bullet/RoleBullet/bullet_bubble"},
            { BulletCampExplode,"Prefabs/Bullet/RoleBullet/bullet_camp_explode"},
        };
    }
}

