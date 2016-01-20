using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


    public int nextLevelEnergyBase;
    public int nextLevelEnergyRequirements
    {
        get
        {
            int req = nextLevelEnergyBase + (level * 2);
            return req;
        }
    }

    public int lives;

    public int actualEnergy
    {
        get; set;
    }

    public int level
    {
        get; set;
    }

    int tempEnergy;
  

    [Tooltip("procenta uspechu")]
    public float effectivity;


    bool lowEnergyMode;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        print(actualEnergy);
	}

    public void createPlayerStats()
    {
        level = 1;
        actualEnergy = 5;
    }

    public void addEnemyDeadRound(int r)
    {
        tempEnergy += r;
    }

    public void resetTempEnergy()
    {
        lowEnergyMode = false;
        tempEnergy = 0;
    }
    
    public void convertTempEnergy()
    {
        if(lowEnergyMode)
        {
            actualEnergy = tempEnergy;
            return;
        }
        actualEnergy += tempEnergy + level;
    }

    public void wasteEnergyOnStartBattle()
    {
        int en = actualEnergy;
        actualEnergy -= level;
        if(actualEnergy < 0)
        {
            actualEnergy = 0;
            tempEnergy = en;
            lowEnergyMode = true;
        }
    }
}
