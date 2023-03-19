using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class OgreSelector : Selector{

    public OgreSelector(Ogre ogre, OgreSettings settings) : base(){
        SetData("ogre", ogre);
        SetData("ogre_settings", settings);
    }

    public OgreSelector(Ogre ogre, OgreSettings settings, List<AINode> children) : base(children){
        SetData("ogre", ogre);
        SetData("ogre_settings", settings);
    }
}

public class ClubSlamCheck : AINode{
    
    private float wait_time;

    public ClubSlamCheck() : base(){
        wait_time = 0.0f;
    }

    //Basically, if the player is within the club action we can return success and run the club
    public override Status Evaluate(){

        OgreSettings settings = (OgreSettings)GetData("ogre_settings");
        Ogre ogre = (Ogre)GetData("ogre");

        if(ogre.playing_action){
            return FAIL();
        }

        if(wait_time < settings.club_cooldown){
            wait_time += Game.tick;
            return FAIL();
        } else {
            if(ogre.club_collider == null){
                return FAIL();
            }

            if(ogre.club_collider.is_player_in_action_collider){
                wait_time = 0.0f;
                return SUCCESS();
            }
        }

        return FAIL();
    }
}

public class ClubSlam : AINode{

    public override Status Evaluate(){
        OgreSettings settings = (OgreSettings)GetData("ogre_settings");
        Ogre ogre = (Ogre)GetData("ogre");

        //We're just going to tell the ogre to slam their club
        ogre.ClubSlam();

        return RUNNING();
    }

}

public class OgreRoarCheck : AINode{
    private float wait_time;

    public OgreRoarCheck() : base(){
        wait_time = 0.0f;
    }

    //Basically, if the player is within the club action we can return success and run the club
    public override Status Evaluate(){

        OgreSettings settings = (OgreSettings)GetData("ogre_settings");
        Ogre ogre = (Ogre)GetData("ogre");

        if(ogre.playing_action){
            return FAIL();
        }

        if(wait_time < settings.roar_cooldown){
            wait_time += Game.tick;
            return FAIL();
            
        } else {
            if(ogre.roar_collider == null){
                return FAIL();
            }
            if(ogre.roar_collider.is_player_in_action_collider){
                wait_time = 0.0f;
                return SUCCESS();
            }
        }

        return FAIL();
    }
}

public class OgreRoar : AINode{
    public override Status Evaluate(){
        OgreSettings settings = (OgreSettings)GetData("ogre_settings");
        Ogre ogre = (Ogre)GetData("ogre");

        //We're just going to tell the ogre to slam their club
        ogre.Roar();

        return RUNNING();
    }
}

public class OgreSlamCheck : AINode{
private float wait_time;

    public OgreSlamCheck() : base(){
        wait_time = 0.0f;
    }

    //Basically, if the player is within the club action we can return success and run the club
    public override Status Evaluate(){

        OgreSettings settings = (OgreSettings)GetData("ogre_settings");
        Ogre ogre = (Ogre)GetData("ogre");

        if(ogre.playing_action){
            return FAIL();
        }

        if(wait_time < settings.slam_cooldown){
            wait_time += Game.tick;
            return FAIL();
        } else {
            if(ogre.slam_collider == null){
                return FAIL();
            }
            if(ogre.slam_collider.is_player_in_action_collider){
                wait_time = 0.0f;
                return SUCCESS();
            }
        }

        return FAIL();
    }

}

public class OgreSlam : AINode{
    public override Status Evaluate(){
        OgreSettings settings = (OgreSettings)GetData("ogre_settings");
        Ogre ogre = (Ogre)GetData("ogre");

        //We're just going to tell the ogre to slam their club
        ogre.BaseSlam();

        return RUNNING();
    }
}

public struct OgreSettings{
    public float club_cooldown;
    public float roar_cooldown;
    public float slam_cooldown;

    public OgreSettings(float club_cooldown, float roar_cooldown, float slam_cooldown){
        this.club_cooldown = club_cooldown;
        this.roar_cooldown = roar_cooldown;
        this.slam_cooldown = slam_cooldown;
    }

}
