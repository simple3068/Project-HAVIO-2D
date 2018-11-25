using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterController : MonoBehaviour
{
    [Header("Helicopter Info")]
    [SerializeField] Transform trsPlayer;
    [SerializeField] Transform trsCenterOfMass;
    [SerializeField] Rigidbody2D rigidBody;
    [Space]
    [SerializeField] Transform trsMainBlade;
    [SerializeField] float fMainRotorSpeed;
    [SerializeField] Transform trsTailBlade;
    [SerializeField] float fTailRotorSpeed;
    [Space]
    [SerializeField] float fEngineStartingTime;
    [SerializeField] float fEngineStoppingTime;

    const float N_ENGINE_RUNNING_STEPS = 32;
    const float F_ENGINE_RUNNING_DELTA = 1.0f / N_ENGINE_RUNNING_STEPS;

    float fThrottle;
    float fCollective;
    float fCycle;

    bool bEngineStatus = false;
    bool bEngineRunning = false;
    bool bAvailableFlight = true;

    void Start()
    {
        rigidBody.centerOfMass = trsCenterOfMass.localPosition;
    }

    void FixedUpdate()
    {
        if (bEngineStatus)
        {
            rigidBody.AddForce(trsPlayer.up * Physics2D.gravity.magnitude * fThrottle);
        }
    }

    void Update()
    {
        if (bAvailableFlight && !bEngineRunning && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!bEngineStatus)
            {
                StartCoroutine("StartEngine");
            }
            else
            {
                StartCoroutine("StopEngine");
            }
        }

        // TODO: Movable Controllable

        BladeRotation();
    }

    IEnumerator StartEngine()
    {
        bEngineRunning = true;

        while (fThrottle < 1.0f)
        {
            fThrottle += F_ENGINE_RUNNING_DELTA;
            Debug.Log("Engine is starting, throttle: " + fThrottle);
            yield return new WaitForSeconds(fEngineStartingTime / N_ENGINE_RUNNING_STEPS);
        }

        bEngineStatus = true;
        bEngineRunning = false;
    }
    IEnumerator StopEngine()
    {
        bEngineRunning = true;

        while (fThrottle > 0.0f)
        {
            fThrottle -= F_ENGINE_RUNNING_DELTA;
            Debug.Log("Engine is stopping, throttle: " + fThrottle);
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
}
