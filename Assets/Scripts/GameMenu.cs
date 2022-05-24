using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.M))
       // if(Input.GetButtonDown("Fire2"))
        {
            if(theMenu.activeInHierarchy)
            {
                theMenu.SetActive(false);

                GameManager.instance.gameMenuOpen = false;
            }
            else
            {
                theMenu.SetActive(true);
                 GameManager.instance.gameMenuOpen = true;
            }
        }
    }
}
