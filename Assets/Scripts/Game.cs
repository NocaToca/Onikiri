using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls all game behavoir and management
public class Game : MonoBehaviour
{

    //An easy static way to get the player
    public static Player player {get{return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();}}

    public static Game game {get{return GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();}}

    //Used to normalize actor speed and other operations running on FixedUpdate()
    public static float normalizing_speed_constant = 0.01f;
    //Another name for it
    //public static float tick {get{return normalizing_speed_constant;}}

    public float mana_tick_rate = 0.01f;
    public static float tick = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int x = 0; x < Mana.list_of_all_mana.Count; x++){
            float[] current_array = Mana.list_of_all_mana[x];
            for(int y = 0; y < current_array.Length; y++){
                if(current_array[y] <= 1.0f){
                    current_array[y] += mana_tick_rate;
                    break;
                }
            }
        }
    }

    public void Kill(Actor a){
        StartCoroutine(DeathPartum(a));
    }
    

    IEnumerator DeathPartum(Actor a){
        a.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(2.5f);

        if(!a.puppet){
            //No mercy
            Destroy(a.gameObject);
        }
    }
}
