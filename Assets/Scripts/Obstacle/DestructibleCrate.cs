using System;
using Grid;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obstacle
{
    public class DestructibleCrate : MonoBehaviour
    {
        public static event EventHandler OnAnyDestroyed;

        [SerializeField] private Transform destroyedCratePrefab;
        
        private GridPosition _gridPosition;

        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        }

        public GridPosition GetGridPosition()
        {
            return _gridPosition;
        }
        
        public void Damage()
        {
            var crateDestroyedTransform = Instantiate(destroyedCratePrefab, transform.position, transform.rotation);
            
            var randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            ApplyExplosionToChildren(crateDestroyedTransform, 200f, transform.position + randomDir, 10f);
            
            Destroy(gameObject);
            
            OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
        }
        
        private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRadius)
        {
            foreach (Transform child in root)
            {
                if (child.TryGetComponent<Rigidbody>(out var childRigidbody))
                {
                    childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                }
            
                ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRadius);
            }
        }
    }
}
