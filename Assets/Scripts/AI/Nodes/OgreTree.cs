using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class OgreTree : AITree
{

    public float speed;
    public float radius;
    public float radius_buffer;
    public float wait_time;

    public float fov;

    WalkSettings settings;

    Enemy a;

    // Start is called before the first frame update
    protected override void Start()
    {
        a = GetComponent<Enemy>();
        settings = new WalkSettings(speed, wait_time, radius, radius_buffer);

        base.Start();
    }

    protected override AINode SetUpTree(){
        AINode root = new Selector(new List<AINode>{

            new Sequence(new List<AINode>{
                new AttackNode(a, a.weapon),
                new Attack(a, a.weapon)
            }),
            new Sequence(new List<AINode>{
                new EnemyDetection(a, fov),
                new GoToTarget(a, speed)
            }),
            new WalkNode(settings, a)

        });
        return root;
    }

    
}
