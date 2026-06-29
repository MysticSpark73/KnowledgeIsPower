using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        protected const string Button = "Fire";
        
        private const string InputSetPath = "InputSystem_Actions";
        private const string MoveActionKey = "Move";
        
        private readonly InputActionAsset _inputActions = Resources.Load<InputActionAsset>(InputSetPath);

        public abstract Vector2 Axis { get; }
        public bool IsAttackButtonClicked() => SimpleInput.GetButtonUp(Button);
        
        protected Vector2 GetSimpleAxis() => new(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));

        protected Vector2 GetUnityAxis()
        {
            if (_inputActions == null) return Vector2.zero;
            return _inputActions[MoveActionKey].ReadValue<Vector2>();
        }
    }
}