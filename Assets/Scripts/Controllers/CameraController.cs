using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Vector3 offset;

    private Transform target;

    public void Initialize(Transform target)
    {
        this.target = target;
	}
	
	void Update () {
        if (target == null)
            return;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * 4f);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), Time.deltaTime * .7f);	
	}
}
