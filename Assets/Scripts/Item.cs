using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //This script is for any kinds of items: potions, power bonus, weapons, food
    [Header("Item Type")]

    public bool isItem;
    public bool isWeapon;
    public bool isArmor;
    public bool isGold;
    //public bool isFood;
    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;
    [Header("Item Details")]
    public int amountToChange;  //how much hp, strength is gonna give u
    public bool affectHP;
    public bool affectMP;
    public bool affectStr;
    public bool affectGold;
    [Header("Weapon/Armor Details")]
    public int weaponStrength;
    public int armorStrength;

    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn]; 
      
        if(isItem)
        {
            if(affectHP)
            {
                selectedChar.currentHP += amountToChange;
                
                if(selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            }
            if(affectMP)
            {
                selectedChar.currentMP += amountToChange;

                if(selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                }
            }
            if(affectStr)
            {
                 selectedChar.strength += amountToChange;
            }
            if(isWeapon)
            {
                if(selectedChar.equippedWpn !="")
                {
                    GameManager.instance.AddItem(selectedChar.equippedWpn);
                }
                selectedChar.equippedWpn = itemName;
                selectedChar.wpnPwr = weaponStrength; 
            }
            if(isArmor)
            {
                if(selectedChar.equippedArmr !="")
                {
                    GameManager.instance.AddItem(selectedChar.equippedArmr);
                }
                selectedChar.equippedArmr = itemName;
                selectedChar.armrPwr = armorStrength; 
            } 
          /*  if(affectGold)
            {
                GameManager.instance.currentGold += amountToChange;
            }
            */
            GameManager.instance.RemoveItem(itemName);
        }
     }

}
