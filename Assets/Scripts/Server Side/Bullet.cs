using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

    [SerializeField]
    float speed;

    void OnCollisionEnter2d(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 5f);
        GetComponent<Rigidbody2D>().velocity = transform.up.normalized * speed;
	}
	

}
