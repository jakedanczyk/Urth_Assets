using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

public abstract class BodyManager : MonoBehaviour {

    public List<Item_Garment> outfit;
    public Inventory inventory; // all carried items
    public LootInventory lootInventory;
    public Transform aimPoint, arrowReleasePoint;

    public Animator anim;
    public Item_Weapon primaryWeapon;
    public CreatureStats stats;
    public AudioManager audioManager;
    public AudioSource audioSource;
    public List<Collision> primaryAttackCollisionList = new List<Collision>();
    public List<Collision> offHandAttackCollisionList = new List<Collision>();
    public AudioClip deathGasp;

    //status bools
    public bool isAttackingPrimary, isAttackingSecondary, guardRaised, sneaking;
    public bool alive = true;
    public bool butchered = false;
    int mainWeaponAnimationLayer = 5;
    public IEnumerator onCompleteMainWeaponAttackAnimation;
    public GameObject ragdollPrefab,bodyModel;

    //Attack pseudo-code
    //calc damage, speed from:(weapon, attributes, status, skill)
    //Damage:
    //Speed:
    //run animation, use rays and colliders to build collisionList
    //if collision
    //      object type... Terrain, Construction, Item, SmallPlant, Tree, BodyPart ?
    //Terrain... Hard, Compact, Loose

    public void Awake()
    {
        onCompleteMainWeaponAttackAnimation = OnCompleteMainWeaponAttackAnimation();
    }

    void Start()
    {
        audioManager = AudioManager.audioManagerGameObject.GetComponent<AudioManager>();
        var health = stats.GetStat<RPGVital>(RPGStatType.Health);
        print(stats.GetStat<RPGVital>(RPGStatType.Health).StatName);
        health.OnCurrentValueChange += OnStatValueChange;
    }

    public IEnumerator OnCompleteMainWeaponAttackAnimation()
    {
        while(anim.GetCurrentAnimatorStateInfo(mainWeaponAnimationLayer).loop)
            yield return null;
        while (!anim.GetCurrentAnimatorStateInfo(mainWeaponAnimationLayer).loop)
            yield return null;
        isAttackingPrimary = false;
        primaryWeapon.weaponCollList[0].isActive = false;
        yield return null;
    }

    void OnStatValueChange(object sender, EventArgs args)
    {
        print("stat change");
        RPGVital vital = (RPGVital)sender;
        if (vital != null)
        {
            print(string.Format("Vital {0}'s OnStatValueChange event was triggered", vital.StatName));
            print(vital.StatCurrentValue);
        }
        if (vital.StatCurrentValue <= 0)
        {
            alive = false;
            anim.SetBool("isAlive", false);
        }
    }

    public void PickupItem(Item anItem)
    {
        inventory.AddItem(anItem);
        //anItem.loose = false;
        //        anItem.transform.SetParent(transform.parent);
        //anItem.gameObject.SetActive(false);
        //Destroy(hit.collider.gameObject);
    }

    public void DropItem(Item anItem)
    {
        if (anItem == null)
            return;
        inventory.selectedItem = null;
        inventory.RemoveItem(anItem);
        anItem.loose = true;
        anItem.gameObject.SetActive(true);
        anItem.transform.position = this.transform.position;
        anItem.transform.parent = null;
    }

    public int[] SendArmorNumbers(RPGStatType bodyPart)
    {
        int[] z = new int[] { 1, 1, 1 };
        z[0] = stats.GetStat<RPGBodyPart>(bodyPart).protection[0];
        z[1] = stats.GetStat<RPGBodyPart>(bodyPart).protection[1];
        z[2] = stats.GetStat<RPGBodyPart>(bodyPart).protection[2];
        return z;
   }

    public float encumbrance = 0; //percent 
    public float carryWeight;
    void Encumbrance()
    {
        encumbrance = (inventory.SumWeight() - (outfit.Select(c => c.itemWeight).ToList().Sum() * .5f)) / stats.GetStat<RPGAttribute>(RPGStatType.CarryWeight).StatValue;
        carryWeight = stats.GetStat<RPGAttribute>(RPGStatType.CarryWeight).StatValue;
    }

    public void Dodge() { }

    public void Deflect() { }

    public void Absorb() { }

    public List<GameObject> butcheringReturns; // Outside in. Feathers, skin/exoskeloton, fat, muscle, organs, bones
    public int butcherTime; // seconds
    public int butcherSkillFactor; // how variable is quality?
    public abstract void ProcessThisBody();
    public abstract void PrimaryAttack();

    public void Run()
    {
        anim.SetBool("isRunning", true);
    }

    public void Walk()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);
    }

    public void Idle()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
    }

    public virtual void Death()
    {
        GetComponent<LocalNavMeshBuilder>().enabled = false;
        GetComponent<AICharacterControl>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<ThirdPersonCharacter>().enabled = false;
        this.tag = "DeadCreature";
        alive = false;
        anim.enabled = false;
        audioSource.PlayOneShot(deathGasp);
        bodyModel.SetActive(false);
        GetComponentInParent<Rigidbody>().useGravity = false;
        Instantiate(ragdollPrefab, transform);
        StopAllCoroutines();
    }


    public AudioClip aggressiveBellow;
    public virtual void AggressiveBellow()
    {
        audioSource.PlayOneShot(aggressiveBellow);
    }
}
