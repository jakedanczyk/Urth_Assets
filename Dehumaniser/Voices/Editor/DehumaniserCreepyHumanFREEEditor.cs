using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dehumaniser
{
    class CreepyHumanFREEAbout : EditorWindow
    {
        [MenuItem("Dehumaniser/CreepyHuman.FREE/About")]
        static void Init()
        {
            var window = ScriptableObject.CreateInstance<CreepyHumanFREEAbout>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 280, 40);
            window.ShowAuxWindow();
        }

        void OnGUI()
        {
            string version = "v1.0";

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Dehumaniser CreepyHuman FREE, Version: " + version);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public class CreepyHumanFREEAudioComponent : Fabric.Dehumaniser.DehumaniserAudioComponentEditorInternal
    {
        [MenuItem("Dehumaniser/CreepyHuman.FREE/AudioComponent")]
        static void Create()
        {
            GameObject target = Selection.activeGameObject;
            if (target == null)
            {
                return;
            }

            GameObject component = new GameObject("CreepyHumanFREE");

            component.transform.parent = target.transform;

            component.AddComponent<DehumaniserAudioComponent>();

            component.AddComponent<DehumaniserCreepyHumanFREE>();
        }
    }

    [CustomEditor(typeof(DehumaniserCreepyHumanFREE))]
    public class CreepyHumanEditorFREE : Fabric.Dehumaniser.DehumaniserDSPVoiceEditor { }
}
