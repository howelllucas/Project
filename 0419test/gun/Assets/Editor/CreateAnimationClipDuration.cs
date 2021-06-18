using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AnimationClipDuration {
    //[MenuItem("KungFu/生成动画时长信息")]
    public static void CreateAnimationClipDuration()  
    {
        string[] rootpath = new string[] { "Assets/Resources/Character" };
        string filterType = "t:Prefab";
        string target_lua_file = "../lua/AutoConfig/KFCAnimTimeCfg.lua";
        string line_break = "\n";
        string line_tab = "    ";

        string[] ids = AssetDatabase.FindAssets(filterType, rootpath);
        FileStream targetFile = File.Create(target_lua_file);
        string header = "local data = {"+ line_break;
        targetFile.Write(Encoding.ASCII.GetBytes(header), 0, header.Length);
        string content;
        Debug.Log("ids.Length == " + ids.Length);
        for (int i = 0; i < ids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(ids[i]);
            GameObject asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            
            GameObject obj = PrefabUtility.InstantiatePrefab(asset) as GameObject;
            Animator animator = obj.GetComponent<Animator>();
            
            if (animator != null)
            {
                path = System.IO.Path.GetFileNameWithoutExtension(path);
                content = line_tab + path + " = {"+ line_break;
                string animatorPath = AnimatorPrefabUtils.GenerateAnimatorPath + path + ".controller";
                RuntimeAnimatorController ac = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(animatorPath);
                AnimationClip[] tAnimationClips = ac.animationClips;
                if (null == tAnimationClips || tAnimationClips.Length <= 0)
                {
                    UnityEngine.Object.DestroyImmediate(obj);
                    continue;
                }
                Debug.Log("生成>>>" + path);
                AnimationClip tAnimationClip;

                for (int tCounter = 0, tLen = tAnimationClips.Length; tCounter < tLen; tCounter++)
                {
                    tAnimationClip = ac.animationClips[tCounter];
                    if (null != tAnimationClip)
                    {
                        //var info = AnimationUtility.GetAllCurves(tAnimationClip);
                        var curcontent = line_tab + line_tab + tAnimationClip.name + " = " + Math.Round(tAnimationClip.length, 3) + "," + line_break;
                        //Debug.Log("curcontent == " + curcontent);
                        content = content + curcontent;
                    }
                }
                content = content + line_tab + "}," + line_break;
                targetFile.Write(Encoding.ASCII.GetBytes(content), 0, content.Length);
            }
            UnityEngine.Object.DestroyImmediate(obj);
        }
        string end =  "}" + line_break;
        targetFile.Write(Encoding.ASCII.GetBytes(end), 0, end.Length);
        string funcstr = "if gApp ~= nil and gApp.addDataByName ~= nil then" + line_break + "    gApp:addDataByName('KFCAnimTimeCfg', data)" + line_break + "end " + line_break + "return data";
        targetFile.Write(Encoding.ASCII.GetBytes(funcstr), 0, funcstr.Length);

        targetFile.Close();
        Debug.Log("------------生成动作时长信息成功");
    }

}
