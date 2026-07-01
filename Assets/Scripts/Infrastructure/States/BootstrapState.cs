using Infrastructure.AssetsManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Services.Input;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string BootstrapSceneName = "Bootstrap";
        private const string MainSceneName = "Main";
        
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

        public void Exit()
        {
            
        }

        private void RegisterServices()
        {
            _serviceProvider.RegisterSingle<IInputService>(CreateInputService());
            _serviceProvider.RegisterSingle<IAssetsProvider>(new AssetsProvider());
            _serviceProvider.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _serviceProvider.RegisterSingle<IGameFactory>(new GameFactory(_serviceProvider.Single<IAssetsProvider>()));
            _serviceProvider.RegisterSingle<ISaveLoadService>(new SaveLoadService(_serviceProvider.Single<IPersistentProgressService>(), _serviceProvider.Single<IGameFactory>()));
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