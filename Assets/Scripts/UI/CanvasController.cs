using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Augments.Display;
using TMPro;

//Main class to display the UI functions
public class CanvasController : MonoBehaviour
{

    public List<Image> mana_sprites;
    public Player player;

    public Slider health_bar_prefab;
    public List<HealthBar> health_bars;
    public Slider player_health_bar;

    public CanvasCommunicator augment_comminicator;

    public AugmentDisplay[] boons;
    public AugmentDisplay[] skills;

    [Header("Damage Text")]
    public GameObject damage_display;
    public float damage_text_display_duration;

    public DisplayType test_display_type;
    public bool test_display;

    public float health_bar_display_time = 1.0f;

    bool displaying;

    void Awake(){
        health_bars = new List<HealthBar>();
        augment_comminicator = new CanvasCommunicator(boons, skills);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        displaying = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(test_display){
            StartCoroutine(LateStart(1));
        }
    }

    public void SpawnDamageText(float damage, Actor inflicted_actor){
        StartCoroutine(PlayDamageText(damage, inflicted_actor));
    }

    IEnumerator PlayDamageText(float damage, Actor inflicted_actor){
        GameObject new_text = Instantiate(damage_display);

        new_text.transform.SetParent(this.transform);

        Vector3 screen_point = Camera.main.WorldToScreenPoint(inflicted_actor.gameObject.transform.position);
        Vector2 rect_point = new Vector2(screen_point.x, screen_point.y + 35.0f);

        new_text.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        new_text.transform.position = rect_point;

        for(int i = 0; i < 50; i++){
            new_text.gameObject.transform.position = new Vector2(new_text.gameObject.transform.position.x, new_text.gameObject.transform.position.y + 5.0f/(i+1));
            yield return new WaitForSeconds(0.5f/50.0f);
        }

        Destroy(new_text);
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if(test_display_type == DisplayType.Boon){
            DisplayBoonChoices();
        } else {
            DisplaySkillChoices();
        }
    }

    void DisplayBoonChoices(){
        augment_comminicator.ChooseDisplayUpgrades(player, DisplayType.Boon);
        displaying = true;
    }

    void DisplaySkillChoices(){
        augment_comminicator.ChooseDisplayUpgrades(player, DisplayType.Skill);
        displaying = true;
    }

    private void TurnOffDisplay(){
        displaying = false;
        augment_comminicator.TurnOffDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAugmentDisplay();
        UpdateManaSprites();
        UpdateHealth();
        
    }

    private void UpdateAugmentDisplay(){
        if(displaying){
            if(Input.GetMouseButtonDown(0)){
                Augments.Node n = augment_comminicator.CheckOverlap(Input.mousePosition);
                if(n != null){
                    player.UpdateTrees(n);
                    TurnOffDisplay();
                }
            }
        }
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
        for(int i = 0; i < health_bars.Count; i++){
            HealthBar bar = health_bars[i];
            if(bar.actor == null){
                health_bars.RemoveAt(i);
                i--;
                GameObject.Destroy(bar.display.gameObject);
            }
        }

        player_health_bar.value = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().stats.percent_health;

        foreach(HealthBar bar in health_bars){
            bar.display.value = bar.actor.stats.percent_health;

            Vector3 screen_point = Camera.main.WorldToScreenPoint(bar.actor.gameObject.transform.position);
            Vector2 rect_point = new Vector2(screen_point.x, screen_point.y + 25.0f);

            //Debug.Log(rect_point);


            bar.display.gameObject.transform.position = rect_point;
        }
    }

    public void PokeHealthBar(Actor actor){
        if(actor is Player){
            return;
        }
        foreach(HealthBar bar in health_bars){
            if(bar.actor == actor){
                StartCoroutine(ShowHealthBar(bar));
                return;
            }
        }
    }

    public void CreateHealthBar(Actor actor){
        bool player = actor is Player;

        HealthBar bar = new HealthBar(Instantiate(health_bar_prefab), actor, player);
        bar.display.gameObject.transform.SetParent(this.transform);
        health_bars.Add(bar);

        if(!player){
            StartCoroutine(ShowHealthBar(bar));
        }
    }

    IEnumerator ShowHealthBar(HealthBar bar){

        bar.display.gameObject.SetActive(true);

        yield return new WaitForSeconds(health_bar_display_time);

        if(bar.display != null){
            bar.display.gameObject.SetActive(false);
        }
    }


}

public struct HealthBar{

    public Slider display;
    public Actor actor;
    public bool player;

    public HealthBar(Slider display, Actor actor, bool player = false){
        this.display = display;
        this.actor = actor;
        this.player = player;
    }

}