using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering;

public class Water : MonoBehaviour{

    public ComputeShader waterWaves;
    public Material waterMat;

    public RenderTexture renderTexture;

    private bool active = false;

    private Actor a;

    // Start is called before the first frame update
    void Start(){
        //currentObjects = new List<Actor>();
        
        if(renderTexture == null){
            renderTexture = new RenderTexture(256, 256, 0);

            
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            renderTexture.filterMode = FilterMode.Bilinear;
            renderTexture.graphicsFormat = GraphicsFormat.R16G16B16A16_SFloat;

            renderTexture.enableRandomWrite = true;
            renderTexture.Create();
        }

        waterWaves.SetTexture(0, "Input", renderTexture);
        waterWaves.Dispatch(0, renderTexture.width/8, renderTexture.height/8, 1);

        waterMat.SetTexture("_WaterNormals", renderTexture);
        //a = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void FixedUpdate() {

        waterMat.SetVector("_PressedWorld", new Vector2(-1.0f, -1.0f));
        waterWaves.SetVector("pressedUV", new Vector2(-0.0f, 0.0f));

            
        CheckRipples();
        
        waterWaves.SetTexture(0, "Input", renderTexture);

        waterWaves.Dispatch(0, renderTexture.width/8, renderTexture.height/8, 1);

        waterMat.SetTexture("_WaterNormals", renderTexture);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        active = true;
        a = other.gameObject.GetComponent<Actor>();
    }

    private void OnTriggerExit2D(Collider2D other) {
        active = false;
    }

    void CheckRipples(){


        //foreach(Actor a in currentObjects){

            if(!active || a == null){
                return;
            }
            Vector3 position = a.transform.position;

            if(a.movedLastFrame){

                Vector2 origin = new Vector2(position.x, position.y);

                // RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down);
                // if(hit.collider != null){
                //     if(hit.transform.gameObject == this.gameObject){

                        float width = this.transform.localScale.x;
                        float height = this.transform.localScale.y;

                        Vector3 local_origin = this.transform.position;

                        local_origin.x -= width/2.0f;
                        local_origin.y -= height/2.0f;

                        Vector2 uvSpace = new Vector2(origin.x - local_origin.x, origin.y - local_origin.y);
                        uvSpace.x /= width;
                        uvSpace.y /= height;

                        //waterWaves.SetTexture(1, "Base_Input", renderTexture);
                        waterWaves.SetVector("pressedUV", uvSpace);
                       // waterWaves.Dispatch(0, renderTexture.width/8, renderTexture.height/8, 1);
                        //waterWaves.Dispatch(1, renderTexture.width/8, renderTexture.height/8, 1);
                        
                        
                //     }
                // }

            }

        //}

        
    }

}
