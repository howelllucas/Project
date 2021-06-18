using UnityEngine;

public class IoUtils
{
    public static string GetDocumentPath()
    {
        string persitentPath;
        RuntimePlatform platform = Application.platform;
        if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.OSXEditor)
        {
            persitentPath = Application.dataPath + "/../gamedata/";
        }
        else
        {
            persitentPath = Application.persistentDataPath + "/gamedata/";
        }
        PreparePath(persitentPath);
        return persitentPath;
    }
    private static void PreparePath(string path)
    {
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory(path);
        }
    }
}
