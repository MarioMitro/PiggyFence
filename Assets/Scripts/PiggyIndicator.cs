using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiggyFence
{
    public class PiggyIndicator : MonoBehaviour
    {
        [SerializeField] private PiggyInfo piggyInfo;
        [SerializeField] private Camera piggyCamera;
        [SerializeField] private LayerMask groundMask;

        private Vector3 lastPiggyPosition;
        private GameObject piggy;
        private MeshRenderer piggyMeshRenderer;

        private GridManager gridManager;

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

                    if (newPosition == Vector3.one)
                        piggy.transform.position = lastPiggyPosition;
                    else if (newPosition != lastPiggyPosition)
                    {
                        lastPiggyPosition = newPosition;
                        piggy.transform.position = gridManager.GetMousePosition(hit.point) - Vector3.up * 0.5f;
                    }

                    piggyMeshRenderer.sharedMaterial = gridManager.IsCellInTheFence(hit.point) ? piggyInfo.piggyInMaterial : piggyInfo.piggyOutMaterial;
                }
            }
        }
    }
}
