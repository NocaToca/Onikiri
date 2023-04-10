using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Augments;


/*
    Controls all of the movement regarding the player
    Basically is the buffer parsing input before sending it to the player object

    That is why the player holds most environment variables and not this class, regardless whether it is based off of movement
*/
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Controller
{

    private Animator anime;
    private Player p;
    private Rigidbody2D rb;

    Animation.PlayerAnimationHandler pah;

    // List<SkillListener> skills;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        anime = GetComponent<Animator>();
        p = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        accepting_movement = true;

        DebugCheck();
        pah = new Animation.PlayerAnimationHandler(anime);
    }

    void DebugCheck(){
        if(p == null){
            Debug.LogError("Error: GameObject" + gameObject.name + "Player Controller is attached to a game object without a player component");
        }
        if(this.gameObject.tag != "Player"){
            Debug.LogError("Error: GameObject" + gameObject.name + "Player Controller is attached to a game object that is not tagged as \"Player\"");
        }
        if(anime == null){
            Debug.LogWarning("Warning: GameObject" + gameObject.name + "Player Controller is attached to a game object that has no animator. Animations will not play.");
        }
    }

    void Update(){
        HandleInteractionInput();
        pah.UpdateAnimation();
    }

    void HandleInteractionInput(){
        if(Input.GetMouseButtonDown(0)){
            p.AttemptAttack();
        }
        if(Input.GetKeyDown(KeyCode.T)){
            p.SwapWeapons();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            p.Respawn();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        HandleMovement(1);
    }

    private void HandleMovement(float deltaTime){
        if(!accepting_movement){
            return;
        }

        Vector2 velocity = Vector2.zero;

        //Northhow to tell if no keys are being pressed unity
        if(Input.GetKey(KeyCode.W)){
            velocity.y += p.speed * deltaTime;
        }
        //South
        if(Input.GetKey(KeyCode.S)){
            velocity.y -= p.speed * deltaTime;
        }
        //West
        if(Input.GetKey(KeyCode.A)){
            velocity.x -= p.speed * deltaTime;
        }
        //East
        if(Input.GetKey(KeyCode.D)){
            velocity.x += p.speed * deltaTime;
        }

        velocity = velocity.normalized;
        //Debug.Log(velocity);

        if(Input.GetKey(KeyCode.LeftShift)){
            velocity.y *= p.boost_speed;
            velocity.x *= p.boost_speed;
        }
        if(velocity != Vector2.zero){
            p.movedLastFrame = true;
        } else {
            p.movedLastFrame = false;
        }

        rb.velocity = velocity;

    }
}

//Structure to add a specific listener to the player, used for skills
// public class SkillListener{

//     List<WeaponPass> weapons;
//     KeyCode key;
//     UnityEvent event;

//     public SkillListener(List<WeaponPass> weapons, KeyCode key, UnityEvent event){
//         this.weapons = weapons;
//         this.key = key;
//         this.event = event;
//     }

// }


//Just for animating the player, handles all animation operations.
namespace Animation{
    #pragma warning disable 0168

    public class PlayerAnimationHandler{

        private enum AnimationType{
            ORB_ATTACK,
            SWORD_ATTACK_1,
            SWORD_ATTACK_2,
            SWORD_ATTACK_3,
            SEATHE,
            IDLE,
            WALK
        }

        private Animator main_animator;
        Direction current_direction;

        private AnimationType current_animation;

        public PlayerAnimationHandler(Animator main_animator){
            this.main_animator = main_animator;
            current_direction = Direction.West;
            current_animation = AnimationType.IDLE;
            Play("Idle_Left");
        }

        public void UpdateAnimation(){
            Direction direction = CheckDirection();

            if(direction != current_direction){
                UpdateDirection(direction);
            }
            CheckAnimation();
        }

        public string AssembleString(){
            string state = ConvertCurrentAnimationTypeToString();
            state = (current_direction == Direction.West) ? state+"_Left" : state+"_Right";
            return state;
        }

        public void CheckAnimation(){
            //Basically, if we're idle, we want to check our rigid body, otherwise we return our animation
            try{
                Rigidbody2D game_object_rigid_body = main_animator.gameObject.GetComponent<Rigidbody2D>();
                if(game_object_rigid_body == null){
                    throw new NullRigidBodyReference();
                }

                //We can really only change our direction if we move
                if(current_animation == AnimationType.IDLE){
                    if(game_object_rigid_body.velocity.magnitude > 0.01f){
                        current_animation = AnimationType.WALK;
                        Play(AssembleString());
                        return;
                    }
                } else {
                    if(game_object_rigid_body.velocity.magnitude < 0.01f){
                        current_animation = AnimationType.IDLE;
                        Play(AssembleString());
                        return;
                    }
                }
                

            }catch(NullRigidBodyReference e){
                Debug.LogError("Error: " + main_animator.gameObject.name + " has no rigid body");
                return;
            }
        }

        public Direction CheckDirection(){
            Direction return_direction = Direction.West;

            try{
                Rigidbody2D game_object_rigid_body = main_animator.gameObject.GetComponent<Rigidbody2D>();
                if(game_object_rigid_body == null){
                    throw new NullRigidBodyReference();
                }

                //We can really only change our direction if we move
                if(game_object_rigid_body.velocity.x > 0.01f){
                    return Direction.East;
                } else
                if(game_object_rigid_body.velocity.x < -0.01f){
                    return Direction.West;
                }

            }catch(NullRigidBodyReference e){
                Debug.LogError("Error: " + main_animator.gameObject.name + " has no rigid body");
                return return_direction;
            }

            return current_direction;
        }

        public void Play(string name){
            main_animator.Play(name);
        }

        public void UpdateDirection(Direction direction){
            //Left = west
            //Right = east
            string state_name = "";
            try{
                state_name = ConvertCurrentAnimationTypeToString();
            }catch(InvalidAnimationType e){
                Debug.LogError("Animation type is not set up. Please make sure it is!");
                return;
            }

            if(direction == Direction.West){
                state_name += "_Left";
            } else {
                state_name += "_Right";
            }

            //Will have more transition logic done here later "I.E not reset sword swings when turning in the middle of them"
            current_direction = direction;
            Play(state_name);
        }

        //Basically a to_string for our animation types so our animator can read em
        public string ConvertCurrentAnimationTypeToString(){
            if(current_animation == AnimationType.ORB_ATTACK){
                return "Orb_Attack";
            }
            if(current_animation == AnimationType.SWORD_ATTACK_1){
                return "Sword_Attack_1";
            }
            if(current_animation == AnimationType.SWORD_ATTACK_2){
                return "Sword_Attack_2";
            }
            if(current_animation == AnimationType.SWORD_ATTACK_3){
                return "Sword_Attack_3";
            }
            if(current_animation == AnimationType.SEATHE){
                return "Seathe";
            }
            if(current_animation == AnimationType.IDLE){
                return "Idle";
            }
            if(current_animation == AnimationType.WALK){
                return "Run";
            }

            throw new InvalidAnimationType();
        }

    }


    //I like using exceptions more than just using Debug.LogError honest to god because it is so much more readable!
    public class NullRigidBodyReference : System.Exception {
        public NullRigidBodyReference(){

        }
    }

    public class InvalidAnimationType : System.Exception {
        public InvalidAnimationType(){

        }
    }
}
