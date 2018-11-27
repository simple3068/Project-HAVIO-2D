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
    [Space]
    [SerializeField] float fVerticalSpeed;
    [SerializeField] float fCyclicSpeed;

    const float N_ENGINE_RUNNING_STEPS = 32;
    const float F_ENGINE_RUNNING_DELTA = 1.0f / N_ENGINE_RUNNING_STEPS;

    float fThrottle;
    float fCollective;
    float fCyclic;

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

            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, transform.up * fThrottle * fCollective * fVerticalSpeed, 0.0625f);
            rigidBody.angularVelocity = Mathf.Lerp(rigidBody.angularVelocity, -fThrottle * fCyclic * fCyclicSpeed, 0.125f);
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

        fCollective = Input.GetAxis("Vertical");
        fCyclic = Input.GetAxis("Horizontal");

        BladeRotation();
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
}
