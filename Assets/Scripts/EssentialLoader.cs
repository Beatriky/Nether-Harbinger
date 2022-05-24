using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject player;
    public GameObject gamMan;
    void Start()
    {
        if(UIFade.instance==null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }
        if(PlayerController.instance==null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }   
        if(GameManager.instance == null) 
        {
            Instantiate(gamMan);
        }
    }

   
    void Update()
    {
        
    }
}
