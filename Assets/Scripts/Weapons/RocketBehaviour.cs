using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    [SerializeField] float fSpeed;
    [SerializeField] float fAutoDetonateTime;

    Rigidbody2D rigidBody;

    string[] strIgnoreTags = { "Player" };

	void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = transform.right * fSpeed;

        Destroy(gameObject, fAutoDetonateTime);
	}
	
	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string it in strIgnoreTags)
        {
            if (other.CompareTag(it))
                return;
        }

        Destroy(gameObject);
    }
}
