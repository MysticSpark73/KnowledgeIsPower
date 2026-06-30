using Logic;
using Services.Input;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _stateMachine;
        public static IInputService InputService;
        
        public static void SetInputService(IInputService inputService) => InputService = inputService;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            _stateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain);
        }
    }
}