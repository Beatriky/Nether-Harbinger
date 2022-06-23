using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public GameObject enemyAttackEffect;

    public DamageNumber dmgNumber;
    public Text[] playerNames, playerHP, playerMP;

    public GameObject targetMenu;
    public BattleTarget[] targetButtons;

    public GameObject magicMenu;

    public BattleMagic[] magicButtons;

    public Notification notice;
    public int chanceToFlee = 40;

    public string gameOverScene;

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
            {   //making sure the playerstas are active
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefab.Length; j++)
                    {   //check if the player prefab set from the battlechar scrpt has the same name with the player fronm the player stats
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

            UpdateUIStats();
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
        UpdateUIStats();

    }
    //checks it the combatants are dead
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
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }
        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //end battle VICTORYYY
                StartCoroutine(EndBattleCO());
            }
            else
            {
                //failure
                StartCoroutine(GameOverCo());
            }
       
        }
        else
        {   //if they still have hp the battle goes on
            while (activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }
    //adding a coroutine for whom attacks
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
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }

        int selectedTarget = players[Random.Range(0, players.Count)];
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);

        int movePower = 0;


        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {   //calculating the damage
        float atkPower = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPwr;
        float defPower = activeBattlers[target].defense + activeBattlers[target].armrPwr;
        float damageCalculation = (atkPower / defPower) * movePower * Random.Range(.8f, 1.2f);
        int damageToGive = Mathf.RoundToInt(damageCalculation);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalculation + "(" + damageToGive + ")" + activeBattlers[target].charName);
        //damaging
        activeBattlers[target].currentHP -= damageToGive;

        Instantiate(dmgNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for (int i = 0; i < playerNames.Length; i++)
        {   //check if there are still combatants
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    playerNames[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;
                }
                else
                {
                    playerNames[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerNames[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            if (movesList[i].moveName == moveName)
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);

        UIButtonHolder.SetActive(false);

        targetMenu.SetActive(false);

        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {       //before we attack we choose the target
        targetMenu.SetActive(true);

        List<int> Enemy = new List<int>();

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemy.Add(i);
            }
        }
        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (Enemy.Count > i && activeBattlers[Enemy[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemy[i];
                targetButtons[i].targetName.text = activeBattlers[Enemy[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }
    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);
        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);
                //naming the buttons - fire ice etx
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;
                //getting the cost of the spells
                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }

            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {   //we randomize our chance to flee the battle
        int fleeSuccess = Random.Range(0, 100);
        if (fleeSuccess < chanceToFlee)
        {
            battleActive = false;
            battleScene.SetActive(false);
            StartCoroutine(EndBattleCO());
        }
        else
        {
            NextTurn();
            notice.theText.text = "Could not escape!";
            notice.Activate();
        }
    }

    public  IEnumerator EndBattleCO()
    {   //end battle
        battleActive = false;
        UIButtonHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);

        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(.5f);
        

        yield return new WaitForSeconds(1.6f);
        for(int i =0; i<activeBattlers.Count; i++)
        {
            if(activeBattlers[i].isPlayer)
            {
                  for(int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if(activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHP;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                    }
                }
            }//making sure that the player doesnt stay 
            Destroy(activeBattlers[i].gameObject);
        }
        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        GameManager.instance.battleActive = false;
        
        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(2f);
        battleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }
}