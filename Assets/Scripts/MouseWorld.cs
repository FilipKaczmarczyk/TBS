using System;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask mousePlaneLayerMask;

    private static MouseWorld _instance;

    private void Awake()
    {
        _instance = this;
    }
    
    public static Vector3 GetPosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out var raycastHit, float.MaxValue, _instance.mousePlaneLayerMask);

        return raycastHit.point;
    }
}
