using UnityEngine;

namespace InputControls
{
    // class controls keyboard input

    public class InputController : MonoBehaviour
    {
        public delegate void OnKeyBoardClickedEvent();
        public event OnKeyBoardClickedEvent OnSpaceClicked;
        public event OnKeyBoardClickedEvent OnRKeyClicked;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpaceClicked?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                OnRKeyClicked?.Invoke();
            }
        }
    }
}
