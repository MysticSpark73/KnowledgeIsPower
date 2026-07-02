using UnityEngine;

namespace Enemies
{
    public class RotateToHero : FollowBase
    {
        [SerializeField] private float _rotationSpeed;
        
        private Vector3 _positionToLookAt;

        private void Update()
        {
            if (_heroTransform == null) return;

            RotateTowardsHero();
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLookAt();

            transform.rotation = SmoothedRotation(transform.rotation, _positionToLookAt);
        }

        private void UpdatePositionToLookAt()
        {
            Vector3 positionDiff = _heroTransform.position - transform.position;
            _positionToLookAt = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLookAt) => 
            Quaternion.Lerp(rotation, TargetRotation(positionToLookAt), SpeedFactor());

        private Quaternion TargetRotation(Vector3 positionToLookAt) => 
            Quaternion.LookRotation(positionToLookAt);

        private float SpeedFactor() => _rotationSpeed * Time.deltaTime;
    }
}