using Services.Input;

namespace Infrastructure
{
    public class BootstrapState : IState
    {
        private const string BootstrapSceneName = "Bootstrap";
        private const string MainSceneName = "Main";
        
        private readonly GameStateMachine _gameStateMachine;
        private SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegisterServices();
            _sceneLoader.LoadScene(BootstrapSceneName, EnterLoadLevel);
        }

        public void Exit()
        {
            
        }

        private void RegisterServices()
        {
            Game.SetInputService(CreateInputService());
        }

        private void EnterLoadLevel() => _gameStateMachine.Enter<LoadLevelState, string>(MainSceneName);

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