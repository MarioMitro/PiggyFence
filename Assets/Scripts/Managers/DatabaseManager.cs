using UnityEngine;

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

        private DatabaseReference dbReference;

        void Awake()
        {
            dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            //SaveData();
            LoadData();
        }

        private void OnDestroy()
        {
            fenceInfo = null;
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
                fenceInfo.fenceCellCoordinates = JsonUtility.FromJson<FenceData>(snapShot.GetRawJsonValue()).cells;
            }
        }
    }
}