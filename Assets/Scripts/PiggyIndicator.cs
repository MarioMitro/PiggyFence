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

        private void Awake()
        {
            lastPiggyPosition = Vector3.zero;
            piggy = Instantiate(piggyInfo.piggyPrefab, transform);
            piggyMeshRenderer = piggy.GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            Ray mouseRay = piggyCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out RaycastHit hit, groundMask))
            {
                if (hit.point != null)
                {
                    var newPosition = GridManager.instance.GetMousePosition(hit.point);

                    if (newPosition == Vector3.one)
                        piggy.transform.position = lastPiggyPosition;
                    else if (newPosition != lastPiggyPosition)
                    {
                        lastPiggyPosition = newPosition;
                        piggy.transform.position = GridManager.instance.GetMousePosition(hit.point);
                    }

                    piggyMeshRenderer.sharedMaterial = GridManager.instance.IsCellInTheFence(hit.point) ? piggyInfo.piggyInMaterial : piggyInfo.piggyOutMaterial;
                }
            }
        }
    }
}
