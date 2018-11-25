using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] float fFollowingSpeed;
    [SerializeField] Transform trsTarget;

    Vector3 v3NewPosition;

	void Update ()
    {
        v3NewPosition = trsTarget.position;
        v3NewPosition.z = transform.position.z;

        transform.position = Vector3.Slerp(transform.position, v3NewPosition, fFollowingSpeed * Time.deltaTime);
    }
}
