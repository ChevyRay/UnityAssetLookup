using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
[CustomEditor(typeof(AssetLookup))]
public class AssetLookupEditor : Editor
{
    static AssetLookupEditor()
    {
        EditorApplication.playModeStateChanged += state => UpdateAllAssetLookups();
    }

    static void UpdateAllAssetLookups()
    {
        foreach (var guid in AssetDatabase.FindAssets("t:AssetLookup"))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var lookup = AssetDatabase.LoadAssetAtPath<AssetLookup>(path);
            if (lookup != null)
                UpdateAssetLookup(lookup);
        }
    }

    static void UpdateAssetLookup(AssetLookup lookup)
    {
        lookup.assets = new List<Object>();
        lookup.paths = new List<string>();
        if (!string.IsNullOrEmpty(lookup.typeName))
        {
            var dir = lookup.rootDirectory;
            while (dir.Length > 0 && dir.EndsWith("/", System.StringComparison.Ordinal))
                dir = dir.Substring(0, dir.Length - 1);
            if (string.IsNullOrEmpty(dir))
                dir = "Assets";
            if (!System.IO.Directory.Exists(dir))
                return;
            foreach (var guid in AssetDatabase.FindAssets("t:" + lookup.typeName, new string[] { dir }))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (asset.GetType().Name == lookup.typeName)
                {
                    lookup.assets.Add(asset);
                    var i = dir.Length + 1;
                    var j = path.IndexOf('.');
                    lookup.paths.Add(path.Substring(i, j - i));
                }
            }
        }
    }

    Vector2 scroll;
    SerializedProperty dirProp;
    SerializedProperty typeProp;

    void OnEnable()
    {
        dirProp = serializedObject.FindProperty("rootDirectory");
        typeProp = serializedObject.FindProperty("typeName");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(dirProp);

        var dir = dirProp.stringValue;
        if (string.IsNullOrEmpty(dir) || !System.IO.Directory.Exists(dir))
            EditorGUILayout.HelpBox("Root directory does not exist.", MessageType.Error);

        EditorGUILayout.PropertyField(typeProp);
        serializedObject.ApplyModifiedProperties();

        var lookup = (AssetLookup)target;

        UpdateAssetLookup(lookup);

        GUILayout.Label("Assets:", EditorStyles.boldLabel);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUI.BeginDisabledGroup(true);
        for (int i = 0; i < lookup.assets.Count; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(lookup.paths[i], lookup.assets[i], typeof(Object), false);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndScrollView();
    }
}
