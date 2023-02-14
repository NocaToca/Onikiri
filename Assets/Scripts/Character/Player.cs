using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Handles all logic regarding the player

    For input look at PlayerController
*/
[RequireComponent(typeof(PlayerController))]
public class Player : Actor
{
    
    public float speed;
    public float boost_speed;
    
    public Weapon main_hand;
    public Weapon off_hand;

<<<<<<< Updated upstream
=======
    public List<Augment> augments;

    //Our mana
    public Mana kitsunebi;

    //Holds the functions that we call do update
    [HideInInspector]
    public UnityEvent update_boons; 

>>>>>>> Stashed changes

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        actor_controller = this.GetComponent<PlayerController>();
        main_hand.holding_actor = this;
<<<<<<< Updated upstream
=======

        //We have 9 mana
        kitsunebi = new Mana(9);

        augments.Add(this.gameObject.AddComponent<Dash>());
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AttemptAttack(){
        if(main_hand != null){
            main_hand.Attack();
        }

        return main_hand != null;
    }
}

//Mana class
public class Mana{

    static List<float[]> list_of_all_mana;
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