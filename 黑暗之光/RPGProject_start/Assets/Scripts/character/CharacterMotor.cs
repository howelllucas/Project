using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour
{
    private CharacterController cc;
    
    public int speed=5;

    public Vector3 point;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
        cc =this.GetComponent<CharacterController>();

        MonoMgr.GetInstance().AddUpdateListener(move);
        point = mouseRay.GetInstance().point;
        EventCenter.GetInstance().AddEventListener("鼠标按下", () =>
        {
            move();
        });
    }

   public void move()
    {
        point = mouseRay.GetInstance().point;
        if (point != Vector3.zero)
        {
            point.y = this.transform.position.y;
            this.transform.LookAt(point);
            
            if (Vector3.Distance(point,this.transform.position)>0.2f)
            {
                cc.SimpleMove(this.transform.forward * speed);
            }
        }
    }

    
}
