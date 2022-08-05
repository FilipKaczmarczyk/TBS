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
        [SerializeField] private GameObject opponentsTurnVisualGameObject;
        
        [SerializeField] private GameObject actionButtonsContainerGameObject;
        [SerializeField] private GameObject actionPointsContainerGameObject;
        [SerializeField] private GameObject endTurnButtonGameObject;
        

        private void Start()
        {
            TurnSystem.Instance.OnTurnNumberChanged += TurnSystem_OnTurnNumberChanged;
            
            endTurnButton.onClick.AddListener(() =>
            {
                TurnSystem.Instance.NextTurn();
            });

            UpdateTurnNumberText();
            UpdateOpponentsTurnVisual();
        }
        
        private void TurnSystem_OnTurnNumberChanged(object sender, EventArgs empty)
        {
            UpdateTurnNumberText();
            UpdateOpponentsTurnVisual();
        }
        
        private void UpdateTurnNumberText()
        {
            turnNumberText.text = "TURN: " + TurnSystem.Instance.GetTurnNumber();
        }

        private void UpdateOpponentsTurnVisual()
        {
            opponentsTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
            
            actionButtonsContainerGameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
            actionPointsContainerGameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
            endTurnButtonGameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());            
        }
    }
}
