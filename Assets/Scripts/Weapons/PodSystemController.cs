using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodSystemController : MonoBehaviour
{
    [Header("Pod System Info")]
    [SerializeField] Sprite sprButtonIcon;
    [SerializeField] string strName;

    [HideInInspector] public bool bReloading = false;
    [HideInInspector] public float fReloadTime;

    public virtual void Fire()
    {
        /* EMPTY */
    }

    public virtual void Reload()
    {
        /* EMPTY */
    }
}
