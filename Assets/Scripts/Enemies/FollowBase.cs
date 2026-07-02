using Infrastructure.Factory;
using Infrastructure.Services;
using UnityEngine;

namespace Enemies
{
    public abstract class FollowBase : MonoBehaviour
    {
        private IGameFactory _gameFactory;
        protected Transform _heroTransform;

        protected virtual void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            GetHeroFromFactory();
        }

        private void GetHeroFromFactory()
        {
            if (_gameFactory.HeroObject != null)
            {
                _heroTransform = _gameFactory.HeroObject.transform;
            }
            else
            {
                _gameFactory.HeroCreated += OnHeroCreated;
            }
        }

        private void OnHeroCreated()
        { 
            _gameFactory.HeroCreated -= OnHeroCreated;
            _heroTransform = _gameFactory.HeroObject.transform;
        }
    }
}