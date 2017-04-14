using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceVertexColors : MonoBehaviour {

    [SerializeField]
    private Gradient colorGradient;

	void Start () {
        SkinnedMeshRenderer meshf = gameObject.GetComponent<SkinnedMeshRenderer>();
        meshf.material.color = colorGradient.Evaluate(Random.Range(0f, 1f));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
