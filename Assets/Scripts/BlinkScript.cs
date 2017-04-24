using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour {

    [SerializeField]
    private float minDelay;
    [SerializeField]
    private float maxDelay;

    private float delay;
    private float lastBlink;

    [SerializeField]
    private Transform[] eyes;

    [SerializeField]
    private Vector3 blinkScale;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (Time.time - lastBlink > delay)
        {
            delay = Random.Range(minDelay, maxDelay);
            lastBlink = Time.time;
        }

        float blink = Mathf.Pow(Time.time - lastBlink, 12f);
        if (blink > 2f)
        {
            blink *= -1f;
            blink += 4f;
        }
        Vector3 targetScale = Vector3.Lerp(Vector3.one, blinkScale, blink);
        for(int i = 0; i < eyes.Length; i++)
        {
            eyes[i].localScale = targetScale;
        }
	}
}
