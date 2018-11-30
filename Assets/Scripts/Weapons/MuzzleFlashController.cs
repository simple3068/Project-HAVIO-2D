using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashController : MonoBehaviour
{
    [SerializeField] Sprite[] sprMuzzleFlash;

    SpriteRenderer spriteRenderer;

    bool bBlinking = false;

	void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

    void TurnOn()
    {
        spriteRenderer.sprite = sprMuzzleFlash[Random.Range(0, sprMuzzleFlash.Length)];
    }

    void TurnOff()
    {
        spriteRenderer.sprite = null;
    }

    public void FlashOneShot(float fShotInterval)
    {
        if (!bBlinking)
        {
            if (fShotInterval >= 0.125f)
            {
                StartCoroutine("_Flash", 0.125f);
            }
            else
            {
                StartCoroutine("_Flash", fShotInterval);
            }
        }
    }

    IEnumerator _Flash(float fBlinkTime)
    {
        bBlinking = true;
        TurnOn();
        yield return new WaitForSeconds(fBlinkTime);
        TurnOff();
        bBlinking = false;
    }
}
