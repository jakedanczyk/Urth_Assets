using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dehumaniser
{
    class CreepyHumanAbout : EditorWindow
    {
        [MenuItem("Dehumaniser/CreepyHuman/About")]
        static void Init()
        {
            var window = ScriptableObject.CreateInstance<CreepyHumanAbout>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 280, 40);
            window.ShowAuxWindow();
        }

        void OnGUI()
        {
            string version = "v1.0";

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Dehumaniser CreepyHuman, Version: " + version);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public class CreepyHumanAudioComponent : Fabric.Dehumaniser.DehumaniserAudioComponentEditorInternal
    {
        [MenuItem("Dehumaniser/CreepyHuman/AudioComponent")]
        static void Create()
        {
            GameObject target = Selection.activeGameObject;
            if (target == null)
            {
                return;
            }

            GameObject component = new GameObject("CreepyHuman");

            component.transform.parent = target.transform;

            component.AddComponent<DehumaniserAudioComponent>();

            component.AddComponent<DehumaniserCreepyHuman>();
        }
    }

    [CustomEditor(typeof(DehumaniserCreepyHuman))]
    public class CreepyHumanEditor : Fabric.Dehumaniser.DehumaniserDSPVoiceEditor { }
}
