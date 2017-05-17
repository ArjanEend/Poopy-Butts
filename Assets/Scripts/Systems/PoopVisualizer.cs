using RocketWorks.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Implementation.Components;
using RocketWorks.Grouping;
using RocketWorks.Entities;

public class PoopVisualizer : UnitySystemBase
{
    private Group poopGroup;

    public override void Initialize(Contexts contexts)
    {
        poopGroup = contexts.Main.Pool.GetGroup(typeof(PoopComponent), typeof(VisualizationComponent), typeof(TransformComponent));
    }
    public override void Destroy()
    {
        throw new NotImplementedException();
    }

    public override void Execute(float deltaTime)
    {
        List<Entity> newEntities = poopGroup.NewEntities;
        for (int i = 0; i < newEntities.Count; i++)
        {
            VisualizationComponent comp = newEntities[i].GetComponent<VisualizationComponent>();
            comp.go.transform.localScale = Vector3.one * newEntities[i].GetComponent<PoopComponent>().size;
        }
    }

}
