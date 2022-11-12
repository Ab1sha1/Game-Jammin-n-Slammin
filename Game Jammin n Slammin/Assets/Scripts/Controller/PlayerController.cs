using UnityEngine;

namespace Luke
{
    [CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
    public class PlayerController : InputController
    {
        public override bool RetrieveJumpInput()
        {
            return Input.GetButtonDown("Jump");
        }

        public override float RetrieveMoveInput()
        {
            return Input.GetAxisRaw("Horizontal");
        }

        public override bool RetrieveWeaponInput()
        {
            return Input.GetKeyDown(KeyCode.K);
        }
    }
}
