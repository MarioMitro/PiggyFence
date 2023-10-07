using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiggyFence
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GameObject ground;

        private int gridSizePerSide;
        private Dictionary<Vector2Int, Transform> grid;


        private void Start()
        {
            grid = new Dictionary<Vector2Int, Transform>(gridSizePerSide * gridSizePerSide);
            gridSizePerSide = (int)ground.transform.localScale.x;

            ground.transform.position = new Vector3(gridSizePerSide / 2f, -1, gridSizePerSide / 2f);
            Camera.main.transform.position += Vector3.right * gridSizePerSide / 2;

            CreateGrid();
        }

        private void Update()
        {

        }

        private void OnDestroy()
        {
            ground = null;

            grid.Clear();
            grid = null;
        }

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
