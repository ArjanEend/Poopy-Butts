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

    private int vId;
    private int tId;

    private Dictionary<VisualizationComponent, GameObject> visBindings;

    public override void Destroy()
    {
        throw new NotImplementedException();
    }

    public override void Execute()
    {
        List<Entity> newEntities = group.NewEntities;
        for(int i = 0; i < newEntities.Count; i++)
        {
            VisualizationComponent comp = newEntities[i].GetComponent<VisualizationComponent>(vId);
            visBindings.Add(comp, Instantiate<GameObject>(Resources.Load<GameObject>(comp.resourceId)));
        }
        for(int i = 0; i < group.Count; i++)
        {
            VisualizationComponent vComp = group[i].GetComponent<VisualizationComponent>(vId);
            TransformComponent tComp = group[i].GetComponent<TransformComponent>(tId);
            if (visBindings.ContainsKey(vComp))
            {
                GameObject go = visBindings[vComp];
                go.transform.position = new Vector3(tComp.position.x, 0f, tComp.position.y);
            }
        }
    }

    public override void Initialize(EntityPool pool)
    {
        visBindings = new Dictionary<VisualizationComponent, GameObject>();
        vId = pool.GetIndexOf(typeof(VisualizationComponent));
        tId = pool.GetIndexOf(typeof(TransformComponent));
        group = pool.GetGroup(typeof(VisualizationComponent), typeof(TransformComponent));
    }
}