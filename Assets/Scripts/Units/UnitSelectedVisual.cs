using System;
using UnityEngine;

namespace Units
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit unit;

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            UnitActionSystem.Instance.OnSelectedUnitChange += UnityActionSystem_OnSelectedUnitChange;
        }

        private void UnityActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            _meshRenderer.enabled = UnitActionSystem.Instance.GetSelectedUnit() == unit;
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedUnitChange -= UnityActionSystem_OnSelectedUnitChange;
        }
    }
}
