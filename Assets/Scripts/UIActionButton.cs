using Actions;
using UnityEngine;
using UnityEngine.UI;

public class UIActionButton : MonoBehaviour
{
    [SerializeField] private Image actionImage;
    [SerializeField] private Button actionButton;

    public void SetBaseAction(BaseAction baseAction)
    {
        actionImage.sprite = baseAction.GetActionImage();
        
        actionButton.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }
    
}
