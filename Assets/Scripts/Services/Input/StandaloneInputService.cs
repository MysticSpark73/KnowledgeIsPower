using UnityEngine;

namespace Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 Axis
        {
            get
            {
                var axis = GetSimpleAxis();
                if (Mathf.Approximately(axis.magnitude, 0f))
                {
                    axis = GetUnityAxis();
                }
                return axis;
            }
        }
    }
}