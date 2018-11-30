using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] float fSpeed;
    [SerializeField] float fAutoDetonateTime;
    [Space]
    [SerializeField] Sprite[] sprBulletTrace;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    string[] strIgnoreTags = { "Player" };

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rigidBody.velocity = transform.right * fSpeed;
        spriteRenderer.sprite = sprBulletTrace[Random.Range(0, sprBulletTrace.Length)];

        Destroy(gameObject, fAutoDetonateTime);
    }

    void Update()
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
