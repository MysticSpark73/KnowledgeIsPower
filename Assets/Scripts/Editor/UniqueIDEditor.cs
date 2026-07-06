using System;
using System.Linq;
using Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(UniqueID))]
    public class UniqueIDEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var uniqueId = (UniqueID)target;

            if (IsPrefab(uniqueId)) return;

            if (string.IsNullOrEmpty(uniqueId.ID))
            {
                Generate(uniqueId);
            }
            else
            {
                UniqueID[] uniqueIds = FindObjectsByType<UniqueID>();
                if (uniqueIds == null || uniqueIds.Length == 0) return;
                
                if (uniqueIds.Any(i => i != uniqueId && i.ID != null && i.ID.Equals(uniqueId.ID)))
                {
                    Generate(uniqueId);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var uniqueId = (UniqueID)target;
            EditorGUILayout.SelectableLabel(uniqueId.ID);
        }

        /// <summary>
        /// The bug he's trying to fix relevant for 2019 version of Unity seems to be already fixed in Unity 6.5
        /// </summary>
        [Obsolete]
        private bool IsPrefab(UniqueID uniqueId) => uniqueId.gameObject.scene.rootCount == 0;

        public void Generate(UniqueID uniqueId)
        {
            uniqueId.SetId($"{uniqueId.gameObject.scene.name}_{Guid.NewGuid().ToString()}");

            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(uniqueId);
                EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene);
            }
        }
    }
}