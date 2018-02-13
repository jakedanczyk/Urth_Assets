using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    //[System.Serializable]
    [SerializeAll]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
        public static GameObject playerControlsGameObject;

        public World thisWorld;
        public BodyManager_Human_Player player_bodyManager;
        //public RPGCreature player;
        public RPGStatCollection playerStats;
        public RPGStat jump;
        public bool crouching, autoMove, aboveDetailedChunk;
        public Rigidbody m_Rigidbody;
        public Animator anim;
        public GameObject shoulderGirdle;
        public GameObject craftingUI, fireUIPanel;
        public GameObject playerObject;
        public Transform playerTransform;

        GameObject _inventoryUI;
        GameObject _tooltip;
        GameObject _character;
        GameObject _dropBox;
        public bool showInventory = false;
        public bool terrainMode = false;
        public Block setBlock;
        public Item_TerrainBlock buildingBlock;

        GameObject inventoryUI;
        GameObject craftSystem;
        GameObject characterSystem;

        public GameObject characterUI;
        public GameObject actionUI;

        public PlayerInventory inventory;
        public Inventory playerInventory;
        public LootInventory lootInventory;
        public CraftingSystem playerCrafting;
        public bool inventoryActive, lootActive;
        public LootInventoryFocusPanel lootPanelScript;

        GameObject modelViewObject;
        public RectTransform modelViewUI;
        public GameObject butcheringUI;
        public RectTransform lootingUI;
        public RectTransform lootPanel;
        public RectTransform inventoryUIPanel;
        public RectTransform firePanel;
        public StartFireButtonScript fireButton;

        float m_CharacterControllerHeight;
        Vector3 m_CharacterControllerCenter;

        const float k_Half = 0.5f;

        public bool m_IsNotSprinting;
        public float m_WalkSpeed;
        public float m_JogSpeed;
        public float m_RunSpeed;
        [SerializeField]
        [Range(0f, 1f)]
        private float m_RunstepLengthen;
        public float m_JumpSpeed;
        public float m_StickToGroundForce;
        [SerializeField]
        public float m_GravityMultiplier;
        [SerializeField]
        public MouseLook m_MouseLook;
        [SerializeField]
        private bool m_UseFovKick;
        [SerializeField]
        private FOVKick m_FovKick = new FOVKick();
        [SerializeField]
        private bool m_UseHeadBob;
        [SerializeField]
        private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField]
        private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField]
        private float m_StepInterval;
        [SerializeField]
        private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField]
        private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField]
        private AudioClip m_LandSound;           // the sound played when character touches back on ground.


        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        public CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        public bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        public PlayerAudio_Manager playerAudioManager;

        public bool e_down;
        Camera m_Camera;
        public Collider cameraBubble;

        CharacterController characterController;
        public StaticBuildingSystem buildingSystem;
        public BlockBuildingSystem blockBuildingSystem;

        private void Awake()
        {
            crouching = false;
            lastPosition = this.transform.position;
            playerControlsGameObject = this.gameObject;
        }

        // Use this for initialization
        private void Start()
        {
            modelViewObject = ModelViewer.modelViewObject;
            modelViewUI = ModelViewer.modelViewerObject.GetComponent<ModelViewer>().parentCanvas;
            m_AudioSource = GetComponent<AudioSource>();
            if (LevelSerializer.IsDeserializing) return;

            m_Camera = Camera.main.GetComponent<Camera>();
            characterController = GetComponent<CharacterController>();
            jump = playerStats.GetStat<RPGDerived>(RPGStatType.JumpHeight);
            m_JumpSpeed = (float)(jump.StatValue)/10f;
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_MouseLook.Init(transform, m_Camera.transform);
            m_CharacterControllerHeight = m_CharacterController.height;
            m_CharacterControllerCenter = m_CharacterController.center;
            inventory = player_bodyManager.baseInventory;
            InvokeRepeating("SetJumpSpeed", 1.1f, 1.1f);
        }

        public Vector3 lastPosition;
        bool check;
        int fallingCount;
        public int FallingCount { get { return fallingCount; } set { fallingCount = value; } }
        float fallTime,fallSpeed;
        public float FallSpeed { get { return fallSpeed; } set { fallSpeed = value; } }
        public int requiredFallTime;
        // Update is called once per frame
        private void Update()
        {
            if (LevelSerializer.IsDeserializing) return;

            RaycastHit ground;
            if (Physics.Raycast(this.transform.position + Vector3.up, Vector3.down, out ground, 512f, mask1))
            {
                aboveDetailedChunk = true;
                lastPosition = this.transform.position;
            }
            else
            {
                aboveDetailedChunk = false;
                this.transform.position = lastPosition;
            }
            lastPosition = this.transform.position;

            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            fallingCount = (m_CharacterController.isGrounded) ? 0 : fallingCount + 1;
            if(fallingCount > 0)
            {
                fallSpeed = m_MoveDir.y;
            }
            else if(fallSpeed < -10f && fallingCount == 0)
            {
                playerStats.GetStat<RPGVital>(RPGStatType.Health).StatCurrentValue += (int)((fallSpeed + 10f) * 10);
                fallSpeed = 0;
            }


            if (!showInventory)
            {
                RotateView();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (player_bodyManager.meleeWeaponDrawn)
                    {
                        player_bodyManager.MainAttack();
                    }

                    if (player_bodyManager.rangedWeaponDrawn)
                    {
                        player_bodyManager.KnockArrow();
                    }

                    if (terrainMode)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(player_bodyManager.aimPoint.position, player_bodyManager.aimPoint.forward, out hit, 12f))
                        {
                            WorldPosFloat pos1 = EditTerrain.GetBlockPos(hit);
                            Vector3 direction = player_bodyManager.aimPoint.position - hit.point;
                            direction = direction.normalized;
                            WorldPos pos2 = new WorldPos((int)(pos1.x + (2 * direction.x)), (int)(pos1.y + (2 * direction.y)), (int)(pos1.z + (2 * direction.z)));
                            Chunk chunk = thisWorld.GetChunk(pos2.x, pos2.y, pos2.z);
                            if (!(chunk == null))
                            {
                                if (chunk.world.GetBlock(pos2.x, pos2.y, pos2.z) is BlockAir) { chunk.airCount--; } //decrement aircount if existing block is air
                                chunk.world.SetBlock(pos2.x, pos2.y, pos2.z, setBlock);
                                if (!chunk.gameObject.activeSelf)
                                {
                                    chunk.gameObject.SetActive(true);
                                }
                                Destroy(buildingBlock.gameObject);
                            }
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Mouse1) && player_bodyManager.wieldingBow && player_bodyManager.arrowKnocked && !player_bodyManager.bowDrawn)
                {
                    player_bodyManager.DrawBow();
                }

                if (Input.GetKeyUp(KeyCode.Mouse1) && player_bodyManager.drawingBow)
                {
                    player_bodyManager.ReleaseBow();
                }

                if (Input.GetKeyDown(KeyCode.Mouse0) && player_bodyManager.drawingBow)
                {
                    player_bodyManager.FireBow();
                }

                if (Input.GetKeyDown(KeyCode.E)) //pickup, interact
                {
                    e_down = true;
                    RaycastHit hit;
                    if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, 20f))
                    {
                        string s = hit.collider.gameObject.tag;
                        print(s);
                        print(player_bodyManager.rHandWeapon);
                        if (hit.collider.gameObject.tag == "Item")
                        {
                            Item hitItem = hit.collider.gameObject.GetComponent<Item>();
                            if (hitItem is Item_Weapon)
                            {
                                Item_Weapon hitWeapon = (Item_Weapon)hitItem;
                                if (hitWeapon.wielded)
                                { }
                                else
                                    player_bodyManager.PickupItem(hitItem);

                            }
                            else
                                player_bodyManager.PickupItem(hitItem);
                            //Destroy(hit.collider.gameObject);
                        }
                        else if (hit.collider.gameObject.tag == "Tree" && (player_bodyManager.rHandWeapon is Item_Weapon_Axe || player_bodyManager.rHandWeapon is Item_Weapon_Hatchet))
                        {
                            Tree hitTree = hit.collider.gameObject.GetComponent<Tree>();
                            player_bodyManager.FellTree(hitTree, (Item_Weapon_Axe)player_bodyManager.rHandWeapon);
                        }
                        else if (hit.collider.gameObject.tag == "Perennial")// && player_bodyManager.rHandWeapon.gameObject == null)
                        {
                            Perennial hitPerennial = hit.collider.gameObject.GetComponent<Perennial>();
                            player_bodyManager.GatherPlant(hitPerennial);
                        }
                        else if (hit.collider.gameObject.tag == "FelledTree" && player_bodyManager.rHandWeapon is Item_Weapon_Axe)
                        {
                            Tree hitTree = hit.collider.gameObject.GetComponent<Tree>();
                            player_bodyManager.ProcessDownedTree(hitTree, (Item_Weapon_Axe)player_bodyManager.rHandWeapon);
                        }
                        else if (hit.collider.gameObject.tag == "StackContainer")
                        {
                            StackContainer hitStack = hit.collider.gameObject.GetComponentInParent<StackContainer>();
                            print(hitStack.name);
                            hitStack.PullItem(inventory);
                        }
                        //else if (hit.collider.gameObject.tag == "UnlitFire")
                        //{
                        //  Fire hitFire = hit.collider.gameObject.GetComponentInParent<Fire>();
                        //  player_bodyManager.StartFire(hitFire, 1);
                        //}
                        else if (hit.collider.gameObject.tag == "BodyPart")
                        {
                            BodyManager parentBody = hit.collider.gameObject.GetComponentInParent<BodyManager> ();
                            print(parentBody.name);
                            print(parentBody.tag);
                            if (parentBody.gameObject.tag == "DeadCreature")
                            {
                                inventoryUIPanel.gameObject.SetActive(true);
                                lootingUI.gameObject.SetActive(true);
                                //butcheringUI.gameObject.SetActive(true);
                                BodyManager hitBody = hit.collider.gameObject.GetComponentInParent<BodyManager>();
                                print(hitBody.name);
                                lootInventory = hitBody.lootInventory;
                                hitBody.lootInventory.playerControls = this;
                                hitBody.lootInventory.itemsPanel = lootPanel;
                                hitBody.lootInventory.inventoryUIPanel = lootingUI;
                                hitBody.lootInventory.RebuildUIPanel(this);
                                showInventory = true;
                                print(hitBody.lootInventory.name);
                                lootPanelScript.attachedInventory = hitBody.lootInventory;
                            }
                        }
                        else if (hit.collider.gameObject.tag == "Water")
                        {
                            if(inventory.waterVesselItems.Count > 0)
                            {
                                for (int i = 0; i < inventory.waterVesselItems.Count; i++)
                                {
                                    Item_WaterVessel vessel = (Item_WaterVessel)inventory.waterVesselItems[i];
                                    vessel.Fill(10000);
                                    vessel.full = true;
                                }
                            }
                            else
                                player_bodyManager.hydration += (100*500/ playerStats.GetStat<RPGAttribute>(RPGStatType.Weight).StatValue);
                        }
                        else if (hit.collider.gameObject.tag == "LitFire" || hit.collider.gameObject.tag == "UnlitFire")
                        {
                            //  player_bodyManager.StartFire(hitFire, 1);
                            Fire fire = hit.collider.gameObject.GetComponentInParent<Fire>();
                            fireUIPanel.SetActive(true);
                            fireButton.currentFire = fire;
                            fireButton.currentFireInventory = fire.fireLoot;
                            playerCrafting.isCrafting = true;
                            //butcheringUI.gameObject.SetActive(true);
                            lootInventory = fire.fireLoot;
                            lootInventory.inventoryUIPanel = firePanel;
                            lootInventory.RebuildUIPanel(this);
                            showInventory = true;
                            lootPanelScript.attachedInventory = fire.fireLoot;
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    player_bodyManager.weaponReadied = !player_bodyManager.weaponReadied;
                    player_bodyManager.meleeWeaponDrawn = !player_bodyManager.meleeWeaponDrawn;
                    anim.SetBool("ArmsRaised", !anim.GetBool("ArmsRaised"));
                }
            }//--^^--inventory UI closed--^^--

            if (Input.GetKeyDown(KeyCode.Q))
            {
                autoMove = !autoMove;
                if (autoMove)
                {
                    player_bodyManager.moving = true;
                    if (player_bodyManager.gait <= 1)
                    {
                        anim.SetBool("isWalking", true);
                        anim.SetBool("isRunning", false);
                    }
                    else
                    {
                        anim.SetBool("isWalking", true);
                        anim.SetBool("isRunning", true);
                    }
                }

            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (player_bodyManager.gait <= 1)
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isRunning", false);
                }
                else
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isRunning", true);
                }
                player_bodyManager.moving = true;
                autoMove = false;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                player_bodyManager.moving = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("isSprinting", true);
                player_bodyManager.sprinting = true;
                player_bodyManager.SprintTime = Time.time;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isSprinting", false);
                player_bodyManager.sprinting = false;
                float sprintTime = Time.time - player_bodyManager.SprintTime;
                playerStats.GetStat<RPGAttribute>(RPGStatType.Strength).TrainingValue += (int)(sprintTime / 3f);
                playerStats.GetStat<RPGAttribute>(RPGStatType.Speed).TrainingValue += (int)(sprintTime);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                print("crouch");
                crouching = !crouching;
                player_bodyManager.crouching = crouching;
                anim.SetBool("isCrouching", crouching);
                //ScaleCapsuleForCrouching(crouching);
                //if (!crouching)
                //{
                //    print("crouch");
                //    crouching = true;
                //    anim.SetBool("isCrouching", true);
                //}
                //else
                //{
                //    print("uncrouch");
                //    crouching = false;
                //    anim.SetBool("isCrouching", false);
                //}
            }

            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                player_bodyManager.NextGait();
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }

            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;


            if (showInventory)
            {
                if (inventoryActive)
                {
                    if (Input.GetKey(KeyCode.E) && Input.GetMouseButtonDown(0))
                    {
                        if (inventory.selectedItem is Item_Weapon)
                        {
                            Item_Weapon selectedItem = (Item_Weapon)inventory.selectedItem;

                            if (!selectedItem.wielded)
                            {
                                player_bodyManager.DrawWeapon(selectedItem);
                            }
                            else if (selectedItem.wielded)
                            {
                                player_bodyManager.SheatheWeapon(selectedItem);
                            }
                        }
                    }
                    if (Input.GetKey(KeyCode.E) && Input.GetMouseButtonDown(1))
                    {
                        if (inventory.selectedItem is Item_Weapon)
                        {
                            Item_Weapon selectedItem = (Item_Weapon)inventory.selectedItem;

                            if (!selectedItem.wielded)
                            {
                                player_bodyManager.OffHandDrawWeapon(selectedItem);
                            }
                            else if (selectedItem.wielded)
                            {
                                player_bodyManager.SheatheWeapon(selectedItem);
                            }
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        terrainMode = false;
                        //if (inventory.selectedItem != null)
                        //{ }
                        if (inventory.selectedItem is Item_Garment)
                        {
                            Item_Garment selectedItem = (Item_Garment)inventory.selectedItem;
                            if (!selectedItem.equipped)
                            {
                                m_AudioSource.PlayOneShot(playerAudioManager.equip);
                                player_bodyManager.EquipWearable(selectedItem);
                            }
                            else if (selectedItem.equipped)
                            {
                                m_AudioSource.PlayOneShot(playerAudioManager.equip);
                                player_bodyManager.RemoveGarment(selectedItem);
                            }
                        }
                        else if (inventory.selectedItem is Item_Weapon)
                        {
                            Item_Weapon selectedItem = (Item_Weapon)inventory.selectedItem;

                            if (selectedItem.wielded)
                            {
                                player_bodyManager.SheatheWeapon(selectedItem);
                            }
                            else
                            {
                                if (player_bodyManager.rHandWeapon != null)
                                {
                                    player_bodyManager.DropItem(player_bodyManager.rHandWeapon);
                                }
                                if (!selectedItem.wielded)
                                {
                                    player_bodyManager.DrawWeapon(selectedItem);
                                }
                                else if (selectedItem.wielded)
                                {
                                    player_bodyManager.SheatheWeapon(selectedItem);
                                }
                            }
                        }
                        else if (inventory.selectedItem is Item_Food)
                        {
                            Item_Food selectedFood = (Item_Food)inventory.selectedItem;
                            player_bodyManager.Eat(selectedFood);
                        }

                        if (inventory.selectedItem is Item_Ammo)
                        {
                            Item_Ammo selectedAmmo = inventory.selectedItem.GetComponent<Item_Ammo>();
                            player_bodyManager.currentAmmo = selectedAmmo;
                            player_bodyManager.currentAmmoPrefab = selectedAmmo.itemPrefab;
                        }
                        //if (inventory.selectedItem.GetType().IsAssignableFrom(typeof(Item_WaterVessel)))
                        if (inventory.selectedItem is Item_WaterVessel)
                        {
                            Item_WaterVessel vessel = (Item_WaterVessel)inventory.selectedItem;
                            float x = vessel.content;
                            vessel.Fill(-500);
                            x = x - vessel.content;
                            player_bodyManager.hydration += (100 * x / playerStats.GetStat<RPGAttribute>(RPGStatType.Weight).StatValue);
                            m_AudioSource.PlayOneShot(playerAudioManager.drink);
                        }

                        if (inventory.selectedItem is Item_TerrainBlock)
                        {
                            terrainMode = true;
                            Item_TerrainBlock setItem = (Item_TerrainBlock)inventory.selectedItem;
                            setBlock = setItem.block;
                            buildingBlock = setItem;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        player_bodyManager.DropItem(inventory.selectedItem);
                    }
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        if (inventory.selectedItem == null || !lootingUI.gameObject.activeSelf)
                            return;
                        Item anItem = inventory.selectedItem;
                        player_bodyManager.DropItem(inventory.selectedItem);
                        lootInventory.AddItem(anItem);
                        anItem.itemUIElementScript.panel = (RectTransform)lootInventory.inventoryUIPanel;
                        anItem.itemUIElementScript.playerControls = this;
                        Destroy(anItem.itemUIelement.GetComponent<InventoryFocusPanel>());

                        //LootInventoryFocusPanel lifp = anItem.itemUIelement.AddComponent<LootInventoryFocusPanel>();
                        //lifp.attachedInventory = lootInventory;
                        //lifp.panel = (RectTransform)lootInventory.inventoryUIPanel;
                        //lifp.playerControls = this;
                        lootInventory.selectedItem = null;
                        inventory.selectedItem = null;
                    }
                }
                else if (lootActive)
                {
                    if (Input.GetKey(KeyCode.E) && Input.GetMouseButtonDown(0))
                    {
                        if (lootInventory.selectedItem is Item_Weapon)
                        {
                            Item_Weapon selectedItem = (Item_Weapon)lootInventory.selectedItem;
                            lootInventory.selectedItem = null;

                            if (!selectedItem.wielded)
                            {
                                player_bodyManager.DrawWeapon(selectedItem);
                            }
                            else if (selectedItem.wielded)
                            {
                                player_bodyManager.SheatheWeapon(selectedItem);
                            }

                        }
                    }
                    if (Input.GetKey(KeyCode.E) && Input.GetMouseButtonDown(1))
                    {
                        if (lootInventory.selectedItem is Item_Weapon)
                        {
                            Item_Weapon selectedItem = (Item_Weapon)lootInventory.selectedItem;

                            if (!selectedItem.wielded)
                            {
                                player_bodyManager.OffHandDrawWeapon(selectedItem);
                            }
                            else if (selectedItem.wielded)
                            {
                                player_bodyManager.SheatheWeapon(selectedItem);
                            }
                        }
                    }

                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        if (lootInventory.selectedItem == null)
                            return;

                        //if (lootInventory.selectedItem != null)
                        //{ }
                        if (lootInventory.selectedItem is Item_Garment)
                        {
                            Item_Garment selectedItem = (Item_Garment)lootInventory.selectedItem;
                            if (!selectedItem.equipped)
                            {
                                player_bodyManager.EquipWearable(selectedItem);
                                m_AudioSource.PlayOneShot(playerAudioManager.equip);
                            }
                            else if (selectedItem.equipped)
                            {
                                player_bodyManager.RemoveGarment(selectedItem);
                                m_AudioSource.PlayOneShot(playerAudioManager.equip);
                            }
                        }

                        if (lootInventory.selectedItem is Item_Weapon)
                        {
                            Item_Weapon selectedItem = (Item_Weapon)lootInventory.selectedItem;

                            if (selectedItem.wielded)
                            {
                                player_bodyManager.SheatheWeapon(selectedItem);
                                m_AudioSource.PlayOneShot(playerAudioManager.sheathe);
                            }

                            else if (player_bodyManager.rHandWeapon == null)
                            {
                                if (!selectedItem.wielded)
                                {
                                    player_bodyManager.DrawWeapon(selectedItem);
                                    m_AudioSource.PlayOneShot(playerAudioManager.draw);
                                }
                                else if (selectedItem.wielded)
                                {
                                    player_bodyManager.SheatheWeapon(selectedItem);
                                    m_AudioSource.PlayOneShot(playerAudioManager.sheathe);
                                }
                            }
                            else if (player_bodyManager.lHandWeapon == null)
                            {
                                if (!selectedItem.wielded)
                                {
                                    player_bodyManager.OffHandDrawWeapon(selectedItem);
                                    m_AudioSource.PlayOneShot(playerAudioManager.draw);
                                }
                                else if (selectedItem.wielded)
                                {
                                    player_bodyManager.SheatheWeapon(selectedItem);
                                    m_AudioSource.PlayOneShot(playerAudioManager.sheathe);
                                }
                            }
                        }

                        if (lootInventory.selectedItem is Item_Ammo)
                        {
                            Item_Ammo selectedAmmo = lootInventory.selectedItem.GetComponent<Item_Ammo>();
                            player_bodyManager.currentAmmo = selectedAmmo;
                            player_bodyManager.currentAmmoPrefab = selectedAmmo.itemPrefab;
                        }

                        if (lootInventory.selectedItem is Item_TerrainBlock)
                        {
                            terrainMode = true;
                            Item_TerrainBlock setItem = (Item_TerrainBlock)lootInventory.selectedItem;
                            setBlock = setItem.block;
                            buildingBlock = setItem;
                        }
                        Destroy(lootInventory.selectedItem.itemUIelement.GetComponent<LootInventoryFocusPanel>());
                        lootInventory.RemoveItem(lootInventory.selectedItem);
                        player_bodyManager.PickupItem(lootInventory.selectedItem);
                        terrainMode = false;
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        if (lootInventory.selectedItem == null)
                            return;
                        lootInventory.selectedItem.loose = true;
                        lootInventory.selectedItem.gameObject.SetActive(true);
                        lootInventory.selectedItem.transform.position = this.transform.position;
                        lootInventory.selectedItem.transform.parent = null;
                        Destroy(lootInventory.selectedItem.itemUIelement.GetComponent<LootInventoryFocusPanel>());
                        lootInventory.RemoveItem(lootInventory.selectedItem);
                        lootInventory.selectedItem = null;

                    }
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        if (lootInventory.selectedItem == null)
                            return;
                        Item anItem = lootInventory.selectedItem;
                        Destroy(anItem.itemUIelement.GetComponent<LootInventoryFocusPanel>());
                        lootInventory.RemoveItem(lootInventory.selectedItem);
                        player_bodyManager.PickupItem(lootInventory.selectedItem);
                        InventoryFocusPanel ifp = anItem.itemUIelement.AddComponent<InventoryFocusPanel>();
                        ifp.attachedInventory = inventory;
                        ifp.panel = (RectTransform)inventory.inventoryUIPanel;
                        ifp.playerControls = this;
                        lootInventory.selectedItem = null;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (showInventory)
                {
                    modelViewObject.SetActive(false);
                    modelViewUI.gameObject.SetActive(false);
                    player_bodyManager.baseInventory.selectedItem = null;
                    inventoryUIPanel.gameObject.SetActive(false);
                    lootInventory = null;
                    lootingUI.gameObject.SetActive(false);
                    fireUIPanel.SetActive(false);
                    showInventory = false;
                    playerCrafting.isCrafting = false;
                    m_AudioSource.PlayOneShot(playerAudioManager.openInventory);
                    Cursor.lockState = CursorLockMode.Locked;

                }
                else
                {
                    modelViewObject.SetActive(true);
                    modelViewUI.gameObject.SetActive(true);
                    inventoryUIPanel.gameObject.SetActive(true);
                    showInventory = true;
                    m_AudioSource.PlayOneShot(playerAudioManager.openInventory);
                    Cursor.lockState = CursorLockMode.None;

                }
            }

            if(Input.GetKeyDown(KeyCode.L))
            {
                if (actionUI.activeSelf)
                {
                    //actionUI.GetComponent<ActionUIScript>().panelActive = false;
                    actionUI.SetActive(false);
                    //Destroy(outfittingUI.GetComponent<OutfitUIScript>());
                    return;
                }
                if (!actionUI.activeSelf)
                {
                    //actionUI.GetComponent<ActionUIScript>().panelActive = true;
                    actionUI.SetActive(true);
                    //outfittingUI.AddComponent<OutfitUIScript>();
                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                if (characterUI.activeSelf)
                {
                    characterUI.SetActive(false);
                    return;
                }
                if (!characterUI.activeSelf)
                {
                    characterUI.SetActive(true);
                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                if (craftingUI.activeSelf)
                {
                    craftingUI.SetActive(false);
                    playerCrafting.isCrafting = false;
                    playerCrafting.selectedRecipe = null;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    craftingUI.SetActive(true);
                    playerCrafting.isCrafting = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        } //end of Update


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;

            //bool crouch = Input.GetKey(KeyCode.C); //re-enable these lines for toggle crouching

            ScaleCapsuleForCrouching(crouching);
            //PreventStandingInLowHeadroom();

            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                    playerStats.GetStat<RPGAttribute>(RPGStatType.Agility).TrainingValue += 5;
                    playerStats.GetStat<RPGAttribute>(RPGStatType.Strength).TrainingValue += 1;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        m_MouseLook.UpdateCursorLock();
        }

        public LayerMask mask1, mask2;
        public bool falling = false;

        private void LateUpdate()
        {
            //m_Camera = Camera.main.GetComponent<Camera>();

            shoulderGirdle.transform.Rotate(m_Camera.transform.rotation.eulerAngles.x, 0, 0);
        }

            //Debug.DrawLine(this.transform.position, hit.point, Color.blue);
            //    if (!falling)
            //    {
            //        falling = true;
            //        m_Rigidbody.constraints = RigidbodyConstraints.None;
            //        m_GravityMultiplier = 3;
            //    }
            //}
            //else
            //{
            //    if (falling)
            //    {
            //        falling = false;
            //        m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            //        m_GravityMultiplier = 0;
            //    }
            //}


        void OnGUI()
            {
                for(int i = 0; i< player_bodyManager.outfit.Count; i++)
                {
                    GUI.Label(new Rect(800, 375 + i*25, 500, 500), player_bodyManager.outfit[i].itemName);
                }
            }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }

        IEnumerator PauseMovement()
        {
            print("pausing");
            //Backup and clear velocities
            Vector3 linearBackup = m_Rigidbody.velocity;
            Vector3 angularBackup = m_Rigidbody.angularVelocity;
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;

            //Finally freeze the body in place so forces like gravity or movement won't affect it
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            //Wait for a bit (two seconds)
            yield return new WaitForSeconds(2);
            //And unfreeze before restoring velocities
            m_Rigidbody.constraints = RigidbodyConstraints.None;
            //restore the velocities
            m_Rigidbody.velocity = linearBackup;
            m_Rigidbody.angularVelocity = angularBackup;
        }

        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsNotSprinting ? 1f : m_RunstepLengthen))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            //PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        void ScaleCapsuleForCrouching(bool crouch)
        {
            if (crouch)
            {
                m_CharacterController.height = m_CharacterController.height / 2f;
                m_CharacterController.center = m_CharacterController.center / 2f;
            }
            else
            {
                //Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_CharacterController.radius * k_Half, Vector3.up);
                //float crouchRayLength = m_CharacterControllerHeight - m_CharacterController.radius * k_Half;
                //if (Physics.SphereCast(crouchRay, m_CharacterController.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                //{
                //    crouching = true;
                //    return;
                //}
                m_CharacterController.height = m_CharacterControllerHeight;
                m_CharacterController.center = m_CharacterControllerCenter;
                //crouching = false;
            }
        }

        void PreventStandingInLowHeadroom()
        {
            // prevent standing up in crouch-only zones
            if (!crouching)
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_CharacterController.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CharacterControllerHeight - m_CharacterController.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_CharacterController.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                        crouching = true;
                    anim.SetBool("isCrouching", crouching);
                }
            }
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed * (m_IsNotSprinting ? 1f : m_RunstepLengthen)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            float vertical, horizontal = 0;
            if (autoMove)
            {
                vertical = 1;
            }
            else
            {
                // Read input
                horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
                vertical = CrossPlatformInputManager.GetAxis("Vertical");
            }

            bool waswalking = m_IsNotSprinting;


            #if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsNotSprinting = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsNotSprinting ? player_bodyManager.speed : m_RunSpeed;
            //speed = falling ? speed : 0;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsNotSprinting != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsNotSprinting ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }

        public float JumpSpeed
        {
            get { return m_JumpSpeed; }
            set { m_JumpSpeed = value; }
        }

        public ModelViewer modelViewer;
        
        public void SetLootActive()
        {
            if (lootInventory.selectedItem != null)
            {
                GameObject model = Instantiate(lootInventory.selectedItem.itemModel);
                modelViewer.ChangeModel(model);
                model.transform.SetParent(modelViewUI.gameObject.transform);
                model.transform.localPosition = Vector3.zero;
            }
            lootActive = true;
            inventoryActive = false;
        }

        public void SetInventoryActive()
        {
            if (inventory.selectedItem != null)
            {
                GameObject model = Instantiate(inventory.selectedItem.itemModel);
                modelViewer.ChangeModel(model);
                model.transform.SetParent(modelViewUI.gameObject.transform);
                model.transform.localPosition = Vector3.zero;
            }
            inventoryActive = true;
            lootActive = false;
        }

        void SetJumpSpeed()
        {
            jump = playerStats.GetStat<RPGDerived>(RPGStatType.JumpHeight);
            m_JumpSpeed = (float)(jump.StatValue)/10;
        }

        public void RevertCam()
        {

        }

        void OnDeserialized()
        {
            m_FovKick.Setup(m_Camera);
            m_MouseLook.Init(transform, m_Camera.transform);
        }
    }
}
