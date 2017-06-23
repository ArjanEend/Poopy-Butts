using System;
using RocketWorks.Pooling;
using RocketWorks.Systems;
using RocketWorks.Grouping;
using Implementation.Components;
using System.Collections.Generic;
using UnityEngine;
using RocketWorks.Entities;

class VisualizationSystem : UnitySystemBase
{
    private Group group;

    private int localUser;

    public override void Initialize(Contexts contexts)
    {
        EntityPool pool = contexts.Main.Pool;
        group = contexts.Main.Pool.GetGroup(typeof(VisualizationComponent));
    }

    private void EntityRemoved(Entity obj)
    {
    }

    public override void Destroy()
    {
    }

    public override void Execute(float deltaTime)
    {
        List<Entity> newEntities = group.NewEntities;
        for (int i = 0; i < newEntities.Count; i++)
        {
            VisualizationComponent comp = newEntities[i].GetComponent<VisualizationComponent>();
            TransformComponent trans = newEntities[i].GetComponent<TransformComponent>();
            comp.go = Instantiate<GameObject>(Resources.Load<GameObject>(comp.resourceId.ToString()));
            comp.go.name += " Entity: " + newEntities[i].CreationIndex;
            comp.go.transform.position = trans.position;
            IEntityVisualizer[] visualizers = comp.go.GetComponentsInChildren<IEntityVisualizer>(true);
            for (int j = 0; j < visualizers.Length; j++)
                visualizers[j].Init(newEntities[i]);
        }
    }
}