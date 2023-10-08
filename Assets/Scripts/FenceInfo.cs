using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiggyFence
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
                    if (cell.x >= gridCell.x && !right)
                        right = true;

                    if (cell.x <= gridCell.x && !left)
                        left = true;
                }

                if (cell.x == gridCell.x)
                {
                    if (cell.y >= gridCell.y && !up)
                        up = true;

                    if (cell.y <= gridCell.y && !down)
                        down = true;
                }
            }

            return down && up && left && right;
        }
    }
}
