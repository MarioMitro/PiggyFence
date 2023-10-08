using UnityEngine;

using PiggyFence.UI;
using PiggyFence.Fence;

using System.Collections.Generic;

namespace PiggyFence.Managers
{
    // Class is responsible for creating grid, generating objects on it and providing information about objects on the grid such as their position on the grid.
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private GameObject groundPrefab;
        [Space]
        [SerializeField] private FenceInfo fenceInfo;
        [Space]
        [SerializeField] private UIData uiData;

        private Dictionary<Vector2Int, Transform> grid;
        public Dictionary<Vector2Int, Transform> fence;

        private int gridSizePerSide;

        private void Start()
        {
            gridSizePerSide = (int)groundPrefab.transform.localScale.x;
            uiData.gridSize = gridSizePerSide;
            grid = new Dictionary<Vector2Int, Transform>(gridSizePerSide * gridSizePerSide);

            groundPrefab.transform.position = new Vector3(gridSizePerSide / 2f, -1, gridSizePerSide / 2f);

            CreateGrid();
            BuildFence();
        }

        private void OnDestroy()
        {
            groundPrefab = null;

            grid.Clear();
            grid = null;

            fence.Clear();
            fence = null;

            fenceInfo = null;
        }

        public Vector3 GetMousePosition(Vector3 mousePosition)
        {
            var gridCell = new Vector2Int((int)mousePosition.x, (int)mousePosition.z);

            if (grid.ContainsKey(gridCell))
                return grid[gridCell].position;

            return Vector3.one;
        }

        public bool IsCellInTheFence(Vector3 mousePosition) => fenceInfo.IsCellInTheFence(new Vector2Int((int)mousePosition.x, (int)mousePosition.z));

        private void CreateGrid()
        {
            for (int x = 0; x < gridSizePerSide; x++) // Create a grid of cells
            {
                for (int z = 0; z < gridSizePerSide; z++)
                {
                    Transform newCellTrans = new GameObject($"Cell [{x} | {z}]").transform;
                    newCellTrans.parent = transform;
                    newCellTrans.position = new Vector3((x + 0.5F), 0, (z + 0.5F));

                    grid.Add(new Vector2Int(x, z), newCellTrans);
                }
            }
        }

        private void BuildFence()
        {
            fence = new Dictionary<Vector2Int, Transform>();

            foreach (var cell in fenceInfo.fenceCellCoordinates)
            {
                if (grid.ContainsKey(cell))
                {
                    var griddCell = grid[cell];
                    var fencePiece = Instantiate(fenceInfo.fencePiecePrefab, griddCell.position, Quaternion.identity, griddCell);
                    fence.Add(cell, fencePiece.transform);

                    fencePiece.GetComponentInChildren<MeshRenderer>().material = fenceInfo.GetFenceMaterial();
                }
                else
                    Debug.LogError($"Grid does not contain cell with coordinates X:{cell.x} Y:{cell.y}");
            }

            fenceInfo.AlignFence(fence);
            uiData.fenceLength = fenceInfo.GetFenceLength(fence);
        }

        private void OnDrawGizmosSelected() // Grid visualizer
        {
            if (grid == null)
                return;

            foreach (var cellData in grid)
            {
                Vector2 cellPositionGS = cellData.Key;
                Vector3 center = new Vector3((cellPositionGS.x + 0.5F), 2 * 0.5F, (cellPositionGS.y + 0.5F));

                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(center, Vector3.one);
            }
        }
    }
}
