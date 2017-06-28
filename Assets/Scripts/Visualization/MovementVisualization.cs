using Implementation.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementVisualization : ComponentVisualizerBase<MovementComponent>
{

    private MovementComponent component;

    [SerializeField]
    private Animator animator;

    public override void Init(MovementComponent component)
    {
        this.component = component;
        enabled = true;
    }

    private void Update()
    {
        RocketWorks.Vector3 velocity = component.velocity;
        Quaternion oldRot = transform.rotation;
        transform.eulerAngles = new Vector3(0f, Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, 0f);
        if (velocity.Magnitude() > .01f)
            transform.rotation = Quaternion.RotateTowards(oldRot, transform.rotation, Time.deltaTime * 180f);
        else
            transform.rotation = oldRot;
        if (animator != null)
           animator.SetFloat("Speed", velocity.Magnitude());
    }

    public override void OnRemove(MovementComponent component)
    {
        this.enabled = false;
    }
}
