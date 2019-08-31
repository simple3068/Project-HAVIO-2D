using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleInputNamespace;

public class HelicopterController : MonoBehaviour
{
    [Header("Helicopter Info")]
    [SerializeField] Transform trsPlayer;
    [SerializeField] Transform trsCenterOfMass;
    [SerializeField] Rigidbody2D rigidBody;
    [Space]
    [SerializeField] Button btnEngine;
    [SerializeField] Sprite sprEngineOn;
    [SerializeField] Sprite sprEngineOff;
    [Space]
    [SerializeField] Button btnTurn;
    [Space]
    [SerializeField] EnhancedButton ebtnFire;
    [Space]
    [SerializeField] Button btnReload;
    [SerializeField] Image imgReloadBar;
    [Space]
    [SerializeField] Transform trsMainBlade;
    [SerializeField] float fMainRotorSpeed;
    [SerializeField] Transform trsTailBlade;
    [SerializeField] float fTailRotorSpeed;
    [Space]
    [SerializeField] float fEngineStartingTime;
    [SerializeField] float fEngineStoppingTime;
    [Space]
    [SerializeField] float fVerticalSpeed;
    [SerializeField] float fHorizontalSpeed;
    [SerializeField] float fCyclicAngle;
    [Space]
    [Header("Pod Weapon System")]
    [SerializeField] PodSystemController pscPodWeapon;
    [Space]
    [Header("Android Option")]
    [SerializeField] bool bUseJoystick;
    [SerializeField] Joystick joystick;

    string[] strIgnoreTag = { "Bullet" };

    const float N_ENGINE_RUNNING_STEPS = 32;
    const float F_ENGINE_RUNNING_DELTA = 1.0f / N_ENGINE_RUNNING_STEPS;

    Vector2 v2ThrottleForce;

    float fThrottle;
    float fCollective;
    float fCyclic;

    bool bEngineStatus = false;
    bool bEngineRunning = false;
    bool bAvailableFlight = true;

    bool bHeadingRight = true;

    bool bReloadingProgess = false;

    void Start()
    {
        rigidBody.centerOfMass = trsCenterOfMass.localPosition;

        btnEngine.onClick.AddListener(delegate
        {
            if (bAvailableFlight && !bEngineRunning)
            {
                if (!bEngineStatus)
                {
                    btnEngine.image.sprite = sprEngineOn;
                    StartCoroutine("StartEngine");
                }
                else
                {
                    btnEngine.image.sprite = sprEngineOff;
                    StartCoroutine("StopEngine");
                }
            }
        });
        btnTurn.onClick.AddListener(delegate
        {
            if (bAvailableFlight && bEngineStatus)
            {
                if (bHeadingRight)
                {
                    transform.Rotate(0.0f, 180.0f, 0.0f);
                    bHeadingRight = false;
                }
                else
                {
                    transform.Rotate(0.0f, 180.0f, 0.0f);
                    bHeadingRight = true;
                }
            }
        });
        btnReload.onClick.AddListener(delegate
        {
            if (bEngineStatus && pscPodWeapon)
            {
                pscPodWeapon.Reload();
            }
        });
    }

    void FixedUpdate()
    {
        if (bEngineStatus)
        {
            // Calculate and Apply Throttle Force
            v2ThrottleForce = trsPlayer.up * Physics2D.gravity.magnitude * fThrottle;
            if (Vector3.Dot(Vector3.up, trsPlayer.up) > 0.0f)
                rigidBody.AddForce(v2ThrottleForce / ((Vector3.Dot(Vector3.up, trsPlayer.up) + 1.0f) / 2.0f));

            // Calculate and Apply Net Velocity
            Vector3 v3Vel = new Vector3(
                fThrottle * fCyclic * fHorizontalSpeed,
                fThrottle * fCollective * fVerticalSpeed
            );

            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, v3Vel, 0.0625f);
            rigidBody.rotation = Mathf.Lerp(rigidBody.rotation, -fThrottle * fCyclic * fCyclicAngle, 0.03125f);
        }
    }

    void Update()
    {
        if (bUseJoystick)
        {
            fCollective = joystick.yAxis.value;
            fCyclic = joystick.xAxis.value;
        }
        else
        {
            fCollective = Input.GetAxis("Vertical");
            fCyclic = Input.GetAxis("Horizontal");
        }

        BladeRotation();

        if (bEngineStatus && pscPodWeapon)
        {
            if (Input.GetKey(KeyCode.Space) || ebtnFire.isPressed)
                pscPodWeapon.Fire();
            if (Input.GetKey(KeyCode.R))
                pscPodWeapon.Reload();
        }

        if (pscPodWeapon.bReloading)
        {
            if (!bReloadingProgess)
            {
                bReloadingProgess = true;
                StartCoroutine("Reloading");
            }
        }
    }

    IEnumerator StartEngine()
    {
        bEngineRunning = true;
        bEngineStatus = true;

        while (fThrottle < 1.0f)
        {
            fThrottle += F_ENGINE_RUNNING_DELTA;
            yield return new WaitForSeconds(fEngineStartingTime / N_ENGINE_RUNNING_STEPS);
        }
        
        bEngineRunning = false;
    }

    IEnumerator StopEngine()
    {
        bEngineRunning = true;

        while (fThrottle > 0.0f)
        {
            fThrottle -= F_ENGINE_RUNNING_DELTA;
            yield return new WaitForSeconds(fEngineStoppingTime / N_ENGINE_RUNNING_STEPS);
        }

        bEngineStatus = false;
        bEngineRunning = false;
    }

    void BladeRotation()
    {
        trsMainBlade.Rotate(0.0f, fThrottle * fMainRotorSpeed, 0.0f);
        trsTailBlade.Rotate(0.0f, 0.0f, -fThrottle * fTailRotorSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string it in strIgnoreTag)
        {
            if (other.CompareTag(it)) return;
        }

        if (bEngineStatus)
            StartCoroutine("StopEngine");

        btnEngine.image.sprite = sprEngineOff;
        bAvailableFlight = false;
    }

    IEnumerator Reloading()
    {
        float startTime = Time.time;

        imgReloadBar.fillAmount = 0.0f;
        imgReloadBar.GetComponent<Image>().color = new Color32(248, 84, 84, 255);

        while (Time.time - startTime < pscPodWeapon.fReloadTime)
        {
            imgReloadBar.fillAmount = (Time.time - startTime) / pscPodWeapon.fReloadTime;
            yield return new WaitForEndOfFrame();
        }

        imgReloadBar.fillAmount = 1.0f;
        bReloadingProgess = false;
        imgReloadBar.GetComponent<Image>().color = new Color32(186, 236, 186, 255);
    }
}
