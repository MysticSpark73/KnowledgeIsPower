using UnityEngine;

namespace Enemies
{
    public static class PhysicsDebug
    {
        public static void DrawSphere(Vector3 position, float radius, Color color, float duration)
        {
            Debug.DrawRay(position, Vector3.up * radius, color, duration);
            Debug.DrawRay(position, Vector3.down * radius, color, duration);
            Debug.DrawRay(position, Vector3.left * radius, color, duration);
            Debug.DrawRay(position, Vector3.right * radius, color, duration);
            Debug.DrawRay(position, Vector3.forward * radius, color, duration);
            Debug.DrawRay(position, Vector3.back * radius, color, duration);
        }

        public static void DrawMultidirectionalSphere(Vector3 position, float radius, Color color, float duration)
        {
            Debug.DrawRay(position, Vector3.up * radius, color, duration);
            Debug.DrawRay(position, Vector3.down * radius, color, duration);
            Debug.DrawRay(position, Vector3.left * radius, color, duration);
            Debug.DrawRay(position, Vector3.right * radius, color, duration);
            Debug.DrawRay(position, Vector3.forward * radius, color, duration);
            Debug.DrawRay(position, Vector3.back * radius, color, duration);
            
            Debug.DrawRay(position, (Vector3.up + Vector3.right + Vector3.forward).normalized * radius, color, duration);
            Debug.DrawRay(position, (Vector3.down + Vector3.left + Vector3.back).normalized * radius, color, duration);
            Debug.DrawRay(position, (Vector3.up + Vector3.left + Vector3.forward).normalized * radius, color, duration);
            Debug.DrawRay(position, (Vector3.down + Vector3.right + Vector3.back).normalized * radius, color, duration);
            Debug.DrawRay(position, (Vector3.up + Vector3.right + Vector3.back).normalized * radius, color, duration);
            Debug.DrawRay(position, (Vector3.down + Vector3.left + Vector3.forward).normalized * radius, color, duration);
            Debug.DrawRay(position, (Vector3.up + Vector3.left + Vector3.back).normalized * radius, color, duration);
            Debug.DrawRay(position, (Vector3.down + Vector3.right + Vector3.forward).normalized * radius, color, duration);
        }
    }
}