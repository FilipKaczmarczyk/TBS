using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float stoppingDistance = 0.05f;
    
    private Vector3 _targetPosition;

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) < stoppingDistance)
        {
            transform.position = _targetPosition;
        }
        else
        {
            var moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector3(4, 0, 4));
        }
    }

    private void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
