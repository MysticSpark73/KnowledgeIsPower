using UnityEngine;

namespace Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _bootstrapperPrefab;
        private void Awake()
        {
            var bootstrapper = FindAnyObjectByType<GameBootstrapper>();

            if (bootstrapper != null) return;

            Instantiate(_bootstrapperPrefab);
        }
    }
}