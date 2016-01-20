using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum SceneState
{
    FirstScene,MainMenu,First,Stats,Fight,EvaluationScene,GameOverScene,LevelUpScene
}

[System.Serializable]
public class Scene
{
    public GameObject[] objects;

    public void disableButtons()
    {
        foreach (GameObject b in objects)
        {
            b.SetActive(false);
        }
    }

    public void enableButtons()
    {
        foreach (GameObject b in objects)
        {
            b.SetActive(true);
        }
    }
}


public class ScenesManager : MonoBehaviour {

    public bool sceneDebug;

    public SceneState actualSceneState;

    public Scene firstScene;
    public Scene mainMenu;
    public Scene StatsScene;
    public Scene fightScene;
    public Scene evaluationScene;
    public Scene levelUpScene;

    Camera mainCamera;

    PlayerController playerController;

	void Start () {
        mainCamera = Camera.main;

        playerController = GetComponent<PlayerController>();

        if(!sceneDebug)
        {
            actualSceneState = SceneState.FirstScene;
            disableAllButtons();
            firstScene.enableButtons();
        }
    }

    void Update () {

        if(sceneDebug)
        {
            disableAllButtons();
            sceneDebug = false;
        }

	    switch(actualSceneState)
        {
            case SceneState.FirstScene:
                {
                    firstScene.enableButtons();
                    break;
                }
            case SceneState.EvaluationScene:
                {
                    /// !!! IMPORTANT !!!
                    /// NextBattleButton must have index 0
                    /// NextLevelButton must have index 1
                    ///

                    evaluationScene.objects[0].SetActive(true);
                    evaluationScene.objects[2].SetActive(true);
                    evaluationScene.objects[3].SetActive(true);

                    if (playerController.nextLevelEnergyRequirements < playerController.actualEnergy)
                    {
                        evaluationScene.objects[1].SetActive(true);
                    }else
                    {
                        evaluationScene.objects[1].SetActive(false);
                    }
                    break;
                }
        }
	}

    /// <summary>
    /// method for activate StartButton
    /// </summary>
    public void StartGame()
    {
        disableAllButtons();
        mainMenu.enableButtons();
        actualSceneState = SceneState.MainMenu;
    }
    
    /// <summary>
    /// method for activate fight
    /// </summary>
    public void startFight()
    {
        disableAllButtons();

        fightScene.enableButtons();
        GetComponent<BattleLogic>().createBoard();

        actualSceneState = SceneState.Fight;
    }

    public void activateStatsScene()
    {
        
        disableAllButtons();
        StatsScene.enableButtons();
        actualSceneState = SceneState.Stats;
    }

    /*
    public void continueAfterLevelUpScreen()
    {
        disableAllButtons();
        StatsScene.enableButtons();
        actualSceneState = SceneState.Stats;
    }
    */

    public void activateLevelUpScene()
    {
        disableAllButtons();
        levelUpScene.enableButtons();
        actualSceneState = SceneState.LevelUpScene;
    }

    void disableAllButtons()
    {
        fightScene.disableButtons();
        mainMenu.disableButtons();
        firstScene.disableButtons();
        StatsScene.disableButtons();
        evaluationScene.disableButtons();
        levelUpScene.disableButtons();
    }
}
