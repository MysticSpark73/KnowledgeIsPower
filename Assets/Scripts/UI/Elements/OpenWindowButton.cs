using UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private WindowType _windowType;
        
        private IWindowsService _windowService;

        private void Awake()
        {
            _button.onClick.AddListener(OpenWindow);
        }

        public void Initialize(IWindowsService windowsService)
        {
            _windowService = windowsService;
        }

        private void OpenWindow()
        {
            _windowService.Show(_windowType);
        }
    }
}