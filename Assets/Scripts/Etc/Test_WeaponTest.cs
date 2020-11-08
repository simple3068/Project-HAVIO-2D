using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test_WeaponTest : MonoBehaviour
{
    [SerializeField] HelicopterController hc;
    [Space]
    [SerializeField] Dropdown ddWeaponSelect;
    [Space]
    [SerializeField] List<PodSystemController> psc;
    [Space]
    [SerializeField] Text txtTotalAmmo;
    [SerializeField] Text txtCurrAmmo;

    int nPrevSelectedWeapon = 0;
    int nCurrSelectedWeapon = 0;

    void Start()
    {

    }

    void Update()
    {
        if (nCurrSelectedWeapon != ddWeaponSelect.value)
        {
            nPrevSelectedWeapon = nCurrSelectedWeapon;
            nCurrSelectedWeapon = ddWeaponSelect.value;

            hc.dbg_PodSystemController = psc[nCurrSelectedWeapon];
            psc[nCurrSelectedWeapon].gameObject.SetActive(true);
            psc[nPrevSelectedWeapon].gameObject.SetActive(false);
        }

        txtCurrAmmo.text = psc[nCurrSelectedWeapon].gameObject.GetComponent<MachineGunController>().dbg_nRemainedRound.ToString();
        txtTotalAmmo.text = psc[nCurrSelectedWeapon].gameObject.GetComponent<MachineGunController>().dbg_nTotalRound.ToString();
    }

    public void OnButtonResetClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
