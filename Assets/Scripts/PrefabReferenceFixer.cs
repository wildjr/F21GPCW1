using UnityEditor;
using UnityEngine;

//Not written by me, taken from https://forum.unity.com/threads/mixamo-unity-does-not-import-any-textures.516301/
[CreateAssetMenu()]
public class PrefabReferenceFixer : ScriptableObject
{
    [MenuItem("Assets/Force Reserialize")]
    private static void ForceReserialize()
    {
        GameObject[] selection = Selection.gameObjects;
        string[] objectPaths = new string[selection.Length];

        for (int i = 0; i < selection.Length; ++i)
        {
            objectPaths[i] = AssetDatabase.GetAssetPath(selection[i]);
        }

        AssetDatabase.ForceReserializeAssets(objectPaths);

        Debug.Log("done");
    }
}
