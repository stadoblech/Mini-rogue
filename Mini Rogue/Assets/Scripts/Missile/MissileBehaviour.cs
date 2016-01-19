using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

    public float speed = 0.2f;

    PlayerController playerController;

	void Start () {

        playerController = GameObject.FindGameObjectWithTag("GameController").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(speed * Time.deltaTime, 0);
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Enemy")
        {
            playerController.addEnemyDeadRound(coll.GetComponent<EnemyMove>().getActualEnemyRound());
            Destroy(coll.gameObject);
            Destroy(gameObject);
        }
    }
}
