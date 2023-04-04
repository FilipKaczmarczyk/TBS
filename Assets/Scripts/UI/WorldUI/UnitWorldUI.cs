using System;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI.WorldUI
{
    public class UnitWorldUI : MonoBehaviour
    {
        [Header("Action Points")]
        [SerializeField] private Unit unit;
        [SerializeField] private Transform unitWorldUIActionPointsContainer;
        [SerializeField] private Transform unitWorldUIActionPointPrefab;
        
        [Header("Health Bar")]
        [SerializeField] private Image healthBarImage;
        [SerializeField] private HealthSystem healthSystem;

        private readonly List<UnitWorldUIActionPoint> _unitWorldUIActionPoints = new();
        private int _currentActionPoints;

        private void Start()
        {
            CreateActionPoints();

            UpdateHealthBar();

            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
        }

        private void OnDestroy()
        {
            Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
            healthSystem.OnDamaged -= HealthSystem_OnDamaged;
        }

        private void CreateActionPoints()
        {
            var actionPoints = unit.GetActionPoints();

            for (var i = 0; i < actionPoints; i++)
            {
                var unitWorldUIActionPointTransform = Instantiate(unitWorldUIActionPointPrefab, unitWorldUIActionPointsContainer);
                
                var unitWorldUIActionPoint = unitWorldUIActionPointTransform.GetComponent<UnitWorldUIActionPoint>();
                
                _unitWorldUIActionPoints.Add(unitWorldUIActionPoint);

                _currentActionPoints++;
            }
        }
        
        private void UpdateActionPoints()
        {
            var newActionPoints = unit.GetActionPoints();
            
            if (newActionPoints >= _currentActionPoints)
            {
                for (var i = 0; i < newActionPoints; i++)
                {
                    _unitWorldUIActionPoints[i].FillImage();
                }
            }
            else
            {
                for (var i = _unitWorldUIActionPoints.Count - 1; i >= newActionPoints; i--)
                {
                    _unitWorldUIActionPoints[i].EmptyImage();
                }
            }

            _currentActionPoints = newActionPoints;
        }

        private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }
        
        private void UpdateHealthBar()
        {
            healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
        }

        private void HealthSystem_OnDamaged(object sender, EventArgs e)
        {
            UpdateHealthBar();
        }
    }
}
