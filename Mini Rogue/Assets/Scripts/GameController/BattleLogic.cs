using UnityEngine;
using System.Collections;

public class BattleLogic : MonoBehaviour {

    enum RoundPhase
    {
        steady,shooting,moving,evaluating,enemyMove,endGame
    }

    RoundPhase actualRoundPhase;

    #region prefabs region
    public GameObject player;
    public GameObject enemy;
    public GameObject missile;

    public GameObject succesFireIcon;
    public GameObject failedFireIcon;

    public GameObject doubleEnergyButton;
    public GameObject fightButton;
    #endregion


    public float enemySpacing = 1.5f;

    float odds = 30; // THIS HAVE TO CHANGE. GAMEPLAY IMPORTANT

    GameObject _player;

    Vector3 playerStartPosition;
    Vector3 enemyStartPosition;

    PlayerController playerController;

    void Start() {
        actualRoundPhase = RoundPhase.steady;
        playerController = GetComponent<PlayerController>();
    }


    void Update() {

        switch (actualRoundPhase)
        {
            case RoundPhase.steady:
                {
                    if(_player != null)
                    {
                        if (_player.GetComponent<PlayerInFightHandler>().collision && (playerController.actualEnergy <= 0 /* or actual HP <= 0*/))
                        {
                            GetComponent<ScenesManager>().actualSceneState = SceneState.GameOverScene;
                            quitBattle();
                        }
                        else if ((_player.GetComponent<PlayerInFightHandler>().collision))
                        {
                            GetComponent<ScenesManager>().actualSceneState = SceneState.EvaluationScene;
                            quitBattle();
                        }else if(isEnemiesDestroyed())
                        {
                            playerController.convertTempEnergy();
                            GetComponent<ScenesManager>().actualSceneState = SceneState.EvaluationScene;
                            quitBattle();
                        }
                    }
                    return;
                }
            case RoundPhase.evaluating:
                {
                    evaluateFight();
                    break;
                }
            case RoundPhase.shooting:
                {
                    shootingPhase();
                    break;
                }
            case RoundPhase.moving:
                {
                    movingPhase();
                    break;
                }
            case RoundPhase.enemyMove:
                {
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("Enemy"))
                    {
                        o.GetComponent<EnemyMove>().enemyMoving = true;
                    }
                    setFightButtonActive();
                    actualRoundPhase = RoundPhase.steady;
                    break;
                }
        }
    }

    
    #region moving region

    Vector3 newPosition;
    bool newPositionPicked = false;

    void movingPhase()
    {
        if(!newPositionPicked)
        {
            newPosition = new Vector3(_player.transform.position.x,_player.transform.position.y - enemySpacing);
            newPositionPicked = true;
        }

        _player.transform.position = Vector3.MoveTowards(_player.transform.position,newPosition,2*Time.deltaTime);

        if(_player.transform.position == newPosition)
        {
            newPositionPicked = false;
            actualRoundPhase = RoundPhase.shooting;
            succesHits--;
        }
    }

    #endregion

    #region shooting region

    float shootingDelay = 1f;
    bool missileShooted = false;

    void shootingPhase()
    {
        if (succesHits > 0 && !missileShooted)
        {
            Instantiate(missile, _player.transform.position, Quaternion.identity);
            missileShooted = true;
        }else if(succesHits == 0)
        {
            Invoke("destroyIcons",0.3f);
            actualRoundPhase = RoundPhase.enemyMove;
            
            //destroyIcons();
        }

        shootingDelay -= Time.deltaTime;
        if(shootingDelay <= 0)
        {
            shootingDelay = 1f;
            missileShooted = false;
            if (isEnemiesDestroyed())
            {
                /// END 
                /// OF
                /// FIGHT
                /// HERE !!!!
                Invoke("destroyIcons",0.5f);
                actualRoundPhase = RoundPhase.steady;
            }else
                actualRoundPhase = RoundPhase.moving;
        }
    }

    bool isEnemiesDestroyed()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region evaluating fight region

    float pauseInterval = 0.5f;
    float pauseTimer;

    

    int evaluationCounter;

    Vector3 evaluationCounterPosition;


    int succesHits;

    void evaluateFight()
    {
        evaluationCounterPosition = new Vector3(-1.5f,4.5f);
        pauseTimer -= Time.deltaTime;
        if(pauseTimer <= 0)
        {
            if(Random.Range(0,100) < odds)
            {
                Instantiate(succesFireIcon, new Vector3(evaluationCounterPosition.x + evaluationCounter, evaluationCounterPosition.y), Quaternion.identity);
                succesHits++;
            }else
            {
                Instantiate(failedFireIcon, new Vector3(evaluationCounterPosition.x + evaluationCounter, evaluationCounterPosition.y), Quaternion.identity);
            }
            
            pauseTimer = pauseInterval;
            evaluationCounter++;
        }

        if(evaluationCounter == 4)
        {
            pauseTimer = 0;
            evaluationCounter = 0;
            actualRoundPhase = RoundPhase.shooting; /// change to next phase!! 
        }
    }

    #endregion

    #region create board
    public void createBoard()
    {
        energyOnStartWasted = false;
        playerController.resetTempEnergy();
        succesHits = 0;
        actualRoundPhase = RoundPhase.steady;
        playerStartPosition = new Vector3(-2f, 3.5f, 0);
        enemyStartPosition = new Vector3(2, 3.5f, 0);
        createGame();
    }

    
    void createGame()
    {
        _player = (GameObject)Instantiate(player, playerStartPosition, Quaternion.identity);
        _player.GetComponent<PlayerInFightHandler>().collision = false;
        createEnemies();
    }

    void createEnemies()
    {
        Vector3 enemyPos = enemyStartPosition;
        for (int i = 0; i < 4; i++)
        {
            Instantiate(enemy, enemyPos, Quaternion.identity);
            enemyPos.y -= enemySpacing;
        }
    }
    #endregion

    #region buttons 

    bool energyOnStartWasted;
    public void startFighting()
    {
        if(!energyOnStartWasted)
        {
            energyOnStartWasted = true;
            playerController.wasteEnergyOnStartBattle();
        }
        
        fightButton.SetActive(false);
        doubleEnergyButton.SetActive(false);
        Invoke("invokedStartFighting",0.6f);   
    }

    void invokedStartFighting()
    {
        
        if (actualRoundPhase == RoundPhase.steady)
        {
            actualRoundPhase = RoundPhase.evaluating;
        }
        else
            return;
    }
    
    /// <summary>
    /// for test quitting battle
    /// </summary>
    public void quitBattleTest()
    {
        quitBattle();
        GetComponent<ScenesManager>().actualSceneState = SceneState.MainMenu;
    }

    void setFightButtonActive()
    {
        fightButton.SetActive(true);
    }

    #endregion

    void quitBattle()
    {
        doubleEnergyButton.SetActive(false);
        fightButton.SetActive(false);
        Destroy(_player);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(g);
        }
        destroyIcons();
    }

    void destroyIcons()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("FireIcon"))
        {
            Destroy(g);
        }
    }
}
