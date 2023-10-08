using System.Collections.Generic;
using UnityEngine;

namespace PiggyFence
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