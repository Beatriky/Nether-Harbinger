using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public Text amountText;
    public int buttonValue;
    
    
    public void Press()
    {   if(GameMenu.instance.theMenu.activeInHierarchy)
    {
        if(GameManager.instance.itemsHeld[buttonValue]!= "")
        {
            GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
        }
    }
    if(Shop.instance.shopMenu.activeInHierarchy)
    {
        if(Shop.instance.buyMenu.activeInHierarchy)
        {   //get the details of the selected item to buy
            Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
        }
        
        if(Shop.instance.sellMenu.activeInHierarchy)
        {
            Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
        }
    }
    }
}
