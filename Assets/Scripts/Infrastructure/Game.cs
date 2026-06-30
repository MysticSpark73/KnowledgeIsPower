using Infrastructure.Services;
using Infrastructure.States;
using Logic;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _stateMachine;
        
        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            _stateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain, AllServices.Container);
        }
    }
}