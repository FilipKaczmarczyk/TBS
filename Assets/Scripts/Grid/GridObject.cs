using System.Collections.Generic;
using Units;

namespace Grid
{
    public class GridObject
    {
        private GridSystem<GridObject> _gridSystem;
        private GridPosition _gridPosition;
        private List<Unit> _units;
    
        public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
        {
            _gridSystem = gridSystem;
            _gridPosition = gridPosition;
            _units = new List<Unit>();
        }

        public override string ToString()
        {
            var unitsString = "";
            
            foreach (var unit in _units)
            {
                unitsString += unit + "\n";
            }
            
            return _gridPosition + "\n" + unitsString;
        }

        public List<Unit> GetUnits()
        {
            return _units;
        }

        public void RemoveUnit(Unit unit)
        {
            _units.Remove(unit);
        }

        public void AddUnit(Unit unit)
        {
            _units.Add(unit);
        }

        public bool CheckIsOccupied()
        {
            return _units.Count > 0;
        }

        public Unit GetUnit()
        {
            if (CheckIsOccupied())
            {
                return _units[0];
            }
            else
            {
                return null;
            }
        }
    }
}
