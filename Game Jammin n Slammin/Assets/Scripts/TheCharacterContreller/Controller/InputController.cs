using UnityEngine;

namespace Luke
{
    public abstract class InputController : ScriptableObject
    {
        public abstract float RetrieveMoveInput();
        public abstract bool RetrieveJumpInput();
        public abstract bool RetrieveWeaponInput();
        public abstract bool RetrieveJumpHoldInput();
    }
}
