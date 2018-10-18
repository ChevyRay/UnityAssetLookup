using UnityEngine;

public class Example : MonoBehaviour
{
    public AssetLookup assets;

    void Start()
    {
        var first = assets.Find<MyAssetType>("FirstAsset");
        var third = assets.Find<MyAssetType>("ThirdAsset");
        Debug.Log("FirstAsset: " + first);
        Debug.Log("ThirdAsset: " + third);
    }
}
