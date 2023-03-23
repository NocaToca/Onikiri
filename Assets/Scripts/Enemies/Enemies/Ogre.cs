using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : Enemy
{

    public ActionCollider club_collider;
    public ActionCollider slam_collider;
    public ActionCollider roar_collider;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClubSlam(){
        playing_action = true;
    }

    public void BaseSlam(){
        playing_action = true;
    }

    public void Roar(){
        playing_action = true;
    }

    void OnValidate(){
        if(debug){
            if(roar_collider != null){
                roar_collider.debug_display = true;
            }
            if(club_collider != null){
                club_collider.debug_display = true;
            }
            if(club_collider != null){
                slam_collider.debug_display = true;
            }
        }
    }
}
