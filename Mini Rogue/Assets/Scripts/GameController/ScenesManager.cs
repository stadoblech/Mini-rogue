using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum SceneState
{
    FirstScene,MainMenu,First,Stats,Fight,EvaluationScene,GameOverScene
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

    Camera mainCamera;

    bool battleCreated;

    PlayerController playerController;
	void Start () {
        battleCreated = false;
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

            case SceneState.MainMenu:
                {
                    mainMenu.enableButtons();
                    break;
                }
            case SceneState.Fight:
                {
                    if (!battleCreated)
                    {
                        fightScene.enableButtons();
                        GetComponent<BattleLogic>().createBoard();
                        battleCreated = true;
                    }
                    break;
                }
            case SceneState.Stats:
                {
                    StatsScene.enableButtons();
                    break;
                }
            case SceneState.EvaluationScene:
                {
                    /// !!! IMPORTANT !!!
                    /// NextBattleButton must have index 0
                    /// NextLevelButton must have index 1
                    ///

                    evaluationScene.objects[0].SetActive(true);

                    if(playerController.nextLevelEnergyRequirements < playerController.actualEnergy)
                    {
                        evaluationScene.objects[1].SetActive(true);
                    }

                    break;
                }
            case SceneState.GameOverScene:
                {
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
        actualSceneState = SceneState.MainMenu;
    }
    
    /// <summary>
    /// method for activate fight
    /// </summary>
    public void startFight()
    {
        disableAllButtons();
        actualSceneState = SceneState.Fight;
    }

    public void activateStatsScene()
    {
        disableAllButtons();
        actualSceneState = SceneState.Stats;
    }

    void disableAllButtons()
    {
        battleCreated = false; /// care for this boolean here. Can cause problems. maybe it will need to shitf somewhere else
        fightScene.disableButtons();
        mainMenu.disableButtons();
        firstScene.disableButtons();
        StatsScene.disableButtons();
        evaluationScene.disableButtons();
    }


}
