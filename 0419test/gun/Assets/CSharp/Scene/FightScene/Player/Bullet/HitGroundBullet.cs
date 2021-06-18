using UnityEngine;
namespace EZ
{
    public class HitGroundBullet : BaseBullet
    {
        public void Init(Vector3 position)
        {
            transform.position = position;
            transform.SetParent(Global.gApp.gBulletNode.transform,false);
            InitBulletEffect();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(GameConstVal.DamageRangeTag))
            {
                collision.gameObject.GetComponentInParent<Player>().StartBackActOnPos(transform);
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
