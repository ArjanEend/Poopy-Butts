using UnityEngine;
using System.Collections;

public class UpdateComp : MonoBehaviour {

	public float offset;

	// Use this for initialization
	void Start () {
		offset = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.y = Mathf.Sin (Time.time + offset) * 5f;
		transform.position = pos;
	}
}
