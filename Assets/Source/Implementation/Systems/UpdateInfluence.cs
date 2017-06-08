using System;
using RocketWorks.Systems;
using Implementation.Components;
using RocketWorks.Grouping;
using RocketWorks.Entities;
using RocketWorks.Networking;
using RocketWorks.Commands;
using BulletSharp;
using System.Collections.Generic;

public class UpdateInfluence : SystemBase
{
    private Group spawnerGroup;

    private SocketController socket;
    private float elapsedTime = 0f;

    public UpdateInfluence(SocketController socket)
    {
        this.socket = socket;
    }

    public override void Initialize(Contexts contexts)
    {
        base.Initialize(contexts);

        spawnerGroup = contexts.Main.Pool.GetGroup(typeof(SpawnerComponent), typeof(TriggerComponent), typeof(TransformComponent));
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }

    public override void Execute(float deltaTime)
    {
        elapsedTime += deltaTime;
        for(int i = 0; i < spawnerGroup.Count; i++)
        {
            TriggerComponent trigger = spawnerGroup[i].GetComponent<TriggerComponent>();
            OwnerComponent spawnOwner = spawnerGroup[i].GetComponent<OwnerComponent>();
            SpawnerComponent spawner = spawnerGroup[i].GetComponent<SpawnerComponent>();

            bool ownerChanged = false;
            for(int j = 0; j < trigger.GhostObject.OverlappingPairs.Count; j++)
            {
                if (trigger.GhostObject.OverlappingPairs[j] is GhostObject)
                    continue;
                Entity entity = trigger.GhostObject.OverlappingPairs[j].UserObject as Entity;
                if (entity == null)
                    continue;

                MovementComponent movement = entity.GetComponent<MovementComponent>();
                if (movement == null)
                    continue;

                if (entity.GetComponent<PlayerIdComponent>() != null)
                {
                    //contested = true;
                    if (InfluenceObject(spawner, spawnOwner, entity, deltaTime))
                        ownerChanged = true;
                }

                OwnerComponent owner = entity.GetComponent<OwnerComponent>();
                if(owner != null)
                {
                    if (InfluenceObject(spawner, spawnOwner, owner.playerReference, deltaTime))
                        ownerChanged = true;
                }
            }

            ReduceInfluences(spawner, spawnOwner, deltaTime);

            if (ownerChanged)
            {
                socket.WriteSocket(new MainContextUpdateComponentCommand(spawnOwner, spawnerGroup[i].CreationIndex));
            }
        }
    }

    private bool InfluenceObject(SpawnerComponent spawner, OwnerComponent owner, Entity entity, float deltaTime)
    {
        if (entity == null || owner.playerReference == entity)
            return false;
        var dict = spawner.Influences;
        if (!dict.ContainsKey(entity))
            dict.Add(entity, 0f);

        dict[entity] += deltaTime;
        if(dict[entity] >= 5f)
        {
            owner.playerReference = entity;
            spawner.lastTime = elapsedTime; 
            return true;
        }
        return false;
    }

    private void ReduceInfluences(SpawnerComponent spawner, OwnerComponent owner, float deltaTime)
    {
        var dict = spawner.Influences;
        var keys = new List<Entity>(dict.Keys);
        for (int i = keys.Count - 1; i >= 0; i--)
        {

            if (owner != null && keys[i] == owner.playerReference)
                continue;
            dict[keys[i]] -= deltaTime * .5f;
            if (dict[keys[i]] <= 0f)
                dict.Remove(keys[i]);
        }
    }

}
