using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAssetLookup", menuName = "Asset Lookup")]
public class AssetLookup : ScriptableObject
{
    [HideInInspector]
    public string rootDirectory = "Assets";

    [HideInInspector]
    public string typeName;

    [HideInInspector]
    public List<Object> assets;

    [HideInInspector]
    public List<string> paths;

    Dictionary<string, Object> assetDict;
    Dictionary<Object, string> pathDict;

    void BuildDictionaries()
    {
        if (assetDict == null || assetDict.Count != assets.Count)
        {
            assetDict = new Dictionary<string, Object>(System.StringComparer.Ordinal);
            pathDict = new Dictionary<Object, string>();
            for (int i = 0; i < assets.Count; ++i)
            {
                assetDict[paths[i]] = assets[i];
                pathDict[assets[i]] = paths[i];
            }
        }
    }

    public T Find<T>(string name) where T : Object
    {
        BuildDictionaries();
        Object obj;
        assetDict.TryGetValue(name, out obj);
        return obj as T;
    }

    public IEnumerable<T> FindAll<T>() where T : Object
    {
        foreach (var obj in assets)
            if (obj is T)
                yield return obj as T;
    }

    public string GetPath(Object asset)
    {
        BuildDictionaries();
        string path;
        pathDict.TryGetValue(asset, out path);
        return path;
    }
}
