using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleMagic : MonoBehaviour
{
    public string spellName;
    public int spellCost;
    public Text nameText;
    public Text costText;
    public void Press()
    {   //check if there is enough mp
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            BattleManager.instance.magicMenu.SetActive(false);  //close the menu
            BattleManager.instance.OpenTargetMenu(spellName);   //open
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;   //"paying" the spell 
        }
        else
        {
            BattleManager.instance.notice.theText.text ="Not enough MP!";
            BattleManager.instance.notice.Activate();
            BattleManager.instance.magicMenu.SetActive(false);
        }


    }
}
