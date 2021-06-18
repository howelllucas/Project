using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EZ
{

    public class MainRoleDamageRange : MonoBehaviour
    {
        private Player m_Player;
        private void Start()
        {
            m_Player = GetComponentInParent<Player>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                Rigidbody2D rigidbody2D = collision.GetComponentInParent<Rigidbody2D>();
                if (rigidbody2D.velocity.sqrMagnitude > 100 && collision.gameObject.layer == GameConstVal.BossLayer)
                {
                    m_Player.StartBackActOnPos(collision.transform);
                }
                m_Player.OnHitted(m_Player.MonsterDamage);
            }
            else if(collision.gameObject.CompareTag(GameConstVal.MonsterBulletTag)){
                m_Player.OnHitted(m_Player.MonsterDamage);
            }
        }
    }
}
