using System;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitActionSystem : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonsContainer;
    [SerializeField] private Transform actionPointPrefab;
    [SerializeField] private Transform actionPointsContainer;
    
    private readonly List<UIActionButton> _uiActionButtons = new ();
    
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnityActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnityActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted += UnityActionSystem_OnActionStarted;
        
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        CreateActionPoints();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransforms in actionButtonsContainer)
        {
            Destroy(buttonTransforms.gameObject);
        }
        
        _uiActionButtons.Clear();
        
        var unit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (var baseAction in unit.GetBaseActions())
        {
            var actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonsContainer);
            var actionButton = actionButtonTransform.GetComponent<UIActionButton>();
            actionButton.SetBaseAction(baseAction);

            _uiActionButtons.Add(actionButton);
        }
    }

    private void CreateActionPoints()
    {
        foreach (Transform actionPointTransform in actionPointsContainer)
        {
            Destroy(actionPointTransform.gameObject);
        }
        
        var unit = UnitActionSystem.Instance.GetSelectedUnit();
        
        var points = unit.GetActionPoints();

        for (var i = 0; i < points; i++)
        {
            Instantiate(actionPointPrefab, actionPointsContainer);
        }
    }
    
    private void UnityActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        CreateActionPoints();
    }

    private void UnityActionSystem_OnSelectedActionChange(object sender, EventArgs empty)
    {
        UpdateSelectedVisual();
    }

    private void UnityActionSystem_OnActionStarted(object sender, EventArgs empty)
    {
        CreateActionPoints();
    }

    private void UpdateSelectedVisual()
    {
        foreach (var actionButton in _uiActionButtons)
        {
            actionButton.UpdateSelectedVisual();
        }
    }
}
