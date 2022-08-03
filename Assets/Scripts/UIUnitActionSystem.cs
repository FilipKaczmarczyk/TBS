using System;
using UnityEngine;

public class UIUnitActionSystem : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonsContainer;
    
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnityActionSystem_OnSelectedUnitChange;
        
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransforms in actionButtonsContainer)
        {
            Destroy(buttonTransforms.gameObject);
        }
        
        var unit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (var baseAction in unit.GetBaseActions())
        {
            var actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonsContainer);
            var actionButton = actionButtonTransform.GetComponent<UIActionButton>();
            actionButton.SetBaseAction(baseAction);
        }
    }
    
    private void UnityActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        CreateUnitActionButtons();
    }
}
