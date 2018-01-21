using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Dehumaniser
{
    class GrowlingBeastAbout : EditorWindow
    {
        [MenuItem("Dehumaniser/GrowlingBeast/About")]
        static void Init()
        {
            var window = ScriptableObject.CreateInstance<GrowlingBeastAbout>();
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 280, 40);
            window.ShowAuxWindow();
        }

        void OnGUI()
        {
            string version = "v1.0";

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Dehumaniser GrowlingBeast, Version: " + version);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public class GrowlingBeast : Fabric.Dehumaniser.DehumaniserAudioComponentEditorInternal
    {
        [MenuItem("Dehumaniser/GrowlingBeast/AudioComponent")]
        static void Create()
        {
            GameObject target = Selection.activeGameObject;
            if (target == null)
            {
                return;
            }

            GameObject component = new GameObject("GrowlingBeast");

            component.transform.parent = target.transform;

            component.AddComponent<DehumaniserAudioComponent>();

            component.AddComponent<DehumaniserGrowlingBeast>();
        }
    }

    [CustomEditor(typeof(DehumaniserGrowlingBeast))]
    public class GrowlingBeastEditor : Fabric.Dehumaniser.DehumaniserDSPVoiceEditor { }

}
