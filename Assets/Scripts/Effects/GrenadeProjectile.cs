using System;
using Grid;
using Obstacle;
using Units;
using Unity.Mathematics;
using UnityEngine;

namespace Effects
{
    public class GrenadeProjectile : MonoBehaviour
    {
        public static event EventHandler OnAnyGrenadeExploded;

        [SerializeField] private Transform grenadeExplosionEffect;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private AnimationCurve arcYAnimationCurve;
        [SerializeField] private float moveSpeed = 15f;
        [SerializeField] private float damageRadius = 4f;
        [SerializeField] private int damage = 30;

        private Vector3 _targetPosition;
        private Action _onGranadeBehaviourComplete;
        private float _totalDistance;
        private Vector3 positionXZ;

        private void Update()
        {
            var moveDir = (_targetPosition - positionXZ).normalized;

            positionXZ += moveDir * (moveSpeed * Time.deltaTime);

            var distance = Vector3.Distance(positionXZ, _targetPosition);
            var distanceNormalized =  1 - distance / _totalDistance;

            var maxHeight = _totalDistance / 4f;
            var positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;

            transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

            if (Vector3.Distance(positionXZ, _targetPosition) < .2f)
            {
                var colliderArray = Physics.OverlapSphere(_targetPosition, damageRadius);

                foreach (var collider in colliderArray)
                {
                    if (collider.TryGetComponent(out Unit targetUnit))
                    {
                        targetUnit.Damage(damage);
                    }
                    
                    if (collider.TryGetComponent(out DestructibleCrate destructibleCrate))
                    {
                        destructibleCrate.Damage();
                    }
                }
            
                OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            
                Destroy(gameObject);

                trailRenderer.transform.parent = null;
                Instantiate(grenadeExplosionEffect, _targetPosition + Vector3.up * 1f, quaternion.identity);

                _onGranadeBehaviourComplete();
            }
        }

        public void Setup(GridPosition targetGridPosition, Action onGranadeBehaviourComplete)
        {
            _onGranadeBehaviourComplete = onGranadeBehaviourComplete;
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

            positionXZ = transform.position;
            positionXZ.y = 0;
        
            _totalDistance = Vector3.Distance(transform.position, _targetPosition);
        }
    }
}
