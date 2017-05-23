using System;
using RocketWorks.Systems;
using Implementation.Components;
using RocketWorks.Grouping;
using RocketWorks.Entities;

public class UpdateInfluence : SystemBase
{
    private Group spawnerGroup;

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
        for(int i = 0; i < spawnerGroup.Count; i++)
        {
            TriggerComponent trigger = spawnerGroup[i].GetComponent<TriggerComponent>();
            OwnerComponent spawnOwner = spawnerGroup[i].GetComponent<OwnerComponent>();
            Entity influenceEntity = spawnOwner.playerReference.Entity;
            bool contested = false;
            for(int j = 0; j < trigger.GhostObject.OverlappingPairs.Count; j++)
            {
                Entity entity = trigger.GhostObject.OverlappingPairs[j].UserObject as Entity;
                if (entity == null)
                    continue;

                MovementComponent movement = entity.GetComponent<MovementComponent>();
                if (movement == null)
                    continue;

                if (entity.GetComponent<PlayerIdComponent>() != null)
                {
                    if(influenceEntity == null)
                        influenceEntity = entity;
                    if (influenceEntity != entity)
                        contested = true;
                }

                OwnerComponent owner = entity.GetComponent<OwnerComponent>();
                if(owner != null)
                {
                    if (influenceEntity == null)
                        influenceEntity = owner.playerReference.Entity;
                    if (influenceEntity != entity)
                        contested = true;
                }
            }

            if (!contested && influenceEntity != null)
                spawnOwner.playerReference = influenceEntity;
        }
    }

}
