using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupScript : MonoBehaviour
{

    public static GameObject startupObject;
    public UnityStandardAssets.Characters.FirstPerson.PlayerControls playerControl;
    BodyManager_Human_Player bodyManager;

    public MessageLog messageLog;

    public GameObject playerObject;
    public LayerMask mask = 174592;

    public List<GameObject> starterItems;

    Dictionary<int, int> heightsDict = new Dictionary<int, int>(); //<terrain layermask value, player height above terrain>

    public int currentHeight = 10000;
    public bool ready = false;
    public void Awake()
    {
        startupObject = this.gameObject;
        bodyManager = playerControl.GetComponent<BodyManager_Human_Player>();
        heightsDict[174592] = 513;
        heightsDict[174080] = 65;
        heightsDict[172032] = 1;
        heightsDict[163840] = 1;
        heightsDict[131072] = 1;
        mask = 174592;
        playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + heightsDict[mask.value], this.transform.position.z);
        playerControl.lastPosition = playerControl.transform.position;
    }

    private void Start()
    {
        messageLog.NewMessage("Initial terrain generation in progress");
    }

    //void Update()
    //{
    //    if (ready)
    //    {
    //        RaycastHit ground;
    //        if (Physics.Raycast(this.transform.position, Vector3.down, out ground, currentHeight * 1.5f, mask))
    //        {
    //            if (mask == 174592)
    //            {
    //                if (ground.collider.gameObject.name == "Chunk256(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                    playerControl.lastPosition = playerControl.transform.position;
    //                }
    //                else if (ground.collider.gameObject.name == "Chunk64(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    mask = 174080;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                }
    //                else if (ground.collider.gameObject.name == "Chunk16(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    mask = 172032;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                }
    //                else if (ground.collider.gameObject.name == "Chunk4(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    mask = 163840;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                }
    //                else
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + 2, this.transform.position.z);
    //                    playerControl.enabled = true;
    //                    this.enabled = false;
    //                }
    //            }
    //            else if (mask == 174080)
    //            {
    //                if (ground.collider.gameObject.name == "Chunk64(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                }
    //                else if (ground.collider.gameObject.name == "Chunk16(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    mask = 163840;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                    messageLog.text.text = "Initial terrain generation completed";
    //                    foreach (GameObject item in starterItems)
    //                    {
    //                        item.SetActive(true);
    //                    }
    //                    this.enabled = false;
    //                }
    //                else if (ground.collider.gameObject.name == "Chunk4(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    mask = 163840;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                    this.enabled = false;
    //                }
    //                else
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + 2, this.transform.position.z);
    //                    playerControl.enabled = true;
    //                    this.enabled = false;
    //                }
    //            }
    //            else if (mask == 172032)
    //            {
    //                if (ground.collider.gameObject.name == "Chunk16(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    mask = 163840;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                    messageLog.text.text = "Initial terrain generation completed";
    //                    foreach (GameObject item in starterItems)
    //                    {
    //                        item.SetActive(true);
    //                    }
    //                    this.enabled = false;
    //                }
    //                else if (ground.collider.gameObject.name == "Chunk4(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    mask = 163840;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                    messageLog.text.text = "Initial terrain generation completed";
    //                    foreach (GameObject item in starterItems)
    //                    {
    //                        item.SetActive(true);
    //                    }
    //                    this.enabled = false;
    //                }
    //                else
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + 2, this.transform.position.z);
    //                    playerControl.enabled = true;
    //                    this.enabled = false;
    //                }
    //            }
    //            else if (mask == 163840)
    //            {
    //                if (ground.collider.gameObject.name == "Chunk4(Clone)")
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    //playerControl.enabled = false;
    //                    messageLog.text.text = "Initial terrain generation completed";
    //                    foreach (GameObject item in starterItems)
    //                    {
    //                        item.SetActive(true);
    //                    }
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + heightsDict[mask.value], this.transform.position.z);
    //                    this.enabled = false;
    //                }
    //                else
    //                {
    //                    playerControl.FallSpeed = 0;
    //                    playerControl.FallingCount = 0;
    //                    playerControl.gameObject.transform.position = new Vector3(this.transform.position.x, ground.point.y + 2, this.transform.position.z);
    //                    playerControl.enabled = true;
    //                    this.enabled = false;
    //                }
    //            }
    //        }
    //    }
    //}

    void Update()
    {
        RaycastHit ground;
        if (Physics.Raycast(this.transform.position, Vector3.down, out ground, 20000, mask) || Physics.Raycast(this.transform.position + Vector3.up * 3, Vector3.up, out ground, 20000, mask))
        {
            if (ground.collider.GetComponent<Chunk256>() != null)
            {
                playerControl.FallSpeed = 0;
                playerControl.FallingCount = 0;
                playerControl.gameObject.transform.position = ground.point;
                playerControl.m_GravityMultiplier = 1;
                bodyManager.enabled = true;
            }
            if (ground.collider.GetComponent<Chunk16>() != null)
            {
                messageLog.NewMessage("Initial terrain generation completed");
                this.enabled = false;
            }

        }
    }
}

