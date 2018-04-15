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
    public BodyManager_Human_Player playerBodyManager;
    public WorldTime time;
    public DataWindow dataWindow;

    public bool isWorldChangedSinceLastSave;

    // Use this for initialization
    void Start() {
        dataWindow = DataWindow.dataWindowGameObject.GetComponent<DataWindow>();
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
    void Update() {
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
                    Cursor.lockState = CursorLockMode.None;

                }
                else if (!paused)
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!escapeMenuActive)
            {
                Cursor.lockState = CursorLockMode.None;
                paused = true;
                escapeMenuActive = true;
                EscapeMenu.SetActive(true);
                Time.timeScale = 0;
                return;
            }
            if (escapeMenuActive)
            {
                Cursor.lockState = CursorLockMode.Locked;
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
                dataWindow.SetGameSpeed(Time.timeScale);
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
                dataWindow.SetGameSpeed(Time.timeScale);
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

        if (Input.GetKeyDown(KeyCode.F5))
        {
            QuickSave();
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
            QuickLoad();
        }
    }

    public void Resume()
    { paused = false; escapeMenuActive = false; }

    public void Save()
    { PlayerPrefs.SetInt("currentscenesave", SceneManager.GetActiveScene().buildIndex); }

    public void Save2()
    { SaveManager.Save(); Debug.Log(Application.persistentDataPath); }

    public void Save3()
    { LevelSerializer.SaveGame("main"); }

    public void Load3()
    {
        int i = LevelSerializer.SavedGames.Count;
        LevelSerializer.LoadSavedLevel(LevelSerializer.SavedGames[LevelSerializer.PlayerName][0].Data);
    }

    public void Load()
    { SceneManager.LoadScene(PlayerPrefs.GetInt("currentscenesave")); }

    public void Load2()
    { SaveManager.Load(); Debug.Log(Application.persistentDataPath); }

    public void QuickSave()
    {
        //world16
        System.IO.DirectoryInfo quicksaveDir = new DirectoryInfo("terrain16saves/quicksave/");
        foreach (FileInfo file in quicksaveDir.GetFiles())
        {
            file.Delete();
        }
        string currDirPath = "terrain16saves/" + World16.worldGameObject.GetComponent<World16>().worldName + "/";
        System.IO.DirectoryInfo dir = new DirectoryInfo(currDirPath);
        foreach (FileInfo file in dir.GetFiles())
        {
            File.Copy(currDirPath + file.Name, "terrain16saves/quicksave/" + file.Name, true);
        }

        //world1
        quicksaveDir = new DirectoryInfo("terrain1saves/quicksave/");
        foreach (FileInfo file in quicksaveDir.GetFiles())
        {
            file.Delete();
        }
        currDirPath = "terrain1saves/" + World1.worldGameObject.GetComponent<World1>().worldName + "/";
        dir = new DirectoryInfo(currDirPath);
        foreach (FileInfo file in dir.GetFiles())
        {
            File.Copy(currDirPath + file.Name, "terrain1saves/quicksave/" + file.Name, true);
        }
        LevelSerializer.SaveGame("quicksave");
    }

    public void QuickLoad()
    {
        //if (isWorldChangedSinceLastSave)
        //{
        //    string directoryName = "terrain16saves/" + World16.worldGameObject.GetComponent<World16>().worldName + "/";

        //    System.IO.DirectoryInfo quickSaveDi = new DirectoryInfo("terrain16saves/quicksave/");

        //    foreach (FileInfo file in quickSaveDi.GetFiles())
        //    {
        //        File.Copy("terrain16saves/quicksave/" + file.Name, directoryName + file.Name, true);
        //    }
        //}
        LevelSerializer.LoadSavedLevel(LevelSerializer.SavedGames[LevelSerializer.PlayerName].Find(delegate(LevelSerializer.SaveEntry sv){return sv.Name == "quicksave";}).Data);
        World16.worldGameObject.GetComponent<World16>().worldName = "quicksave";
        World1.worldGameObject.GetComponent<World1>().worldName = "quicksave";
    }

    public void Mute()
    { muted = !muted; }

    public void Help()
    { helpPage.SetActive(!helpPage.activeSelf); }

    public void MainMenu()
    { SceneManager.LoadScene(1); }

    public void Quit()
    { Application.Quit(); }

    public void NumericHUDToggle()
    {
        playerBodyManager.IsGUIActive = !playerBodyManager.IsGUIActive;
        time.IsGUIActive = playerBodyManager.IsGUIActive;
    }

}
