using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

    public float speed;

    Transform player;

    float distanceToMove;

    Vector3 newPosition;

    public bool enemyMoving = false;

    void Start()
    {
        enemyMoving = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        distanceToMove = Vector3.Distance(new Vector3(player.transform.position.x,0),new Vector3(transform.position.x,0))/3;
        newPosition = new Vector3(transform.position.x - distanceToMove, transform.position.y);
        print(transform.position.x - distanceToMove);
    }
	// Update is called once per frame
	void Update () {
        if(enemyMoving)
        {
            moveEnemy();
        }

	}

    public void moveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position,newPosition,speed*Time.deltaTime);

        if(transform.position == newPosition)
        {
            newPosition = new Vector3(transform.position.x - distanceToMove, transform.position.y);
            enemyMoving = false;
        }

    }
}
