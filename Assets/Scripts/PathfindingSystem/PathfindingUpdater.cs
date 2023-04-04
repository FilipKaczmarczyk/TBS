using System;
using Obstacle;
using UnityEngine;

namespace PathfindingSystem
{
    public class PathfindingUpdater : MonoBehaviour
    {
        private void Start()
        {
            DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
        }

        private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
        {
            var destructibleCrate = sender as DestructibleCrate;

            Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
        }
    }
}
