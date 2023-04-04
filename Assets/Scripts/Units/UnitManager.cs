using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class UnitManager : MonoBehaviour
    {
        private readonly List<Unit> _unitList = new List<Unit>();
        private readonly List<Unit> _playerUnitList = new List<Unit>();
        private readonly List<Unit> _enemyUnitList = new List<Unit>();
        public static UnitManager Instance { get; private set; }

        public List<Unit> UnitList => _unitList;
        public List<Unit> PlayerUnitList => _playerUnitList;
        public List<Unit> EnemyUnitList => _enemyUnitList;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more than one UnitManager " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
        
            Instance = this;
        }
    
        private void Start()
        {
            Unit.OnAnyUnitSpawn += Unit_OnAnyUnitSpawn;
            Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        }

        private void Unit_OnAnyUnitSpawn(object sender, EventArgs e)
        {
            var unit = sender as Unit;
        
            if (unit.IsEnemy())
            {
                EnemyUnitList.Add(unit);
            }
            else
            {
                PlayerUnitList.Add(unit);
            }
        
            UnitList.Add(unit);
        }

        private void Unit_OnAnyUnitDead(object sender, EventArgs e)
        {
            var unit = sender as Unit;
        
            if (unit.IsEnemy())
            {
                EnemyUnitList.Remove(unit);
            }
            else
            {
                PlayerUnitList.Remove(unit);
            }
        
            UnitList.Remove(unit);
        }
    }
}

