using RocketWorks.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RocketWorks.Grouping;
using Implementation.Components;
using RocketWorks.Entities;

public class EatinSystem : UnitySystemBase
{
    private Group playerGroup;
    private Group foodGroup;

    private List<Entity> pickupEntities;

    public override void Initialize(Contexts contexts)
    {
        playerGroup = contexts.Main.Pool.GetGroup(typeof(Stomach), typeof(VisualizationComponent));
        playerGroup.OnEntityAdded += OnStomachUpdate;
        foodGroup = contexts.Main.Pool.GetGroup(typeof(PickupComponent), typeof(VisualizationComponent));
        pickupEntities = new List<Entity>();
    }

    private void OnStomachUpdate(Entity obj)
    {
        Stomach stomach = obj.GetComponent<Stomach>();
        for(int i = 0; i < stomach.pickups.Count; i++)
        {
            pickupEntities.Add(stomach.pickups[i].Entity);
        }
    }

    public override void Destroy()
    {
    }

    public override void Execute(float deltaTime)
    {
        for(int i = 0; i < pickupEntities.Count; i++)
        {
            GameObject go = pickupEntities[i].GetComponent<VisualizationComponent>().go;
            if (go != null && go.transform.parent == null)
            {
                playerGroup[0].GetComponent<VisualizationComponent>().go.GetComponentInChildren<HandController>().EatObject(go.transform);
            }
        }
        pickupEntities.Clear();
    }

}
