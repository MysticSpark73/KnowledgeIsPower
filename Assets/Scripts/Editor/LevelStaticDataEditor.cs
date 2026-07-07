using System.Linq;
using Logic.EnemySpawners;
using StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var levelData = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                CollectLevelData(levelData);
            }
        }

        private void CollectLevelData(LevelStaticData levelData)
        {
            levelData.EnemySpawnerDatas =
                FindObjectsByType<EnemySpawnMarker>().Select(i =>
                    new EnemySpawnerData(i.GetData().id, i.GetData().monsterType, i.transform.position)).ToList();
            
            levelData.LevelKey = SceneManager.GetActiveScene().name;
            EditorUtility.SetDirty(target);
        }
    }
}