using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{
    private characterMove move;
    private Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        move = this.transform.GetComponent<characterMove>();
        anim = this.transform.GetComponent<Animation>();
       
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (move.state==PlayerState.Idle)
        {
            playAnim("Idle");
        }
        else if (move.state == PlayerState.Run)
        {
            playAnim("Run");
        }
    }

    private void playAnim(string clip)
    {
        anim.CrossFade(clip);
        
    }
}
