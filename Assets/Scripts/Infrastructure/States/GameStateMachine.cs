using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using Logic;

namespace Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;
        private SceneLoader _sceneLoader;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices serviceProvider)
        {
            _sceneLoader = sceneLoader;

            _states = new Dictionary<Type, IExitableState>()
            {
                { typeof(BootstrapState), new BootstrapState(this, _sceneLoader, serviceProvider) },
                {
                    typeof(LoadLevelState),
                    new LoadLevelState(this, _sceneLoader, loadingCurtain, serviceProvider.Single<IGameFactory>(),
                        serviceProvider.Single<IPersistentProgressService>())
                },
                {
                    typeof(LoadProgressState),
                    new LoadProgressState(this, serviceProvider.Single<IPersistentProgressService>(),
                        serviceProvider.Single<ISaveLoadService>())
                },
                { typeof(GameLoopState), new GameLoopState(this) }
            };
            
            Enter<BootstrapState>();
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _currentState?.Exit();
            TState state = GetState<TState>();
            _currentState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => _states[typeof(TState)] as TState;
    }
}