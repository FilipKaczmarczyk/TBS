using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public event EventHandler OnTurnNumberChanged;
    public static TurnSystem Instance { get; private set; }

    private int _turnNumber = 1;

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
        
        OnTurnNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnNumber()
    {
        return _turnNumber;
    }
}
