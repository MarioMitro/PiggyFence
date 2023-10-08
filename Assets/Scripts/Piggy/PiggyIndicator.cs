using UnityEngine;

using PiggyFence.Managers;

namespace PiggyFence.Piggy
{
    // Class is responsible for tracking mouse movement and projecting it on the ground.
    public class PiggyIndicator : MonoBehaviour
    {
        [SerializeField] private PiggyInfo piggyInfo;
        [Space]
        [SerializeField] private Camera piggyCamera;
        [Space]
        [SerializeField] private LayerMask groundMask;

        private GameObject piggy;
        private MeshRenderer piggyMeshRenderer;
        private GridManager gridManager;

        private Vector3 lastPiggyPosition;

        private void Awake()
        {
            lastPiggyPosition = Vector3.zero;
            piggy = Instantiate(piggyInfo.piggyPrefab, transform);
            piggyMeshRenderer = piggy.GetComponent<MeshRenderer>();

            gridManager = GridManager.instance;
        }

        private void OnDestroy()
        {
            gridManager = null;
            piggy = null;
            piggyMeshRenderer = null;
            piggyInfo = null;
            piggyCamera = null;
        }

        private void Update()
        {
            Ray mouseRay = piggyCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hit, groundMask))
            {
                if (hit.point != null)
                {
                    var newPosition = gridManager.GetMousePosition(hit.point);

                    if (newPosition == Vector3.one)                      // if mouse is outside the ground move piggy to last known position
                        piggy.transform.position = lastPiggyPosition;
                    else if (newPosition != lastPiggyPosition)           // if mouse is on new position on the ground
                    {
                        lastPiggyPosition = newPosition;
                        piggy.transform.position = gridManager.GetMousePosition(hit.point) - Vector3.up * 0.5f;

                        piggyMeshRenderer.sharedMaterial = gridManager.IsCellInTheFence(hit.point) ? piggyInfo.piggyInMaterial : piggyInfo.piggyOutMaterial;
                    }
                }
            }
        }
    }
}
