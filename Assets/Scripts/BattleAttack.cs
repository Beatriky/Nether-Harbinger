using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]       //making it a component for any object in Unity
public class BattleAttack
{    public string moveName;
    public int movePower;
    public int moveCost;
    public AttackEffect theEffect;
}
