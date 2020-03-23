using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class LayouTaroEditorTool
{
    [MenuItem("Window/LayouTaro/Update UnityPackage")]
    public static void UnityPackage()
    {
        var assetPaths = new List<string>();

        var frameworkPath = "Assets/LayouTaro";
        CollectPathRecursive(frameworkPath, assetPaths);

        AssetDatabase.ExportPackage(assetPaths.ToArray(), "LayouTaro.unitypackage", ExportPackageOptions.IncludeDependencies);
    }

    private static void CollectPathRecursive(string path, List<string> collectedPaths)
    {
        var filePaths = Directory.GetFiles(path);
        foreach (var filePath in filePaths)
        {
            collectedPaths.Add(filePath);
        }

        var modulePaths = Directory.GetDirectories(path);
        foreach (var folderPath in modulePaths)
        {
            CollectPathRecursive(folderPath, collectedPaths);
        }
    }

    static LayouTaroEditorTool()
    {
        // create unitypackage if compiled.
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            UnityPackage();
        }
    }
}