using Logic;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        private Game _game;
        private void Awake()
        {
            _game = new Game(this, _loadingCurtain);
            
            DontDestroyOnLoad(gameObject);
        }
    }
}