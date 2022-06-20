using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private bool battleActive;
    public GameObject battleScene;
    public Transform[] playerPos;
    public Transform[] enemyPos;

    public BattleChar[] playerPrefab;
    public BattleChar[] enemyPrefab;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;
    public GameObject UIButtonHolder;

    public BattleAttack[] movesList;

    //for deactivating the buttons when isnt not our turn to attack
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {   // creating a new string array with the name of the CHARNAME
            BattleStart(new string[] { "Skelet", "Stingy" });
        }

        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    UIButtonHolder.SetActive(true);
                }
                else
                {
                    UIButtonHolder.SetActive(false);
                    
                    //enemies should attack
                    StartCoroutine(EnemyMoveCo());
                }
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        //checking if a battle is active
        if (!battleActive)
        {
            battleActive = true;
            GameManager.instance.battleActive = true;
            //bg fixated on camera
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);
            //playing battle music
            AudioManager.instance.PlayBGM(0);

            for (int i = 0; i < playerPos.Length; i++)
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefab.Length; j++)
                    {
                        if (playerPrefab[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefab[j], playerPos[i].position, playerPos[i].rotation);
                            newPlayer.transform.parent = playerPos[i];
                            activeBattlers.Add(newPlayer);

                            CharStats thePlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defense = thePlayer.defense;
                            activeBattlers[i].wpnPwr = thePlayer.wpnPwr;
                            activeBattlers[i].armrPwr = thePlayer.armrPwr;
                        }
                    }
                }
            }
            //depends on how enemies we want to spawn not the enemypos
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefab.Length; j++)
                    {//checking if the name of the enemy prefabs is the same 

                        if (enemyPrefab[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefab[j], enemyPos[i].position, enemyPos[i].rotation);//instantiating enemies
                            newEnemy.transform.parent = enemyPos[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }

            }
            //we randomize who attacks
            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count);
        }
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }
        turnWaiting = true;
        UpdateBattle();

    }
    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHP < 0)
            {
                activeBattlers[i].currentHP = 0;
            }
            if (activeBattlers[i].currentHP == 0)
            {
                //handle dead ppl
            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;

                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }
        if(allEnemiesDead || allPlayersDead)
        {
            if(allEnemiesDead)
            {
                //end battle VICTORYYY
            }
            else
            {
                //failure
            }
            battleScene.SetActive(false);
            GameManager.instance.battleActive=false;
            battleActive=false;
        }
    }
    //adding a coroutine
    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }
    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for(int i = 0; i< activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer && activeBattlers[i].currentHP >0)
            {
                players.Add(i);
            }
        }

        int selectedTarget = players[Random.Range(0, players.Count)];
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);

        int movePower = 0; 
        
        
        for(int i =0; i<movesList.Length; i++)
        {
            if(movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }
       // Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {   //calculating the damage
        float atkPower = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPwr;
        float defPower = activeBattlers[target].defense + activeBattlers[target].armrPwr;

        float damageCalculation = (atkPower / defPower) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalculation);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing "+ damageCalculation + "("+damageToGive+")"+ activeBattlers[target].charName);
        
        //damaging
        activeBattlers[target].currentHP -= damageToGive;
    }
}
