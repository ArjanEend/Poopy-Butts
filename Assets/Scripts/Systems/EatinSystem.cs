using RocketWorks.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RocketWorks.Grouping;
using Implementation.Components;
using RocketWorks.Entities;

public struct EatingRef
{
    public Entity stomach;
    public EntityReference pickup;
    public EatingRef(Entity stomach, EntityReference pickup)
    {
        this.stomach = stomach;
        this.pickup = pickup;
    }
}

public class EatinSystem : UnitySystemBase
{
    private Group playerGroup;
    private Group foodGroup;

    private List<EatingRef> pickupEntities;

    public override void Initialize(Contexts contexts)
    {
        playerGroup = contexts.Main.Pool.GetGroup(typeof(Stomach), typeof(VisualizationComponent));
        playerGroup.OnEntityAdded += OnStomachUpdate;
        foodGroup = contexts.Main.Pool.GetGroup(typeof(PickupComponent), typeof(VisualizationComponent));
        pickupEntities = new List<EatingRef>();
    }

    private void OnStomachUpdate(Entity obj)
    {
        Stomach stomach = obj.GetComponent<Stomach>();
        for(int i = 0; i < stomach.pickups.Count; i++)
        {
            pickupEntities.Add(
                new EatingRef(obj, stomach.pickups[i].Entity));
        }
    }

    public override void Destroy()
    {
    }

    public override void Execute(float deltaTime)
    {
        for(int i = pickupEntities.Count -1; i >= 0; i--)
        {
            GameObject go = pickupEntities[i].pickup.Entity.GetComponent<VisualizationComponent>().go;
            if (go == null)
                continue;

            RocketLog.Log("Check on : " + go.name, this);
            if (go != null && go.transform.parent == null)
            {
                RocketLog.Log("Eating : " + go.name, this);
                pickupEntities[i].stomach.GetComponent<VisualizationComponent>().go.GetComponentInChildren<HandController>().EatObject(go.transform);
            }
            pickupEntities.RemoveAt(i);
        }
    }

}
