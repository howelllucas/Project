
using UnityEditor;
using UnityEngine;

public class GenerateUV3 {

    [MenuItem("Tools/SetUV2")]
    private static void SetUV2()
    {
        if (Selection.activeTransform)
        {

            Mesh mesh = GameObject.Instantiate<Mesh>(Selection.activeTransform.GetComponent<MeshFilter>().sharedMesh);

            Vector4 lightmapOffsetAndScale = Selection.activeTransform.GetComponent<MeshRenderer>().lightmapScaleOffset;
            //Mesh的UV2重新赋值
            Vector2[] modifiedUV2s = mesh.uv2;
            for (int i = 0; i < mesh.uv2.Length; i++)
            {
                modifiedUV2s[i] = new Vector2(mesh.uv2[i].x * lightmapOffsetAndScale.x +
                lightmapOffsetAndScale.z, mesh.uv2[i].y * lightmapOffsetAndScale.y +
                lightmapOffsetAndScale.w);
            }
            mesh.uv3 = modifiedUV2s;
            //mesh写到文件中
            AssetDatabase.CreateAsset(mesh, "Assets/newMesh.asset");

            Selection.activeTransform.GetComponent<MeshFilter>().sharedMesh = mesh;
        }
    }
}
