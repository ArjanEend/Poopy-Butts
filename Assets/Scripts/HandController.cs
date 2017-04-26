using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {
    
    [SerializeField]
    private Transform mouth;

    private List<Transform> objects = new List<Transform>();

    private Coroutine routine;

    private Vector3 normalPos;
    private Quaternion normalRot;

    private void Start()
    {
        normalPos = transform.localPosition;
        normalRot = transform.localRotation;
    }

    public void EatObject(Transform trans)
    {
        objects.Add(trans);
        if(routine == null)
           routine = StartCoroutine(EatTargets());
    }

    private IEnumerator EatTargets()
    {
        while (objects.Count != 0)
        {
            Transform trans = objects[0];
            objects.RemoveAt(0);
            float timer = 0f;
            Vector3 pos = transform.position;
            while (timer < .2f)
            {
                transform.position = Vector3.Lerp(pos, trans.position, timer / .2f);
                transform.LookAt(trans.position);
                timer += Time.deltaTime;
                yield return null;
            }
            timer = 0f;
            pos = transform.position;
            trans.SetParent(transform);
            while (timer < .4f)
            {
                transform.position = Vector3.Lerp(pos, mouth.position, timer / .4f);
                transform.LookAt(mouth.position);
                timer += Time.deltaTime;
                yield return null;
            }
            trans.gameObject.SetActive(false);
        }
        transform.localPosition = normalPos;
        transform.localRotation = normalRot;
        routine = null;
    }
}
