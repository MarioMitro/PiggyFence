using UnityEngine;

using System.Collections.Generic;

namespace PiggyFence.Fence
{
    [CreateAssetMenu(fileName = "FenceInfo", menuName = "FenceInfo")]
    public class FenceInfo : ScriptableObject
    {
        public GameObject fencePiecePrefab;
        public List<Vector2Int> fenceCellCoordinates { get; set; }

        private void Awake()
        {
            fenceCellCoordinates = new List<Vector2Int>();
        }

        private void OnDestroy()
        {
            fencePiecePrefab = null;

            fenceCellCoordinates.Clear();
            fenceCellCoordinates = null;
        }

        public bool IsCellInTheFence(Vector2Int gridCell)
        {
            bool left = false, right = false, up = false, down = false;

            foreach (var cell in fenceCellCoordinates)
            {
                if (cell.y == gridCell.y)
                {
                    if (!right && cell.x >= gridCell.x)
                        right = true;

                    if (!left && cell.x <= gridCell.x)
                        left = true;
                }

                if (cell.x == gridCell.x)
                {
                    if (!up && cell.y >= gridCell.y)
                        up = true;

                    if (!down && cell.y <= gridCell.y)
                        down = true;
                }
            }

            return down && up && left && right;
        }
    }
}
