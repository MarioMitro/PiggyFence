using UnityEngine;

using PiggyFence.UI;
using PiggyFence.Fence;

using Firebase.Database;

using System.Collections;
using System.Collections.Generic;

namespace PiggyFence.Managers
{
    // Class is responsible for retrieving data from Firestore DB
    public class DatabaseManager : Singleton<DatabaseManager>
    {
        [SerializeField] private FenceInfo fenceInfo;
        [Space]
        [SerializeField] private UIData uiData;

        private DatabaseReference dbReference;

        void Awake()
        {
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            //SaveData();
            LoadData();
        }

        private void OnDestroy()
        {
            dbReference = null;
        }

        private void LoadData()
        {
            StartCoroutine(LoadDataAsync());
        }

        private void SaveData()
        {
            FenceData fence = new FenceData(new List<Vector2Int> { new Vector2Int(5,5), new Vector2Int(5, 11), new Vector2Int(5,12), new Vector2Int(6,4), new Vector2Int(6,6), new Vector2Int(6,10), new Vector2Int(6, 13),
                                         new Vector2Int(7,4), new Vector2Int(7, 7), new Vector2Int(7,8), new Vector2Int(7,9), new Vector2Int(7, 13), new Vector2Int(8,4), new Vector2Int(8, 13),
                                         new Vector2Int(9, 4), new Vector2Int(9, 14), new Vector2Int(10, 5), new Vector2Int(10,6), new Vector2Int(10, 7), new Vector2Int(10, 8), new Vector2Int(10, 13),
                                         new Vector2Int(11, 9), new Vector2Int(11, 12), new Vector2Int(12,10), new Vector2Int(12,11)});

            //FenceData fence = new FenceData(new List<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 2), new Vector2Int(2, 1) });

            //FenceData fence = new FenceData(new List<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(1, 2), new Vector2Int(2, 3), new Vector2Int(3, 4), new Vector2Int(4, 3), new Vector2Int(4, 2), new Vector2Int(4, 1), new Vector2Int(3, 0), new Vector2Int(2, 0), new Vector2Int(1, 0) });

            //FenceData fence = new FenceData(new List<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(1, 2), new Vector2Int(2, 3), new Vector2Int(3, 4), new Vector2Int(4, 5), new Vector2Int(5, 4), new Vector2Int(5, 3), new Vector2Int(5, 2), new Vector2Int(5, 1), new Vector2Int(4, 0), new Vector2Int(3, 0), new Vector2Int(2, 0), new Vector2Int(1, 0) });

            string json = JsonUtility.ToJson(fence);

            dbReference.Child("fence").SetRawJsonValueAsync(json);
        }

        private IEnumerator LoadDataAsync()
        {
            var data = dbReference.Child("fence").GetValueAsync();

            yield return new WaitUntil(() => data.IsCompleted);

            if (data != null)
            {
                DataSnapshot snapShot = data.Result;
                uiData.dbDataLoadingTime = System.DateTime.Now;

                fenceInfo.LoadData(JsonUtility.FromJson<FenceData>(snapShot.GetRawJsonValue()).cells);

                uiData.fencePieceCount = fenceInfo.fenceCellCoordinates.Count;
            }
        }
    }
}