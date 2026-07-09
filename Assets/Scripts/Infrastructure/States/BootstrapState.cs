using Infrastructure.AssetsManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Services;
using Services.Input;
using StaticData;
using UI.Services.Factory;
using UI.Services.Windows;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string BootstrapSceneName = "Bootstrap";
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _serviceProvider;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices serviceProvider)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _serviceProvider = serviceProvider;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.LoadScene(BootstrapSceneName, EnterLoadProgressState);
        }

        public void Exit() { }

        private void RegisterServices()
        {
            RegisterStaticDataService();

            _serviceProvider.RegisterSingle<IInputService>(CreateInputService());
            _serviceProvider.RegisterSingle<IRandomService>(new RandomService());
            _serviceProvider.RegisterSingle<IAssetsProvider>(new AssetsProvider());
            _serviceProvider.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _serviceProvider.RegisterSingle<IUIFactory>(new UIFactory(
                _serviceProvider.Single<IAssetsProvider>(),
                _serviceProvider.Single<IStaticDataService>(),
                _serviceProvider.Single<IPersistentProgressService>()));
            _serviceProvider.RegisterSingle<IWindowsService>(new WindowsService(
                _serviceProvider.Single<IUIFactory>()));
            _serviceProvider.RegisterSingle<IGameFactory>(new GameFactory(
                _serviceProvider.Single<IAssetsProvider>(),
                _serviceProvider.Single<IStaticDataService>(),
                _serviceProvider.Single<IPersistentProgressService>(),
                _serviceProvider.Single<IRandomService>(),
                _serviceProvider.Single<IWindowsService>()));
            _serviceProvider.RegisterSingle<ISaveLoadService>(new SaveLoadService(
                _serviceProvider.Single<IPersistentProgressService>(),
                _serviceProvider.Single<IGameFactory>()));
        }

        private void RegisterStaticDataService()
        {
            var staticDataService = new StaticDataService();
            staticDataService.LoadData();
            _serviceProvider.RegisterSingle<IStaticDataService>(staticDataService);
        }

        private void EnterLoadProgressState() => _gameStateMachine.Enter<LoadProgressState>();

        private IInputService CreateInputService()
        {
#if UNITY_EDITOR
            return new StandaloneInputService();
#else
            return new MobileInputService();
#endif
        }
    }
}