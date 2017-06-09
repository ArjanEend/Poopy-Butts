using Implementation.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GuardLine : ComponentUpdaterBase<GuardComponent> {
    [SerializeField]
    private LineRenderer lineRenderer;

    private Vector3 target = Vector3.zero;

    public override void Init(GuardComponent component)
    {
        target = component.position;
    }

    public override void OnRemove(GuardComponent component)
    {
        target = Vector3.zero;
    }

    public override void OnUpdate(GuardComponent component)
    {
        target = component.position;
    }

    // Use this for initialization
    void Start()
    {

    }


    void Update()
    {
        lineRenderer.enabled = target != Vector3.zero;
        if (target == Vector3.zero)
            return;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, (transform.position + transform.forward + target) * .5f);
        lineRenderer.SetPosition(2, target);
    }
}
