using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow", menuName = "Weapon/Bow")]
public class Bow : Weapon
{

    public GameObject arrow_prefab;

    public override void Attack(){
        base.Attack(); //Calls base attack function to do default actions
        FireProjectile();
    }

    //Fires our projectile stored in our bow
    public void FireProjectile(){
        Vector3 spawn_position = holding_actor.transform.position;
        Vector3 spawn_rotation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawn_rotation.z = spawn_position.z;
        float angle = 0.0f;
        if(spawn_rotation.y - spawn_position.y < 0 ){
            angle = -Mathf.Acos((spawn_rotation.x - spawn_position.x)/(spawn_position-spawn_rotation).magnitude);
        } else {
            angle = Mathf.Acos((spawn_rotation.x - spawn_position.x)/(spawn_position-spawn_rotation).magnitude);
        }
        Vector3 rotation_vector = new Vector3(0, 0, Mathf.Rad2Deg * angle);


        Quaternion rot = Quaternion.Euler(rotation_vector);

        GameObject new_arrow = Instantiate(arrow_prefab, spawn_position, rot);
        new_arrow.GetComponent<Arrow>().SetSender(holding_actor);
        new_arrow.GetComponent<Arrow>().SetInitialVelocity(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
    }
}
