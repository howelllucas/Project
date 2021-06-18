using UnityEngine;

namespace EZ
{
    public class Robot001PursueState :PetPursueState 
    {

        public override void Init(GameObject playerGo,BasePet pet)
        {
            base.Init(playerGo,pet);
        }
        public override void StartState()
        {
            base.StartState();
        }

        public override bool CheckState()
        {
            return base.CheckState();
        }

        public override void EndState()
        {
            base.EndState();
        }
    }
}
