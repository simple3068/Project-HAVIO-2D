using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashController : MonoBehaviour
{
    [SerializeField] Sprite[] sprMuzzleFlash;

    SpriteRenderer spriteRenderer;

	void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void TurnOn()
    {
        spriteRenderer.sprite = sprMuzzleFlash[Random.Range(0, sprMuzzleFlash.Length)];
    }

    public void TurnOff()
    {
        spriteRenderer.sprite = null;
    }
}
