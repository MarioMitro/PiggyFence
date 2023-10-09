using PiggyFence.Fence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FenceType
{
    SquareFence,
    MoreComplexFence,
    MoreComplexFence2,
    DBFence
}


[CreateAssetMenu(fileName = "TestFenceInfo", menuName = "TestFenceInfo")]
public class TestFenceInfo : FenceInfo
{
    public FenceType fenceType;

    public override void LoadData(List<Vector2Int> fenceData)
    {
        switch (fenceType)
        {
            case FenceType.SquareFence:
                fenceCellCoordinates = new List<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 2), new Vector2Int(2, 1) };
                break;
            case FenceType.MoreComplexFence:
                fenceCellCoordinates = new List<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(1, 2), new Vector2Int(2, 3), new Vector2Int(3, 4), new Vector2Int(4, 3), new Vector2Int(4, 2), new Vector2Int(4, 1), new Vector2Int(3, 0), new Vector2Int(2, 0), new Vector2Int(1, 0) };
                break;
            case FenceType.MoreComplexFence2:
                fenceCellCoordinates = new List<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(1, 2), new Vector2Int(2, 3), new Vector2Int(3, 4), new Vector2Int(4, 5), new Vector2Int(5, 4), new Vector2Int(5, 3), new Vector2Int(5, 2), new Vector2Int(5, 1), new Vector2Int(4, 0), new Vector2Int(3, 0), new Vector2Int(2, 0), new Vector2Int(1, 0) };
                break;
            case FenceType.DBFence:
                fenceCellCoordinates = new List<Vector2Int> { new Vector2Int(5,5), new Vector2Int(5, 11), new Vector2Int(5,12), new Vector2Int(6,4), new Vector2Int(6,6), new Vector2Int(6,10), new Vector2Int(6, 13),
                                         new Vector2Int(7,4), new Vector2Int(7, 7), new Vector2Int(7,8), new Vector2Int(7,9), new Vector2Int(7, 13), new Vector2Int(8,4), new Vector2Int(8, 13),
                                         new Vector2Int(9, 4), new Vector2Int(9, 14), new Vector2Int(10, 5), new Vector2Int(10,6), new Vector2Int(10, 7), new Vector2Int(10, 8), new Vector2Int(10, 13),
                                         new Vector2Int(11, 9), new Vector2Int(11, 12), new Vector2Int(12,10), new Vector2Int(12,11)};
                break;
        }

        isDataLoaded = true;
    }
}
