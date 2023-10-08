using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PiggyInfo", menuName = "PiggyInfo")]
public class PiggyInfo : ScriptableObject
{
    public GameObject piggyPrefab;

    public Material piggyInMaterial;
    public Material piggyOutMaterial;
}
