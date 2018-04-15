using UnityEngine;
using System.Text;
using System.Collections;

public class SaveAndLoad : MonoBehaviour {
    [SerializeField]
    private int[] array;
    [SerializeField]
    private object arrayObject;
    [SerializeField]
    private byte[] data;
    [SerializeField]
    private string test;

    [SerializeField]
    private GameObject theObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            LevelSerializer.SaveGame("quicksave");
            //data = LevelSerializer.SerializeLevel(false, theObject.GetComponent<UniqueIdentifier>().Id);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
            {
                Debug.Log(sg.Level);
                Debug.Log(sg.Name);
                Debug.Log(sg.When);
            }
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            int i = LevelSerializer.SavedGames.Count;
            LevelSerializer.LoadSavedLevel(LevelSerializer.SavedGames[LevelSerializer.PlayerName][0].Data);

            //foreach (LevelSerializer.SaveEntry sg in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
            //{
            //    LevelSerializer.LoadSavedLevel(sg.Data);
            //}
        }
    }

    static public string EncodeTo64(string toEncode)

    {

        byte[] toEncodeAsBytes

              = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

        string returnValue

              = System.Convert.ToBase64String(toEncodeAsBytes);

        return returnValue;

    }
}
