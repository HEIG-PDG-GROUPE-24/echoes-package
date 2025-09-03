#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using System.Linq;
using Echoes.Runtime;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Echoes.Runtime
{
    public static class LoadAction
    {
        public static void ResolveReferences()
        {
            Object.FindObjectsByType<EchoesNpcComponent>(FindObjectsSortMode.None)
                .Where(npc => npc.npcData != null)
                .ForEach(npc =>
                {
                    npc.npcData.ResolveContactsReferences();
                    npc.LoadFromSo();
                });
        }
    }
    
    public static class SceneLoadOpen
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("Scene Loaded");
            LoadAction.ResolveReferences();
        }
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public static class EditorSceneWatcher
{
    static EditorSceneWatcher()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
    }

    private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        Debug.Log($"[Editor] Scene opened: {scene.name}");
        LoadAction.ResolveReferences();
    }
}
#endif