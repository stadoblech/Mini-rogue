using UnityEngine;
using System.Collections;

public class LevelingUpHandler : MonoBehaviour {

    public GameObject rerollButton;

    PlayerController player;

    public int hpBonus
    {
        get; set;
    }

    public int efficientyBonus
    {
        get; set;
    }

    public float luckBonus
    {
        get; set;
    }

    int numberOfRerolls = 3;
	void Start () {
        player = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(numberOfRerolls == 0)
        {
            rerollButton.SetActive(false);
        }
	}

    public void createLevelUpStats()
    {
        numberOfRerolls = 3;
        calculateBonus();
    }

    public void rerollButtonAction()
    {
        calculateBonus();
        numberOfRerolls--;
    }

    void calculateBonus()
    {
        
    }
}

