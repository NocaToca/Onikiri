#define none

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    [HideInInspector]
    public Player p;
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
        if(pah.current_direction == Direction.West){
            p.sword_collider.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
        } else {
            p.sword_collider.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        }

        HandleInteractionInput();
        pah.UpdateAnimation();
    }

    void HandleInteractionInput(){
        if(Input.GetMouseButtonDown(0)){
            if(p.main_hand is Orb){
                pah.TryAttackOrb(p.AttemptAttack);
            } else
            if(p.main_hand is MeleeWeapon){
                pah.TryAttackSword(p.AttemptAttack);
            } else {
                Debug.LogWarning("No animation for given weapon");
            }
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
        //rb.velocity = Vector2.zero;
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
        velocity *= p.speed;
        //Debug.Log(velocity);

        KeyCode boost = KeyCode.LeftShift;

        #if UNITY_EDITOR
        boost = KeyCode.Z;
        #endif

        if(Input.GetKey(boost)){
            velocity.y *= p.boost_speed;
            velocity.x *= p.boost_speed;
        }
        if(velocity != Vector2.zero){
            p.movedLastFrame = true;
        } else {
            p.movedLastFrame = false;
        }

        if(velocity == Vector2.zero){
            return;
        }

        //Debug.Log("Two");

        rb.velocity = velocity;

    }

    public void SetAttacking(){
        accepting_movement = false;
        p.attacking = true;
    }

    public void DisableAttacking(){
        accepting_movement = true;
        p.attacking = false;
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
        public Direction current_direction;

        private AnimationType current_animation;

        //Functions that we want to listen to the update
        UnityEvent UpdateListeners;
        Dictionary<string, UnityAction> actions_to_remove;

        Player p {get{return main_animator.GetComponent<Player>();}}

        public PlayerAnimationHandler(Animator main_animator){
            this.main_animator = main_animator;
            current_direction = Direction.West;
            current_animation = AnimationType.IDLE;
            Play("Idle_Left");
            actions_to_remove = new Dictionary<string, UnityAction>();

            UpdateListeners = new UnityEvent();
        }

        public void UpdateAnimation(){
            Direction direction = CheckDirection();

            if(direction != current_direction){
                UpdateDirection(direction);
            }
            if(current_animation == AnimationType.IDLE || current_animation == AnimationType.WALK){
                main_animator.gameObject.GetComponent<PlayerController>().DisableAttacking();
                CheckAnimation();
            }

            var clip_info = main_animator.GetCurrentAnimatorClipInfo(0);
            if(clip_info.Length != 0){
                if(main_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Substring(0,6) == "Seathe"){
                    current_animation = AnimationType.SEATHE;
                }
            }

            if(current_animation == AnimationType.SEATHE){
                if(actions_to_remove.Count == 0){
                    var check = main_animator.GetCurrentAnimatorClipInfo(0);
                    if(check.Length != 0){
                        if(check[0].clip.name.Substring(0,4) == "Idle"){
                            main_animator.gameObject.GetComponent<PlayerController>().DisableAttacking();
                            current_animation = AnimationType.IDLE;
                        }
                    }
                }
            }
            #if Noca_Debug
            Debug.Log(AssembleString());
            #endif

            UpdateListeners.Invoke();
        }

        public bool ValidateAnimation(){
            string clip_name = main_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            return clip_name == AssembleString();
        }

        public void FlushListener(){
            UpdateListeners = new UnityEvent();
        }

        public void TryAttackOrb(System.Action attacking_action){
            if(current_animation == AnimationType.IDLE || current_animation == AnimationType.WALK){

                Vector3 mouse_pos = Input.mousePosition;
                Vector3 screen_pos_of_mouse = Camera.main.ScreenToWorldPoint(mouse_pos);

                Vector3 direction_vector = screen_pos_of_mouse - main_animator.gameObject.transform.position;

                if(direction_vector.x > 0){
                    current_direction = Direction.East;
                } else {
                    current_direction = Direction.West;
                }

                current_animation = AnimationType.ORB_ATTACK;

                //What a round-about way LMFAO
                main_animator.gameObject.GetComponent<PlayerController>().StartCoroutine(OrbOffset(attacking_action));
                Play(AssembleString());
            }
        }

        IEnumerator OrbOffset(System.Action attacking_action){
            yield return new WaitForSeconds(.15f);
            attacking_action();
        }

        public void TryAttackSword(System.Action<float> attacking_action){
            //We're going to want to:
            /*
            1. Check what sword attack stage we are on (1-3)
            2. Queue a sword attack if it is pressed multiple times
            3. Queue seathe for when we stop attacking
            */
            System.Func<bool> current_attack = delegate(){
                return current_animation == AnimationType.SWORD_ATTACK_1 || current_animation == AnimationType.SWORD_ATTACK_2 || current_animation == AnimationType.SWORD_ATTACK_3;
            };

            Vector3 mouse_pos = Input.mousePosition;
            Vector3 screen_pos_of_mouse = Camera.main.ScreenToWorldPoint(mouse_pos);

            Vector3 direction_vector = screen_pos_of_mouse - main_animator.gameObject.transform.position;

            if(direction_vector.x > 0){
                current_direction = Direction.East;
            } else {
                current_direction = Direction.West;
            }

            if(current_attack() || current_animation == AnimationType.SEATHE){
                System.Func<AnimationType> move_next = delegate(){
                    return AnimationType.IDLE;
                };

                if(current_animation == AnimationType.SWORD_ATTACK_1){
                    #if Noca_Debug
                    Debug.Log("Check one");
                    #endif
                    move_next = delegate(){
                        p.SetPushbackForce(50);
                        attacking_action(1.0f);
                        PushPlayer(10);
                        return AnimationType.SWORD_ATTACK_2;
                    };
                } else 
                if(current_animation == AnimationType.SWORD_ATTACK_2){
                    #if Noca_Debug
                    Debug.Log("Check two");
                    #endif

                    move_next = delegate(){
                        p.SetPushbackForce(200);
                        attacking_action(1.5f);
                        PushPlayer(400);
                        return AnimationType.SWORD_ATTACK_3;
                    };
                } else
                if(current_animation == AnimationType.SWORD_ATTACK_3) {
                    #if Noca_Debug
                    Debug.Log("Check three");
                    #endif
                    return;

                    // move_next = delegate(){
                    //     PushPlayer(100);
                    //     return AnimationType.SWORD_ATTACK_1;
                    // };
                }

                main_animator.gameObject.GetComponent<PlayerController>().SetAttacking();

                //bool moved = false;

                //If this is confusing, we're basically just setting our increment correctly, then telling our class to consistently check if
                //our animation transitioned, and if so move onto the next animation before flushing the listener;
                UnityAction increment_attack = delegate(){
                    if(!current_attack()){
                        current_animation = move_next();
                        Play(current_animation);
                        FlushQueue();
                        main_animator.gameObject.GetComponent<PlayerController>().SetAttacking();
                    }
                };

                UnityAction check_if_seathe = delegate(){
                    if(main_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Substring(0,6) == "Seathe"){
                        current_animation = AnimationType.SEATHE;
                        increment_attack();
                    }
                };

                // if(!actions_to_remove.ContainsKey("attack_queue")){
                //     actions_to_remove.Add("attack_queue", increment_attack);
                // }
                if(!actions_to_remove.ContainsKey("seathe")){
                    actions_to_remove.Add("seathe", check_if_seathe);
                    UpdateListeners.AddListener(check_if_seathe);
                }

                //UpdateListeners.AddListener(increment_attack);
                

            } else
            if(!(current_animation == AnimationType.SEATHE)) {
                current_animation = AnimationType.SWORD_ATTACK_1;
                main_animator.gameObject.GetComponent<PlayerController>().SetAttacking();
                p.SetPushbackForce(50);
                PushPlayer(10);
                attacking_action(1.0f);
                Play(AssembleString());
            }
            
        }

        public void FlushQueue(){
            foreach(KeyValuePair<string, UnityAction> pair in actions_to_remove){
                UpdateListeners.RemoveListener(pair.Value);
            }

            actions_to_remove = new Dictionary<string, UnityAction>();
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
                    if(game_object_rigid_body.velocity.magnitude > 0.25f){
                        current_animation = AnimationType.WALK;
                        Play(AssembleString());
                        return;
                    }
                } else {
                    if(game_object_rigid_body.velocity.magnitude < 0.25f){
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

        private void Play(AnimationType type){
            current_animation = type;
            Play(AssembleString());
        }
        private void Play(string name){
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

        //Push function
        public void PushPlayer(float magnitude){
            Rigidbody2D rb = main_animator.gameObject.GetComponent<Rigidbody2D>();

            Vector2 force_direction = new Vector2(0,0);

            if(current_direction == Direction.West){
                force_direction.x -= 1.0f;
            } else 
            if(current_direction == Direction.East){
                force_direction.x += 1.0f;
            }

            force_direction *= magnitude;
            rb.AddForce(force_direction);
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
