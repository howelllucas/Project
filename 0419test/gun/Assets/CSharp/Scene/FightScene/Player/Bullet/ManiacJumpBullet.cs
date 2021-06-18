using UnityEngine;
namespace EZ
{
    public class ManiacJumpBullet : BaseBullet
    {
        public void Init(double damage,Vector3 position)
        {
            m_Damage = damage;
            transform.position = position;
            transform.SetParent(Global.gApp.gBulletNode.transform,false);
            Global.gApp.gAudioSource.PlayOneShot(HittedEnemyClip);
            InitBulletEffect();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.MonsterTag))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnHit_Vec(m_Damage, transform);
            }
        }
        private void InitBulletEffect()
        {
            GameObject effect = Instantiate(BulletEffect);
            effect.transform.position = transform.position;
            DelayDestroy delayDestroy = effect.AddComponent<DelayDestroy>();
            delayDestroy.SetLiveTime(2.0f);
        }
    }
}
