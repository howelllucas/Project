using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//整体数据类，存储在字典
public class SkillsInfo : MonoBehaviour
{
    public TextAsset skillInfoText;

    public static SkillsInfo instance;
    public Dictionary<int, SkillInfo> skillInfoDict = new Dictionary<int, SkillInfo>();

    //public SkillInfo skillInfo;
    private void Awake()
    {
        instance = this;
        InitDkillInfoDict();
    }
    
    private void InitDkillInfoDict()
    {
        string text = skillInfoText.text;
        string[] skillInfoArray = text.Split('\n');
        foreach (var skillInfoStr in skillInfoArray)
        {
            string[] pa = skillInfoStr.Split(',');
            SkillInfo info = new SkillInfo();
            info.id = int.Parse( pa[0]);
            info.name = pa[1];
            info.icon_name = pa[2];
            info.des = pa[3];
            string str_applytype = pa[4];
            switch (str_applytype)
            {
                case "Passive":
                    info.applyType = ApplyType.Passive;
                    break;
                case "Buff":
                    info.applyType = ApplyType.Buff;
                    break;
                case "SingleTarget":
                    info.applyType = ApplyType.SingleTarget;
                    break;
                case "MultiTarget":
                    info.applyType = ApplyType.MultiTarget;
                    break;

                default:
                    break;
            }
            string str_applyProperty = pa[5];
            switch (str_applyProperty)
            {
                case "Attack":
                    info.applyProperty = ApplyProperty.Attack;
                    break;
                case "Def":
                    info.applyProperty = ApplyProperty.Def;
                    break;
                case "Speed":
                    info.applyProperty = ApplyProperty.Speed;
                    break;
                case "AttackSpeed":
                    info.applyProperty = ApplyProperty.AttackSpeed;
                    break;
                case "HP":
                    info.applyProperty = ApplyProperty.HP;
                    break;
                case "MP":
                    info.applyProperty = ApplyProperty.MP;
                    break;
                default:
                    break;
            }
            info.applyValue = int.Parse(pa[6]);
            info.applyTime = int.Parse(pa[7]);
            info.mp = int.Parse(pa[8]);
            info.coldTime = int.Parse(pa[9]);
            string applicableRole = pa[10];
            switch (applicableRole)
            {
                case "Swordman":
                    info.applicableRole = ApplicableRole.Swordman;
                    break;
                case "Magician":
                    info.applicableRole = ApplicableRole.Magician;
                    break;

                default:
                    break;
            }
            info.level = int.Parse(pa[11]);
            string releaseType = pa[12];
            switch (releaseType)
            {
                case "Enemy":
                info.releaseType = ReleaseType.Enemy;
                    break;
                case "Position":
                    info.releaseType = ReleaseType.Position;
                    break;
                case "Self":
                    info.releaseType = ReleaseType.Self;
                    break;
                default:
                    break;
            }
            info.distance = float.Parse(pa[13]);
            skillInfoDict.Add(info.id, info);
        }
    }

    public SkillInfo GetSkillInfoById(int id)
    {
        SkillInfo skillInfo=null;
        skillInfoDict.TryGetValue(id,out skillInfo);
        return skillInfo;
    }
}


public enum ApplicableRole
{
    Swordman,
    Magician
}
public enum ApplyType
{
    Passive,
    Buff,
    SingleTarget,
    MultiTarget
}
public enum ApplyProperty
{
    Attack,
    Def,
    Speed,
    AttackSpeed,
    HP,
    MP
}
public enum ReleaseType
{
    Self,
    Enemy,
    Position
}
//单行技能数据类
public class SkillInfo
{
    public int id;
    public string name;
    public string icon_name;
    public string des;
    public ApplyType applyType;
    public ApplyProperty applyProperty;
    public int applyValue;
    public int applyTime;
    public int mp;
    public int coldTime;
    public ApplicableRole applicableRole;
    public int level;
    public ReleaseType releaseType;
    public float distance;

}
