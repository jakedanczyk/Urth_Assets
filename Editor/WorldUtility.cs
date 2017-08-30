using UnityEngine;
using UnityEditor;
using System.Collections;

public class WorldUtilitiy : MonoBehaviour {
    [MenuItem("Assets/Create/World/Weather")]
    static public void CreateWeather()
    { ScriptableObjectUtility.CreateAsset<Weather>();}
}
