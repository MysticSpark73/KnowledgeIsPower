using UnityEngine;

namespace Infrastructure.AssetsManagement
{
    public class AssetsProvider : IAssetsProvider
    {
        public GameObject InstantiatePrefabFromResources(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public GameObject InstantiatePrefabFromResources(string path, Vector3 position)
        {
            var  prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, position, Quaternion.identity);
        }
    }
}