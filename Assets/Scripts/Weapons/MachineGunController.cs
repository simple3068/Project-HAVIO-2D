using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController : PodSystemController
{
    [Space]
    [Header("Machine Gun Controller")]
    [SerializeField] GameObject objBullet;
    [SerializeField] Transform trsFirePos;
    [Space]
    [SerializeField] MuzzleFlashController muzzleFlashController;
    [Space]
    [SerializeField] int nRemainedRound;
    [SerializeField] int nMagazineCapacity = 25;
    [SerializeField] int nTotalRound;
    [Space]
    [SerializeField] float fFireInterval;
    [Space]
    [SerializeField] AudioClip acFire;
    [SerializeField] AudioClip acReloadStart;
    [SerializeField] AudioClip acReloadEnd;
    [SerializeField] bool bLoopable = true;

    AudioSource asFire;
    AudioSource asReload;

    bool bActivating = false;

    void Start()
    {
        asFire = gameObject.AddComponent<AudioSource>();
        asReload = gameObject.AddComponent<AudioSource>();

        asFire.clip = acFire;

        nRemainedRound = nMagazineCapacity;
    }

    void Update()
    {
        if (!bActivating)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Fire();
            }
            else
            {
                asFire.Stop();
            }

            if (Input.GetKey(KeyCode.R))
            {
                Reload();
            }
        }
    }

    public override void Fire()
    {
        if (!bActivating)
            StartCoroutine("_Fire");
    }

    public override void Reload()
    {
        if (!bActivating && nRemainedRound != nMagazineCapacity)
            StartCoroutine("_Reload");
    }

    IEnumerator _Fire()
    {
        if (nRemainedRound > 0)
        {
            bActivating = true;

            muzzleFlashController.FlashOneShot(fFireInterval);

            Instantiate(objBullet, trsFirePos.position, trsFirePos.rotation, null);
            nRemainedRound--;

            if (bLoopable)
            {
                if (!asFire.isPlaying)
                {
                    asFire.Play();
                }
            }
            else
            {
                asFire.time = Random.Range(0, 3) * acFire.length / 3.0f;
                asFire.Play();
            }    
            yield return new WaitForSeconds(fFireInterval);
            if (!bLoopable)
                asFire.Stop();

            bActivating = false;
        }
        else
        {
            yield return StartCoroutine("_Reload");
        }
    }

    IEnumerator _Reload()
    {
        int nDelta = nMagazineCapacity - nRemainedRound;

        asFire.Stop();

        if (nTotalRound >= nDelta)
        {
            bReloading = bActivating = true;

            asReload.PlayOneShot(acReloadStart);
            yield return new WaitForSeconds(fReloadTime);
            asReload.Stop();
            asReload.PlayOneShot(acReloadEnd);

            nRemainedRound += nDelta;
            nTotalRound -= nDelta;

            bReloading = bActivating = false;
        }
        else if (nTotalRound > 0)
        {
            bReloading = bActivating = true;
            
            asReload.PlayOneShot(acReloadStart);
            yield return new WaitForSeconds(fReloadTime);
            asReload.Stop();
            asReload.PlayOneShot(acReloadEnd);

            nRemainedRound += nTotalRound;
            nTotalRound = 0;

            bReloading = bActivating = false;
        }
    }

    /* 테스트 */

    public int dbg_nRemainedRound
    {
        get { return nRemainedRound; }
        set { nRemainedRound = value; }
    }
    public int dbg_nTotalRound
    {
        get { return nTotalRound; }
        set { nTotalRound = value; }
    }
}
