using UnityEngine;

namespace Luke
{
    [CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]
    public class AIController : InputController
    {
        public override bool RetrieveJumpInput()
        {
            return true;
        }

        public override float RetrieveMoveInput()
        {
            return 1f;
        }

        public override bool RetrieveWeaponInput()
        {
            return false;
        }

        public override bool RetrieveJumpHoldInput()
        {
            return false;
        }
    }
}
