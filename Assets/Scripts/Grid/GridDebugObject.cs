using System;
using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro gridObjectPositionText;
        private GridObject _gridObject;
        
        public void SetGridObject(GridObject gridObject)
        {
            _gridObject = gridObject;
        }

        private void Update()
        {
            SetPositionText();
        }

        public void SetPositionText()
        {
            gridObjectPositionText.SetText(_gridObject.ToString());
        }
    }
}
