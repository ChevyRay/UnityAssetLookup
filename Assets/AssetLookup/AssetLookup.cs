using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAssetLookup", menuName = "Asset Lookup")]
public class AssetLookup : ScriptableObject
{
    public string rootDirectory = "Assets";
    public string typeName;

    [HideInInspector]
    public List<Object> assets;

    [HideInInspector]
    public List<string> paths;

    Dictionary<string, Object> dict;

    public T Find<T>(string name) where T : Object
    {
        if (dict == null)
        {
            dict = new Dictionary<string, Object>(System.StringComparer.Ordinal);
            for (int i = 0; i < assets.Count; ++i)
                dict[paths[i]] = assets[i];
        }
        Object obj;
        dict.TryGetValue(name, out obj);
        return obj as T;
    }

    public IEnumerable<T> FindAll<T>() where T : Object
    {
        foreach (var obj in assets)
            if (obj is T)
                yield return obj as T;
    }
}
