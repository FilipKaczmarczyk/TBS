using System;
using Actions;
using UnityEngine;
using UnityEngine.UI;

public class UIActionButton : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image actionImage;
    [SerializeField] private Button actionButton;
    [SerializeField] private GameObject actionBusyGameObject;
    
    private readonly Color _activeColor = Color.green;
    private readonly Color _unActiveColor = Color.white;

    private BaseAction _baseAction;

    private void OnEnable()
    {
        UnitActionSystem.Instance.OnBusyChange += UnityActionSystem_OnBusyChange;
    }
    
    private void OnDisable()
    {
        UnitActionSystem.Instance.OnBusyChange -= UnityActionSystem_OnBusyChange;
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        actionImage.sprite = baseAction.GetActionImage();
        _baseAction = baseAction;
        
        actionButton.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        if (_baseAction == UnitActionSystem.Instance.GetSelectedAction())
        {
            ActivateButton();
        }
        else
        {
            DeactivateButton();
        }
    }

    private void ActivateButton()
    {
        backgroundImage.color = _activeColor;
    }

    private void DeactivateButton()
    {
        backgroundImage.color = _unActiveColor;
    }

    private void UnityActionSystem_OnBusyChange(object sender, bool isBusy)
    {
        ToggleBusy(isBusy);
    }
    
    private void ToggleBusy(bool state)
    {
        actionBusyGameObject.SetActive(state);
    }
    
}
