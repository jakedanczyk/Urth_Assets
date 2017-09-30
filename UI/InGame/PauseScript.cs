using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseScript : MonoBehaviour {

    GameObject EscapeMenu;
    bool escapeMenuActive;
    bool paused;
    bool muted;
    [SerializeField]
    Text mutetext;

    	// Use this for initialization
	void Start () {
        EscapeMenu = GameObject.Find("EscapeMenu");
        EscapeMenu.SetActive(false);
        paused = true;
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
            if (!paused && Time.timeScale > 1)
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

    public void MainMenu()
    { Application.LoadLevel(1); }

    public void Save()
    { PlayerPrefs.SetInt("currentscenesave", SceneManager.GetActiveScene().buildIndex); }

    public void Load()
    { SceneManager.LoadScene(PlayerPrefs.GetInt("currentscenesave")); }

    public void Mute()
    { muted = !muted; }

    public void Quit()
    { Application.Quit(); }
}
