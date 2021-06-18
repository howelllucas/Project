using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run
} 

public class characterMove : MonoBehaviour
{
    public PlayerState state = PlayerState.Idle;
    public int speed=5;
    private playerDir playerdir;
    private float characterdir;
    private CharacterController characterCtl;
    public bool isMoving=false;
    // Start is called before the first frame update
    void Start()
    {
        playerdir = this.transform.GetComponent<playerDir>();
        characterCtl = this.transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        characterdir = Vector3.Distance(playerdir.targetPoint, this.transform.position);
        if (characterdir>0.1f)
        {
            characterCtl.SimpleMove(transform.forward * speed);
            state = PlayerState.Run;
            isMoving = true;
        }
        else
        {
            state = PlayerState.Idle;
            isMoving = false;
        }
    }
}
