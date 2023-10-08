using UnityEngine;

using PiggyFence.Fence;

using System.Collections.Generic;


namespace PiggyFence.Managers
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private GameObject ground;
        [SerializeField] private FenceInfo fenceInfo;

        private int gridSizePerSide;
        private Dictionary<Vector2Int, Transform> grid;
        public Dictionary<Vector2Int, Transform> fence;


        private void Start()
        {
            grid = new Dictionary<Vector2Int, Transform>(gridSizePerSide * gridSizePerSide);
            gridSizePerSide = (int)ground.transform.localScale.x;

            ground.transform.position = new Vector3(gridSizePerSide / 2f, -1, gridSizePerSide / 2f);

            CreateGrid();
            BuildFence();
        }

        private void OnDestroy()
        {
            ground = null;

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
                Transform c = grid[cell];
                var fP = Instantiate(fenceInfo.fencePiecePrefab, c.position, Quaternion.identity, c);
                fence.Add(cell, fP.transform);
            }

            fenceInfo.AlignFence(fence);
        }

        private void OnDrawGizmosSelected() // Cell bounds visualizer
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
