using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private void OnMouseEnter()
    {
        CursorManager.instence.setNpcTalk();
    }
    private void OnMouseExit()
    {
        CursorManager.instence.setNormal();
    }
}
