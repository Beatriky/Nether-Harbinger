using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleTarget : MonoBehaviour
{
    public string moveName;
    public int activeBattlerTarget;
    public Text targetName;
    public void Press()
    {   // when the button is pressed the player attacks the chosen target
        BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);
    }
}
