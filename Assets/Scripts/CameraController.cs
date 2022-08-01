using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float zoomSpeed = 5f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachineTransposer _cinemachineTransposer;

    private const float MINFollowYOffset = 2f;
    private const float MAXFollowYOffset = 12f;

    private Vector3 _targetFollowOffset;

    private void Awake()
    {
        _cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        var inputMoveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = 1;
        }
        
        var moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
    
    private void HandleRotation()
    {
        var rotationVector = Vector3.zero;
        
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1;
        }

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }
    
    private void HandleZoom()
    {
        const float zoomAmount = 1f;
        
        if (Input.mouseScrollDelta.y > 0)
        {
            _targetFollowOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            _targetFollowOffset.y += zoomAmount;
        }

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MINFollowYOffset, MAXFollowYOffset);

        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset,
            Time.deltaTime * zoomSpeed);
    }
}
