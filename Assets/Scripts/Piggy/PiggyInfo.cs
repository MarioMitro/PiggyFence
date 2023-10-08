using UnityEngine;

namespace PiggyFence.Piggy
{
    [CreateAssetMenu(fileName = "PiggyInfo", menuName = "PiggyInfo")]
    public class PiggyInfo : ScriptableObject
    {
        public GameObject piggyPrefab;

        public Material piggyInMaterial;
        public Material piggyOutMaterial;

        private void OnDestroy()
        {
            piggyPrefab = null;
            piggyInMaterial = null;
            piggyOutMaterial = null;
        }
    }
}
