using Grid;
using TMPro;
using UnityEngine;

namespace PathfindingSystem
{
    public class PathfindingGridDebugObject : GridDebugObject
    {
        [SerializeField] private TextMeshPro gCostText;
        [SerializeField] private TextMeshPro hCostText;
        [SerializeField] private TextMeshPro fCostText;
        [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;

        private PathNode _pathNode;
    
        public override void SetGridObject(object gridObject)
        {
            _pathNode = (PathNode) gridObject;
        
            base.SetGridObject(gridObject);
        }

        protected override void Update()
        {
            base.Update();

            gCostText.text = _pathNode.GetGCost().ToString();
            hCostText.text = _pathNode.GetHCost().ToString();
            fCostText.text = _pathNode.GetFCost().ToString();

            isWalkableSpriteRenderer.color = _pathNode.IsWalkable() ? Color.green : Color.red;
        }
    }
}