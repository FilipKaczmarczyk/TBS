using DG.Tweening;
using Grid;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private float rotateTime = 0.5f;

    [SerializeField] private Animator unitAnimator;
    
    private Vector3 _targetPosition;

    private GridPosition _gridPosition;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
    }

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
        
        var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        
        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(_gridPosition, newGridPosition, this);
            _gridPosition = newGridPosition;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        
        transform.DOLookAt(_targetPosition, rotateTime);
    }
}
