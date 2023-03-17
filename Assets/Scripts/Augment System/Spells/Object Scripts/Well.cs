using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Augments{

        public class Well : MonoBehaviour{

            [HideInInspector]
            public UnityEvent player_event; 

            CircleCollider2D cc;

            void Start(){
                cc = GetComponent<CircleCollider2D>();
                
            }

            void FixedUpdate(){


                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if(cc.IsTouching(player.GetComponent<CircleCollider2D>())){
                    player_event.Invoke();
                }

            }
             

        }
}