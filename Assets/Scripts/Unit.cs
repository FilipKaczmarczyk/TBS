using DG.Tweening;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private float rotateTime = 0.5f;

    [SerializeField] private Animator unitAnimator;
    
    private Vector3 _targetPosition;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) < stoppingDistance)
        {
            transform.position = _targetPosition;
            unitAnimator.SetBool(IsWalking, false);
        }
        else
        {
            var moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            unitAnimator.SetBool(IsWalking, true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
            
            transform.DOLookAt(_targetPosition, rotateTime);
        }
    }

    private void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
