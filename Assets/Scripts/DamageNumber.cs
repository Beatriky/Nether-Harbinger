using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageNumber : MonoBehaviour
{
    public Text damageText;
    public float lifetime = 1f;
    public float moveSpeed=1f;
    
    public float placeJitter = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        Destroy(gameObject,lifetime);
        //going up
        transform.position += new Vector3(0f, moveSpeed* Time.deltaTime, 0f);
    }

    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        transform.position += new Vector3(Random.Range(-placeJitter, placeJitter), Random.Range(-placeJitter, placeJitter),0f);
    }
}
