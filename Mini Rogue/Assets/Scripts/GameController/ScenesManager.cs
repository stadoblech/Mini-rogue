using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum SceneState
{
    FirstScene,MainMenu,First,Stats,Fight,PostFight
}

[System.Serializable]
public class Scene
{
    public GameObject[] buttons;
    public GameObject[] texts;

    public void disableButtons()
    {
        foreach (GameObject b in buttons)
        {
            b.SetActive(false);
        }

        foreach (GameObject t in texts)
        {
            t.SetActive(false);
        }

    }

    public void enableButtons()
    {
        foreach (GameObject b in buttons)
        {
            b.SetActive(true);
        }

        foreach (GameObject t in texts)
        {
            t.SetActive(true);
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

    Camera mainCamera;

    bool battleCreated;

	void Start () {
        battleCreated = false;
        mainCamera = Camera.main;

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
                        GetComponent<BattleLogic>().restartBoard();
                        battleCreated = true;
                    }
                    break;
                }
            case SceneState.Stats:
                {
                    StatsScene.enableButtons();
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
    }


}
