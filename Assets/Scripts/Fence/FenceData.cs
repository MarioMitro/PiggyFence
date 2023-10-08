using UnityEngine;

using System.Collections.Generic;

namespace PiggyFence.Fence
{
    public class FenceData
    {
        public List<Vector2Int> cells;

        public FenceData(List<Vector2Int> cells)
        {
            this.cells = cells;
        }
    }
}