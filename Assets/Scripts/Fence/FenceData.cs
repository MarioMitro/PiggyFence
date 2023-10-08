using UnityEngine;

using System.Collections.Generic;

namespace PiggyFence.Fence
{
    // Class for Firestore data storing/loading.
    public class FenceData
    {
        public List<Vector2Int> cells;

        public FenceData(List<Vector2Int> cells)
        {
            this.cells = cells;
        }
    }
}