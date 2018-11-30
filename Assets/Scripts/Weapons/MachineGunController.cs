using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController : PodSystemController
{
    public string strName;
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
    [SerializeField] float fReloadTime;

    bool bActivating = false;

    void Start()
    {
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
        if (!!bActivating)
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

            yield return new WaitForSeconds(fFireInterval);

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

        if (nTotalRound >= nDelta)
        {
            bActivating = true;

            yield return new WaitForSeconds(fReloadTime);

            nRemainedRound += nDelta;
            nTotalRound -= nDelta;

            bActivating = false;
        }
        else if (nTotalRound > 0)
        {
            bActivating = true;

            yield return new WaitForSeconds(fReloadTime);

            nRemainedRound += nTotalRound;
            nTotalRound = 0;

            bActivating = false;
        }
    }
}
