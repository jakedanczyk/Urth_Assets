using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class PauseScript : MonoBehaviour {

    GameObject EscapeMenu;
    bool escapeMenuActive;
    bool paused;
    bool muted;
    [SerializeField]
    Text mutetext;
    public Text helptext;

    FileInfo theSourceFile = null;
    StringReader reader = null;

    public GameObject helpPage;

    	// Use this for initialization
	void Start () {
        EscapeMenu = GameObject.Find("EscapeMenu");
        EscapeMenu.SetActive(false);
        paused = false;

        TextAsset helpdata = (TextAsset)Resources.Load("help", typeof(TextAsset));
        // puzdata.text is a string containing the whole file. To read it line-by-line:

        helptext.text = "";
        reader = new StringReader(helpdata.text);
        if (reader == null)
        {
            Debug.Log("puzzles.txt not found or not readable");
        }
        else
        {
            bool looping = true;
            while (looping)
            {
                string txt = reader.ReadLine();
                if (txt == null)
                    looping = false;
                helptext.text = helptext.text + txt + "\n";   
            }
        }
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update () {
        if (paused)
        {
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!EscapeMenu.activeSelf)
            {
                paused = !paused;
                if (paused)
                {
                    Time.timeScale = 0;
                }
                else if (!paused)
                {
                    Time.timeScale = 1;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!escapeMenuActive)
            {
                paused = true;
                escapeMenuActive = true;
                EscapeMenu.SetActive(true);
                Time.timeScale = 0;
                return;
            }
            if(escapeMenuActive)
            {
                escapeMenuActive = false;
                EscapeMenu.SetActive(false);
                paused = false;
                Time.timeScale = 1;
                return;
            }
            if (paused)
            {
                Time.timeScale = 0;
            }
            else if (!paused)
            {
                EscapeMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            print(Time.timeScale);
            if (!paused)// && Time.timeScale > 1)
            {
                Time.timeScale = Time.timeScale * .5f;
                Time.fixedDeltaTime = .02f * Time.timeScale;
                print(Time.timeScale);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            print(Time.timeScale);

            if (!paused)
            {
                Time.timeScale = Time.timeScale * 2f;
                Time.fixedDeltaTime = .02f * Time.timeScale;
                print(Time.timeScale);
            }
        }
     
        if (muted)
        {
            AudioListener.volume = 0;
            mutetext.text = "Unmute";
        }       
        else if (!muted)
        { AudioListener.volume = 1;
            mutetext.text = "Mute";
        }
	}

    public void Resume()
    { paused = false; escapeMenuActive = false; }

    public void Save()
    { PlayerPrefs.SetInt("currentscenesave", SceneManager.GetActiveScene().buildIndex); }

    public void Save2()
    { SaveManager.Save(); Debug.Log(Application.persistentDataPath); }

    public void Load()
    { SceneManager.LoadScene(PlayerPrefs.GetInt("currentscenesave")); }

    public void Load2()
    { SaveManager.Load(); Debug.Log(Application.persistentDataPath); }

    public void Mute()
    { muted = !muted; }

    public void Help()
    { helpPage.SetActive(!helpPage.activeSelf); }

    public void MainMenu()
    { SceneManager.LoadScene(1); }

    public void Quit()
    { Application.Quit(); }
}
