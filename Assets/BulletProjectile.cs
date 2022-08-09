using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVfxPrefab;
         
    private Vector3 _targetPosition;
    
    public void Setup(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        var moveDir = (_targetPosition - transform.position).normalized;

        var distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);
        
        var moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        
        var distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = _targetPosition;
            
            trailRenderer.transform.parent = null;

            Instantiate(bulletHitVfxPrefab, _targetPosition, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
}

