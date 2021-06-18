using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TextureUVAnim : MonoBehaviour
{
    public string textureName;
    public float speedX;
    public float speedY;
    private Material mat;
    private float clock;
    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        clock += Time.deltaTime * 0.05f;
        var offsetX = clock * speedX;
        var offsetY = clock * speedY;
        offsetX = offsetX % 1;
        offsetY = offsetY % 1;
        mat.SetTextureOffset(textureName, new Vector2(offsetX, offsetY));
    }
}
