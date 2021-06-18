using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instence;
    private CursorManager() { }

    public Texture2D cursor_normal;
    public Texture2D cursor_npc_talk;
    public Texture2D cursor_attack;
    public Texture2D cursor_lockTarget;
    public Texture2D cursor_pick;

    private void Start()
    {
        instence = this;
    }
    public void setNormal()
    {
        Cursor.SetCursor(cursor_normal, Vector2.zero, CursorMode.Auto);
    }
    public void setNpcTalk()
    {
        Cursor.SetCursor(cursor_npc_talk, Vector2.zero, CursorMode.Auto);
    }

}
