using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAsset", menuName = "MyAssetType")]
public class MyAssetType : ScriptableObject
{
    public Vector3 pos;
    public float speed;
    public string message;
}
