using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{
    public class DamageText : MonoBehaviour
    {

        public TextMesh textMesh;

        public void SetDamage(int damage, bool crit)
        {
            textMesh.text = damage.ToString();
            if (crit)
            {
                transform.localScale *= 1.5f;
                textMesh.color = Color.red;
            }
            else
            {
                transform.localScale = Vector3.one;
                textMesh.color = Color.white;
            }
        }
    }
}
