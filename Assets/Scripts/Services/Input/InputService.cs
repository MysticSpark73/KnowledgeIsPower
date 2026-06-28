using UnityEngine;

namespace Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        protected const string Button = "Fire";

        public abstract Vector2 Axis { get; }
        public bool IsAttackButtonClicked() => SimpleInput.GetButtonUp(Button);
        
        protected Vector2 GetSimpleAxis() => new(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));

        protected Vector2 GetUnityAxis() => new(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
    }
}