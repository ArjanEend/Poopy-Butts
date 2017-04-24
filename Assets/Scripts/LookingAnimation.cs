using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAnimation : MonoBehaviour {

    private List<PointOfInterest> pointsOfInterest;

    [SerializeField]
    private Transform[] eyes;
    
    private Vector3[] upVectors;

    [SerializeField]
    private Transform[] pupils;

    [SerializeField]
    private float eyeLimits;

    [SerializeField]
    private float maxDotLook = .78f;

    private PointOfInterest currentPoint = null;

    void Start()
    {
        pointsOfInterest = new List<PointOfInterest>();

        upVectors = new Vector3[eyes.Length];
        for (int i = 0; i < eyes.Length; i++)
        {
            upVectors[i] = eyes[i].transform.up;
        }
    }
	
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PointOfInterest>() != null)
            pointsOfInterest.Add(other.GetComponent<PointOfInterest>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PointOfInterest>() != null)
            pointsOfInterest.Remove(other.GetComponent<PointOfInterest>());
    }

    void Update () {
		for(int i = pointsOfInterest.Count - 1; i >= 0; i--)
        {
            if (!pointsOfInterest[i].isActiveAndEnabled)
            {
                pointsOfInterest.RemoveAt(i);
                continue;
            }
            Vector3 lookDir = pointsOfInterest[i].transform.position - transform.position;
            if (Vector3.Dot(transform.forward, lookDir.normalized) >= maxDotLook)
            {
                currentPoint = pointsOfInterest[i];
            }
        }

        if (currentPoint == null)
            return;
        
        Vector3 currentDir = currentPoint.transform.position - transform.position;
        if (Vector3.Dot(transform.forward, currentDir.normalized) < maxDotLook)
            return;

        for (int i = 0; i < eyes.Length; i++)
        {
            currentDir = currentPoint.transform.position - eyes[i].position;
            Quaternion lookRotation = Quaternion.LookRotation(i == 1 ? -currentDir : currentDir, upVectors[i]);
            eyes[i].transform.rotation = Quaternion.Lerp(eyes[i].transform.rotation, lookRotation, Time.deltaTime * 2f);
        }

	}
}
