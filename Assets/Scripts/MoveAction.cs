using System.Collections.Generic;
using DG.Tweening;
using Grid;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private int maxMoveDistance = 4;

    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float stoppingDistance = 0.05f;
    [SerializeField] private float rotateTime = 0.5f;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    
    private Vector3 _targetPosition;
    private Unit _unit;
    
    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _targetPosition = transform.position;
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
    }
    
    public void Move(GridPosition targetPosition)
    {
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        
        transform.DOLookAt(_targetPosition, rotateTime);
    }

    public bool CheckIsValidPositionToMove(GridPosition gridPosition)
    {
        var validGridPositions = GetValidActionGridPositionList();

        return validGridPositions.Contains(gridPosition);
    }
    
    public List<GridPosition> GetValidActionGridPositionList()
    {
        var validGridPositions = new List<GridPosition>();

        var unitGridPosition = _unit.GetGridPosition();
        
        for (var x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (var z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                var offsetGridPosition = new GridPosition(x, z);
                var validGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.CheckValidGridPosition(validGridPosition))
                    continue;
                
                if (validGridPosition == unitGridPosition)
                    continue;
                
                if (LevelGrid.Instance.CheckIsUnitAtPosition(validGridPosition))
                    continue;
                
                validGridPositions.Add(validGridPosition);
            }
        }
        
        return validGridPositions;
    }
}
