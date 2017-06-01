using Implementation.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthParticles : ComponentUpdaterBase<HealthComponent> {

    [SerializeField]
    private ParticleSystem particles;

    public override void Init(HealthComponent component)
    {
        //particles.Play();
    }

    public override void OnUpdate(HealthComponent component)
    {
        particles.Play();
    }
}
