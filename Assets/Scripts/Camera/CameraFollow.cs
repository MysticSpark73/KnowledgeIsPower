using UnityEngine;

namespace DefaultNamespace.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float RotationAngleX;
        [SerializeField] private float _distance;
        [SerializeField] private float _offsetY;

        private void LateUpdate()
        {
            if (_target == null) return;

            Quaternion rotation = Quaternion.Euler(RotationAngleX, 0, 0);
            var position = rotation * Vector3.back * _distance + FollowingPosition();
            
            transform.rotation = rotation;
            transform.position = position;
        }
        
        public void SetTarget(Transform target) => _target = target;

        private Vector3 FollowingPosition()
        {
            Vector3 followingPosition = _target.position;
            followingPosition.y += _offsetY;
            return followingPosition;
        }
    }
}