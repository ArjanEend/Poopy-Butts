using Implementation.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSync : ComponentVisualizerBase<TransformComponent>
{
    private TransformComponent transComp;

    public override void Init(TransformComponent component)
    {
        this.transComp = component;
    }

    public override void OnRemove(TransformComponent component)
    {
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.position = transComp.position;
        transform.eulerAngles = transComp.rotation;
	}
}
