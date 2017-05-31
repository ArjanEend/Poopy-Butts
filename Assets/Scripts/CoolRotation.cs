using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolRotation : MonoBehaviour {

    [SerializeField]
    private float maxRotation;
    [SerializeField]
    private float lerpSpeed;

    private Vector3 prevPos;
    private Vector3 prevSpeed;
    private Vector3 eulerAngles = Vector3.zero;

	void Start () {
        prevPos = transform.position;
	}
	
	void Update () {
        Vector3 diff = transform.position - prevPos;
        prevPos = transform.position;

        Vector3 speed = diff / Time.deltaTime;
        Vector3 speedDiff = speed - prevSpeed;
        prevSpeed = speed;

        speedDiff = transform.InverseTransformDirection(speedDiff);

        speedDiff = diff.normalized + speedDiff.normalized + speedDiff;

        eulerAngles.x = Mathf.Lerp(eulerAngles.x, speedDiff.z * maxRotation, Time.deltaTime * lerpSpeed);
        eulerAngles.z = Mathf.Lerp(eulerAngles.z, speedDiff.x * maxRotation, Time.deltaTime * lerpSpeed);

        transform.localEulerAngles = eulerAngles;
	}
}
