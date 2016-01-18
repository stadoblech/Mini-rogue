using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

    public float speed = 0.2f;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(speed * Time.deltaTime, 0);
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Enemy")
        {
            Destroy(coll.gameObject);
            Destroy(gameObject);
        }
    }
}
