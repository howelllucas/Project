using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerAttackState
{
    nomalAttack,
    skillAttack,
    NoAttack
}
public class playerAttack : MonoBehaviour
{
    public int nomalAttackDis = 2;
    public int skillAttackDis = 5;
    public float nomalAttackdate = 1;
    public float skillAttackdate = 1;
    public int timer;
    public playerAttackState attackState = playerAttackState.NoAttack;

    public Transform attackTarget;

    public characterMove chMove;
    // Start is called before the first frame update
    void Start()
    {
        chMove = this.GetComponent<characterMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool iscollider = Physics.Raycast(ray, out hit);
            if (iscollider && hit.collider.tag=="enemy")
            {
                attackTarget = hit.collider.transform;
                //攻击
                attack(attackTarget);
            }
            else
            {
                attackState = playerAttackState.NoAttack;
                attackTarget = null;
            }
        }
    }
    private  void attack(Transform target)
    {
        float dis = Vector3.Distance(target.position, this.transform.position);
        if (dis<=nomalAttackDis)
        {
            //普通攻击
        }
        else if (dis <= skillAttackDis)
        {
            //技能攻击
            Debug.Log("11111");
        }
        else
        {
            //朝向敌人移动
            chMove.Move(target);
        }


    }
}
