using System;
using Grid;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private int damage = 30;
    
    private Vector3 _targetPosition;
    private Action _onGranadeBehaviourComplete;

    private void Update()
    {
        var moveDir = (_targetPosition - transform.position).normalized;

        transform.position += moveDir * (moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < .2f)
        {
            var colliderArray = Physics.OverlapSphere(_targetPosition, damageRadius);

            foreach (var collider in colliderArray)
            {
                if (collider.TryGetComponent(out Unit targetUnit))
                {
                    targetUnit.Damage(damage);
                }
            }
            
            Destroy(gameObject);

            _onGranadeBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGranadeBehaviourComplete)
    {
        _onGranadeBehaviourComplete = onGranadeBehaviourComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }
}
