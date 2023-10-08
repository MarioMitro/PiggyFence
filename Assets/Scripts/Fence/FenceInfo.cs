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
            ConnectShortenedEnds(fence);
            RotateInclinedPieces(fence);
            ConnectHardEdges(fence);
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

        private void ConnectShortenedEnds(Dictionary<Vector2Int, Transform> fence)
        {
            foreach (var fenceP in fence.Keys)
            {
                var downCell = new Vector2Int(fenceP.x + 1, fenceP.y);
                var upCell = new Vector2Int(fenceP.x - 1, fenceP.y);
                var rightCell = new Vector2Int(fenceP.x, fenceP.y + 1);
                var leftCell = new Vector2Int(fenceP.x, fenceP.y - 1);

                if (fence.ContainsKey(downCell) && fence[downCell].localScale.x == 0.5f)
                {
                    if (fence[fenceP].localScale.x == 1f)
                        fence[downCell].localPosition = Vector3.right * -0.25f;
                    else
                    {
                        fence[downCell].localPosition = Vector3.right * -0.25f;
                        fence[fenceP].localPosition = Vector3.right * 0.25f;
                    }
                }

                if (fence.ContainsKey(upCell) && fence[upCell].localScale.x == 0.5f)
                {
                    if (fence[fenceP].localScale.x == 1f)
                        fence[upCell].localPosition = Vector3.right * 0.25f;
                    else
                    {
                        fence[upCell].localPosition = Vector3.right * 0.25f;
                        fence[fenceP].localPosition = Vector3.right * -0.25f;
                    }
                }

                if (fence.ContainsKey(rightCell) && fence[rightCell].localScale.x == 0.5f)
                {
                    if (fence[fenceP].localScale.x == 1f)
                        fence[rightCell].localPosition = Vector3.forward * -0.25f;
                    else
                    {
                        fence[rightCell].localPosition = Vector3.forward * -0.25f;
                        fence[fenceP].localPosition = Vector3.forward * 0.25f;
                    }
                }

                if (fence.ContainsKey(leftCell) && fence[leftCell].localScale.x == 0.5f)
                {
                    if (fence[fenceP].localScale.x == 1f)
                        fence[leftCell].localPosition = Vector3.forward * 0.25f;
                    else
                    {
                        fence[leftCell].localPosition = Vector3.forward * 0.25f;
                        fence[fenceP].localPosition = Vector3.forward * -0.25f;
                    }
                }
            }
        }

        private void RotateInclinedPieces(Dictionary<Vector2Int, Transform> fence)
        {
            foreach (var fenceP in fence.Keys)
            {
                var downRightCell = new Vector2Int(fenceP.x + 1, fenceP.y + 1);
                var downLeftCell = new Vector2Int(fenceP.x + 1, fenceP.y - 1);
                var upLeftCell = new Vector2Int(fenceP.x - 1, fenceP.y - 1);
                var upRightCell = new Vector2Int(fenceP.x - 1, fenceP.y + 1);

                if (fence.ContainsKey(downRightCell) && fence.ContainsKey(upLeftCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, -45, 0);
                    fence[fenceP].localScale = new Vector3(2.5f, 1, 1f);
                    continue;
                }
                else if (fence.ContainsKey(downLeftCell) && fence.ContainsKey(upRightCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, 45, 0);
                    fence[fenceP].localScale = new Vector3(2.5f, 1, 1f);
                    continue;
                }

                if (fence.ContainsKey(upRightCell) && fence.ContainsKey(upLeftCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, -45, 0);
                    fence[fenceP].localPosition = new Vector3(-0.5f, 0, 0.5f);
                    fence[fenceP].localScale = new Vector3(1.25f, 1, 1f);
                    continue;
                }
                else if (fence.ContainsKey(downLeftCell) && fence.ContainsKey(downRightCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, 45, 0);
                    fence[fenceP].localPosition = new Vector3(0.5f, 0, -0.5f);
                    fence[fenceP].localScale = new Vector3(1.25f, 1, 1f);
                    continue;
                }

                if (fence.ContainsKey(downRightCell) && fence.ContainsKey(upRightCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, 45, 0);
                    fence[fenceP].localPosition = new Vector3(0.5f, 0, 0.5f);
                    fence[fenceP].localScale = new Vector3(1.25f, 1, 1f);
                    continue;
                }
                else if (fence.ContainsKey(downLeftCell) && fence.ContainsKey(upLeftCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, -45, 0);
                    fence[fenceP].localPosition = new Vector3(-0.5f, 0, -0.5f);
                    fence[fenceP].localScale = new Vector3(1.25f, 1, 1f);
                    continue;
                }
            }
        }

        private void ConnectHardEdges(Dictionary<Vector2Int, Transform> fence)
        {
            foreach (var fenceP in fence.Keys)
            {
                var downRighCell = new Vector2Int(fenceP.x + 1, fenceP.y + 1);
                var leftCell = new Vector2Int(fenceP.x, fenceP.y - 1);

                if (fence.ContainsKey(downRighCell))
                {
                    if (Mathf.Abs(fence[downRighCell].eulerAngles.y - fence[fenceP].eulerAngles.y) == 90f)
                    {
                        fence[fenceP].localPosition = (fence.ContainsKey(leftCell) ? Vector3.right * 0.25f : Vector3.forward * 0.25f);
                        fence[fenceP].eulerAngles = new Vector3(0, -45, 0);
                        fence[fenceP].localScale = new Vector3(1.05f, 1, 1f);

                        fence[downRighCell].localPosition = (fence.ContainsKey(leftCell) ? Vector3.forward * -0.25f : Vector3.right * -0.25f);
                        fence[downRighCell].eulerAngles = new Vector3(0, -45, 0);
                        fence[downRighCell].localScale = new Vector3(1.05f, 1, 1f);

                    }
                }
            }
        }
    }
}
