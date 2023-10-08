using System;

using UnityEngine;

using System.Collections.Generic;

namespace PiggyFence.Fence
{
    [Serializable]
    public class FenceStyle
    {
        public float chance;
        public Material fenceMaterial;

        public FenceStyle(float chance, Material fenceMaterial)
        {
            this.chance = chance;
            this.fenceMaterial = fenceMaterial;
        }
    }

    [CreateAssetMenu(fileName = "FenceInfo", menuName = "FenceInfo")]
    public class FenceInfo : ScriptableObject
    {
        public GameObject fencePiecePrefab;
        public FenceStyle[] fenceColorPalet;
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

        public Material GetFenceMaterial()
        {
            var chance = UnityEngine.Random.value;

            for (int i = fenceColorPalet.Length - 1; i >= 0; i--)
            {
                if (chance > fenceColorPalet[i].chance)
                    return fenceColorPalet[i].fenceMaterial;
            }

            return fenceColorPalet[0].fenceMaterial;
        }

        /// <summary>
        /// All fence piecies are rotated horizontaly, firstly we find and rotate piecies that are supose to be verticaly.
        /// Then we shorten all horizontal or vertical end pieces to make incline piecies fit better 
        /// </summary>
        private void TurnHorizontalPieces(Dictionary<Vector2Int, Transform> fence)
        {
            var horizontalTurn = new Vector3(0, 90, 0);
            var shorterFenceScale = new Vector3(0.5f, 1f, 1f);

            foreach (var fenceP in fence.Keys)
            {
                var rightCell = new Vector2Int(fenceP.x, fenceP.y + 1);
                var downRightCell = new Vector2Int(fenceP.x + 1, fenceP.y + 1);
                var upRightCell = new Vector2Int(fenceP.x - 1, fenceP.y + 1);

                // || --> --
                if (fence.ContainsKey(rightCell))
                {
                    fence[rightCell].eulerAngles = horizontalTurn;
                    fence[fenceP].eulerAngles = horizontalTurn;
                    continue;
                }

                // if piece have neighbor on the righ up or right down --> shorten both pieces 
                if (fence.ContainsKey(downRightCell))
                {
                    fence[downRightCell].localScale = shorterFenceScale;
                    fence[fenceP].localScale = shorterFenceScale;
                    continue;
                }
                else if (fence.ContainsKey(upRightCell))
                {
                    fence[fenceP].localScale = shorterFenceScale;
                    fence[upRightCell].localScale = shorterFenceScale;
                    continue;
                }
            }
        }

        /// <summary>
        /// After shortening end pieces there are gabs between pieces. We must tighten that gap.
        /// </summary>
        private void ConnectShortenedEnds(Dictionary<Vector2Int, Transform> fence)
        {
            foreach (var fenceP in fence.Keys)
            {
                var downCell = new Vector2Int(fenceP.x + 1, fenceP.y);
                var upCell = new Vector2Int(fenceP.x - 1, fenceP.y);
                var rightCell = new Vector2Int(fenceP.x, fenceP.y + 1);
                var leftCell = new Vector2Int(fenceP.x, fenceP.y - 1);

                // if your down neighbor is shortened
                if (fence.ContainsKey(downCell) && fence[downCell].localScale.x == 0.5f)
                {
                    // if you are also shortened --> move to neighbor
                    if (fence[fenceP].localScale.x == 0.5f)
                        fence[fenceP].localPosition = Vector3.right * 0.25f;

                    fence[downCell].localPosition = Vector3.right * -0.25f;  // move neighbor to you
                }

                // if your up neighbor is shortened
                if (fence.ContainsKey(upCell) && fence[upCell].localScale.x == 0.5f)
                {
                    if (fence[fenceP].localScale.x == 0.5f)
                        fence[fenceP].localPosition = Vector3.right * -0.25f;

                    fence[upCell].localPosition = Vector3.right * 0.25f;
                }

                // if your right neighbor is shortened
                if (fence.ContainsKey(rightCell) && fence[rightCell].localScale.x == 0.5f)
                {
                    if (fence[fenceP].localScale.x == 0.5f)
                        fence[fenceP].localPosition = Vector3.forward * 0.25f;

                    fence[rightCell].localPosition = Vector3.forward * -0.25f;
                }

                // if your left neighbor is shortened
                if (fence.ContainsKey(leftCell) && fence[leftCell].localScale.x == 0.5f)
                {
                    if (fence[fenceP].localScale.x == 0.5f)
                        fence[fenceP].localPosition = Vector3.forward * -0.25f;

                    fence[leftCell].localPosition = Vector3.forward * 0.25f;
                }
            }
        }

        /// <summary>
        /// We need to find and rotate all incline piecies + fit them to right size.
        /// We are examinig righ and left neighbor of all pieceies to fit them correctlty.
        /// </summary>
        private void RotateInclinedPieces(Dictionary<Vector2Int, Transform> fence)
        {
            foreach (var fenceP in fence.Keys)
            {
                var downRightCell = new Vector2Int(fenceP.x + 1, fenceP.y + 1);
                var downLeftCell = new Vector2Int(fenceP.x + 1, fenceP.y - 1);
                var upLeftCell = new Vector2Int(fenceP.x - 1, fenceP.y - 1);
                var upRightCell = new Vector2Int(fenceP.x - 1, fenceP.y + 1);

                // -         -
                //  -   -->   \
                //   -         -
                if (fence.ContainsKey(downRightCell) && fence.ContainsKey(upLeftCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, -45, 0);
                    fence[fenceP].localScale = new Vector3(2.5f, 1, 1f);
                    continue;
                }
                //   -         -
                //  -   -->   /
                // -         -
                else if (fence.ContainsKey(downLeftCell) && fence.ContainsKey(upRightCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, 45, 0);
                    fence[fenceP].localScale = new Vector3(2.5f, 1, 1f);
                    continue;
                }

                // - -        - -
                //  -   -->    \
                if (fence.ContainsKey(upRightCell) && fence.ContainsKey(upLeftCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, -45, 0);
                    fence[fenceP].localPosition = new Vector3(-0.5f, 0, 0.5f);
                    fence[fenceP].localScale = new Vector3(1.25f, 1, 1f);
                    continue;
                }
                //  -          /
                // - -   -->  - -
                else if (fence.ContainsKey(downLeftCell) && fence.ContainsKey(downRightCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, 45, 0);
                    fence[fenceP].localPosition = new Vector3(0.5f, 0, -0.5f);
                    fence[fenceP].localScale = new Vector3(1.25f, 1, 1f);
                    continue;
                }

                //   -        -
                // -   -->   /
                //   -        -
                if (fence.ContainsKey(downRightCell) && fence.ContainsKey(upRightCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, 45, 0);
                    fence[fenceP].localPosition = new Vector3(0.5f, 0, 0.5f);
                    fence[fenceP].localScale = new Vector3(1.25f, 1, 1f);
                    continue;
                }
                // -         -
                //  -   -->   \
                // -         -
                else if (fence.ContainsKey(downLeftCell) && fence.ContainsKey(upLeftCell))
                {
                    fence[fenceP].eulerAngles = new Vector3(0, -45, 0);
                    fence[fenceP].localPosition = new Vector3(-0.5f, 0, -0.5f);
                    fence[fenceP].localScale = new Vector3(1.25f, 1, 1f);
                    continue;
                }
            }
        }

        /// <summary>
        /// Lastly we need to take care of perpendicular connections.
        /// Turn them incline + fit them to right size and position.
        /// </summary>
        private void ConnectHardEdges(Dictionary<Vector2Int, Transform> fence)
        {
            foreach (var fenceP in fence.Keys)
            {
                var downRighCell = new Vector2Int(fenceP.x + 1, fenceP.y + 1);
                var leftCell = new Vector2Int(fenceP.x, fenceP.y - 1);

                // -         \
                //  |  -->    \
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
