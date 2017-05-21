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

    private int localUser;

    public override void Destroy()
    {
        throw new NotImplementedException();
    }

    public override void Execute(float deltaTime)
    {
        List<Entity> newEntities = group.NewEntities;
        for(int i = 0; i < newEntities.Count; i++)
        {
            VisualizationComponent comp = newEntities[i].GetComponent<VisualizationComponent>(vId);
            comp.go = Instantiate<GameObject>(Resources.Load<GameObject>(comp.resourceId));
            comp.go.name += " Entity: " + newEntities[i].CreationIndex;
        }
        for(int i = 0; i < group.Count; i++)
        {
            if (!group[i].IsDirty)
                continue;
            group[i].IsDirty = false;
            VisualizationComponent vComp = group[i].GetComponent<VisualizationComponent>(vId);
            TransformComponent tComp = group[i].GetComponent<TransformComponent>(tId);
            if (vComp.go != null)
            {
                GameObject go = vComp.go;
                go.transform.position = new Vector3(tComp.position.x, tComp.position.z, tComp.position.y);
                MovementComponent mov = group[i].GetComponent<MovementComponent>();
                if (mov == null)
                    continue;
                RocketWorks.Vector2 velocity = mov.velocity;
                Quaternion oldRot = go.transform.rotation;
                go.transform.eulerAngles = new Vector3(0f, Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg, 0f);
                //if(velocity != RocketWorks.Vector2.zero)
                    go.transform.rotation = Quaternion.RotateTowards(oldRot, go.transform.rotation, Time.deltaTime * 270f);
                if(GetComponentInChildren<Animator>() != null)
                    go.GetComponentInChildren<Animator>().SetFloat("Speed", velocity.Magnitude());
            }
        }
    }

    public override void Initialize(Contexts contexts)
    {
        EntityPool pool = contexts.Main.Pool;
        vId = pool.GetIndexOf(typeof(VisualizationComponent));
        tId = pool.GetIndexOf(typeof(TransformComponent));
        group = contexts.Main.Pool.GetGroup(typeof(VisualizationComponent), typeof(TransformComponent));
    }
}