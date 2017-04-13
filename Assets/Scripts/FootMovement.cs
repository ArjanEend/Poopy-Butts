﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootMovement : MonoBehaviour {

    [SerializeField]
    private float maxDistance;

    [SerializeField]
    private Transform targetTransform;
    

    private static Coroutine animationRoutine;

	// Use this for initialization
	void Start () {
        transform.parent = null;
	}
	

	void Update () {
        if (animationRoutine != null)
            return;
		if(Vector3.Distance(transform.position, targetTransform.position) > maxDistance)
        {
            animationRoutine = StartCoroutine(AnimateTowards());
        }
	}

    private IEnumerator AnimateTowards()
    {
        float animationTime = Random.Range(.1f, .2f);
        float currentTime = 0f;
        Vector3 start = transform.position;
        Quaternion startRot = transform.rotation;
        while(currentTime < animationTime)
        {
            transform.position = Vector3.Lerp(start, targetTransform.position, currentTime / animationTime);
            transform.position += Vector3.up * .05f * Mathf.Sin(Mathf.PI * (currentTime / animationTime));
            transform.rotation = Quaternion.Lerp(startRot, targetTransform.rotation, currentTime / animationTime);
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
        animationRoutine = null;
    }

}
