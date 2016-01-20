using UnityEngine;
using System.Collections;

public class PlayerInFightHandler : MonoBehaviour {

    public bool collision;
	void Start () {
        collision = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Enemy")
        {
            collision = true;
        }
    }
}
