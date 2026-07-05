using UnityEngine;

namespace Enemies
{
    public abstract class FollowBase : MonoBehaviour
    {
        protected Transform _heroTransform;
        
        public void Initialize(Transform heroTransform) => _heroTransform = heroTransform;
    }
}