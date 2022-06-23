using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    public bool isPlayer;

    public string charName;
    public int currentHP, currentMP, maxHP, maxMP, defense, strength, wpnPwr, armrPwr;
    public bool hasDied;
    //fire ice acid slash
    public string[] movesAvailable;

    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;
    public bool shouldFade;
    public float fadeSpeed = 1f;

    void Start()
    {

    }


    void Update()
    {//check if the enemy is dead and fades it away
        if (shouldFade)
        {//fades the sprite to a redish color
            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (theSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void EnemyFade()
    {
        shouldFade = true;
    }
}


