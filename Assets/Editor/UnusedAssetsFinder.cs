using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class UnusedAssetsFinder : EditorWindow
{
    [MenuItem("Tools/Find Unused Assets")]
    static void FindUnusedAssets()
    {
        string[] allAssets = AssetDatabase.GetAllAssetPaths()
            .Where(path => path.StartsWith("Assets/") && !AssetDatabase.IsValidFolder(path))
            .ToArray();

        string[] usedAssets = AssetDatabase.GetDependencies("Assets", true);
        
        HashSet<string> usedSet = new HashSet<string>(usedAssets);

        List<string> unusedAssets = new List<string>();

        foreach (var asset in allAssets)
        {
            if (!usedSet.Contains(asset) &&
                !asset.EndsWith(".cs") && // opcional: evitar borrar scripts
                !asset.EndsWith(".dll") &&
                !asset.Contains("Editor")) // opcional: evitar borrar cosas del editor
            {
                unusedAssets.Add(asset);
            }
        }

        Debug.Log($"🔍 Assets no usados encontrados: {unusedAssets.Count}");

        foreach (var asset in unusedAssets)
        {
            Debug.Log(asset);
        }

        if (unusedAssets.Count > 0)
        {
            if (EditorUtility.DisplayDialog("Unused Assets Found",
                $"{unusedAssets.Count} assets no usados encontrados.\n¿Quieres eliminarlos?", "Sí, borrar", "Cancelar"))
            {
                foreach (var asset in unusedAssets)
                {
                    AssetDatabase.DeleteAsset(asset);
                }

                AssetDatabase.Refresh();
                Debug.Log("🗑️ Assets no usados eliminados.");
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Sin archivos no usados", "No se encontraron assets sin usar.", "OK");
        }
    }
}
