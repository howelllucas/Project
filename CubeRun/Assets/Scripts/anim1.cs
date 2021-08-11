using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim1 : MonoBehaviour
{
    private Transform son_trans;

    Vector3 nomalTrans;
    Vector3 targetTrans;

    void Start()
    {
        son_trans = this.transform.Find("moving_spikes_b");
        nomalTrans = son_trans.position;
        targetTrans = nomalTrans + new Vector3(0, 0.15f, 0);
        StartCoroutine("upanddown");
    }

    private IEnumerator up()
    {
        while (true)
        {
            son_trans.position = Vector3.Lerp(son_trans.position, targetTrans, Time.deltaTime * 30);
            yield return null;
        }
        
    }

    private IEnumerator down()
    {
        while (true)
        {
            son_trans.position = Vector3.Lerp(son_trans.position, nomalTrans, Time.deltaTime * 30);
            yield return null;
        }
        
    }
    private IEnumerator upanddown()
    {
        while (true)
        {
            StopCoroutine("down");
            StartCoroutine("up");
            yield return new WaitForSeconds(2f);
            StopCoroutine("up");
            StartCoroutine("down");
            yield return new WaitForSeconds(2f);
        }
    }
}
