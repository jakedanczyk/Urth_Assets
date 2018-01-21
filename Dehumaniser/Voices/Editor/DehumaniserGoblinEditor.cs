using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dehumaniser
{
    class GoblinAbout : EditorWindow
    {
        [MenuItem("Dehumaniser/Goblin/About")]
        static void Init()
        {
            var window = ScriptableObject.CreateInstance<GoblinAbout>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 280, 40);
            window.ShowAuxWindow();
        }

        void OnGUI()
        {
            string version = "v1.0";

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Dehumaniser Goblin, Version: " + version);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public class Goblin : Fabric.Dehumaniser.DehumaniserAudioComponentEditorInternal
    {
        [MenuItem("Dehumaniser/Goblin/AudioComponent")]
        static void Create()
        {
            GameObject target = Selection.activeGameObject;
            if (target == null)
            {
                return;
            }

            GameObject component = new GameObject("Goblin");

            component.transform.parent = target.transform;

            component.AddComponent<DehumaniserAudioComponent>();

            component.AddComponent<DehumaniserGoblin>();
        }
    }

    [CustomEditor(typeof(DehumaniserGoblin))]
    public class GoblinEditor : Fabric.Dehumaniser.DehumaniserDSPVoiceEditor { }

}
