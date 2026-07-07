using Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EnemySpawnMarker))]
    public class EnemySpawnMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(EnemySpawnMarker spawnPoint, GizmoType gizmoType)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawnPoint.transform.position, .5f);
        }
    }
}