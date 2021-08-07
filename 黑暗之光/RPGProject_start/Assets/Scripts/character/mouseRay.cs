using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class mouseRay : BaseManager<mouseRay>
{
    
    public Vector3 point=Vector3.zero;
    public Transform enemyTarget;
    public mouseRay()
    {
        InputMgr.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener("鼠标按下", () =>
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray,out raycastHit))
            {
                switch (raycastHit.collider.tag)
                {
                    case "floor":
                        point = raycastHit.point;
                        break;
                    case "npc":
                        if (raycastHit.collider.gameObject.name=="renwu")
                        {
                            Debug.Log("111");
                            UIManager.GetInstance().ShowPanel<BasePanel>("renwu", E_UI_Layer.Mid, (obj) =>
                            {

                            });
                        }
                        else if (raycastHit.collider.gameObject.name == "shangdian")
                        {
                            UIManager.GetInstance().ShowPanel<BasePanel>("shangdian", E_UI_Layer.Mid, (obj) =>
                            {

                            });
                        }
                        break;
                    case "enemy":
                        enemyTarget.position = raycastHit.point;
                        break;

                    default:
                        break;
                }
                
                
            }
        });
    }

   


}
