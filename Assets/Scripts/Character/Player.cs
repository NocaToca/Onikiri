using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Augments;

/*
    Handles all logic regarding the player

    For input look at PlayerController
*/
[RequireComponent(typeof(PlayerController))]
public class Player : Actor
{
    [Header("Movement")]
    [Tooltip("Base speed of the character")]
    public float speed;

    [Tooltip("Boosted speed of the character when sprinting")]
    public float boost_speed;
    

    [Header("Weapons")]
    [Tooltip("Main hand weapon")]
    public Weapon main_hand;

    [Tooltip("Offhand weapons")]
    public Weapon off_hand;

    [Header("Augment Trees")]
    public SpellTree first_tree;
    public SpellTree second_tree;
    public BoonTree[] boon_trees;

    [Header("Dash Tree")]
    public SpellTree dash_tree;

    public Dash dash_root;

    public ActionCollider sword_collider;

    //Calls colission effects to anything interested
    UnityEvent collision_listener;

    //Enables damaging others when the player colliders with them
    bool on_enter_damage;

    //public List<Augment> augments;

    //Our mana
    public Mana kitsunebi;

    public bool attacking = false;

    //Holds the functions that we call do update

    // Start is called before the first frame update\\

    //On Awake, we want to initialize the augment trees
    void Awake(){

        //Grabbing our base tree prefab, and if it is not null creates it
        GameObject tree_prefab = null;
        if(first_tree != null){
            tree_prefab = first_tree.gameObject;
            if(tree_prefab == this.gameObject){
                Debug.LogError("Please put skill trees and boon trees on seperate game objects to the player. Thank you!");
            }
        } else {

            //Our tree is on a new gameobject we make that is a child to this
            tree_prefab = new GameObject();
            GameObject t = Instantiate(tree_prefab);
            t.transform.SetParent(this.transform);
            first_tree = t.AddComponent<SpellTree>();
            t.gameObject.name = "Spell Tree One";
        }

        //Basically the same process for the rest of the Augment trees
        GameObject t2 = Instantiate(tree_prefab);
        t2.transform.SetParent(this.transform);
        second_tree = t2.AddComponent<SpellTree>();
        t2.gameObject.name = "Spell Tree Two";

        GameObject t3 = Instantiate(tree_prefab);
        t3.transform.SetParent(this.transform);
        dash_tree = t3.AddComponent<SpellTree>();
        t3.gameObject.name = "Dash Spell Tree";

        boon_trees = new BoonTree[3];
        for(int i = 0; i < 3; i++){
            GameObject t_boon = Instantiate(tree_prefab);
            t_boon.transform.SetParent(this.transform);
            boon_trees[i] = t_boon.AddComponent<BoonTree>();
            t_boon.gameObject.name = "Boon Tree " + i;
        }
    }

    protected override void Start()
    {
        base.Start();
        //augments = new List<Augment>();
        actor_controller = this.GetComponent<PlayerController>();
        main_hand.holding_actor = this;

        //We have 9 mana
        kitsunebi = new Mana(9);


        //augments.Add(this.gameObject.AddComponent<Dash>());
        //augments.Add(this.gameObject.AddComponent<SpeedIncrease>());
    }

    public void Dash(){
        if(dash_tree.root.Name() == "N/A"){
            dash_tree.root = new Dash(50.0f, 5.0f, this);

        }
            Debug.Log("2");
        dash_tree.Poke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapWeapons(){
        Weapon temp = main_hand;
        main_hand = off_hand;
        off_hand = temp;
        main_hand.holding_actor = this;
    }

    //Updates our trees with the correlated children; if there are multiple it will do the first
    public void UpdateTrees(Node n){

        if(n is SkillNode){
            first_tree.root = n;
        } else {
            boon_trees[0].root = n;
        }

    }

    private void OnTriggerEnter2D(Collider2D col){
        //collision_listener.Invoke();
        if(on_enter_damage){
            if(col.gameObject.tag == "Enemy"){
                Actor a = ExtractActor(col.gameObject);
                Debug.Log("Damaged " + a.name);
                a.TakeDamage(20.0f);
            }
        }
    }

    //Toggle functions (used as listeners for skills)
    public void ToggleMovement(){
        actor_controller.accepting_movement = !actor_controller.accepting_movement;
    }
    public void ToggleMovement(bool status){
        actor_controller.accepting_movement = status;
    }
    public void ToggleOnEnterDamage(){
        on_enter_damage = !on_enter_damage;
    }
    public void ToggleOnEnterDamage(bool status){
        on_enter_damage = status;
    }
    //Easiest thing would be to turn off collider, but this could cause problems if we need for otherthings
    public void ToggleCollisionImmunity(){
        immune = !immune;
    }
    public void ToggleCollisionImmunity(bool status){
        immune = status;
    }
    //Used for main collider, not the trigger collider
    public void ToggleCollider(){
        immune = !immune;
    }
    public void ToggleCollider(bool status){
        immune = status;
    }

    //Gets all of our augment trees and then returns them easily
    public List<AugmentTree> GetAugmentTrees(){
        List<AugmentTree> trees = new List<AugmentTree>();

        if(first_tree == null){
            Debug.LogWarning("First Tree is null");
        }

        trees.Add(first_tree);
        trees.Add(second_tree);
        foreach(BoonTree tree in boon_trees){
            if(tree == null){
                Debug.LogWarning("Returning null boon tree");
            }
            trees.Add(tree);
        }

        return trees;
    }

    // //Sends information to the controller to add the keycode in regards to the event and weapon types
    // public void AddControllerListenerForWeapons(List<WeaponPass> weapon_types, KeyCode key, UnityEvent event){

    // }

    // public void RefreshBoons(){
    //     foreach(Augment aug in augments){
    //         if(aug is Boon){
    //             Boon boon_cast = (Boon)aug;
    //             boon_cast.ActivateBoon(this);
    //         }
    //     }
    // }

    // public bool AttemptAugmentAbility(){
    //     foreach(Augment aug in augments){
    //         if(aug is Spell){
    //             Spell spell_cast = (Spell)aug;
    //             spell_cast.OnActivate();
    //             return true;
    //         }
    //     }

    //     return false;
    // }

    public float force_magnitude = 10.0f;
    public void SetPushbackForce(float mag){
        this.sword_collider.force = mag;
    }
    
    //Attempts an attack
    public void AttemptAttack(){
        if(main_hand != null){
            main_hand.Attack();
        }
    }

    public void AttemptAttack(float damage){
        if(main_hand != null){
            main_hand.Attack(damage);
        }
    }

    public void Respawn(){
        stats.health = stats.max_health;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<PlayerController>().ToggleMovement(true);
        this.transform.position = Vector3.zero;
    }

    protected override void Die(){
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<PlayerController>().ToggleMovement(false);
    }
}

//Mana class
public class Mana{

    public static List<float[]> list_of_all_mana;
    static float tick_rate;

    //We have a mana if the tick is above 1.0f 
    float[] mana;

    public Mana(int num_mana){
        mana = new float[num_mana];
        if(list_of_all_mana == null){
            list_of_all_mana = new List<float[]>();
        }

        list_of_all_mana.Add(mana);
    }

    public bool SpendMana(ManaCost num_mana){
        if(num_mana.mana_cost > mana.Length || num_mana.mana_cost > EvaluateAvailableMana()){
            return false;
        }

        DrainMana(num_mana);
        return true;

    }

    public void DrainMana(ManaCost num_mana){
        int num_to_drain = num_mana.mana_cost;

        for(int i = mana.Length - 1; num_to_drain > 0 && i >= 0; i--){
            if(mana[i] > 1.0f){
                mana[i] = 0.0f;
                num_to_drain--;
            }
        }
    }

    public int EvaluateAvailableMana(){
        int sum = 0;

        foreach(float mana_val in mana){
            if(mana_val > 1.0f){
                sum++;
            }
        }

        return sum;
    }

    public static void UpdateAllMana(){
        for(int x = 0; x < list_of_all_mana.Count; x++){
            for(int y = 0; y < list_of_all_mana[x].Length; y++){
                list_of_all_mana[x][y] += tick_rate;
            }
        }
    }

}

//A different struct for readability
public struct ManaCost{
    public int mana_cost;

    public ManaCost(int mana_cost){
        this.mana_cost = mana_cost;
    }

}