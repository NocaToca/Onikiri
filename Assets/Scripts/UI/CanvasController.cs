using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Main class to display the UI functions
public class CanvasController : MonoBehaviour
{

    public List<Image> mana_sprites;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateManaSprites();
        
    }

    private void UpdateManaSprites(){
        int display_num = player.kitsunebi.EvaluateAvailableMana();
        
        for(int i = 0; i < 9; i++){
            if(i <= display_num){
                mana_sprites[i].gameObject.SetActive(true);
            } else {
                mana_sprites[i].gameObject.SetActive(false);
            }
        }
    }
}
