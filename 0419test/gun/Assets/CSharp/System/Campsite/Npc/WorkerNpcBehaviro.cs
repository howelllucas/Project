using UnityEngine;

namespace EZ
{
    public class WorkerNpcBehaviro : NpcBehavior
    {
        protected GameObject m_EffectGo;
        static string WorkAnimName = "work";
        protected override void PlayAnim(int index)
        {
            base.PlayAnim(index);
            if(m_CurAnimName == WorkAnimName)
            {
                AddWorkEffect();
            }
            else
            {
                RemoveWorkEffect();
            }
        }

        private void RemoveWorkEffect()
        {
            if(m_EffectGo != null)
            {
                m_EffectGo.SetActive(false);
            }
        }
        private void AddWorkEffect()
        {

            if(m_EffectGo == null)
            {
                m_EffectGo = Global.gApp.gResMgr.InstantiateObj(EffectConfig.Npc_worker_work);
                m_EffectGo.transform.SetParent(transform, false);
            }
            m_EffectGo.SetActive(true);
        }
    }
}
