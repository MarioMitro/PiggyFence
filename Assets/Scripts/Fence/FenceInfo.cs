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

        public void AlignFence(Dictionary<Vector2Int, Transform> fence)
        {
            TurnHorizontalPieces(fence);
        }

        private void TurnHorizontalPieces(Dictionary<Vector2Int, Transform> fence)
        {
            var horizontalTurn = new Vector3(0, 90, 0);
            var shorterFenceScale = new Vector3(0.5f, 1f, 1f);

            foreach (var fenceP in fence.Keys)
            {
                var rightCell = new Vector2Int(fenceP.x, fenceP.y + 1);
                var downRightCell = new Vector2Int(fenceP.x + 1, fenceP.y + 1);
                var upRightCell = new Vector2Int(fenceP.x - 1, fenceP.y + 1);

                if (fence.ContainsKey(rightCell))
                {
                    fence[rightCell].eulerAngles = horizontalTurn;
                    fence[fenceP].eulerAngles = horizontalTurn;
                    continue;
                }

                if (fence.ContainsKey(downRightCell))
                {
                    fence[downRightCell].localScale = shorterFenceScale;
                    fence[fenceP].localScale = shorterFenceScale;
                    continue;
                }

                if (fence.ContainsKey(upRightCell))
                {
                    fence[fenceP].localScale = shorterFenceScale;
                    fence[upRightCell].localScale = shorterFenceScale;
                    continue;
                }
            }
        }
    }
}
