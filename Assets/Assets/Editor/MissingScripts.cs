using UnityEngine;
using UnityEditor;

public class FindMissingScripts : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        GetWindow<FindMissingScripts>("Find Missing Scripts");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Buscar objetos con Missing Scripts en la escena"))
        {
            FindInScene();
        }
    }

    private void FindInScene()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int missingCount = 0;

        foreach (GameObject go in allObjects)
        {
            Component[] components = go.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.LogWarning($"¡Missing Script encontrado en objeto: {GetFullPath(go)}", go);
                    missingCount++;
                }
            }
        }

        if (missingCount == 0)
        {
            Debug.Log("✅ No se encontraron Missing Scripts en la escena.");
        }
        else
        {
            Debug.Log($"🚨 Se encontraron {missingCount} objetos con Missing Scripts.");
        }
    }

    private string GetFullPath(GameObject go)
    {
        if (go.transform.parent == null)
            return go.name;
        return GetFullPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}