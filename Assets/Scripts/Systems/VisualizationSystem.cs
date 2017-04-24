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

    private Dictionary<VisualizationComponent, GameObject> visBindings;

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
            visBindings.Add(comp, Instantiate<GameObject>(Resources.Load<GameObject>(comp.resourceId)));
            if(newEntities[i].GetComponent<PlayerIdComponent>() != null)
            {
                GameObject.FindObjectOfType<CameraController>().Initialize(visBindings[comp].transform);
            }
        }
        for(int i = 0; i < group.Count; i++)
        {
            VisualizationComponent vComp = group[i].GetComponent<VisualizationComponent>(vId);
            TransformComponent tComp = group[i].GetComponent<TransformComponent>(tId);
            if (visBindings.ContainsKey(vComp))
            {
                GameObject go = visBindings[vComp];
                go.transform.position = new Vector3(tComp.position.x, 0f, tComp.position.y);
                RocketWorks.Vector2 velocity = group[i].GetComponent<MovementComponent>().velocity;
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
        visBindings = new Dictionary<VisualizationComponent, GameObject>();
        vId = pool.GetIndexOf(typeof(VisualizationComponent));
        tId = pool.GetIndexOf(typeof(TransformComponent));
        group = contexts.Main.Pool.GetGroup(typeof(VisualizationComponent), typeof(TransformComponent));
    }
}