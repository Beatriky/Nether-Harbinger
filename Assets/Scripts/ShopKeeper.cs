using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    private bool canOpen;
    public string[] ItemsForSale = new string[40];
   
  
    void Update()
    {
        if(canOpen && Input.GetButtonDown("Fire1")  && PlayerController.instance.canMove && !Shop.instance.shopMenu.activeInHierarchy)  //|| Input.GetKeyDown(KeyCode.F) )
        {
            Shop.instance.itemsForSale = ItemsForSale;
            Shop.instance.OpenShop();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
         if(other.tag == "Player")
        {
            canOpen = false;

        }
    }
}
