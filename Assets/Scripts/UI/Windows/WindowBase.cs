using Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] protected Button _closeButton;

        protected PlayerProgress Progress => _progressService.PlayerProgress;
        
        protected IPersistentProgressService _progressService;

        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            OnStart();
            SubscribeToEvents();
        }

        public void Initialize(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        protected virtual void OnStart() { }

        protected virtual void SubscribeToEvents() { }
        
        protected virtual void UnSubscribeFromEvents() { }

        protected virtual void OnAwake()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            UnSubscribeFromEvents();
        }

        private void Close()
        {
            Destroy(gameObject);
        }
    }
}