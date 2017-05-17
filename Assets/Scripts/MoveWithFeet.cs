using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithFeet : MonoBehaviour {

    [SerializeField]
    private Transform[] feet;

    [SerializeField]
    private float distanceAboveFeet;

    [SerializeField]
    private Vector3 originalPosition;

    [SerializeField]
    private float dampenRotation;

    private Vector3 prevPos;

	// Use this for initialization
	void Start () {
        prevPos = transform.position;
	}
	
	void LateUpdate () {
        Vector3 midPoint = Vector3.zero;
        Vector3 upVector = Vector3.up;
        Vector3 forwardVector = transform.parent.forward;
        for(int i = 0; i < feet.Length; i++)
        {
            midPoint += feet[i].position / feet.Length;
        }

        midPoint += forwardVector * .05f;
        
        transform.position = Vector3.Lerp(prevPos, midPoint + upVector * distanceAboveFeet, Time.deltaTime * 5f);
        prevPos = transform.position;
	}
}
