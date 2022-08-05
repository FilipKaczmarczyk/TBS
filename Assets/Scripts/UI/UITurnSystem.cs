using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITurnSystem : MonoBehaviour
    {
        [SerializeField] private Button endTurnButton;
        [SerializeField] private TextMeshProUGUI turnNumberText;

        private void Start()
        {
            TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;
            
            endTurnButton.onClick.AddListener(() =>
            {
                TurnSystem.Instance.NextTurn();
            });

            UpdateTurnNumberText();
        }
        
        private void TurnSystem_OnTurnNumberChanged(object sender, EventArgs empty)
        {
            UpdateTurnNumberText();
        }
        
        private void UpdateTurnNumberText()
        {
            turnNumberText.text = "TURN: " + TurnSystem.Instance.GetTurnNumber();
        }
    }
}
