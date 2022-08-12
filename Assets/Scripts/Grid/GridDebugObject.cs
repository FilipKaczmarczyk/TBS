using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro gridObjectPositionText;
        
        private object _gridObject;
        
        public virtual void SetGridObject(object gridObject)
        {
            _gridObject = gridObject;
        }

        protected virtual void Update()
        {
            SetPositionText();
        }

        private void SetPositionText()
        {
            gridObjectPositionText.SetText(_gridObject.ToString());
        }
    }
}
