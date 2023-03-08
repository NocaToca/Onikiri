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
    OgreSettings skill_settings;

    Ogre a;

    // Start is called before the first frame update
    protected override void Start()
    {
        a = GetComponent<Ogre>();
        settings = new WalkSettings(speed, wait_time, radius, radius_buffer);
        skill_settings = new OgreSettings(1,1,1);

        base.Start();
    }

    protected override AINode SetUpTree(){
        AINode root = new OgreSelector(a, skill_settings, new List<AINode>{
            new Selector(new List<AINode>{
                new Sequence(new List<AINode>{
                    new ClubSlamCheck(),
                    new ClubSlam()
                }),
                new Sequence(new List<AINode>{
                    new OgreRoarCheck(),
                    new OgreRoar()
                }),
                new Sequence(new List<AINode>{
                    new OgreSlamCheck(),
                    new OgreSlam()
                })
            }),
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
