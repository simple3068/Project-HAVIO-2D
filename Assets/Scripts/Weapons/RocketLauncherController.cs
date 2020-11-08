using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherController : PodSystemController
{
    [Space]
    [Header("Rocket Launcher Controller")]
    [SerializeField] GameObject objRocket;
    [SerializeField] Transform trsFirePos;
    [Space]
    [SerializeField] int nRemainedRocket;
    [SerializeField] int nPodCapacity = 19;
    [SerializeField] int nTotalRocket;
    [Space]
    [SerializeField] float fFireInterval;
    //[SerializeField] new float fReloadTime;

    bool bActivating = false;

    void Start()
    {
        nRemainedRocket = nPodCapacity;
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
        if (nRemainedRocket > 0)
        {
            bActivating = true;

            Instantiate(objRocket, trsFirePos.position, trsFirePos.rotation, null);
            nRemainedRocket--;

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
        int nDelta = nPodCapacity - nRemainedRocket;

        if (nTotalRocket >= nDelta)
        {
            bActivating = true;

            yield return new WaitForSeconds(fReloadTime);

            nRemainedRocket += nDelta;
            nTotalRocket -= nDelta;

            bActivating = false;
        }
        else if (nTotalRocket > 0)
        {
            bActivating = true;

            yield return new WaitForSeconds(fReloadTime);

            nRemainedRocket += nTotalRocket;
            nTotalRocket = 0;

            bActivating = false;
        }
    }
}
