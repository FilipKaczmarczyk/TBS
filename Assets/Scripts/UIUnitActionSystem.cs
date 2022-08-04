using System;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitActionSystem : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonsContainer;

    private readonly List<UIActionButton> _uiActionButtons = new ();
    
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnityActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnityActionSystem_OnSelectedActionChange;
        
        CreateUnitActionButtons();
        UpdateSelectedVisual();
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
    
    private void UnityActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void UnityActionSystem_OnSelectedActionChange(object sender, EventArgs empty)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach (var actionButton in _uiActionButtons)
        {
            actionButton.UpdateSelectedVisual();
        }
    }
}
