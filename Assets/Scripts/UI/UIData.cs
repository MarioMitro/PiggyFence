using UnityEngine;

namespace PiggyFence.UI
{
    [CreateAssetMenu(fileName = "UIData", menuName = "UIData")]
    public class UIData : ScriptableObject
    {
        public int gridSize;
        public int fencePieceCount;
        public float fenceLength;
        public System.DateTime dbDataLoadingTime;

        private void Awake()
        {
            dbDataLoadingTime = new System.DateTime();
        }
    }
}
