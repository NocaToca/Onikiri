using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Main class to display the UI functions
public class CanvasController : MonoBehaviour
{

    public List<Image> mana_sprites;
    public Player player;

    public Slider health_bar_prefab;
    public List<HealthBar> health_bars;

    void Awake(){
        health_bars = new List<HealthBar>();
    }

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
        if(mana_sprites.Count != 9){
            return;
        }
        
        for(int i = 0; i < 9; i++){
            if(i <= display_num){
                mana_sprites[i].gameObject.SetActive(true);
            } else {
                mana_sprites[i].gameObject.SetActive(false);
            }
        }
    }

    public bool HealthBarExist(Actor a){
        foreach(HealthBar bar in health_bars){
            if(bar.actor == a){
                return true;
            }
        }

        return false;
    }

    private void UpdateHealth(){
        foreach(HealthBar bar in health_bars){
            bar.display.value = bar.actor.stats.health;

            Vector3 screen_point = Camera.main.WorldToScreenPoint(bar.actor.gameObject.transform.position);
            Vector2 rect_point = new Vector2(screen_point.x, screen_point.y + 1.0f);


            bar.display.handleRect.position = rect_point;
        }
    }

    public void CreateHealthBar(Actor actor){
        HealthBar bar = new HealthBar(Instantiate(health_bar_prefab), actor);
        bar.display.gameObject.transform.parent = this.transform;
        bar.display.gameObject.SetActive(true);
        health_bars.Add(bar);

    }


}

public struct HealthBar{

    public Slider display;
    public Actor actor;

    public HealthBar(Slider display, Actor actor){
        this.display = display;
        this.actor = actor;
    }

}