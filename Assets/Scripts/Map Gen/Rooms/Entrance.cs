using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    public Orient orientation {get; private set;} //Private since we don't want anything else to edit it
    public Direction main_direction; //We just need this here to work with the unity editor to streamline editing 
    //(P.S This might change since I want to work on editing the editor but that is not a main focus)

    public bool is_connected = false; //Let's us know if this entrance is taken

    //Since we have the Direction set up but not the orient let's just do that on awake
    void Awake(){
        orientation = new Orient(main_direction);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //The orient we're passing in here should be the orient of the opposing entrance
    //We're going to be looking for orientations that match

    //Had to edit because I realize that we don't actually call awake until we instantiate the object (fml)
    public bool IsValidForOrientation(Direction opposing_entrance_orient){

        return orientation.Validify(opposing_entrance_orient);

    }
}
