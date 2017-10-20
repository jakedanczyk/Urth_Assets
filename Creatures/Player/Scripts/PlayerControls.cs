using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{ 
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControls : MonoBehaviour
    {
        public World thisWorld;
        public BodyManager_Human_Player pc_bodyManager;
        //public RPGCreature player;
        public RPGStatCollection playerStats;
        public RPGStat jump;
        public float f1;
        public bool crouching, autoMove, aboveDetailedChunk;
        public Rigidbody m_Rigidbody;
        public Animator anim;
        public GameObject shoulderGirdle;
        public GameObject inventoryUIPanel, craftingUI;
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

            public GameObject outfittingUI;

        public Inventory playerInventory;
        public CraftingSystem playerCrafting;

        public GameObject butcheringUI;

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
        private MouseLook m_MouseLook;
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
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        Camera m_Camera;

        CharacterController characterController;
        public StaticBuildingSystem buildingSystem;
        public BlockBuildingSystem blockBuildingSystem;

        private void Awake()
        {
            crouching = false;
            lastPosition = this.transform.position;
        }

        // Use this for initialization
        private void Start()
        {
            m_Camera = Camera.main.GetComponent<Camera>();
            characterController = GetComponent<CharacterController>();
            playerStats = GetComponent<HumanDefaultStats>();
            jump = playerStats.GetStat<RPGDerived>(RPGStatType.JumpHeight);
            f1 = (float)(jump.StatValue);
            m_JumpSpeed = (float)(jump.StatValue);
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
            m_CharacterControllerHeight = m_CharacterController.height;
            m_CharacterControllerCenter = m_CharacterController.center;
            playerInventory = pc_bodyManager.inventory;
        }

        public Vector3 lastPosition;
        bool check;

        // Update is called once per frame
        private void Update()
        {

                RaycastHit ground;
                if (Physics.Raycast(this.transform.position, Vector3.down, out ground, 512f, mask1))
                {
                    aboveDetailedChunk = true;
                    lastPosition = this.transform.position;
                }
                else if (Physics.Raycast(this.transform.position, Vector3.down, out ground, 512f, mask2))
                {
                    aboveDetailedChunk = false;
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

            if (!showInventory)
            {
                RotateView();

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (pc_bodyManager.meleeWeaponDrawn)
                    {
                        pc_bodyManager.MainAttack();
                    }

                    if (pc_bodyManager.rangedWeaponDrawn)
                    {
                        pc_bodyManager.KnockArrow();
                    }

                    if (terrainMode)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(pc_bodyManager.aimPoint.position, pc_bodyManager.aimPoint.forward, out hit, 12f))
                        {
                            WorldPosFloat pos1 = EditTerrain.GetBlockPos(hit);
                            Vector3 direction = pc_bodyManager.aimPoint.position - hit.point;
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

                if (Input.GetKeyDown(KeyCode.Mouse1) && pc_bodyManager.wieldingBow && pc_bodyManager.arrowKnocked && !pc_bodyManager.bowDrawn)
                {
                    pc_bodyManager.DrawBow();
                }

                if (Input.GetKeyUp(KeyCode.Mouse1) && pc_bodyManager.drawingBow)
                {
                    pc_bodyManager.ReleaseBow();
                }

                if (Input.GetKeyDown(KeyCode.Mouse0) && pc_bodyManager.drawingBow)
                {
                    pc_bodyManager.FireBow();
                }

                if (Input.GetKeyDown(KeyCode.E)) //pickup, interact
                {
                    RaycastHit hit;
                    if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, 20f))
                    {
                        string s = hit.collider.gameObject.tag;
                        print(s);
                        print(pc_bodyManager.rHandWeapon);
                        if (hit.collider.gameObject.tag == "Item")
                        {
                            Item hitItem = hit.collider.gameObject.GetComponent<Item>();
                            pc_bodyManager.PickupItem(hitItem);
                            //Destroy(hit.collider.gameObject);
                            return;
                        }
                        if (hit.collider.gameObject.tag == "Tree" && (pc_bodyManager.rHandWeapon is Item_Weapon_Axe || pc_bodyManager.rHandWeapon is Item_Weapon_Hatchet))
                        {
                            Tree hitTree = hit.collider.gameObject.GetComponent<Tree>();
                            pc_bodyManager.FellTree(hitTree, (Item_Weapon_Axe)pc_bodyManager.rHandWeapon);
                            return;
                        }
                        if (hit.collider.gameObject.tag == "Perennial")// && pc_bodyManager.rHandWeapon.gameObject == null)
                        {
                            Perennial hitPerennial = hit.collider.gameObject.GetComponent<Perennial>();
                            pc_bodyManager.GatherPlant(hitPerennial);
                            return;
                        }
                        if (hit.collider.gameObject.tag == "FelledTree" && pc_bodyManager.rHandWeapon is Item_Weapon_Axe)
                        {
                            Tree hitTree = hit.collider.gameObject.GetComponent<Tree>();
                            pc_bodyManager.ProcessDownedTree(hitTree, (Item_Weapon_Axe)pc_bodyManager.rHandWeapon);
                            return;
                        }
                        if (hit.collider.gameObject.tag == "StackContainer")
                        {
                            StackContainer hitStack = hit.collider.gameObject.GetComponentInParent<StackContainer>();
                            print(hitStack.name);
                            hitStack.PullItem(playerInventory);
                            return;
                        }
                        if (hit.collider.gameObject.tag == "UnlitFire")
                        {
                            Fire hitFire = hit.collider.gameObject.GetComponentInParent<Fire>();
                            pc_bodyManager.StartFire(hitFire, 1);
                            return;
                        }
                        if (hit.collider.gameObject.tag == "DeadCreature")
                        {
                            butcheringUI.gameObject.SetActive(true);
                            BodyManager hitBody = hit.collider.gameObject.GetComponentInParent<BodyManager>();
                            return;
                        }
                    }
                }
            }//--^^--inventory UI closed--^^--

            if (Input.GetKeyDown(KeyCode.Q))
            {
                autoMove = !autoMove;
                if (autoMove)
                {
                    pc_bodyManager.moving = true;
                    if (pc_bodyManager.gait <= 1)
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
                if (pc_bodyManager.gait <= 1)
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isRunning", false);
                }
                else
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isRunning", true);
                }
                pc_bodyManager.moving = true;
                autoMove = false;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                pc_bodyManager.moving = false;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("isSprinting", true);
                pc_bodyManager.sprinting = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isSprinting", false);
                pc_bodyManager.sprinting = false;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                print("crouch");
                crouching = !crouching;
                pc_bodyManager.crouching = crouching;
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
                pc_bodyManager.NextGait();
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    terrainMode = false;

                    if (playerInventory.selectedItem is Item_Garment)
                    {
                        Item_Garment selectedItem = (Item_Garment)playerInventory.selectedItem;
                        if (!selectedItem.equipped)
                        {
                            pc_bodyManager.EquipWearable(selectedItem);
                        }
                        else if (selectedItem.equipped)
                        {
                            pc_bodyManager.RemoveGarment(selectedItem);
                        }
                    }

                    if (playerInventory.selectedItem is Item_Weapon)
                    {
                        pc_bodyManager.DrawWeapon((Item_Weapon)playerInventory.selectedItem);
                    }

                    if (playerInventory.selectedItem is Item_Ammo)
                    {
                        Item_Ammo selectedAmmo = playerInventory.selectedItem.GetComponent<Item_Ammo>();
                        pc_bodyManager.currentAmmo = selectedAmmo;
                        pc_bodyManager.currentAmmoPrefab = selectedAmmo.itemPrefab;
                    }

                    if (playerInventory.selectedItem is Item_TerrainBlock)
                    {
                        terrainMode = true;
                        Item_TerrainBlock setItem = (Item_TerrainBlock)playerInventory.selectedItem;
                        setBlock = setItem.block;
                        buildingBlock = setItem;
                    }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    pc_bodyManager.DropItem(playerInventory.selectedItem);
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUIPanel.activeSelf)
                {
                    pc_bodyManager.inventory.selectedItem = null;
                    inventoryUIPanel.SetActive(false);
                    showInventory = false;
                }
                else
                {
                    inventoryUIPanel.SetActive(true);
                    showInventory = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                if (outfittingUI.activeSelf)
                {
                    outfittingUI.GetComponent<OutfittingUIScript>().panelActive = false;
                    outfittingUI.SetActive(false);
                    //Destroy(outfittingUI.GetComponent<OutfitUIScript>());
                    return;
                }
                if (!outfittingUI.activeSelf)
                {
                    outfittingUI.GetComponent<OutfittingUIScript>().panelActive = true;
                    outfittingUI.SetActive(true);
                    //outfittingUI.AddComponent<OutfitUIScript>();
                    return;
                }
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                if (craftingUI.activeSelf)
                {
                    craftingUI.SetActive(false);
                    playerCrafting.isCrafting = false;
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
                for(int i = 0; i<pc_bodyManager.outfit.Count; i++)
                {
                    GUI.Label(new Rect(800, 375 + i*25, 500, 500), pc_bodyManager.outfit[i].itemName);
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
            speed = m_IsNotSprinting ? pc_bodyManager.speed : m_RunSpeed;
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
    }
}
