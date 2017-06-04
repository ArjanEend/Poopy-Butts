using Implementation.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonDisplay : ComponentUpdaterBase<ButtonComponent>
{

    [SerializeField]
    private ParticleSystem particles;

    [SerializeField]
    private int buttonId = 0;

    public override void Init(ButtonComponent component)
    {
        if (component.id != buttonId)
            return;

        particles.Play();
    }

    public override void OnUpdate(ButtonComponent component)
    {
        if (component.id != buttonId)
            return;

        particles.Stop();
    }
}
