using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolRotation : MonoBehaviour {

    private Vector3 prevPos;
    private Vector3 eulerAngles = Vector3.zero;

	void Start () {
        prevPos = transform.position;
	}
	
	void Update () {
        Vector3 diff = transform.position - prevPos;
        prevPos = transform.position;

        diff = transform.InverseTransformDirection(diff);

        eulerAngles.x = Mathf.Lerp(eulerAngles.x, diff.z * 90f, Time.deltaTime * 6f);
        eulerAngles.z = Mathf.Lerp(eulerAngles.z, diff.x * 90f, Time.deltaTime * 6f);

        transform.localEulerAngles = eulerAngles;
	}
}
