using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class EndBorder : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                collision.gameObject.GetComponent<Monster>().OnHittedDeathByEndBorder();
            }
        }
    }
}
