using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game : MonoBehaviour
{

    public static Game current;
    public BodyManager_Human_Player player;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls controls;
    public WorldTime worldTime;
    public World world;
    public Save save;
    public Save1 save1;
    public Save4 save4;

    public Game()
    {
        
    }

}