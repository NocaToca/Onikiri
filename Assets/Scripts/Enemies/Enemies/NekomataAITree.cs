using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class NekomataAITree : AITree
{

    public float spell_range;

    public float speed;
    public float radius;
    public float radius_buffer;
    public float wait_time;

    public float fov;

    Enemy pawn;
    WalkSettings walk_settings;
    // Start is called before the first frame update
    protected override void Start(){
        pawn = GetComponent<Enemy>();
        if (pawn == null){
            Debug.LogError("'Enemy' Component could not be found.");
        }
        walk_settings = new WalkSettings(speed, wait_time, radius, radius_buffer);
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override AINode SetUpTree(){
        AINode root = new Selector(new List<AINode>{
            new Selector(new List<AINode>{
                new Necropuppet(),
                new FireWisp()
            }),
            new Sequence(new List<AINode>{
                new EnemyDetection(pawn, fov),
                new GoToTarget(pawn, speed)
            }),
            new WalkNode(walk_settings, pawn)

        });
        root.SetData("pawn", pawn);
        root.SetData("Proximity Distance", spell_range);
        return root;
    }
}
