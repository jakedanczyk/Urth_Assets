using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dehumaniser
{
    class GiantAbout : EditorWindow
    {
        [MenuItem("Dehumaniser/Giant/About")]
        static void Init()
        {
            var window = ScriptableObject.CreateInstance<GiantAbout>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 280, 40);
            window.ShowAuxWindow();
        }

        void OnGUI()
        {
            string version = "v1.0";

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Dehumaniser Giant, Version: " + version);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public class Giant : Fabric.Dehumaniser.DehumaniserAudioComponentEditorInternal
    {
        [MenuItem("Dehumaniser/Giant/AudioComponent")]
        static void Create()
        {
            GameObject target = Selection.activeGameObject;
            if (target == null)
            {
                return;
            }

            GameObject component = new GameObject("Giant");

            component.transform.parent = target.transform;

            component.AddComponent<DehumaniserAudioComponent>();

            component.AddComponent<DehumaniserGiant>();
        }
    }

    [CustomEditor(typeof(DehumaniserGiant))]
    public class GiantEditor : Fabric.Dehumaniser.DehumaniserDSPVoiceEditor { }
}
