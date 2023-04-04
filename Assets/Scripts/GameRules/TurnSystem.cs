using System;
using UnityEngine;

namespace GameRules
{
    public class TurnSystem : MonoBehaviour
    {
        public event EventHandler OnTurnNumberChanged;
        public static TurnSystem Instance { get; private set; }

        private int _turnNumber = 1;
        private bool _isPlayerTurn = true;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more than one TurnSystem " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
        
            Instance = this;
        }
    
        public void NextTurn()
        {
            _turnNumber++;

            _isPlayerTurn = !_isPlayerTurn;
        
            OnTurnNumberChanged?.Invoke(this, EventArgs.Empty);
        }

        public int GetTurnNumber() => _turnNumber;

        public bool IsPlayerTurn() => _isPlayerTurn;
    }
}
